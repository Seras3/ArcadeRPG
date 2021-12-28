using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class TopScores : MonoBehaviour
{
    private string path;

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
        path = Application.dataPath + "/TopScores.json";
        JsonInput();
    }
}
