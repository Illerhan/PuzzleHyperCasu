using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public LevelLoader levelLoader;

    [Space(10)]
    public GameObject loseUIGameObject;
    public Image loseBackground;
    [SerializeField] int loseBgAlpha;

    [Space(10)]
    public GameObject winUIGameObject;
    public Image winBackground;
    [SerializeField] int winBgAlpha;
    [Space(10)]
    public float bgFadeTime = 1f;
    //false : fade in le win bg, true : fade in le lose bg
    bool isWinBg = false;

    //étoiles
    [Space(10)]
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

    //gestion du temps pour l'UI
    bool bgClockStarted = false;
    float bgClock = 0;


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
            isWinBg = false;
            bgClockStarted = true;
            isUIDrawn = true;
            loseUIGameObject.SetActive(true);
        }

    }

    public void WinUI()
    {
        //interface de win activée
        if (!isUIDrawn)
        {
            isWinBg = true;
            bgClockStarted = true;
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
            Win1Star.sprite = obtainedStarImage;
            //LevelManager.Instance.starsNumber++;
            
            if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[1].Moves)
            {
                Win2Star.sprite = obtainedStarImage;
                //LevelManager.Instance.starsNumber++;
                if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[0].Moves)
                {
                    Win3Star.sprite = obtainedStarImage;
                    //LevelManager.Instance.starsNumber++;
                }
            }
        }
        finalNumberOfMoves = 0;
    }
    

    public void ResetUI()
    {
        loseUIGameObject.SetActive(false);
        winUIGameObject.SetActive(false);

        //reset transparence
        winBackground.color = new Color(winBackground.color.r, winBackground.color.g, winBackground.color.b, 0);
        loseBackground.color = new Color(loseBackground.color.r, loseBackground.color.g, loseBackground.color.b, 0);

        //reset clocks
        bgClock = 0;
        bgClockStarted = false;

        isWinBg = false;

        isUIDrawn = false;
        
        
    }

    public void UpdateFinalMoveNumber(int numberOfMoves)
    {
        finalNumberOfMoves = numberOfMoves;
        levelLoader.movesDone++;
        levelLoader.movesLeft.text = levelLoader.movesDone.ToString();
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
        
    }
    
    public void NextLevel()
    {
        ResetUI();
        LevelManager.Instance.SetNextLevel();
        SceneManager.LoadScene("Level");
    }


    private void Update()
    {
        if (bgClockStarted)
        {
            if(bgClock < bgFadeTime)
            {
                bgClock += Time.deltaTime;
                
                Debug.Log(bgClock);
            }
            else
            {
                bgClockStarted = false;
            }

            if (isWinBg)
            {
                //transition du winbg
                //calcul : alpha voulu = alpha max * pourcentage (pourcentage = temps actuel / temps max)
                winBackground.color = new Color(winBackground.color.r, winBackground.color.g, winBackground.color.b, winBgAlpha * bgClock / bgFadeTime / 255);
                Debug.Log(winBgAlpha * bgClock / bgFadeTime);

                
            }
            else
            {
                //transition du losebg
                //calcul : alpha voulu = alpha max * pourcentage (pourcentage = temps actuel / temps max)
                loseBackground.color = new Color(loseBackground.color.r, loseBackground.color.g, loseBackground.color.b, loseBgAlpha * bgClock / bgFadeTime / 255);
                Debug.Log(loseBgAlpha * bgClock / bgFadeTime);
            }
        }

    }


}
