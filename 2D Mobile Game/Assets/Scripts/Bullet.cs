using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    private ObjectToAttack objectToAttack = ObjectToAttack.None;

    private Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && objectToAttack == ObjectToAttack.Enemy)
        {
            DamageEnemy(collision);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FlipSprite();
    }

    private void FlipSprite()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon || Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;

        if (isMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1);
        }
    }

    private void DamageEnemy(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }

    private void DamagePlayer(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        player.TakeDamage(damage);
        Destroy(gameObject);
    }

    public void SetObjectToAttack(ObjectToAttack obj)
    {
        objectToAttack = obj;
    }

    public enum ObjectToAttack { None, Player, Enemy }
}
