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
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UI.Image;

public class LevelManager : MonoBehaviour
{
    public GameObject buttonLevelsContainers;
    private List<GameObject> buttonList;
    public LevelContainer currentLevel;
    public List<bool> unlockedlevels;
    public Color lockColor;
    public static LevelManager Instance;
    
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
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
            
            if (i!= 0)
            {
                buttonList[i].GetComponent<Image>().color = lockColor;
                buttonList[i].transform.GetChild(1).gameObject.SetActive(true);
                
            }

        }
        //unlockedlevels = new List<bool>(buttonList.Count);
        
        
    }

    public void StartLevel()
    {
        
        int index = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        
        if ( index == 0 || unlockedlevels[index-1])
        {
            currentLevel.selectedLevel = currentLevel.levelSo[index];
            SceneManager.LoadScene("Level");
           
        }
        else
        {
            buttonList[index].transform.GetChild(1).GetComponent<Animator>().SetTrigger("Trigger");
            Debug.Log("lock, level" + (index+1));
        }
    }

    public void SetNextLevel()
    {
        int index = Array.IndexOf(currentLevel.levelSo, currentLevel.selectedLevel);
        currentLevel.selectedLevel = currentLevel.levelSo[index + 1];
        SceneManager.LoadScene("Level");
    }
}

