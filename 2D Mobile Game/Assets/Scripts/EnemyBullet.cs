using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [HideInInspector] public int damage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DamagePlayer(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        player.TakeDamage(damage);
        Destroy(gameObject);
    }
}
