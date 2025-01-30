using UnityEngine;

[CreateAssetMenu(fileName = "FoodType", menuName = "Scriptable Objects/FoodType")]
public class FoodType : ScriptableObject
{
    [SerializeField] public float influenceRadius;
    [SerializeField] public string itemName;
    [SerializeField] public DragableObject foodPrefab;
}
