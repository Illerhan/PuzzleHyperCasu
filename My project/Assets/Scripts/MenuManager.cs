using System;
using System.Collections;
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
    public float timeBeforeLose = 1;

    [Space(10)]
    public GameObject winUIGameObject;
    public Image winBackground;
    [SerializeField] int winBgAlpha;
    public float timeBeforeWin = 1;

    [Space(10)]
    public float bgFadeTime = 1f;
    public AnimationCurve bgCurve;

    public int starsNumber =-1;

    //false : fade in le win bg, true : fade in le lose bg
    [HideInInspector] public bool playerWon = false;

    //étoiles du win screen
    [Space(10)]
    public Image Win1Star;
    public Image Win2Star;
    public Image Win3Star;
    
    //les images des étoiles in game
    [Space(10)]
    public Image inGameStar1;
    public Image inGameStar2;
    public Image inGameStar3;
    

    public Sprite obtainedStarImage;
    public Sprite lostStarImage;

    //bool pour éviter des situations où on affiche les 2 interfaces en méme temps
    bool isUIDrawn = false;


    private int finalNumberOfMoves;

    //gestion du temps pour l'UI
    bool bgClockStarted = false;
    float bgClock = 0;

    float fadeLerpFactor;

    //effet de bounce/squish
    [Space(5)]
    public AnimationCurve verticalSquish;
    public AnimationCurve horizontalSquish;
    public float squishTime = 1;
    [Space(5)]
    public GameObject loseSquishable;
    public GameObject winSquishable;


    bool squishClockStarted = false;
    float squishClock = 0;
    float verticalSquishLerpFactor;
    float horizontalSquishLerpFactor;

    Vector3 newUIScale;

    //temps pour chaque étoile
    public float waitBetweenStars = 0.8f;
    float starClock = 0;
    bool starClockStarted = false;

    //confettis
    [Space(10)]
    public float waitBeforeParticle = 0.1f;
    public ParticleSystem confetti1;




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

    public IEnumerator LoseUI()
    {
        //interface de lose activée
        if (!isUIDrawn)
        {
            //on active la lose
            playerWon = false;
            

            isUIDrawn = true;
            yield return new WaitForSeconds(timeBeforeLose);

            bgClockStarted = true;
            squishClockStarted = true;
            loseUIGameObject.SetActive(true);
        }

    }

    public IEnumerator WinUI()
    {
        //interface de win activée
        if (!isUIDrawn)
        {

            //on active la win
            playerWon = true;

            yield return new WaitForSeconds(waitBeforeParticle);
            Yippee();

            StartCoroutine(CheckStar());

            isUIDrawn = true;
            yield return new WaitForSeconds(timeBeforeWin - waitBeforeParticle);

            bgClockStarted = true;
            squishClockStarted = true;

            loseUIGameObject.SetActive(false);
            winUIGameObject.SetActive(true);
        }
        
    }

    /*
    void CheckStar()
    {
        starsNumber =0;
        if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[2].Moves)
        {
            yield return new WaitForSeconds(waitBetweenStars + squishTime);          
            Win1Star.sprite = obtainedStarImage;
            starsNumber=1;
            
            if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[1].Moves)
            {
               
                yield return new WaitForSeconds(waitBetweenStars);
                Win2Star.sprite = obtainedStarImage;
                starsNumber=2;
                if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[0].Moves)
                {
                    
                    yield return new WaitForSeconds(waitBetweenStars);
                    Win3Star.sprite = obtainedStarImage;
                    starsNumber=3;
                }
            }
        }
        yield return new WaitForSeconds(0);
        finalNumberOfMoves = 0;
    }
    */
    //une fois le système de save de stars en place, remplacer CheckStar par CheckStar2
    
    void Yippee()
    {
        confetti1.Play();
    }




    public IEnumerator CheckStar()
    {
        starsNumber =0;
        if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[2].Moves)
        {
            yield return new WaitForSeconds(waitBetweenStars + squishTime + timeBeforeWin);          
            Win1Star.sprite = obtainedStarImage;
            starsNumber=1;
            
            if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[1].Moves)
            {
               
                yield return new WaitForSeconds(waitBetweenStars);
                Win2Star.sprite = obtainedStarImage;
                starsNumber=2;
                if (finalNumberOfMoves <= levelLoader.currentLevel.nbMovesToGainStars[0].Moves)
                {
                    
                    yield return new WaitForSeconds(waitBetweenStars);
                    Win3Star.sprite = obtainedStarImage;
                    starsNumber=3;
                }
            }
        }
        yield return new WaitForSeconds(0);
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
        squishClock = 0;
        squishClockStarted = false;

        //on minimize les UIs
        winSquishable.transform.localScale = new Vector3(0.001f, 0.001f, 1);
        loseSquishable.transform.localScale = new Vector3(0.001f, 0.001f, 1);

        playerWon = false;
        isUIDrawn = false;
        
        
    }

    public void UpdateFinalMoveNumber(int numberOfMoves)
    {
        finalNumberOfMoves = numberOfMoves;
        int moves = 6 - numberOfMoves;
        levelLoader.movesLeft.text = moves.ToString();

        if (numberOfMoves > levelLoader.currentLevel.nbMovesToGainStars[0].Moves)
        {
            inGameStar3.sprite = lostStarImage;
            if (numberOfMoves > levelLoader.currentLevel.nbMovesToGainStars[1].Moves)
            {
                inGameStar2.sprite = lostStarImage;
                if (numberOfMoves > levelLoader.currentLevel.nbMovesToGainStars[2].Moves)
                {
                    inGameStar1.sprite = lostStarImage;
                    
                
                }
            }
        }
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
                fadeLerpFactor = bgCurve.Evaluate(bgClock / bgFadeTime);

                //Debug.Log(bgClock);
            }
            else
            {
                bgClockStarted = false;
            }

            if (playerWon)
            {
                //transition du winbg
                //calcul : alpha voulu = alpha max * pourcentage (pourcentage = temps actuel / temps max)


                //winBackground.color = new Color(winBackground.color.r, winBackground.color.g, winBackground.color.b, winBgAlpha * bgClock / bgFadeTime / 255);
                //Debug.Log(winBgAlpha * bgClock / bgFadeTime);

                
                winBackground.color = new Color(winBackground.color.r, winBackground.color.g, winBackground.color.b, winBgAlpha * fadeLerpFactor / 255);


            }
            else
            {
                //transition du losebg
                //calcul : alpha voulu = alpha max * pourcentage (pourcentage = temps actuel / temps max)

                //loseBackground.color = new Color(loseBackground.color.r, loseBackground.color.g, loseBackground.color.b, loseBgAlpha * bgClock / bgFadeTime / 255);
                //Debug.Log(loseBgAlpha * bgClock / bgFadeTime);

                loseBackground.color = new Color(loseBackground.color.r, loseBackground.color.g, loseBackground.color.b, loseBgAlpha * fadeLerpFactor / 255);
            }
        }

        if (squishClockStarted)
        {
            if (squishClock < squishTime)
            {
                squishClock += Time.deltaTime;
                verticalSquishLerpFactor = verticalSquish.Evaluate(squishClock / squishTime);
                horizontalSquishLerpFactor = horizontalSquish.Evaluate(squishClock / squishTime);
                //Debug.Log(bgClock);
            }
            else
            {
                squishClockStarted = false;
            }

            if (playerWon)
            {
                winSquishable.transform.localScale = new Vector3(horizontalSquishLerpFactor, verticalSquishLerpFactor, 1);
            }
            else
            {
                loseSquishable.transform.localScale = new Vector3(horizontalSquishLerpFactor, verticalSquishLerpFactor, 1);
            }

        }

        if (starClockStarted)
        {
            if (starClock < waitBetweenStars)
            {
                starClock += Time.deltaTime;
            }
            else
            {
                starClockStarted = false;
            }


        }
    }

}
