using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    int level;
    private WaveController waveController;
    void Start()
    {
        waveController = gameObject.AddComponent<WaveController>();
        level = 0;
        StartNewLevel();
    }

    void Update()
    {
        if (waveController.levelFinished())
        {
            StartNewLevel();
        }

    }

    void StartNewLevel()
    {
        level++;
        Debug.Log("Level " + level.ToString());
        waveController.startLevel(level);
    }


    public void killEnemy()
    {
        waveController.killEnemy();
    }
}
