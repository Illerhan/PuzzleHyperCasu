using System;
using System.Security.Cryptography;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class DragableObject : MonoBehaviour
{
    Vector3 mousePosition;
    private bool isMooved;
    private GameObject range;
    private float influenceRadius = 10f;
    [SerializeField] private string itemName;

    public static event Action<DragableObject> OnItemDropped;
    public GameObject rangePrefab;
    

    private void Start()
    {
        ObjectManager.Instance.RegisterItem(this);
    }

    private Vector3 getMousePosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    
    private void OnMouseDown()
    {
        range = Instantiate(rangePrefab);
        range.transform.localScale = new Vector3(influenceRadius*1.5f,1,influenceRadius*1.5f);
        mousePosition = Input.mousePosition - getMousePosition();
        
    }

    private void OnMouseDrag()
    {
        if(!isMooved)
        {
            if (Camera.main != null)
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
                range.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        SlimeController slim = other.GetComponent<SlimeController>();
        if (slim.slimeData.compatibleItemTag != this.itemName)
        {
            Debug.Log("Hola");
            slim.Bounce();
        }
    }

    private void OnMouseUp()
    {
        isMooved = true;
        ObjectManager.Instance.UpdateItemPosition(this);
        Destroy(range.gameObject);
        OnItemDropped?.Invoke(this);
    }

    public float GetInfluenceRadius()
    {
        return influenceRadius;
    }
    
}