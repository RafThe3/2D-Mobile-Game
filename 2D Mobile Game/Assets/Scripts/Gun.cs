using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [Min(0), SerializeField] private float bulletLifeTime = 1;
    [Min(0), SerializeField] private float bulletSpeed = 1;

    [Header("Gun")]
    [SerializeField] private bool automaticFire = true;
    [Min(0), SerializeField] private float shootDelay = 1;
    [Min(0)] public int damageToDeal = 1;
    [Min(0)] public int startingAmmo = 30;

    [Header("Other")]
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip reloadSFX;
    [SerializeField] private TMPro.TextMeshProUGUI ammoText;

    //Internal Variables
    private float shootTimer;
    private int currentAmmo;

    private void Start()
    {
        shootTimer = shootDelay;
        currentAmmo = startingAmmo;
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if ((Input.GetButtonDown("Fire1") && !automaticFire || Input.GetButton("Fire1") && automaticFire) && shootTimer >= shootDelay)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        ammoText.text = $"= {currentAmmo}";
    }

    public void Reload()
    {
        if (currentAmmo <= 0)
        {
            currentAmmo = startingAmmo;
            Camera.main.GetComponent<AudioSource>().PlayOneShot(reloadSFX);
        }
    }

    private void Shoot()
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
