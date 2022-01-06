using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class MainMenuLogic : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }
    
    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGameFromPauseState()
    {
        GameObject.Find("GameManagerObject")?.GetComponent<GameManager>().ResumeGame();
    }

    public void LoadLeaderboardScreen()
    {
        GameObject.Find("InGameCanvas").transform.GetChild(3).gameObject.SetActive(true);
        GameObject.Find("Leaderboard").GetComponent<LeaderboardLoader>().DisplayInput(false);
        GameObject.Find("GameManagerObject").GetComponent<TopScores>().LoadLeaderboard();
        
        GameObject.Find("GameOver").gameObject.SetActive(false);
    }
}
