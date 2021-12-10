using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Utils;


public class UIHandler : MonoBehaviour
{
    private TMP_Text ScoreTextMesh;

    void Start()
    {
        ScoreTextMesh = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
    }

    void Update() 
    {
        ScoreTextMesh.text = "SCORE: " + GameManager.Score;
    }

}
