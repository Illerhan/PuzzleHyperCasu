using System;
using System.Collections.Generic;
using System.Net.Mime;
using NUnit.Framework;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.PlasticSCM.Editor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public GameObject buttonLevelsContainers;
    private List<GameObject> buttonList;
    public LevelContainer currentLevel;
    

    
    private void Start()
    {
        buttonList = new List<GameObject>();
      
        int count = buttonLevelsContainers.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject child = buttonLevelsContainers.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;
            buttonList.Add(child);
            buttonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            buttonList[i].name = (i).ToString();


        }
        
    }

    public void StartLevel()
    {
        int index = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        currentLevel.selectedLevel = currentLevel.levelSo[index];
        SceneManager.LoadScene("Level");
    }
}

