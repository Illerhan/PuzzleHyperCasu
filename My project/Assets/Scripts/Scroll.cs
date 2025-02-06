using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float speed;

    public Transform background;
    private Vector3 startpos;
    public Vector3 endpos;
    private Vector3 actualpos;
    
    private float duration;
    private float position;
    private float elapsedTime;
    private bool canMove = false;
    private float percentageComplete;

    [SerializeField] private AnimationCurve curve;

    private void OnEnable()
    {
        scrollRect.onValueChanged.AddListener(scrollRectCallBack);
    }

    private void OnDisable()
    {
        scrollRect.onValueChanged.RemoveListener(scrollRectCallBack);
    }

    void scrollRectCallBack(Vector2 value)
    {
        PlayerPrefs.SetFloat("ScrollPosition", value.y);
    }

    private void Start()
    {
       
        startpos =background.position;
        actualpos = startpos;
        position = PlayerPrefs.GetFloat("ScrollPosition");
        if ( scrollRect.verticalNormalizedPosition == position)
        {
            canMove = false;
          
        }
        else
        {
            duration = position *10 * speed;
            canMove = true;
            
        }

    }

    private void Update()
    {
        if (canMove)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / duration;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, position, curve.Evaluate(percentageComplete));
            
        }

        float pos = Mathf.Round((scrollRect.verticalNormalizedPosition * 1000));
        float endpos = Mathf.Round((position * 1000));
       
        if (pos == endpos)
        {
            canMove = false;
            duration = 0;
        }
        
    }

    public void MoveBackground(Vector2 normalizedpos)
    {
        actualpos = new Vector3(-13,(1 - normalizedpos.y) * startpos.y - 30,(1 - normalizedpos.y) * startpos.z + 50);
        
        background.transform.position= actualpos;
        

    }
}
