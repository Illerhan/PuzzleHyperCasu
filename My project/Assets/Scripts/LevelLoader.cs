using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelLoader : MonoBehaviour
{

    public LevellSO currentLevel;
    public Tilemap tilemap;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadLevel();
    }


    void LoadLevel()
    {
        if (currentLevel == null || tilemap == null)
            return;
        foreach (var obj in currentLevel.objectsToSpawn)
        {
            if (obj.prefab != null)
            {
                Vector3 position = tilemap.GetCellCenterWorld(obj.tilePosition);
                Instantiate(obj.prefab, position, Quaternion.identity);
            }
        }
        foreach (var slime in currentLevel.slimeToSpawn)
        {
            if (slime.slimePrefab != null)
            {
                Instantiate(slime.slimePrefab, slime.slimePosition, quaternion.identity);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
