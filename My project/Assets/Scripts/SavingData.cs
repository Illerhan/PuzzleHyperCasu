
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavingData : MonoBehaviour
{
    private string filePath ="";
    public LevelData levelData = new LevelData();
    void Awake()
    {
        if (filePath == "")
        {
            filePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        }

        if (!File.Exists(filePath))
        {
            int[] stars = {};
            for (int i = 0; i < 34; i++)
            {
                stars[i]=-1;
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
        Debug.Log(levelData.stars[0]);
        File.WriteAllText(filePath, json);
        //LoadGameData();
    }
    
    public void LoadGameData()
    {
        string json=""; 
        foreach (string lvl in File.ReadAllLines(filePath))
        {
            json+=lvl;
        }
        Debug.Log(JsonUtility.FromJson<List<int>>(json).Count);
        levelData = JsonUtility.FromJson<LevelData>(json);
    }
    
    
   
}

[System.Serializable]

public class LevelData
{
    public int[] stars;
}