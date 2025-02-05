using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public LevelLoader levelLoader;

    public GameObject loseUIGameObject;
    public GameObject winUIGameObject;

    //étoiles

    public Image Win1Star;
    public Image Win2Star;
    public Image Win3Star;
    

    public Sprite obtainedStarImage;

    //score pour chaque étoile

    private int score_pl1;
    private int score_pl2;
    private int score_pl3;

    //bool pour éviter des situations où on affiche les 2 interfaces en méme temps
    bool isUIDrawn = false;


    private int finalNumberOfMoves;
 



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
            
            CheckStar();

            isUIDrawn = true;
            loseUIGameObject.SetActive(false);
            winUIGameObject.SetActive(true);
        }
        
    }
    
    
    void CheckStar()
    {
        
        if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[2].Moves)
        {
            Debug.Log(2);
            Win1Star.sprite = obtainedStarImage;
            
            if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[1].Moves)
            {
                Debug.Log(3);
                Win2Star.sprite = obtainedStarImage;
                
                if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[0].Moves)
                {
                    Debug.Log(4);
                    Win3Star.sprite = obtainedStarImage;
                }
            }
        }
    }
    

    public void ResetUI()
    {
        loseUIGameObject.SetActive(false);
        winUIGameObject.SetActive(false);
        isUIDrawn = false;
        
    }

    public void UpdateFinalMoveNumber(int numberOfMoves)
    {
        finalNumberOfMoves = numberOfMoves;
        levelLoader.totalmoves--;
        levelLoader.movesLeft.text = levelLoader.totalmoves.ToString();
    }


    public void Onback()
    {
        ResetUI();
        SceneManager.LoadScene("LevelMap");
    }

    public void Retry()
    {
        ResetUI();
        SceneManager.LoadScene("Level");
        //levelLoader.LoadLevel();
    }
    
    public void NextLevel()
    {
        ResetUI();
        LevelManager.Instance.SetNextLevel();
        SceneManager.LoadScene("Level");
        //levelLoader.LoadLevel();
    }
}
