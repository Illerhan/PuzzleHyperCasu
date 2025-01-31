using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;

    private Dictionary<DragableObject, Vector3> itemPosition = new Dictionary<DragableObject, Vector3>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterItem(DragableObject item)
    {
        if (!itemPosition.ContainsKey(item))
            itemPosition[item] = item.transform.position;
    }
    public void UpdateItemPosition(DragableObject item)
    {
        if (itemPosition.ContainsKey(item))
        {
            itemPosition[item] = item.transform.position;
        }
    }
    
    public void UnregisterItem(DragableObject item)
    {
        if (itemPosition.ContainsKey(item))
        {
            itemPosition.Remove(item);
            Debug.Log($"{item.name} has been unregistered.");
        }
    }
    

    public List<DragableObject> GetItemsByTag(string tag)
    {
        List<DragableObject> items = new List<DragableObject>();
        foreach (var item in itemPosition.Keys)
        {
            if (item.CompareTag(tag))
            {
                items.Add(item);
            }
        }

        return items;
    }
    
    public List<DragableObject> GetItemsWithinRadius(Vector3 position, float radius)
    {
        List<DragableObject> nearbyItems = new List<DragableObject>();
        foreach (var item in itemPosition.Keys)
        {
            float distance = Vector3.Distance(position, item.transform.position);
            if (distance <= item.GetInfluenceRadius())
            {
                nearbyItems.Add(item);
            }
        }
        return nearbyItems;
    }
}
