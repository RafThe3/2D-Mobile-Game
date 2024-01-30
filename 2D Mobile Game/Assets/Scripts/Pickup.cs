using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Player player;
    private Gun gun;
    [SerializeField] private bool isHealth = true, isAmmo = false;
    [SerializeField] private int ammoToGive = 30;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gun = FindObjectOfType<Gun>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(isHealth)
            {
                player.AddHealthPack();
            }
            else if (isAmmo)
            {
                gun.AddAmmo(ammoToGive);
            }
            Destroy(gameObject);
        }
    }
}
