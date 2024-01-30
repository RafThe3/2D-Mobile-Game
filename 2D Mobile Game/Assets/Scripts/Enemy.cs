using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Variables

    #region Movement
    [Header("Movement")]
    [SerializeField] private bool canMove = true;
    [Min(0), SerializeField] private float moveSpeed = 1;
    #endregion

    #region Health
    [Header("Health")]
    [Min(0) ,SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthBar;

    //Internal Variables
    private int currentHealth = 0;
    #endregion

    #region Damage
    [Header("Damage")]
    [Min(0) ,SerializeField] private int damageToDeal = 1;
    [Min(0) ,SerializeField] private float damageDelay = 1;

    //Internal Variables
    private float damageTimer;
    #endregion

    #region Other
    private GameObject player;
    private Rigidbody2D rb;

    //Game specific only - remove if unnecessary
    private EnemyCounter enemyCounter;
    #endregion

    #endregion

    private void Start()
    {
        #region Health
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = healthBar.maxValue;
        healthBar.gameObject.SetActive(false);
        #endregion

        #region Damage
        damageTimer = damageDelay;
        #endregion

        #region Other
        //Game specific only
        enemyCounter = FindObjectOfType<EnemyCounter>();
        #endregion
    }

    private void Awake()
    {
        #region Other
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        #endregion
    }

    private void Update()
    {
        #region Movement
        if (canMove)
        {
            MoveEnemy();
        }
        #endregion

        #region Health
        DisplayHealth();

        if (currentHealth <= 0)
        {
            Die();
        }

        FixHealthBugs();
        #endregion

        #region Damage
        damageTimer += Time.deltaTime;
        #endregion
    }


    #region Methods

    #region Movement
    private void MoveEnemy()
    {
        float moveMultiplier = 10 * moveSpeed;
        Vector3 playerPosition = player.transform.position;
        Vector3 playerPositionFromEnemy = playerPosition - transform.position;
        playerPositionFromEnemy.Normalize();
        Vector3 moveEnemy = playerPositionFromEnemy * moveMultiplier;

        rb.velocity = moveEnemy;
    }
    #endregion

    #region Health
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
    #endregion

    #endregion
}
