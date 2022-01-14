using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class TopScores : MonoBehaviour
{
    private string path;
    public GameObject Row;

    public float PositionOffset;

    private GameObject lead;

        [System.Serializable]
    private class Record : IComparable<Record>
    {
        public Record(string name, int score){
            this.name = name;
            this.score = score;
        }
        public string name;
        public int score;
        public int CompareTo(Record that)
        {
            return (-this.score).CompareTo(-that.score); // minus signs for reversing the sort order from ascending to descending
        }
    }

    public void LoadLeaderboard()
    {
        JsonInput();
        var container = GameObject.Find("LeaderboardContainer");
        foreach(Transform child in container.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        var id = 1;
        foreach (var row in recordList.Records)
        {
            var leadRow = Instantiate(Row, container.transform);
            leadRow.transform.GetChild(0).GetComponent<TMP_Text>().text = id.ToString();
            leadRow.transform.GetChild(1).GetComponent<TMP_Text>().text = row.name;
            leadRow.transform.GetChild(2).GetComponent<TMP_Text>().text = row.score.ToString();
            var position = leadRow.transform.position;
            position = new Vector3(position.x, -id * PositionOffset + position.y, 0);
            leadRow.transform.position = position;
            id++;
        }
    }

    public int GetLastScore()
    {
        return recordList.Records[recordList.Records.Count - 1].score;
    }


    private class RecordList{
        public List<Record> Records;
    }

    private RecordList recordList;
    public void ProcessNewScore(string name, int score){
        recordList.Records.Add(new Record(name, score));
        recordList.Records.Sort();
        recordList.Records.RemoveAt(recordList.Records.Count-1);

        JsonOutput();
    }

    private void JsonInput(){
        recordList = JsonUtility.FromJson<RecordList> ( File.ReadAllText(path) );
    }

    private void JsonOutput(){
        File.WriteAllText(path, JsonUtility.ToJson(recordList));
    }

    private void Start() {
        path = Application.persistentDataPath + "/TopScores.json";
        Debug.Log("path: " + path.ToString());
        JsonInput();
        // lead = GameObject.Find("LeaderboardContainer");
        // Debug.Log("LEAD: " + lead.transform.name);
    }
}
