using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private bool canMove = true;
    [Min(0), SerializeField] private float moveSpeed = 1;
    
    [Header("Health")]
    [Min(0) ,SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthBar;

    [Header("Damage")]
    [Min(0) ,SerializeField] private int damageToDeal = 1;
    [Min(0) ,SerializeField] private float damageDelay = 1;

    [Header("Other")]
    [SerializeField] private AudioClip hurtSFX;

    //Internal Variables
    private int currentHealth = 0;
    private float damageTimer;
    private GameObject player;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    //Game specific only - remove if unnecessary
    private EnemyCounter enemyCounter;
    //

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = healthBar.maxValue;
        healthBar.gameObject.SetActive(false);
        damageTimer = damageDelay;
    }

    private void Awake()
    {
        enemyCounter = FindObjectOfType<EnemyCounter>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (canMove)
        {
            MoveEnemy();
        }

        DisplayHealth();

        if (currentHealth <= 0)
        {
            Die();
        }

        FixHealthBugs();

        damageTimer += Time.deltaTime;
        
    }

    //Movement
    private void MoveEnemy()
    {
        float moveMultiplier = 10 * moveSpeed;
        Vector3 playerPosition = player.transform.position;
        Vector3 playerPositionFromEnemy = playerPosition - transform.position;
        playerPositionFromEnemy.Normalize();
        Vector3 moveEnemy = playerPositionFromEnemy * moveMultiplier;

        rb.velocity = moveEnemy;
    }
    //

    //Health and Damage
    private void DisplayHealth()
    {
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
        player.GetComponent<Player>().TakeDamage(damage);
        damageTimer = 0;
    }

    private void Die()
    {
        canMove = false;
        healthBar.gameObject.SetActive(false);
        Destroy(gameObject);

        //Game specific only - remove if unnecessary
        enemyCounter.enemiesRemaining--;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && damageTimer >= damageDelay)
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
    //
}
