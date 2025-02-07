using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class LevelManager : MonoBehaviour
{
    public GameObject buttonLevelsContainers;
    private List<GameObject> buttonList;
    public LevelContainer currentLevel;
    public Sprite obtainedStar;
    public Color lockColor;
    public static LevelManager Instance;
    public TextMeshProUGUI starCount;
    
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    private void Start()
    {
        int nb = 0;
        for (int i = 0; i < 30; i++)
        {
            nb = PlayerPrefs.GetInt(i.ToString()) + nb;
        }
        starCount.text = nb.ToString();
        buttonList = new List<GameObject>();
      
        int count = buttonLevelsContainers.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            int nbstars = PlayerPrefs.GetInt((i).ToString());
            GameObject child = buttonLevelsContainers.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;
            buttonList.Add(child);
            buttonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            buttonList[i].name = (i).ToString();
            Debug.Log(nbstars);
            if (i!= 0 && PlayerPrefs.GetInt((i-1).ToString())==0)
            {
                buttonList[i].GetComponent<Image>().color = lockColor;
                buttonList[i].transform.GetChild(1).gameObject.SetActive(true);
                
            }
            else
            {
                if (nbstars>=1)
                {
                    buttonList[i].transform.GetChild(2).GetComponent<Image>().sprite = obtainedStar;
                    if (nbstars>=2)
                    {
                        buttonList[i].transform.GetChild(4).GetComponent<Image>().sprite = obtainedStar;
                        if (nbstars >=3)
                        {
                            buttonList[i].transform.GetChild(3).GetComponent<Image>().sprite = obtainedStar;
                    
                        }
                    
                    }
                }
            }

        }
        
        
    }

    public void StartLevel()
    {
        
        int index = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        
        if ( index == 0 || PlayerPrefs.GetInt((index-1).ToString())>0)
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

    public void ResetProgression()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LevelMap");
    }

}

