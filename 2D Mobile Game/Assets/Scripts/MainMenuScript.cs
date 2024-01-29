using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public int difficulty;
    public void ExitGame()
    {
        Debug.Log("Quitting Game");
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
