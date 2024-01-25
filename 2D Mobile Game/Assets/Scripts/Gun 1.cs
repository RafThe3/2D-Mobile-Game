using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun1 : MonoBehaviour
{
    #region Variables

    #region Bullet Settings
    [Header("Bullet Settings")]
    [Min(0), SerializeField] private float bulletSpeed = 1;
    [Min(0), SerializeField] private float bulletLife = 1;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletSpawnPoint;
    #endregion

    #region Gun Settings
    [Header("Gun Settings")]
    [SerializeField] private bool infiniteAmmo = false;
    [SerializeField] private bool automaticFire = true;
    [Min(0), SerializeField] private float shootInterval = 1;
    [Min(0), SerializeField] private int maxStartingAmmo = 30;
    [Min(0), SerializeField] private int amountOfRounds = 1;
    [Min(0), SerializeField] private int damageToDeal = 10;
    #endregion

    #region SFX Settings
    [Header("SFX Settings")]
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioSource audioSource;
    #endregion

    #region UI Settings
    [Header("UI Settings")]
    [SerializeField] private TMPro.TextMeshProUGUI ammoText;
    [SerializeField] private TMPro.TextMeshProUGUI lowAmmoText;
    #endregion

    #region Internal Variables
    private float shootTimer;
    private int currentAmmo, reserveAmmo;
    #endregion

    #endregion

    private void Start()
    {
        shootTimer = shootInterval;
        reserveAmmo = (maxStartingAmmo * amountOfRounds) - maxStartingAmmo;
        currentAmmo = maxStartingAmmo;
        bulletPrefab.GetComponent<Bullet>().damage = damageToDeal;
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;

        UpdateText();

        //Shoot

        if (automaticFire && Input.GetKey(KeyCode.Mouse0) || !automaticFire && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        //Reload

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (infiniteAmmo)
            {
                InfiniteReload();
            }
            else
            {
                Reload();
            }
        }
    }


    #region Methods
    private void InfiniteReload()
    {
        if (currentAmmo < maxStartingAmmo)
        {
            currentAmmo = maxStartingAmmo;
        }
    }

    private void Reload()
    {
        if (currentAmmo < maxStartingAmmo && reserveAmmo > 0)
        {
            reserveAmmo -= maxStartingAmmo - currentAmmo;
            currentAmmo = maxStartingAmmo;
        }

        //Code below fixes bugs

        if (reserveAmmo < 0)
        {
            reserveAmmo = 0;
        }

        if (currentAmmo < 0)
        {
            currentAmmo = 0;
        }
    }

    private void Shoot()
    {
        if (shootTimer >= shootInterval && currentAmmo > 0)
        {
            GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);
            bulletClone.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.transform.TransformDirection(10 * bulletSpeed * Vector3.forward);
            audioSource.PlayOneShot(shootSFX);
            Destroy(bulletClone, bulletLife);
            shootTimer = 0;
            currentAmmo--;
        }
    }
    private void UpdateText()
    {
        //Update ammo

        ammoText.text = infiniteAmmo ? $"Ammo : {currentAmmo} / Infinity" : $"Ammo: {currentAmmo} / {reserveAmmo}";

        //Enable the low ammo text if player has low ammo

        lowAmmoText.enabled = currentAmmo <= 10;

        //Update the ammo text between low or no ammo

        if (currentAmmo <= 10)
        {
            if (reserveAmmo > 0)
            {
                lowAmmoText.text = "Low Ammo";
            }
            else if (currentAmmo <= 0 && reserveAmmo <= 0)
            {
                lowAmmoText.text = "No Ammo";
            }
        }
    }
    #endregion
}
