using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canDash = true;
    [Min(0), SerializeField] private float moveSpeed = 1;
    [Min(0), SerializeField] private float jumpForce = 1;
    [Min(0), SerializeField] private float dashForce = 1;
    [Min(0), SerializeField] private float dashCooldown = 1;
    [Min(0), SerializeField] private float dashTime = 0.1f;
    [SerializeField] private AudioClip dashSFX;
    [SerializeField] private Slider dashBar;
    
    [Header("Health")]
    [Min(0), SerializeField] private int maxHealth = 100;
    [Min(0), SerializeField] private int healAmount = 1;
    [Min(0), SerializeField] private int startingHealthPacks = 1;
    [Min(0), SerializeField] private int maxHealthPacks = 10;
    [Min(0), SerializeField] private float healDelay = 1;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthPacksText;
    [SerializeField] private AudioClip healSFX, hurtSFX;
    [SerializeField] private Canvas loseScreen;

    [Header("Attacking")]
    [SerializeField] private bool canAttack = true;
    [SerializeField] private float attackDistance = 1;
    [SerializeField] private float attackDelay = 1;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private AudioClip attackSFX;

    [Header("Controls")]
    [Tooltip("Allows the game to be played with keyboard if set to true."), SerializeField] private bool allowKeyControls = true;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Canvas mobileControls;

    // Internal Variables

    // Movement
    private bool isDashing = false, isMoving = false;

    // Health
    private int healthPacks = 0;
    private int currentHealth = 0;
    private AudioSource audioSource;
    private bool isHealing;

    // Attacking
    private float attackTimer;

    // Other
    private bool allowGravity = false;
    private Collider2D cldr;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector3 dashBarScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cldr = GetComponent<Collider2D>();
        audioSource = Camera.main.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Movement and Controls
        allowGravity = rb.gravityScale > 0;
        mobileControls.enabled = !allowKeyControls;
        joystick.AxisOptions = allowGravity ? AxisOptions.Horizontal : AxisOptions.Both;
        dashBar.maxValue = dashCooldown;
        dashBar.value = dashBar.maxValue;
        dashBarScale = dashBar.transform.localScale;

        // Health
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = healthBar.maxValue;
        healthPacksText.text = $"Health Packs: {healthPacks}";
        healthPacks = startingHealthPacks;
        if (maxHealthPacks == 0)
        {
            maxHealthPacks = startingHealthPacks;
        }

        // Other
        // Game specific - remove if unnecessary
        loseScreen.enabled = false;
        Time.timeScale = 1;
    }

    private void FixedUpdate()
    {
        if (canMove && !isDashing)
        { 
            // Sets the move variable depending on the controls
            Vector2 move = DetermineControls();
            MovePlayer(move);

            if (Input.GetButtonDown("Jump") && allowGravity)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && isMoving)
            {
                StartCoroutine(Dash());
            }
        }

        if (!canMove)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon || Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        FixHealthBugs();
        UpdateUI();

        if (isDashing)
        {
            return;
        }

        if (dashBar.value < dashBar.maxValue && !isDashing)
        {
            dashBar.value += Time.deltaTime;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.E) && !isHealing)
        {
            StartCoroutine(Heal(healAmount, healDelay));
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Attack();
        }

        // Test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(10);
        }
    }

    // Movement and Controls
    private void MovePlayer(Vector2 move)
    {
        float moveMultiplier = moveSpeed * 100;
        Vector3 movePlayer = new(x: move.x, y: !allowGravity ? move.y : rb.velocity.y);

        rb.velocity = moveMultiplier * Time.fixedDeltaTime * movePlayer;
        animator.SetBool("isMoving", isMoving);
        FlipSprite();
    }

    private void FlipSprite()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (isMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1);
        }
    }

    private Vector2 DetermineControls()
    {
        Vector2 move = Vector2.zero;

        if (allowKeyControls)
        {
            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");
        }
        else
        {
            move.x = joystick.Horizontal;
            move.y = joystick.Vertical;
        }

        return move;
    }

    public void Jump()
    {
        if (cldr.IsTouchingLayers(LayerMask.GetMask("Ground")) && allowGravity)
        {
            Vector2 playerJump = new(0, jumpForce * 100);
            rb.AddForce(playerJump);
        }
    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        Vector2 move = DetermineControls();
        float dashMultiplier = dashForce * 10;
        Vector2 dash = dashMultiplier * new Vector2(move.x, move.y);
        rb.velocity = dash;
        animator.SetTrigger("Dash");
        audioSource.PlayOneShot(dashSFX);
        dashBar.value = 0;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public IEnumerator SpeedBoost(float speedMultiplier, float duration)
    {
        float tempMoveSpeed = moveSpeed;
        moveSpeed *= speedMultiplier;
        yield return new WaitForSeconds(duration);
        moveSpeed = tempMoveSpeed;
    }

    // Mobile Controls - Movement and Controls
    public void ButtonDash()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon || Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        if (!isDashing && isMoving)
        {
            StartCoroutine(Dash());
        }
    }

    // Health
    private void FixHealthBugs()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int health)
    {
        if (currentHealth > 0)
        {
            currentHealth -= health;
            audioSource.PlayOneShot(hurtSFX);
        }
    }

    public IEnumerator Heal(int health, float healInterval)
    {
        isHealing = true;

        if (currentHealth < maxHealth && healthPacks > 0)
        {
            currentHealth += health;
            healthPacks--;
            audioSource.PlayOneShot(healSFX);
        }

        yield return new WaitForSeconds(healInterval);
        isHealing = false;
    }

    private void Die()
    {
        canMove = false;
        //Game specific only - remove if unnecessary
        loseScreen.enabled = true;
        Time.timeScale = 0;
    }

    public void AddHealthPack()
    {
        if (healthPacks < maxHealthPacks)
        {
            healthPacks++;
        }
    }

    // Mobile Controls - Health
    public void ButtonHeal()
    {
        if (!isHealing)
        {
            StartCoroutine(Heal(healAmount, healDelay));
        }
    }

    // Attack
    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackDistance, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damageAmount);
            audioSource.PlayOneShot(attackSFX);
        }

        animator.SetTrigger("Attack");
        attackTimer = 0;
    }

    // Other
    private void UpdateUI()
    {
        healthPacksText.text = $"Health Packs: {healthPacks}";
        healthPacksText.color = healthPacks > 0 ? Color.white : Color.red;
        Vector3 originalScale = gameObject.transform.localScale.x < 0 ? new(-dashBarScale.x, dashBarScale.y, dashBarScale.z)
                                : dashBarScale;
        dashBar.transform.localScale = originalScale;
        dashBar.gameObject.SetActive(!canDash);
        healthBar.value = currentHealth;
        Image healthBarFillArea = GameObject.Find("Fill").GetComponent<Image>();
        healthBarFillArea.color = currentHealth > 25 ? Color.green : Color.red;
    }

    public bool AllowsKeyControls()
    {
        return allowKeyControls;
    }
}
