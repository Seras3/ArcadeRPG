using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Utils;


public class UIHandler : MonoBehaviour
{
    private TMP_Text ScoreTextMesh;
    private TMP_Text AmmoTextMesh;
    private GameObject currentWeapon;
    private Weapon currentWeaponStats;

    void Start()
    {
        ScoreTextMesh = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        AmmoTextMesh = GameObject.Find("AmmoText").GetComponent<TMP_Text>();
    }

    void Update() 
    {
        currentWeapon = GameObject.Find("Dummy").GetComponent<Stats.PlayerStats>().ActiveWeapon;
        currentWeaponStats = currentWeapon.GetComponent<Weapon>();

        ScoreTextMesh.text = GameManager.Score + "  SCORE";
        AmmoTextMesh.text = "x " + (currentWeaponStats.HasInfiniteAmmo ? "Inf" : currentWeaponStats.AmmoCount.ToString());
    }

}
