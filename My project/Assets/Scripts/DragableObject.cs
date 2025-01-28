using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class DragableObject : MonoBehaviour
{
   private Vector3 mousePosition;
   private Vector3 offset;
   private Camera mainCamera;
   bool isDraging;

   private void Awake()
   {
       EnhancedTouchSupport.Enable();
   }

   private void OnDestroy()
   {
       EnhancedTouchSupport.Disable();
   }

   private void Update()
   {
       if (Touch.activeTouches.Count > 0)
       {
           Touch touch = Touch.activeTouches[0];

           switch (touch.phase)
           {
               case TouchPhase.Began:
                   OnTouchDown(touch);
                   break;
               case TouchPhase.Moved:
               case TouchPhase.Stationary:
                   if (isDraging)
                        OnTouchDrag(touch);
                   break;
               case TouchPhase.Ended:
               case TouchPhase.Canceled:
                   OnTouchUp();
                   break;
           }
       }
   }

    private void OnTouchDown(Touch touch )
    {
        Vector3 touchWorldPosition = mainCamera.ScreenToWorldPoint((new Vector3(touch.screenPosition.x,
            touch.screenPosition.y, mainCamera.nearClipPlane)));
        Ray ray = mainCamera.ScreenPointToRay(touch.screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            isDraging = true;

            offset = transform.position - touchWorldPosition;
        }
        
    }

    private void OnTouchDrag(Touch touch)
    {
        Vector3 touchWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.screenPosition.x,
            touch.screenPosition.y, mainCamera.nearClipPlane));
        transform.position = touchWorldPosition + offset;
    }

    private void OnTouchUp()
    {
        isDraging = false;
    }
}
