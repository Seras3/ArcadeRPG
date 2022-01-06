using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardLoader : MonoBehaviour
{
    private GameObject _gameManager;
    private TopScores _topScoresScript;

    private bool _shouldLoadLead;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManagerObject");
        _topScoresScript = _gameManager.GetComponent<TopScores>();
        _shouldLoadLead = false;
    }

    private void Update()
    {
        if (!_shouldLoadLead)
        {
            _shouldLoadLead = true;
            _topScoresScript.LoadLeaderboard();
        }
    }

    public void DisplayInput(bool should)
    {
        var backButton = transform.GetChild(0);
        var inputArea = transform.GetChild(3);
        backButton.gameObject.SetActive(!should);
        inputArea.gameObject.SetActive(should);
    }
}
