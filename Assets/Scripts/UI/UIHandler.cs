using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils;


public class UIHandler : MonoBehaviour
{
    private TMP_Text ScoreTextMesh;
    private TMP_Text AmmoTextMesh;
    private TMP_Text LevelTextMesh;
    private TMP_Text CurrentWaveTextMesh;
    private TMP_Text MaxWavesTextMesh;
    private Image WeaponImage;
    private List<GameObject> DropTextMeshList;
    [SerializeField] private int DropListSize;

    private int DropListCycler;

    [SerializeField] private GameObject DropTextPrefab;
    private GameObject currentWeapon;
    private Weapon currentWeaponStats;
   
    

    void Start()
    {
        ScoreTextMesh = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        AmmoTextMesh = GameObject.Find("AmmoText").GetComponent<TMP_Text>();
        LevelTextMesh = GameObject.Find("LevelText").GetComponent<TMP_Text>();
        CurrentWaveTextMesh = GameObject.Find("CurrentWaveText").GetComponent<TMP_Text>();
        MaxWavesTextMesh = GameObject.Find("MaxWavesText").GetComponent<TMP_Text>();
        WeaponImage = GameObject.Find("WeaponImage").GetComponent<Image>();
        DropListCycler = 0;

        Transform parent = GameObject.Find("UI").transform;
        DropTextMeshList = new List<GameObject>();
        for (int i=0; i<=DropListSize; i++){
            DropTextMeshList.Add(Instantiate(DropTextPrefab, parent));
        }
    }

    void Update() 
    {
        currentWeapon = GameObject.Find("Dummy").GetComponent<Stats.PlayerStats>().ActiveWeapon;
        currentWeaponStats = currentWeapon.GetComponent<Weapon>();

        ScoreTextMesh.text = "SCORE:  " + GameManager.Score;
        AmmoTextMesh.text = "x " + (currentWeaponStats.HasInfiniteAmmo ? "Inf" : currentWeaponStats.AmmoCount.ToString());
        LevelTextMesh.text = "LEVEL  " + GameManager.getLevel().ToString();
        WeaponImage.sprite = currentWeaponStats.ImageSprite;

    }

    public void DisplayNewDrop(string text) 
    {
        DropTextMeshList[DropListCycler].GetComponent<TMP_Text>().text = text;
        DropTextMeshList[DropListCycler].GetComponent<Animator>().Play("Base Layer.DropTextAnimation");
        DropListCycler = (DropListCycler + 1) % DropListSize;
    }
    
    public void UpdateWaveInfo(string currentWave, string maxWaves)
    {
        CurrentWaveTextMesh.text = currentWave;
        MaxWavesTextMesh.text = maxWaves;
    }
}
