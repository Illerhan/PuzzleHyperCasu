using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "MovableObject")
                {
                    var objectScript = hit.collider.GetComponent<RotateObject>();
                    objectScript.isActive = !objectScript.isActive;
                }
            }
        }
    }
}
