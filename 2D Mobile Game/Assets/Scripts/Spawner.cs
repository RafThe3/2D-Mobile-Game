using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs, spawnPoints;
    [Min(0), SerializeField] private int numberOfObjects = 1;
    [Min(0), SerializeField] private float spawnInterval = 1;
    [Min(0), SerializeField] private bool endlessSpawn = false;

    //Game specific only - remove if unnecessary
    //private EnemyCounter enemyCounter;
    //

    //Internal Variables
    private float spawnTimer;
    private int objectsSpawned;
    //

    private void Awake()
    {
        //Game specific only - remove if unnecessary
        //enemyCounter = FindObjectOfType<EnemyCounter>();
        //enemyCounter.enemiesToElim = numberOfEnemies;
        //
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        bool isReadyToSpawn = (spawnTimer >= spawnInterval && objectsSpawned < numberOfObjects && !endlessSpawn)
                              || (spawnTimer >= spawnInterval && endlessSpawn);
        if (isReadyToSpawn)
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        int spawn = Random.Range(0, spawnPoints.Length), obj = Random.Range(0, prefabs.Length);
        Instantiate(prefabs[obj], spawnPoints[spawn].transform.position, Quaternion.identity);
        objectsSpawned++;
        spawnTimer = 0;
    }
}
