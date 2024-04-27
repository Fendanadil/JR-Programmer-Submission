using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;
    public string playerName;
    public List<string> highscoreName;
    public List<int> highscore;
    private string playerConfigFile;
    private string highscoreFile;
    private int highscoreLength = 10;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerConfigFile = Application.persistentDataPath + "/save_breakout.json";
        highscoreFile = Application.persistentDataPath + "/save_breakout_highscores.json";
        LoadConfigData();
        LoadHighscoreData();
    }

    public void SaveConfigData() {

        PlayerConfigData data = new PlayerConfigData();
        data.playerName = playerName;

        string json = JsonUtility.ToJson(data);
        
        File.WriteAllText(playerConfigFile, json);
    }

    public void SaveHighscoreData()
    {
        HighscoreData data = new HighscoreData();
        data.highscoreName = highscoreName;
        data.highscore = highscore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(highscoreFile, json);
    }

    public void LoadConfigData()
    {
        if (File.Exists(playerConfigFile))
        {
            string json = File.ReadAllText(playerConfigFile);
            PlayerConfigData data = JsonUtility.FromJson<PlayerConfigData>(json);

            playerName = data.playerName;
        }
    }

    public void LoadHighscoreData()
    {
        if (File.Exists(highscoreFile))
        {
            string json = File.ReadAllText(highscoreFile);
            HighscoreData data = JsonUtility.FromJson<HighscoreData>(json);

            highscoreName = data.highscoreName;
            highscore = data.highscore;
        }
    }

    public int StoreHighScore(int score) {

        int highscorePosition = 0; 

        // check if we have a highscore
        for (int i = 0; i < highscore.Count; i++) 
        {
            if (score > highscore[i])
            {
                highscorePosition = i;
                break;
            }
            else {
                highscorePosition = i + 1; 
            }
        }

        // add the highscore if we have found one
        if (highscorePosition < highscore.Count)
        {
            highscore.Insert(highscorePosition, score);
            highscoreName.Insert(highscorePosition, playerName);
        }
        else {
            highscorePosition = -1;
        }

        // make sure the list isn't longer than 10 entries now
        while (highscore.Count > highscoreLength) 
        {
            highscore.RemoveAt(highscoreLength);
            highscoreName.RemoveAt(highscoreLength);
        }

        return highscorePosition;
    }

    private void AddHighScore(int position, int score) { 
    
    }

    // returns the highscore (name and score)
    public ArrayList GetHighScore() { 
        
        ArrayList returnValues = new ArrayList();

        if (highscore.Count > 0) {
            returnValues.Add(highscoreName[0]);
            returnValues.Add(highscore[0]);
        }

        return returnValues;
    }


    [System.Serializable]
    class PlayerConfigData
    {
        public string playerName;
    }
    class HighscoreData
    {
        public List<string> highscoreName;
        public List<int> highscore;
    }

}
