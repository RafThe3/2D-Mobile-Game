using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Gun gun;
    [HideInInspector] public int damage;

    private void Awake()
    {
        gun = FindObjectOfType<Gun>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DamageEnemy(collision);
        }
    }

    private void DamageEnemy(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }
}
