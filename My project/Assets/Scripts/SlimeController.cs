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
    private DragableObject targetItem = null;
    public int currentSize = 1;
    [SerializeField] private float pulseRadius;
    [SerializeField] private float pulseForce;
    private IEnumerator co;
    public int maxSize = 3;
    private bool isGrown = false;
    public float speed = 30f;
    public SlimeState currentState = SlimeState.Normal;

    public Rigidbody rb;

    [SerializeField] private Animator animBody;
    [SerializeField] private Animator animFace;

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

        if ((!droppedItem.CompareTag(slimeData.compatibleItemTag) && currentState != SlimeState.Hungry) || isGrown ||
            currentState == SlimeState.Sleeping)
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

        Destroy(targetItem.gameObject);
        targetItem = null;
        GrowSlime();

        Debug.Log($"{slimeData.slimeName} reached its compatible item!");
    }

    public void GrowSlime()
    {
        if (isGrown)
            return;
        animBody.Play("Manger");
        currentSize += 1;
        transform.localScale = new Vector3(currentSize,currentSize,currentSize);
        if (currentSize >= maxSize)
        {
            if (currentState == SlimeState.Hungry)
                currentState = SlimeState.Normal;
            isGrown = true;
        }

        SlimeManager.Instance.CheckSlimes();
    }

    private void OnTriggerEnter(Collider other)
    {
        SlimeController smallestSlime = null;

        Collider[] slimeInRange = Physics.OverlapSphere(transform.position, 1f);
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

            SlimeManager.Instance.CheckSlimes();
        }

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
            GrowSlime();

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
        Debug.Log("Bounce");
        
        animFace.Play("Saut");
        
        SlimeController[] hittedSlime = SlimeManager.Instance.GetSlimes();
        
        foreach (var slime in hittedSlime)
        {
            if(Vector3.Distance(slime.transform.position, transform.position) > pulseRadius || slime == this)
                continue;
            
            if (slime.slimeData.slimeName == slimeData.slimeName) continue;
            
            Vector3 forceDirection = (slime.transform.position - transform.position).normalized;
            rb.AddForce(forceDirection * pulseForce,ForceMode.Impulse);
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

    // Update is called once per frame
    void Update()
    {
        CheckToMergeSlime();

        switch (currentState)
        {
            case SlimeState.Normal:
                animFace.Play("Visage dodo test2");
                break;
            case SlimeState.Hungry:
                animFace.Play("Faim");
                break;
            case SlimeState.Sleeping:
                animFace.Play("Visage dodo test2 0");
                break;
            default:
                break;
        }
        SlimeManager.Instance.CheckSlimes();

    }

    void CheckToMergeSlime()
    {
        SlimeController smallestSlime = null;

        SlimeController[] slimeInRange = SlimeManager.Instance.GetSlimes();

        foreach (var slime in slimeInRange)
        {
            if (slime == this) continue;

            if (currentSize >= slime.currentSize)
            {
                if (Vector3.Distance(slime.transform.position, transform.position) > 1)
                    continue;

                smallestSlime = slime;

                if (slime.currentSize <= smallestSlime.currentSize)
                {
                    Debug.Log("Assign smallest Slime");
                    smallestSlime = slime;
                    break;
                }
            }
        }


        if (smallestSlime == null) return;

        if (smallestSlime.slimeData.slimeColor != slimeData.slimeColor)
        {
            // Game Over
            Debug.Log("GameOver");
            StartCoroutine(MenuManager.instance.LoseUI());
            return;
        }

        Debug.Log($"Ate {smallestSlime.slimeData.slimeName}");
        Destroy(smallestSlime.gameObject);

        animFace.Play("Miam Miam");

        GrowSlime();
    }
}

               
