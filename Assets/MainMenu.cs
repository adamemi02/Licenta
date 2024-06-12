using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("Playing_Options");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has Quit the Game");
        Debug.Break();
    }

    public void Resume()
    {
        PlayerAgent.isPaused=!PlayerAgent.isPaused;
        PlayerAgent.count = 0;
        Time.timeScale = 1f; // Resumes the game
        SceneManager.UnloadSceneAsync("PauseScene");
    }
    
    

}
