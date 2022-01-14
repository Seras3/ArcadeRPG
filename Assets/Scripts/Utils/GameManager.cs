using System;
using System.Linq;
using Controllers;
using UnityEngine;

namespace Utils
{
    public class GameManager : MonoBehaviour
    {
        private GameObject _inGameCanvas;
        private GameObject _gameOverScreen;
        private GameObject _gamePausedScreen;
        private GameObject _gameLeaderboardScreen;

        private UIHandler _uiHandler;
        private GameObject _player;
        private HumanoidMovementPlayer _playerMovementScript;
        private PlayerController _playerControllerScript;
        private static LevelController _levelController;
        private TopScores _topScoresScript;
        public static GameStatus CurrentStatus { get; private set; }
        public static int Score { get; private set; }
        

        public enum GameStatus
        {
            Playing,
            Paused,
            GameOver,
        }

        private void Start()
        {
            CurrentStatus = GameStatus.Playing;

            _inGameCanvas = GameObject.Find("Canvases").transform.GetChild(1).gameObject;
            _gameOverScreen = _inGameCanvas.transform.GetChild(1).gameObject;
            _gamePausedScreen = _inGameCanvas.transform.GetChild(2).gameObject;
            _gameLeaderboardScreen = _inGameCanvas.transform.GetChild(3).gameObject;

            _uiHandler = GameObject.Find("UIModifier").GetComponent<UIHandler>();
            _player = GameObject.Find("Dummy");
            _playerMovementScript = _player.GetComponent<HumanoidMovementPlayer>();
            _playerControllerScript = _player.GetComponent<PlayerController>();
            _levelController = GameObject.Find("ObjectSpawner").GetComponent<LevelController>();
            _topScoresScript = GetComponent<TopScores>();

            _playerMovementScript.enabled = true;
            _playerControllerScript.enabled = true;
            Time.timeScale = 1;
            Score = 0;
            DeactivateCanvas();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResumeGame();
            }
        }

        public void GameOver()
        {
            CurrentStatus = GameStatus.GameOver;
            FreezeEverything();
            if (Score <= _topScoresScript.GetLastScore())
            {
                ShowGameOverScreen();
            }
            else
            {
                ShowLeaderboardScreen();
            }
        }

        public void PauseGame()
        {
            if (CurrentStatus is GameStatus.Paused || CurrentStatus is GameStatus.GameOver) return;
            CurrentStatus = GameStatus.Paused;

            FreezeEverything();
            ShowGamePausedScreen();
        }

        public void ResumeGame()
        {
            if (CurrentStatus != GameStatus.Paused) return;
            
            // free hero
            _playerMovementScript.enabled = true;
            _playerControllerScript.enabled = true;
            // free bullets
            Time.timeScale = 1;
            // remove game paused canvas
            DeactivateCanvas();
            
            CurrentStatus = GameStatus.Playing;
        }

        private void FreezeEverything()
        {
            // freeze hero
            _playerMovementScript.enabled = false;
            _playerControllerScript.enabled = false;
            // freeze bullets
            Time.timeScale = 0;
        }

        private void ShowGameOverScreen()
        {
            ActivateCanvas();
            _gameOverScreen.SetActive(true);
        }
        
        private void ShowLeaderboardScreen()
        {
            ActivateCanvas();
            _gameLeaderboardScreen.SetActive(true);
            GameObject.Find("Leaderboard").GetComponent<LeaderboardLoader>().DisplayInput(true);
        }

        private void ShowGamePausedScreen()
        {
            ActivateCanvas();
            _gamePausedScreen.SetActive(true);
        }

        private void ActivateCanvas()
        {
            _inGameCanvas.SetActive(true);
            _inGameCanvas.GetComponent<CanvasFader>().FadeIn();
        }
        
        private void DeactivateCanvas()
        {
            _inGameCanvas.SetActive(false);
            _inGameCanvas.GetComponent<CanvasGroup>().alpha = 0;
            _gamePausedScreen.SetActive(false);
            _gameOverScreen.SetActive(false);
        }

        public static void AddScore(int value) 
        {  
            Score += value;
        }

        public static int getLevel()
        {
            return _levelController.level;
        }
    }
}