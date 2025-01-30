using System;
using System.Collections.Generic;
using System.Net.Mime;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public GameObject LevelsContainers;
    private List<GameObject> buttonList;

    
    private void Start()
    {
        buttonList = new List<GameObject>();
      
        int count = LevelsContainers.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject child = LevelsContainers.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;
            buttonList.Add(child);
            buttonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            

        }
      
     




    }
}
