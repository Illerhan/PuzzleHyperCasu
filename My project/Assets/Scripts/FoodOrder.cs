using UnityEngine;

[CreateAssetMenu(fileName = "FoodOrder", menuName = "Scriptable Objects/FoodOrder")]
public class FoodOrder : ScriptableObject
{
    public FoodType[] food;
}
