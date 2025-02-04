using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "LevellSO", menuName = "Level/LevellSO")]
public class LevellSO : ScriptableObject
{
    [System.Serializable]
    public class LevelObject
    {
        public GameObject prefab;
        public Vector3Int tilePosition;
    }

   [System.Serializable]
    public class LevelSlime
    {
        public GameObject slimePrefab;
        public Vector3 slimePosition;
        public SlimeController.SlimeState defaultState;
    }
    
    [System.Serializable]
    public class LevelFood
    {
        public FoodOrder foodOrder;
    }
    

    public LevelObject[] objectsToSpawn;
    public LevelSlime[] slimeToSpawn;
    public LevelFood foodToSpawn;
}
