using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private bool isHealth = true, isAmmo = false;
    [Min(0), SerializeField] private int ammoToGive = 30;
    [SerializeField] private AudioClip pickupSFX;

    //Internal Variables
    private Player player;
    private Gun gun;
    private AudioSource audioSource;
    //

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gun = FindObjectOfType<Gun>();
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PickupItem();
        }
    }

    private void PickupItem()
    {
        EnablePowerups();
        audioSource.PlayOneShot(pickupSFX);
        Destroy(gameObject);
    }

    private void EnablePowerups()
    {
        if (isHealth)
        {
            player.AddHealthPack();
        }
        else if (isAmmo)
        {
            gun.AddAmmo(ammoToGive);
        }
    }
}
