using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ConvoyeurFood : MonoBehaviour
{
    
    public Transform spawnPoint;  // Leftmost spawn position
    public Transform[] positions; // Fixed positions for 4 objects
    public float moveSpeed = 2f;   // Movement speed
    public FoodOrder foodList;    // Reference to predefined items

    private Queue<DragableObject> itemQueue = new Queue<DragableObject>();
    private List<DragableObject> activeItems = new List<DragableObject>();
    private bool isMoving = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (positions.Length == 0 || positions.Contains(null))
        {
            Debug.LogError("Positions array is not properly initialized.");
            return;
        }
        
        
   
        foreach (var food in foodList.food)
        {
            itemQueue.Enqueue(food.foodPrefab);
        }

        for (int i = 0; i < positions.Length; i++)
        {
            SpawnNewFood(i);
        }
<<<<<<< Updated upstream
        
        //DragableObject.OnItemDropped += HandleItemDropped;
=======

>>>>>>> Stashed changes
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

        DragableObject newFood = Instantiate(foodData.type.foodPrefab, spawnPoint.position, Quaternion.identity);
        activeItems.Add(newFood);

        newFood.transform.position = positions[positionIndex].position;
    }

    public void DroppedFood(int index)
    {
        if(index < 0 || index >= activeItems.Count) 
            return;
        DragableObject droppedItem = activeItems[index];
        activeItems.RemoveAt(index);
        SpawnNewFood(positions.Length-1);

        isMoving = true;
    }
}
