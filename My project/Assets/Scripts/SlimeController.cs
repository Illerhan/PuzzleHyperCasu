using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SlimeController : MonoBehaviour
{

    public SlimeData slimeData;
    private DragableObject targetItem = null;
    public float currentSize = 1;
    [SerializeField] private float pulseRadius;
    [SerializeField] private float pulseForce;
    private IEnumerator co;
    public float maxSize = 3;
    private bool isGrown = false;
    public float speed = 30f;
    void Start()
    {
        transform.localScale *= currentSize;
        Renderer slimRenderer = this.GetComponent<Renderer>();
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
        if (!droppedItem.CompareTag(slimeData.compatibleItemTag) || isGrown) return;
        float distance = Vector3.Distance(transform.position, droppedItem.transform.position);
        if (distance <= droppedItem.GetInfluenceRadius())
        {
            targetItem = droppedItem;
            co = MoveToObject(droppedItem.transform.position);
            StartCoroutine(co);
        }
    }

    private void OnItemEaten(DragableObject droppedItem)
    {
        Debug.Log("Food disapeared");
        if (co != null && droppedItem != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }
    
    private IEnumerator MoveToObject(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        
        Destroy(targetItem.gameObject);
        targetItem = null;
        GrowSlim();
        
        Debug.Log($"{slimeData.slimeName} reached its compatible item!");
    }

    private void GrowSlim()
    {
        if (isGrown)
            return;
        currentSize += 0.5f;
        transform.localScale *= 1.25f;
        if (currentSize == maxSize)
        {
            isGrown = true;
            SlimeManager.Instance.CheckSlimes();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SlimeController otherSlim = other.GetComponent<SlimeController>();
        if(otherSlim != null)
        {
            if (otherSlim.slimeData.slimeName == slimeData.slimeName && currentSize > otherSlim.currentSize)
            {
                Debug.Log("Collided");
                GrowSlim();
                Destroy(other.gameObject);
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

    // Update is called once per frame
    void Update()
    {
    
        
    }
}
