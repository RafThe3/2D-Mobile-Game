using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCounter : MonoBehaviour
{
    [HideInInspector] public int enemiesToElim, enemiesRemaining;
    [SerializeField] private Canvas winScreen;
    [SerializeField] private TMPro.TextMeshProUGUI enemyCounterText;

    // Start is called before the first frame update
    private void Start()
    {
        enemiesRemaining = enemiesToElim;
        winScreen.enabled = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(enemiesRemaining);

        if (enemiesRemaining <= 0)
        {
            winScreen.enabled = true;
            Time.timeScale = 0;
        }
    }
}
