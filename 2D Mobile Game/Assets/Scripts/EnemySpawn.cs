using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject[] spawnPoints;
    [Min(0), SerializeField] private int numberOfEnemies = 1;
    [Min(0), SerializeField] private float spawnDelay = 1;

    //Game specific only - remove if unnecessary
    private EnemyCounter enemyCounter;
    //

    //Internal Variables
    private float timer;
    private int enemiesSpawned;

    private void Awake()
    {
        //Game specific only - remove if unnecessary
        enemyCounter = FindObjectOfType<EnemyCounter>();
        enemyCounter.enemiesToElim = numberOfEnemies;
        //
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnDelay && enemiesSpawned < numberOfEnemies)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int spawn = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawn].transform.position, Quaternion.identity);
        enemiesSpawned++;
        timer = 0;
    }
}
