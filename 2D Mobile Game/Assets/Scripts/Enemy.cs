using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private EnemyChase enemyChase = EnemyChase.None;
    [Min(0), SerializeField] private float moveSpeed = 1;
    [Min(0), SerializeField] private float chaseDistance = 1;
    
    [Header("Health")]
    [Min(0), SerializeField] private int maxHealth = 100;

    [Header("Damage")]
    [SerializeField] private EnemyAttack enemyAttack = EnemyAttack.None;
    [Min(0), SerializeField] private int attackDamageToDeal = 1;
    [Min(0), SerializeField] private float attackDelay = 1;

    [Header("Gun")]
    [Min(0), SerializeField] private int gunDamageToDeal = 1;
    [Min(0), SerializeField] private float shootDelay = 1;
    [Min(0), SerializeField] private float bulletSpeed = 1;
    [Min(0), SerializeField] private float bulletLifeTime = 1;

    [Header("Other")]
    [SerializeField] private int scoreToGiveAfterDeath = 1;
    [SerializeField] private AudioClip hurtSFX;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shootSFX;

    //Internal Variables
    private bool canMove = true;
    private int currentHealth = 0;
    private float attackTimer, shootTimer;
    private Slider healthBar;
    private Player player;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Vector3 healthBarScale;
    private EnemyBullet bullet;
    //Game specific only - remove if unnecessary
    //private EnemyCounter enemyCounter;
    //


    private void Awake()
    {
        //enemyCounter = FindObjectOfType<EnemyCounter>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audioSource = Camera.main.GetComponent<AudioSource>();
        healthBar = GetComponentInChildren<Slider>();
        bullet = bulletPrefab.GetComponent<EnemyBullet>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = healthBar.maxValue;
        healthBar.gameObject.SetActive(false);
        attackTimer = attackDelay;
        shootTimer = shootDelay;
        healthBarScale = healthBar.transform.localScale;
        canMove = enemyChase != EnemyChase.None;
        rb.bodyType = canMove ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        bullet.damage = gunDamageToDeal;
    }

    private void Update()
    {
        DisplayHealth();

        if (currentHealth <= 0)
        {
            Die();
        }

        FixHealthBugs();

        attackTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;

        if (enemyAttack == EnemyAttack.Shoot)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveEnemy();
        }
        else
        {
            rb.velocity = Vector2.zero;
            return;
        }
    }

    //Movement
    private void MoveEnemy()
    {
        float moveMultiplier = 100 * moveSpeed;
        Vector3 playerPosition = player.transform.position - transform.position;
        bool playerIsClose = playerPosition.magnitude < chaseDistance;

        //Chases player based on type of chase
        if (enemyChase == EnemyChase.Instantly || (enemyChase == EnemyChase.Proximity && playerIsClose) || currentHealth < maxHealth)
        {
            playerPosition.Normalize();
            Vector3 moveEnemy = moveMultiplier * Time.fixedDeltaTime * playerPosition;

            rb.velocity = moveEnemy;
            FlipSprite();
        }
        else if (enemyChase == EnemyChase.Proximity && !playerIsClose)
        {
            rb.velocity = Vector2.zero;
            return;
        }
    }

    private void FlipSprite()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (isMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(-rb.velocity.x), 1);
        }
    }
    //

    //Health and Damage
    private void DisplayHealth()
    {
        Vector3 originalScale = gameObject.transform.localScale.x < 0 ? new(-healthBarScale.x, healthBarScale.y, healthBarScale.z) 
                                : healthBarScale;
        healthBar.transform.localScale = originalScale;

        if (currentHealth < maxHealth)
        {
            healthBar.gameObject.SetActive(true);
        }

        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        audioSource.PlayOneShot(hurtSFX);
    }

    private void Attack(int damage)
    {
        player.TakeDamage(damage);
        attackTimer = 0;
    }

    private void Die()
    {
        canMove = false;
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.AddKill();
        playerStats.AddScore(scoreToGiveAfterDeath);
        healthBar.gameObject.SetActive(false);
        Destroy(gameObject);

        //Game specific only - remove if unnecessary
        //enemyCounter.enemiesRemaining--;
    }

    public void Shoot()
    {
        float moveMultiplier = 10 * bulletSpeed;
        Vector3 playerPosition = player.transform.position - transform.position;
        bool playerIsClose = playerPosition.magnitude < chaseDistance;

        if (shootTimer >= shootDelay && (enemyChase == EnemyChase.Instantly
                                         || (enemyChase == EnemyChase.Proximity && playerIsClose)
                                         || currentHealth < maxHealth))
        {
            playerPosition.Normalize();
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = moveMultiplier * playerPosition;
            audioSource.PlayOneShot(shootSFX);
            Destroy(bullet, bulletLifeTime);
            shootTimer = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackTimer >= attackDelay && enemyAttack == EnemyAttack.Melee)
        {
            Attack(attackDamageToDeal);
        }
    }

    private void FixHealthBugs()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    //Other
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    private enum EnemyAttack { None, Melee, Shoot }

    private enum EnemyChase { None, Instantly, Proximity }
    //
}
