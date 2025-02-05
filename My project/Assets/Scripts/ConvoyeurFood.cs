using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ConvoyeurFood : MonoBehaviour
{
    
    public Transform spawnPoint;  // Leftmost spawn position
    public Transform[] positions; // Fixed positions for 4 objects
    public float moveSpeed = 2f;   // Movement speed
    public FoodOrder foodList;    // Reference to predefined items
    public Transform parentFood;
    private int foodCount = 0;
    private Queue<DragableObject> itemQueue = new Queue<DragableObject>();
    private List<DragableObject> activeItems = new List<DragableObject>();
    private bool isMoving = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        DragableObject.OnObjectMoved += HandleFoodMoved;
        
        foreach (var food in foodList.food)
        {
            itemQueue.Enqueue(food.foodPrefab);
        }

        for (int i = 0; i < positions.Length; i++)
        {
            SpawnNewFood(i);
        }
    }

    void OnDestroy()
    {
        DragableObject.OnObjectMoved -= HandleFoodMoved;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            MoveItems();
        }
    }

    void MoveItems()
    {
        bool allReached = true;

        for (int i = 0; i < activeItems.Count; i++)
        {
            DragableObject item = activeItems[i];
            
            item.indexInConvoyeur = i;
            
            if (item.IsMooved)
                continue;
            
            Vector3 targetPos = positions[i].position;

            item.transform.position =
                Vector3.MoveTowards(item.transform.position, targetPos, moveSpeed * Time.deltaTime);
            
            if (Vector3.Distance(item.transform.position, targetPos) > 0.01f)
            {
                allReached = false;
            }
        }

        if (allReached)
        {
            isMoving = false;
        }
    }

    void SpawnNewFood(int positionIndex)
    {
        if (itemQueue.Count <= 0) 
            return;
        DragableObject foodData = itemQueue.Dequeue();
        
        Vector3 offScreenPosition = positions[positions.Length - 1].position;
        offScreenPosition.x += 3f;
        
        DragableObject newFood = Instantiate(foodData.type.foodPrefab, offScreenPosition, Quaternion.identity, parentFood);
        foodCount++;
        if (foodCount <= 3)
            newFood.transform.position = positions[positionIndex].position;
        newFood.indexInConvoyeur = positionIndex;
        
        activeItems.Add(newFood);

        isMoving = true;
    }

    public void DroppedFood(int index)
    {
        if(index < 0 || index >= activeItems.Count) 
            return;
        DragableObject droppedItem = activeItems[index];
        droppedItem.IsMooved = true;
        activeItems.RemoveAt(index);
        
        for (int i = index; i < activeItems.Count; i++)
        {
            activeItems[i].indexInConvoyeur = i;
        }
        SpawnNewFood(activeItems.Count);
        isMoving = true;
    }
    
    private void HandleFoodMoved(DragableObject food)
    {

        DroppedFood(food.indexInConvoyeur);
    }
    //Graou
}
