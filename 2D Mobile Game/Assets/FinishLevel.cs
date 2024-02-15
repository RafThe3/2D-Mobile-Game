using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private Canvas winScreen;

    private EnemyCounter enemyCounter;

    private void Awake()
    {
        enemyCounter = FindObjectOfType<EnemyCounter>();
    }

    private void Start()
    {
        winScreen.enabled = false;
        Time.timeScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && enemyCounter.enemiesRemaining <= 0)
        {
            CompleteLevel();
        }
    }

    public void CompleteLevel()
    {
        winScreen.enabled = true;
        Time.timeScale = 0;
    }
}
