using System;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class GameManager : MonoBehaviour
    {
        private GameObject _inGameCanvas;
        private GameObject _gameOverScreen;
        private GameObject _gamePausedScreen;

        private GameObject _player;
        private HumanoidMovementPlayer _playerMovementScript;
        private PlayerShooting _playerShootingScript;
        public static GameStatus CurrentStatus { get; private set; }

        public enum GameStatus
        {
            Playing,
            Paused,
            GameOver,
        }

        private void Start()
        {
            CurrentStatus = GameStatus.Playing;

            _inGameCanvas = Resources.FindObjectsOfTypeAll<Canvas>().First(obj => obj.gameObject.transform.name == "InGameCanvas").gameObject;;
            _gameOverScreen = _inGameCanvas.transform.GetChild(1).gameObject;
            _gamePausedScreen = _inGameCanvas.transform.GetChild(2).gameObject;

            _player = GameObject.Find("Dummy");
            _playerMovementScript = _player.GetComponent<HumanoidMovementPlayer>();
            _playerShootingScript = _player.GetComponent<PlayerShooting>();
            
            _playerMovementScript.enabled = true;
            _playerShootingScript.enabled = true;
            Time.timeScale = 1;
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
            ShowGameOverScreen();
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
            _playerShootingScript.enabled = true;
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
            _playerShootingScript.enabled = false;
            // freeze bullets
            Time.timeScale = 0;
        }

        private void ShowGameOverScreen()
        {
            _gameOverScreen.SetActive(true);
            ActivateCanvas();
        }

        private void ShowGamePausedScreen()
        {
            _gamePausedScreen.SetActive(true);
            ActivateCanvas();
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
    }
}