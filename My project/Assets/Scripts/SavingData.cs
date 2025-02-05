using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavingData : MonoBehaviour
{
    private string filePath ="";
    private LevelData levelData = new LevelData();
    void Awake()
    {
        if (filePath == "")
        {
            filePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        }

        if (!File.Exists(filePath))
        {
            List<int> stars = new List<int>(34);
            for (int i = 0; i < 34; i++)
            {
                stars.Add(-1);
            }
            levelData.stars = stars;
            SaveGameData();
        }
        else
        {
            LoadGameData();
        }
       
    }
    

    
    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(levelData, true);
        Debug.Log(json);
        File.WriteAllText(filePath, json);
        LoadGameData();
    }
    
    public void LoadGameData()
    {
        string json = File.ReadAllText(filePath);
        levelData.stars = JsonUtility.FromJson<List<int>>(json);
    }
    
   
}

[System.Serializable]

public class LevelData
{
    public List<int> stars;
}