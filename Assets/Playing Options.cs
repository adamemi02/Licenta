using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameMode
{
    MULTIPLAYER = 0,
    EASY,
    MEDIUM,
    HARD
}



public class PlayingOptions : MonoBehaviour
{

    
    public static GameMode s_gameMode;

    public void MultiPlayer()
    {

        s_gameMode = GameMode.MULTIPLAYER;
        SceneManager.LoadScene("SampleScene");


    }

    public void Easy()
    {
        s_gameMode = GameMode.EASY;
        SceneManager.LoadScene("SampleScene");
    }

    public void Medium()
    {
        s_gameMode = GameMode.MEDIUM;
        SceneManager.LoadScene("SampleScene");
    }

    public void Hard()
    {
        s_gameMode = GameMode.HARD;
        SceneManager.LoadScene("SampleScene");
    }

   
}
