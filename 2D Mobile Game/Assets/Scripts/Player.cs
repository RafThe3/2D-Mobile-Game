using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Variables

    #region Movement
    [Header("Movement")]
    public bool canMove = true;
    [Min(0), SerializeField] private float moveSpeed = 1;
    [Min(0), SerializeField] private float jumpForce = 1;
    #endregion

    #region Health
    [Header("Health")]
    [Min(0), SerializeField] private int maxHealth = 100;
    [Min(0), SerializeField] private int healAmount = 1;
    [Min(0), SerializeField] private int startingHealthPacks = 1;
    [Min(0), SerializeField] private float healDelay = 1;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthPacksText;
    [SerializeField] private AudioClip healSFX, healPickupSFX, hurtSFX;
    [SerializeField] private Canvas loseScreen;

    //Internal Variables
    private int healthPacks;
    private int currentHealth;
    private AudioSource audioSource;
    private float healTimer;
    #endregion

    #region Controls
    [Header("Controls")]
    [Tooltip("Allows the game to be played with keyboard if set to true.")] public bool allowKeyControls = true;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Canvas mobileControls;
    #endregion

    #region Other
    //private Animator animator;
    private Collider2D cldr;
    private Rigidbody2D rb;
    private bool allowGravity = false;
    #endregion

    #endregion

    private void Awake()
    {
        #region Other
        rb = GetComponent<Rigidbody2D>();
        cldr = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        #endregion
    }

    private void Start()
    {
        #region Movement and Controls
        allowGravity = rb.gravityScale > 0;
        mobileControls.enabled = !allowKeyControls;
        joystick.AxisOptions = allowGravity ? AxisOptions.Horizontal : AxisOptions.Both;
        #endregion

        #region Health
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = healthBar.maxValue;
        healthPacksText.text = $"Health Packs: {healthPacks}";
        healthPacks = startingHealthPacks;
        healTimer = healDelay;
        audioSource = Camera.main.GetComponent<AudioSource>();
        //animator = GetComponent<Animator>();
        //Game specific only - remove if unnecessary
        //loseScreen.enabled = false;
        Time.timeScale = 1;
        //
        #endregion
    }

    private void Update()
    {
        #region Movement and Controls

        if (canMove)
        {
            //Sets the move variable depending on the controls
            Vector2 move = DetermineControls();
            MovePlayer(move);
        }

        if (Input.GetKeyDown(KeyCode.Space) && allowGravity)
        {
            Jump();
        }
        #endregion

        #region Health
        healTimer += Time.deltaTime;

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
        #endregion
    }
    
    #region Methods

    #region Movement and Controls

    private void MovePlayer(Vector2 move)
    {
        float moveMultiplier = moveSpeed * 10;
        Vector3 movePlayer = new(x: move.x * moveMultiplier, y: !allowGravity ? move.y * moveMultiplier : rb.velocity.y);

        rb.velocity = movePlayer;
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

    #endregion

    #region Health

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
        audioSource.PlayOneShot(healPickupSFX);
    }

    #endregion

    #region Other

    private void UpdateUI()
    {
        healthPacksText.text = $"Health Packs: {healthPacks}";
        healthPacksText.color = healthPacks <= 0 ? Color.red : Color.white;
        healthBar.value = currentHealth;
        Image healthBarFillArea = GameObject.Find("Fill").GetComponent<Image>();
        healthBarFillArea.color = currentHealth <= 25 ? Color.red : Color.green;
    }

    #endregion

    #endregion
}
