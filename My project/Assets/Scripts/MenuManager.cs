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

    //étoiles et textes

    public Image star1;
    public TMP_Text textstar1;

    public Image star2;
    public TMP_Text textstar2;

    public Image star3;
    public TMP_Text textstar3;

    public Sprite obtainedStarImage;
    public Sprite emptyStarImage;

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
            SetMoveNumbers();
            CheckStar();

            isUIDrawn = true;
            loseUIGameObject.SetActive(false);
            winUIGameObject.SetActive(true);
        }
        
    }


    void SetMoveNumbers()
    {
        //1 = plus petit nombre, 3 = plus grand
        score_pl1 = levelLoader.currentLevel.nbMovesToGainStars[0].Moves;
        score_pl2 = levelLoader.currentLevel.nbMovesToGainStars[1].Moves;
        score_pl3 = levelLoader.currentLevel.nbMovesToGainStars[2].Moves;

        //On assigne les bonnes valeurs
        textstar1.text = score_pl3.ToString();
        textstar2.text = score_pl3.ToString();
        textstar3.text = score_pl3.ToString();
    }

    
    void CheckStar()
    {

        //De base toutes les étoiles sont vides
        star1.sprite = emptyStarImage;
        star2.sprite = emptyStarImage;
        star3.sprite = emptyStarImage;

        //On remplit celles qu'on posséde
        if (finalNumberOfMoves <= score_pl1)
        {
            //3 stars
            star1.sprite = obtainedStarImage;
            star2.sprite = obtainedStarImage;
            star3.sprite = obtainedStarImage;
        }
        else if(finalNumberOfMoves <= score_pl2)
        {
            star1.sprite = obtainedStarImage;
            star2.sprite = obtainedStarImage;
            //2 stars
        }
        else if(finalNumberOfMoves <= score_pl3)
        {
            star1.sprite = obtainedStarImage;
            //1 star
        }
        else
        {
            //No stars ???
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
        numberOfMoves = finalNumberOfMoves;
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
