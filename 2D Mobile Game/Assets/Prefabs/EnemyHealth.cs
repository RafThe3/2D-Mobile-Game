using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth = 0;
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    private Vector3 textScale;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthText.enabled = false;
        textScale = healthText.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayEnemyHealth();
        Die();
    }

    private void Die()
    {
        if (currentHealth <= 0)
        {
            GetComponent<EnemyMovement>().canMove = false;
            GetComponent<Animator>().SetTrigger("Die");
            healthText.enabled = false;
            Invoke(nameof(DestroyEnemy), 1f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void DisplayEnemyHealth()
    {
        Vector3 enemyFacing = gameObject.transform.localScale.x < 0 ? new(-textScale.x, textScale.y, textScale.z) : textScale;
        healthText.transform.localScale = enemyFacing;
        string enemyHealthText = $"Health: {currentHealth}";

        if (currentHealth < 100)
        {
            healthText.enabled = true;
        }

        healthText.text = enemyHealthText;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }
}
