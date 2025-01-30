using UnityEngine;

[CreateAssetMenu(fileName = "LevelContainer", menuName = "Scriptable Objects/LevelContainer")]
public class LevelContainer : ScriptableObject
{
    public LevellSO selectedLevel;
    public LevellSO[] levelSo;
}
