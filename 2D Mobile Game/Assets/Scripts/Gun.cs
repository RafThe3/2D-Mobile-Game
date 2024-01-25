using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] private bool canShoot = true;
    [SerializeField] private bool automaticFire = true;
    [Min(0), SerializeField] private float shootDelay = 1;
    [Min(0), SerializeField] private int damageToDeal = 1;
    [SerializeField] private bool infiniteAmmo = true;
    [Min(0), SerializeField] private int startingAmmo = 30;
    [Min(0), SerializeField] private int numberOfRounds = 1;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [Min(0), SerializeField] private float bulletLifeTime = 1;
    [Min(0), SerializeField] private float bulletSpeed = 1;

    [Header("Other")]
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip reloadSFX;
    [SerializeField] private TMPro.TextMeshProUGUI ammoText;

    //Internal Variables
    private float shootTimer;
    private int currentAmmo, reserveAmmo;

    private void Start()
    {
        shootTimer = shootDelay;
        currentAmmo = startingAmmo;
        reserveAmmo = (startingAmmo * numberOfRounds) - startingAmmo;
        bulletPrefab.GetComponent<Bullet>().damage = damageToDeal;
    }

    private void Update()
    {
        FixAmmoBugs();
        UpdateText();

        shootTimer += Time.deltaTime;

        if (canShoot)
        {
            bool isShooting = Input.GetButtonDown("Fire1") && !automaticFire || Input.GetButton("Fire1") && automaticFire;
            if (isShooting && shootTimer >= shootDelay)
            {
                Shoot();
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
                reserveAmmo -= startingAmmo - currentAmmo;
                currentAmmo = startingAmmo;
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
            bullet.GetComponent<Rigidbody2D>().velocity = 10 * bulletSpeed * shootDir;
            Camera.main.GetComponent<AudioSource>().PlayOneShot(shootSFX);
            Destroy(bullet, bulletLifeTime);
            shootTimer = 0;
            currentAmmo--;
        }
    }
}
