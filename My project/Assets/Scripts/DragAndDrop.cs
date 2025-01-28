using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class DragableObject : MonoBehaviour
{
    Vector3 mousePosition;
   
    private Vector3 getMousePosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    
    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - getMousePosition();
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition-mousePosition);
    }
}