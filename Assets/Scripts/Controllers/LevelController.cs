using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int level;
    private WaveController waveController;
    private bool isLevelStarting;

    void Start()
    {
        waveController = gameObject.AddComponent<WaveController>();
        level = 0;
        isLevelStarting = false;
        StartNewLevel();
    }

    void Update()
    {
        if (waveController.levelFinished() && !isLevelStarting)
        {
            isLevelStarting = true;
            StartCoroutine(StartNewLevelWithDelay(5));
        }
    }

    void StartNewLevel()
    {
        level++;
        Debug.Log("Level" + level.ToString());
        waveController.startLevel(level);
        isLevelStarting = false;
    }

    IEnumerator StartNewLevelWithDelay(int delay)
    {
        Debug.Log("Sleeping started");
        yield return new WaitForSeconds(delay);
        Debug.Log("Sleeping stopped");
        StartNewLevel();
    }


    public void killEnemy()
    {
        waveController.killEnemy();
    }
}
