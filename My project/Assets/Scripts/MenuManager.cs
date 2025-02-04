using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public LevelLoader levelLoader;

    public GameObject loseUIGameObject;
    public GameObject winUIGameObject;

    //bool pour éviter des situations où on affiche les 2 interfaces en même temps
    bool isUIDrawn = false;






    //singleton
    public static MenuManager instance;
    private void Awake()
    {
        // activate singleton
        if (instance == null)
            instance = this;

        //on cache les interfaces
        ResetUI();
    }

    public void LoseUI()
    {
        //interface de lose activée
        if (!isUIDrawn)
        {
            isUIDrawn = true;
            loseUIGameObject.SetActive(true);
        }

    }

    public void WinUI()
    {
        //interface de win activée
        if (!isUIDrawn)
        {
            isUIDrawn = true;
            loseUIGameObject.SetActive(false);
            winUIGameObject.SetActive(true);
        }
        
    }

    public void ResetUI()
    {
        loseUIGameObject.SetActive(false);
        winUIGameObject.SetActive(false);
        isUIDrawn = false;
    }


    public void Onback()
    {
        ResetUI();
        SceneManager.LoadScene("LevelMap");
    }

    public void Retry()
    {
        ResetUI();
        levelLoader.LoadLevel();
    }
}
