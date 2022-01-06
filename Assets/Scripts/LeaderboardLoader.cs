using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardLoader : MonoBehaviour
{
    private GameObject _gameManager;
    private TopScores _topScoresScript;

    private bool _isActive;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManagerObject");
        _topScoresScript = _gameManager.GetComponent<TopScores>();
        _isActive = false;
    }

    private void Update()
    {
        if (!_isActive && gameObject.activeInHierarchy)
        {
            _isActive = true;
            _topScoresScript.LoadLeaderboard();
        } 
        else if (!gameObject.activeInHierarchy)
        {
            _isActive = false;
        }
    }
}
