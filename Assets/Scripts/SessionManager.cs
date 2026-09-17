using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;
    public string playerName = "";
    public int score = 0;
    public List<HighScore> highScores = new List<HighScore>(5);

    // private string fileSaveLocation = "C:/Users/Davide/AppData/LocalLow/DefaultCompany/SimpleBreakout";

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadHighScores();

        while (highScores.Count < 5)
        {
            highScores.Add(new HighScore());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    public class HighScore
    {
        public string playerName;
        public int score;

        public HighScore(string playerName = "", int score = 0){}
    }

    public void SaveHighScore()
    {
        string[] json = new string[highScores.Count];

        for (int i = 0; i < highScores.Count; i++)
        {
            json[i] = JsonUtility.ToJson(highScores[i]);

            if (i == 0)
            {
                File.WriteAllText(Application.persistentDataPath + "/highscores.json", json[i]);
            } else
            {
                File.AppendAllText(Application.persistentDataPath + "/highscores.json", Environment.NewLine + json[i]);
            }
        }
    }

    public void LoadHighScores()
    {
        string path = Application.persistentDataPath + "/highscores.json";

        if (File.Exists(path))
        {
            string[] json = File.ReadAllLines(path);
            int i = 0;

            foreach (string line in json)
            {
                highScores.Add(JsonUtility.FromJson<HighScore>(line));
                i++;
            }
        }
    }
}
