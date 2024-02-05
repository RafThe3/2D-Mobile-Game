using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas pauseMenu;
    private bool allowKeyControls = true;

    private void Start()
    {
        allowKeyControls = FindObjectOfType<Player>().AllowsKeyControls();
        pauseMenu.enabled = false;
    }

    private void Update()
    {
        if (allowKeyControls && Input.GetKeyDown(KeyCode.Escape))
        {
            EnablePauseMenu();
        }
    }

    public void EnablePauseMenu()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseMenu.enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.enabled = false;
        }
    }

    public void Resume()
    {
        pauseMenu.enabled = false;
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void LoadAScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
