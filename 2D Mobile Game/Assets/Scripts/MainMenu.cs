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
    }

    private void UpdateDifficultyText()
    {
        difficultyText.text = difficulty == 1 ? "Change Difficulty - Easy"
                    : difficulty == 2 ? "Change Difficulty - Normal"
                    : "Change Difficulty - Hard";
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
    }
    public void ChangeDifficulty()
    {
        difficulty += 1;
        if(difficulty > 3)
        {
            difficulty = 1;
        }
        PlayerPrefs.SetInt("Difficulty", difficulty);
        Debug.Log(PlayerPrefs.GetInt("Difficulty"));
    }
}
