using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class SlimeController : MonoBehaviour
{
    public enum SlimeState
    {
        Normal,
        Hungry,
        Sleeping
    }
    
    public SlimeData slimeData;
    [SerializeField] DragableObject targetItem;
    public int currentSize = 1;
    [SerializeField] private float pulseRadius;
    [SerializeField] private float pulseForce;
    private IEnumerator co;
    public int maxSize = 3;
    private bool isGrown = false;
    public float speed = 30f;
    public SlimeState currentState = SlimeState.Normal;
    
    void Start()
    {
        transform.localScale *= currentSize;
        Renderer slimRenderer = GetComponent<Renderer>();
        Color slimColor = slimeData.slimeColor;
        slimRenderer.material.color = slimColor;
        SlimeManager.Instance.RegisterSlime(this);
    }
    
    private void OnEnable()
    {
        DragableObject.OnItemDropped += OnItemDropped;
        DragableObject.OnItemEaten += OnItemEaten;
    }

    private void OnDisable()
    {
        DragableObject.OnItemDropped -= OnItemDropped;
        DragableObject.OnItemEaten += OnItemEaten;
    }
    
    private void OnItemDropped(DragableObject droppedItem)
    {
        targetItem = droppedItem;
        
        if ((!droppedItem.CompareTag(slimeData.compatibleItemTag) && currentState != SlimeState.Hungry)|| isGrown || currentState == SlimeState.Sleeping) 
            return;
        if (IsPathBlocked(droppedItem.transform.position))
            return;
        
        float distance = Vector3.Distance(transform.position, droppedItem.transform.position);
        if (distance <= droppedItem.GetInfluenceRadius())
        {
            co = MoveToObject(droppedItem.transform.position);
            StartCoroutine(co);
        }
    }

    private void OnItemEaten(DragableObject droppedItem)
    {
        Debug.Log("Food disapeared");
        Destroy(droppedItem.gameObject);
    }
    
    private IEnumerator MoveToObject(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        targetItem = null;
        GrowSlim();
        
        Debug.Log($"{slimeData.slimeName} reached its compatible item!");
    }

    public void GrowSlim()
    {
        if (isGrown) return;
        
        currentSize += 1;
        transform.localScale *= 1.25f;
        
        if (currentSize >= maxSize)
        {
            if (currentState == SlimeState.Hungry)
                currentState = SlimeState.Normal;

            isGrown = true;
        }
        
        SlimeManager.Instance.CheckSlimes();
    }

    private void Update()
    {
        SlimeController smallestSlime = null;
        
        Collider[] slimeInRange = Physics.OverlapSphere(transform.position,1f);
        foreach (Collider col in slimeInRange)
        {
            SlimeController otherSlim = col.GetComponent<SlimeController>();
            if (otherSlim != null && otherSlim != this)
            {
                if (currentSize >= otherSlim.currentSize)
                {
                    if (smallestSlime == null || otherSlim.currentSize < smallestSlime.currentSize)
                    {
                        smallestSlime = otherSlim;
                    }
                }
            }
        }
        
        SlimeManager.Instance.CheckSlimes();

        if (smallestSlime != null)
        {
            if (smallestSlime.slimeData.slimeColor != slimeData.slimeColor)
            {
                // Game Over
                Debug.Log("SayHi");
                MenuManager.instance.LoseUI();
                return;
            }
            
            Destroy(smallestSlime.gameObject);
            Debug.Log($"Ate {smallestSlime.slimeData.slimeName}");
            GrowSlim();

            // If sleeping, wake up and move to target
            if (currentState == SlimeState.Sleeping && targetItem != null)
            {
                co = MoveToObject(targetItem.transform.position);
                currentState = SlimeState.Normal;
                StartCoroutine(co);
            }
        }
    }

    public void Bounce()
    {
        Debug.Log("Hola");
        Collider[] hitSlimes = Physics.OverlapSphere(transform.position, pulseRadius);

        foreach (Collider slim in hitSlimes)
        {
            if (slim.GameObject() != gameObject)
            {
                if (slim.CompareTag("Slime") && slim.GetComponent<SlimeController>().slimeData.slimeName != slimeData.slimeName)
                {
                    Rigidbody rb = slim.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 forceDirection = (slim.transform.position - transform.position).normalized;
                        rb.AddForce(forceDirection * pulseForce,ForceMode.Impulse);
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        SlimeManager.Instance.UnregisterSlime(this);
    }

    private bool IsPathBlocked(Vector3 targetPosition)
    {
        RaycastHit hit;
        Vector3 direction = targetPosition - transform.position;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude))
        {
            if (hit.collider.CompareTag("Grass"))
            {
                return true;
            }
        }

        return false;
    }
}
