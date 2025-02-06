using System;
using UnityEngine;

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
        if (isMooved || MenuManager.instance.playerWon) return;
        
        if (isFirstMove)
        {
            initialPosition = transform.position;
            spawnZone = initialPosition;
            isFirstMove = false;
        }
        
        range = Instantiate(rangePrefab);
        range.transform.localScale = new Vector3(type.influenceRadius * 1.5f, 1, type.influenceRadius * 1.5f);
        mousePosition = Input.mousePosition - getMousePosition();
        
    }

    private void OnMouseDrag()
    {
        if(isMooved || MenuManager.instance.playerWon) return;
        
        if(!isMooved && Camera.main)
        {
            Vector3 screenMousePos = Input.mousePosition;
            screenMousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            transform.position = Camera.main.ScreenToWorldPoint(screenMousePos);
            range.transform.position = transform.position;
        }
    }

    private void OnMouseUp()
    {
        if(MenuManager.instance.playerWon) return;
        
        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, -1));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
            transform.position = ray.GetPoint(distance);
        
        if(isMooved) return;

        if (IsInsideSpawnZone(transform.position))
        {
            transform.position = initialPosition;
            isFirstMove = false;
            isMooved = false;
            Destroy(range.gameObject);
        }
        else if(!isMooved && !IsInsideSpawnZone(transform.position))
        {
            isMooved = true;
            ObjectManager.Instance.UpdateItemPosition(this);
            Destroy(range.gameObject);
            OnItemDropped?.Invoke(this);
            OnObjectMoved?.Invoke(this);
            LevelLoader.actionCount++;
            MenuManager.instance.UpdateFinalMoveNumber(LevelLoader.actionCount);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isMooved && other.CompareTag("Slime"))
        {
            SlimeController slim = other.GetComponent<SlimeController>();
            
            if (slim.slimeData.compatibleItemTag != type.itemName)
            {
                Debug.Log("Hola");
                slim.Bounce();
                Destroy(gameObject);
            }
            
            slim.GrowSlime();
            Destroy(gameObject);
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
        ObjectManager.Instance.UnregisterItem(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, type.influenceRadius);
    }
}