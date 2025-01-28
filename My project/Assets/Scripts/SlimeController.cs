using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeController : MonoBehaviour
{

    public SlimeData slimeData;
    private DragableObject targetItem = null;
    
    public float speed = 30f;
    void Start()
    {
        
    }
    
    private void OnEnable()
    {
        DragableObject.OnItemDropped += OnItemDropped;
    }

    private void OnDisable()
    {
        DragableObject.OnItemDropped -= OnItemDropped;
    }
    
    private void OnItemDropped(DragableObject droppedItem)
    {
        if (!droppedItem.CompareTag(slimeData.compatibleItemTag)) return;
        float distance = Vector3.Distance(transform.position, droppedItem.transform.position);
        if (distance <= droppedItem.GetInfluenceRadius())
        {
            StartCoroutine(MoveToObject(droppedItem.transform.position));
        }
    }
    
    // public void MoveToFood()
    // {
    //     var compatibleItems = ObjectManager.Instance.GetItemsByTag(slimeData.compatibleItemTag);
    //
    //     if (compatibleItems.Count == 0)
    //     {
    //         Debug.Log($"{slimeData.slimeName} has no compatible items!");
    //         return;
    //     }
    //     
    //     DragableObject closeItem = null;
    //     float closestDistance = Mathf.Infinity;
    //
    //     foreach (DragableObject item in compatibleItems)
    //     {
    //         float distance = Vector3.Distance(transform.position, item.transform.position);
    //         if (distance < closestDistance)
    //         {
    //             closestDistance = distance;
    //             closeItem = item;
    //             
    //         }
    //     }
    //
    //     if (closeItem != null)
    //     {
    //         StartCoroutine(MoveToObject(closeItem.transform.position));
    //     }
    // }

    private IEnumerator MoveToObject(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        Debug.Log($"{slimeData.slimeName} reached its compatible item!");
    }
   
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    // Update is called once per frame
    void Update()
    {
    }
}
