using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private EnemyChase enemyChase = EnemyChase.None;
    [Min(0), SerializeField] private float moveSpeed = 1;
    [Min(0), SerializeField] private float chaseDistance = 1;
    
    [Header("Health")]
    [Min(0), SerializeField] private int maxHealth = 100;

    [Header("Damage")]
    [SerializeField] private EnemyAttack enemyAttack = EnemyAttack.None;
    [Min(0), SerializeField] private int damageToDeal = 1;
    [Min(0), SerializeField] private float damageDelay = 1;
    [Min(0), SerializeField] private float bulletSpeed = 1;
    [Min(0), SerializeField] private float bulletLifeTime = 1;

    [Header("Other")]
    [SerializeField] private int scoreToGiveAfterDeath = 1;
    [SerializeField] private AudioClip hurtSFX;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shootSFX;

    //Internal Variables
    private int currentHealth = 0;
    private float damageTimer;
    private Slider healthBar;
    private Player player;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Vector3 healthBarScale;
    //Game specific only - remove if unnecessary
    //private EnemyCounter enemyCounter;
    //

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = healthBar.maxValue;
        healthBar.gameObject.SetActive(false);
        damageTimer = damageDelay;
        healthBarScale = healthBar.transform.localScale;
    }

    private void Awake()
    {
        //enemyCounter = FindObjectOfType<EnemyCounter>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audioSource = Camera.main.GetComponent<AudioSource>();
        healthBar = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        DisplayHealth();

        if (currentHealth <= 0)
        {
            Die();
        }

        FixHealthBugs();

        damageTimer += Time.deltaTime;

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
    }

    //Movement
    private void MoveEnemy()
    {
        float moveMultiplier = 10 * moveSpeed;
        Vector3 playerPosition = player.transform.position;
        Vector3 playerPositionFromEnemy = playerPosition - transform.position;

        if (enemyChase == EnemyChase.Instantly || (enemyChase == EnemyChase.Proximity && playerPositionFromEnemy.magnitude < chaseDistance))
        {
            playerPositionFromEnemy.Normalize();
            Vector3 moveEnemy = moveMultiplier * playerPositionFromEnemy;

            rb.velocity = moveEnemy;
            FlipSprite();
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

    private void DealDamage(int damage)
    {
        player.TakeDamage(damage);
        damageTimer = 0;
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
        if (damageTimer >= damageDelay)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector3 shootDir = playerPosition - transform.position;
            shootDir.Normalize();
            float moveMultiplier = 10 * bulletSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = moveMultiplier * shootDir;
            audioSource.PlayOneShot(shootSFX);
            Destroy(bullet, bulletLifeTime);
            damageTimer = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && damageTimer >= damageDelay && enemyAttack == EnemyAttack.Melee)
        {
            DealDamage(damageToDeal);
        }
    }

    private void FixHealthBugs()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    private enum EnemyAttack { None, Melee, Shoot }

    private enum EnemyChase { None, Instantly, Proximity }
    //
}
