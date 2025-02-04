 using System.Collections.Generic;
 using System.Linq;
 using System.Runtime.InteropServices.WindowsRuntime;
 using TMPro;
 using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class LevelLoader : MonoBehaviour
{

    public LevelContainer levelContainer;
    public LevellSO currentLevel;
    public Tilemap tilemap;
    public TextMeshProUGUI level;
    public List<TextMeshProUGUI> textstarsMoves;
    public ConvoyeurFood convoyeurfood;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        LoadLevel();
        level.text = levelContainer.selectedLevel.name.Remove(0, 3);
        for (int i = 0; i < textstarsMoves.Count; i++)
        {
            textstarsMoves[i].text = levelContainer.selectedLevel.nbMovesToGainStars[i].Moves.ToString();
        }
    }


    public void LoadLevel()
    {
        foreach (Transform child in tilemap.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
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
                GameObject slimeNew = Instantiate(slime.slimePrefab, slime.slimePosition, Quaternion.identity,tilemap.transform);
                slimeNew.GetComponent<SlimeController>().currentState = slime.defaultState;
            }
        }

        convoyeurfood.foodList = currentLevel.foodToSpawn.foodOrder;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
