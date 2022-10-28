using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Results : MonoBehaviour
{

    GameManager manager;

    public TextMeshProUGUI score;
    public TextMeshProUGUI time;
    public TextMeshProUGUI pushedobjects;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        score.text = manager.score + " points";
        time.text = manager.time + " seconds";
        pushedobjects.text = manager.numofpushedobjects + " objects";
        SaveToJson();
    }

    //Method to save results to JSON. It appears on Assets afterwards.
    public void SaveToJson()
    {
        GameResults results = new GameResults(manager.score, manager.time, manager.numofpushedobjects);
        string json = JsonUtility.ToJson(results);
        File.WriteAllText(Application.dataPath + "/SaveFile.json", json);
    }

    private class GameResults
    {
        public float score;
        public float time;
        public float pushedobjects;

        public GameResults(float sco, float tim, float push)
        {
            this.score = sco;
            this.time = tim;
            this.pushedobjects = push;
        }
    }
}
