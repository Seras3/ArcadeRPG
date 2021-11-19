using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIHandler : MonoBehaviour
{
    private int score;
    private TMP_Text ScoreTextMesh;

    void Start()
    {
        ScoreTextMesh = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        score = 0;
    }

    public void AddScore(int value)
    {
        score += value;
        ScoreTextMesh.text = "SCORE: " + score;
    }
}
