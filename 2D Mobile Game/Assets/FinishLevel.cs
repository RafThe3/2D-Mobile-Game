using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    private GameManager gameManager;
    private EnemyCounter enemyCounter;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        enemyCounter = FindObjectOfType<EnemyCounter>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && enemyCounter.enemiesRemaining <= 0)
        {
            gameManager.LoadNextScene();
        }
    }
}
