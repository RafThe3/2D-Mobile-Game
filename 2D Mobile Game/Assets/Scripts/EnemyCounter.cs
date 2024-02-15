using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCounter : MonoBehaviour
{
    [HideInInspector] public int enemiesToElim, enemiesRemaining;

    // Start is called before the first frame update
    private void Start()
    {
        enemiesRemaining = enemiesToElim;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(enemiesRemaining);
    }
}
