using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private Player player;
    public bool isHealth = true;
    public bool isAmmo = false;
    public int ammoGiven = 30;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(isHealth)
            {
                player.CollectHealthPack();
            }
            else if (isAmmo)
            {
                player.CollectAmmoPack(ammoGiven);
            }
            Destroy(gameObject);
        }
    }
}
