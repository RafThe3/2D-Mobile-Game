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
    
    [Header("Health")]
    [Min(0), SerializeField] private int maxHealth = 100;
    [Min(0), SerializeField] private int healAmount = 1;
    [Min(0), SerializeField] private int startingHealthPacks = 1;
    [Min(0), SerializeField] private float healDelay = 1;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthPacksText;
    [SerializeField] private AudioClip healSFX, hurtSFX;
    [SerializeField] private Canvas loseScreen;

    [Header("Controls")]
    [Tooltip("Allows the game to be played with keyboard if set to true.")] public bool allowKeyControls = true;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Canvas mobileControls;

    //Internal Variables

    //Movement
    private bool isDashing = false;

    //Health
    private int healthPacks;
    private int currentHealth;
    private AudioSource audioSource;
    private float healTimer;
    //

    //Other
    //private Animator animator;
    private Collider2D cldr;
    private Rigidbody2D rb;
    private bool allowGravity = false;
    //

    //

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cldr = GetComponent<Collider2D>();
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    private void Start()
    {
        //Controls
        allowGravity = rb.gravityScale > 0;
        mobileControls.enabled = !allowKeyControls;
        joystick.AxisOptions = allowGravity ? AxisOptions.Horizontal : AxisOptions.Both;
        //

        //Health
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = healthBar.maxValue;
        healthPacksText.text = $"Health Packs: {healthPacks}";
        healthPacks = startingHealthPacks;
        healTimer = healDelay;
        //

        //Other
        audioSource = Camera.main.GetComponent<AudioSource>();
        //animator = GetComponent<Animator>();
        //loseScreen.enabled = false;
        Time.timeScale = 1;
        //
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        healTimer += Time.deltaTime;

        if (canMove)
        {
            //Sets the move variable depending on the controls
            Vector2 move = DetermineControls();
            MovePlayer(move);

            if (Input.GetKeyDown(KeyCode.Space) && allowGravity)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.E)) /*&& healTimer >= healDelay && healthPacks > 0*/
        {
            Heal(healAmount);
        }

        //Test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(10);
        }
        //

        FixHealthBugs();
        UpdateUI();
        
    }

    //Movement and Controls
    private void MovePlayer(Vector2 move)
    {
        float moveMultiplier = moveSpeed * 10;
        Vector3 movePlayer = new(x: move.x, y: !allowGravity ? move.y : rb.velocity.y);

        rb.velocity = moveMultiplier * movePlayer;
        FlipPlayer();
    }

    private void FlipPlayer()
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

    public void ButtonDash()
    {
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        Vector2 move = DetermineControls();
        canDash = false;
        isDashing = true;
        float dashMultiplier = dashForce * 10;
        rb.velocity = new Vector2(move.x * dashMultiplier, move.y * dashMultiplier);
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
    //

    //Health
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

    public void Heal(int health)
    {
        if (currentHealth < maxHealth && healthPacks > 0) /*&& healTimer >= healDelay*/
        {
            currentHealth += health;
            healthPacks--;
            audioSource.PlayOneShot(healSFX);
        }

        healTimer = 0;
    }

    private void Die()
    {
        canMove = false;
        //Game specific only - remove if unnecessary
        loseScreen.enabled = true;
        Time.timeScale = 0;
        //
    }

    public void AddHealthPack()
    {
        healthPacks++;
    }
    //

    //Other
    private void UpdateUI()
    {
        healthPacksText.text = $"Health Packs: {healthPacks}";
        healthPacksText.color = healthPacks > 0 ? Color.white : Color.red;
        healthBar.value = currentHealth;
        Image healthBarFillArea = GameObject.Find("Fill").GetComponent<Image>();
        healthBarFillArea.color = currentHealth > 25 ? Color.green : Color.red;
    }
    //
}
