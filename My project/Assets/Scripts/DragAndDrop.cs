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
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector3 spawnZone;
    [SerializeField] private float spawnZoneRadius = 5f;
    private bool isFirstMove = true;
    [SerializeField] public FoodType type;

    public static event Action<DragableObject> OnItemDropped;
    public static event Action<DragableObject> OnItemEaten;
    public static event Action<DragableObject> OnObjectMoved;
    public GameObject rangePrefab;
    public int indexInConvoyeur;
    
    public bool IsMooved
    {
        get => isMooved;
        set
        {
            if (isMooved != value) // Notify only if value changes
            {
                isMooved = value;
                OnObjectMoved?.Invoke(this); // Notify convoyeur
            }
        }
    }

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
        if (isFirstMove)
        {
            initialPosition = this.transform.position;
            spawnZone = initialPosition;
            isFirstMove = false;
        }
        range = Instantiate(rangePrefab);
        range.transform.localScale = new Vector3(type.influenceRadius*1.5f,1,type.influenceRadius*1.5f);
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

    private void OnTriggerStay(Collider other)
    {
        
        if (isMooved)
        {

            if (other.CompareTag("Slime"))
            {
                SlimeController slim = other.GetComponent<SlimeController>();
                
                if (slim.slimeData.compatibleItemTag != type.itemName)
                {
                    Debug.Log("Hola");
                    slim.Bounce();
                }
            }
            
        }
    }

    private void OnMouseUp()
    {
        if (IsInsideSpawnZone(transform.position))
        {
            ObjectManager.Instance.UpdateItemPosition(this);
            Destroy(range.gameObject);
            transform.position = initialPosition;
        }
        else
        {
            Debug.Log("HI");
            isMooved = true;
            ObjectManager.Instance.UpdateItemPosition(this);
            Destroy(range.gameObject);
            OnItemDropped?.Invoke(this);
        }
        
    }
    private bool IsInsideSpawnZone(Vector3 position)
    {
        return Vector3.Distance(position, spawnZone) <= spawnZoneRadius;
    }
    

    public float GetInfluenceRadius()
    {
        return type.influenceRadius;
    }

    private void OnDestroy()
    {
        OnItemEaten?.Invoke(this);
    }
}