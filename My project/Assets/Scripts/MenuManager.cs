using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public LevelLoader levelLoader;

    public GameObject loseUIGameObject;
    public GameObject winUIGameObject;

    public Image star1;
    public GameObject textstar1;

    public Image star2;
    public GameObject textstar2;

    public Image star3;
    public GameObject textstar3;

    public Sprite obtainedStarImage;
    public Sprite emptyStarImage;

    //bool pour éviter des situations où on affiche les 2 interfaces en même temps
    bool isUIDrawn = false;


    private int placeholderInt;
 
    private int score_pl1;
    private int score_pl2;
    private int score_pl3;


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

    /*
    void CheckStar()
    {
        //De base toutes les étoiles sont vides
        star1.sprite = emptyStarImage;
        star2.sprite = emptyStarImage;
        star3.sprite = emptyStarImage;

        //On remplit celles qu'on possède
        if (placeholderInt <= score_pl1)
        {
            //3 stars
            star1.sprite = obtainedStarImage;
            star2.sprite = obtainedStarImage;
            star3.sprite = obtainedStarImage;
        }
        else if(placeholderInt <= score_pl2)
        {
            star1.sprite = obtainedStarImage;
            star2.sprite = obtainedStarImage;
            //2 stars
        }
        else if(placeholderInt <= score_pl3)
        {
            star1.sprite = obtainedStarImage;
            //1 star
        }
        else
        {
            //No stars ???
        }
    }
    */

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
