 using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class LevelLoader : MonoBehaviour
{

    public LevelContainer levelContainer;
    public LevellSO currentLevel;
    public Tilemap tilemap;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadLevel();
    }


    void LoadLevel()
    {
        currentLevel = levelContainer.selectedLevel;
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
                GameObject slimeNew = Instantiate(slime.slimePrefab, slime.slimePosition, Quaternion.identity);
                slimeNew.GetComponent<SlimeController>().currentState = slime.defaultState;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
