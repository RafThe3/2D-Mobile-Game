using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int difficulty;
    [SerializeField] private TMPro.TextMeshProUGUI difficultyText;

    private void Start()
    {
        difficulty = 1;
    }

    private void Update()
    {
        UpdateDifficultyText();
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    private void UpdateDifficultyText()
    {
        difficultyText.text = difficulty == 1 ? "Easy"
                    : difficulty == 2 ? "Normal"
                    : "Hard";
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
    public void PlayGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetInt("Dash", 2300);
        PlayerPrefs.SetInt("Reload", 2000);
        PlayerPrefs.SetInt("Shoot", 3000);
        PlayerPrefs.SetInt("Run", 2500);
        PlayerPrefs.SetFloat("ReloadSpeed", 0.8f);
    }
    public void ChangeDifficulty()
    {
        difficulty += 1;
        if(difficulty > 3)
        {
            difficulty = 1;
        }
        Debug.Log(PlayerPrefs.GetInt("Difficulty"));
    }
}
