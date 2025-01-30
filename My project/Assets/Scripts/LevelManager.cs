using System;
using System.Collections.Generic;
using System.Net.Mime;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public GameObject buttonLevelsContainers;
    private List<GameObject> buttonList;
    public List<ScriptableObject> levels;

    
    private void Start()
    {
        buttonList = new List<GameObject>();
        levels = new List<ScriptableObject>();
      
        int count = buttonLevelsContainers.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject child = buttonLevelsContainers.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;
            buttonList.Add(child);
            buttonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            buttonList[i].name = (i + 1).ToString();


        }
        
    }

    public void StartLevel()
    {
        int index = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        Debug.Log("Load Level " + index);

    }
}

