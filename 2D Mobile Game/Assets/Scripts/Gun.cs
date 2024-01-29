using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] private bool canShoot = true;
    [SerializeField] private bool automaticFire = true;
    [SerializeField] private bool infiniteAmmo = true;
    [Min(0), SerializeField] private float shootDelay = 1;
    [Min(0), SerializeField] private int damageToDeal = 1;
    [Min(0), SerializeField] private int startingAmmo = 30;
    [Min(0), SerializeField] private int startingRounds = 1;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [Min(0), SerializeField] private float bulletLifeTime = 1;
    [Min(0), SerializeField] private float bulletSpeed = 1;

    [Header("Other")]
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip reloadSFX;
    [SerializeField] private TMPro.TextMeshProUGUI ammoText;
    [SerializeField] private Joystick aimingJoystick;

    //Internal Variables
    private float shootTimer;
    private int currentAmmo, reserveAmmo, maxAmmo;

    private void Start()
    {
        shootTimer = shootDelay;
        currentAmmo = startingAmmo;
        reserveAmmo = (startingAmmo * startingRounds) - startingAmmo;
        maxAmmo = reserveAmmo;
        bulletPrefab.GetComponent<Bullet>().damage = damageToDeal;
    }

    private void Update()
    {
        FixAmmoBugs();
        UpdateText();

        shootTimer += Time.deltaTime;

        if (canShoot)
        {
            if (!FindObjectOfType<Player>().allowKeyControls)
            {
                if (shootTimer >= shootDelay)
                {
                    JoystickShoot();
                }
            }
            else
            {
                bool isShooting = Input.GetButtonDown("Fire1") && !automaticFire || Input.GetButton("Fire1") && automaticFire;
                if (isShooting && shootTimer >= shootDelay)
                {
                    Shoot();
                }
            }


            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }
    }

    private void UpdateText()
    {
        //Ammo
        ammoText.text = !infiniteAmmo ? $"Ammo: {currentAmmo} / {reserveAmmo}" : $"Ammo: {currentAmmo} / {Mathf.Infinity}";
        ammoText.color = currentAmmo <= 10 ? Color.red : Color.white;
    }

    private void FixAmmoBugs()
    {
        if (currentAmmo < 0)
        {
            currentAmmo = 0;
        }

        if (reserveAmmo < 0)
        {
            reserveAmmo = 0;
        }
    }

    public void Reload()
    {
        if (infiniteAmmo)
        {
            if (currentAmmo < startingAmmo)
            {
                currentAmmo = startingAmmo;
            }
        }
        else
        {
            if (currentAmmo < startingAmmo && reserveAmmo > 0)
            {
                int reloadAmount = startingAmmo - currentAmmo;
                reloadAmount = (reserveAmmo - reloadAmount) < 0 ? reserveAmmo : reloadAmount;
                currentAmmo += reloadAmount;
                reserveAmmo -= reloadAmount;
            }
        }
        Camera.main.GetComponent<AudioSource>().PlayOneShot(reloadSFX);
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 shootDir = mousePosition - transform.position;
            shootDir.Normalize();
            float moveMultiplier = 10 * bulletSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = moveMultiplier * shootDir;
            Camera.main.GetComponent<AudioSource>().PlayOneShot(shootSFX);
            Destroy(bullet, bulletLifeTime);
            currentAmmo--;
            shootTimer = 0;
        }
    }

    public void JoystickShoot()
    {
        Vector2 aim = Vector2.zero;
        aim.x = aimingJoystick.Horizontal;
        aim.y = aimingJoystick.Vertical;
        bool isAiming = Mathf.Abs(aim.x) > 0 && Mathf.Abs(aim.y) > 0;

        if (currentAmmo > 0 && isAiming)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 shootDir = new(aim.x, aim.y);
            shootDir.Normalize();
            float moveMultiplier = 10 * bulletSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = moveMultiplier * shootDir;
            Camera.main.GetComponent<AudioSource>().PlayOneShot(shootSFX);
            Destroy(bullet, bulletLifeTime);
            currentAmmo--;
            shootTimer = 0;
        }
    }

    public void AddAmmo(int ammo)
    {
        if (reserveAmmo < maxAmmo)
        {
            return;
        }

        reserveAmmo += ammo;

        if (reserveAmmo > maxAmmo)
        {
            reserveAmmo = maxAmmo;
        }
    }
}
