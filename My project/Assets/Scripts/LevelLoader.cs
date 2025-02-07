using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    
    public LevelContainer levelContainer;
    public LevellSO currentLevel;
    public TextMeshProUGUI movesLeft;
    public int movesDone=0;
    public Tilemap tilemap;
    public TextMeshProUGUI level;
    public List<TextMeshProUGUI> textstarsMoves;

    public GameObject tutoTextObject;
    public TextMeshProUGUI tutoText;

    public ConvoyeurFood convoyeurfood;
    
    public static int actionCount = 0 ;

    [Space(10)]
    public GameObject tutoHandObject;
    public AnimationCurve moveCurve;
    public float timeHand;
    public AnimationCurve vanishCurve;
    public float vanishTime;
    public float breakTime;
    [Space(5)]
    public Transform startPos;
    public Transform endPos;

    bool hasTutoClockStarted = false;
    float tutoHandClock;
    float lerpFactor;

    bool hasVanishClockStarted = false;
    bool isFadeOut = true;
    Image handImage;

    bool hasBreakClockStarted = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        LoadLevel();
        movesLeft.text = 6.ToString();
        
        level.text = levelContainer.selectedLevel.name.Remove(0, 3);
        for (int i = 0; i < textstarsMoves.Count; i++)
        {
            textstarsMoves[i].text = levelContainer.selectedLevel.nbMovesToGainStars[i].Moves.ToString();
        }

        handImage = tutoHandObject.GetComponent<Image>();
    }


    public void LoadLevel()
    {
        foreach (Transform child in tilemap.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        currentLevel = levelContainer.selectedLevel;
        
        if (currentLevel == null || tilemap == null)
            return;
        foreach (var obj in currentLevel.objectsToSpawn)
        {
            if (obj.prefab != null)
            {
                Vector3 position = tilemap.GetCellCenterWorld(obj.tilePosition);
                Instantiate(obj.prefab, position, Quaternion.identity);
            }
        }
        foreach (var slime in currentLevel.slimeToSpawn)
        {
            if (slime.slimePrefab != null)
            {
                GameObject slimeNew = Instantiate(slime.slimePrefab, slime.slimePosition, Quaternion.identity,tilemap.transform);
                slimeNew.GetComponent<SlimeController>().currentState = slime.defaultState;
            }
        }

        convoyeurfood.foodList = currentLevel.foodToSpawn.foodOrder;
        actionCount = 0;

        SetTutoTextActive(currentLevel.hasTutoText);
        if (currentLevel.hasTutoText)
        {
            UpdateTutoText(currentLevel.tutoTextToWrite);
        }

        SetTutoHandActive(currentLevel.hasTutoFinger);
        if (currentLevel.hasTutoFinger)
        {
            tutoHandObject.transform.position = startPos.position;
            tutoHandClock = 0;

            //On commence par faire apparaitre la main avec Vanish Clock + fade out en false (et on reset les autres clocks)
            hasBreakClockStarted = false;
            hasTutoClockStarted = false;

            isFadeOut = false;
            hasVanishClockStarted = true;
            
        }
        else
        {
            //On arrête toutes les clocks
            hasTutoClockStarted = false;
            hasBreakClockStarted = false;
            hasVanishClockStarted = false;
        }

    }
    
    public void SetTutoTextActive(bool isTextActivated)
    {
        if (tutoTextObject != null)
        {
            tutoTextObject.SetActive(isTextActivated);
        }
    }


    void UpdateTutoText(string textToWrite)
    {
        tutoText.text = textToWrite;
    }

    public void SetTutoHandActive(bool isTextActivated)
    {
        if (tutoTextObject != null)
        {
            tutoHandObject.SetActive(isTextActivated);
        }
    }


    //tuto hand clock
    private void Update()
    {
        if (hasTutoClockStarted)
        {

            //La main se déplace entre 2 points : la vitesse dépend d'une animationCurve.
            float lerpFactor = moveCurve.Evaluate(tutoHandClock / timeHand);
            tutoHandObject.transform.position = Vector3.Lerp(startPos.position, endPos.position, lerpFactor);

            if (tutoHandClock < timeHand)
            {
                tutoHandClock += Time.deltaTime;

                //Debug.Log(tutoHandClock);
            }
            else
            {
                tutoHandClock = 0;
                hasTutoClockStarted = false;
                hasVanishClockStarted = true;
            }

            

            
            //transition du winbg
            //calcul : alpha voulu = alpha max * pourcentage (pourcentage = temps actuel / temps max)
            //winBackground.color = new Color(winBackground.color.r, winBackground.color.g, winBackground.color.b, winBgAlpha * bgClock / bgFadeTime / 255);
            //Debug.Log(winBgAlpha * bgClock / bgFadeTime);

        }

        if (hasVanishClockStarted)
        {
            

            if (tutoHandClock < vanishTime)
            {
                tutoHandClock += Time.deltaTime;

                //Debug.Log(tutoHandClock);

                if (isFadeOut)
                {
                    //variable entre 0 et 1 : f(animCurve)
                    float lerpFactor = vanishCurve.Evaluate(tutoHandClock / vanishTime);
                    handImage.color = new Color(1, 1, 1, 1 - lerpFactor);
                }
                else
                {
                    //fade in : même chose mais on inverse la direction
                    float lerpFactor = vanishCurve.Evaluate(tutoHandClock / vanishTime);
                    handImage.color = new Color(1, 1, 1, lerpFactor);
                }

            }
            else
            {
                hasVanishClockStarted = false;
                if (isFadeOut)
                {
                    //après fade out, reset et fade in
                    tutoHandClock = 0;
                    tutoHandObject.transform.position = startPos.position;
                    isFadeOut = false;
                    hasBreakClockStarted = true;
                }
                else
                {
                    //après fade in = on bouge + fade out
                    tutoHandClock = 0;
                    hasTutoClockStarted = true;
                    isFadeOut = true;

                }
            }

            
        }

        if (hasBreakClockStarted)
        {
            if (tutoHandClock < breakTime)
            {
                tutoHandClock += Time.deltaTime;

                //Debug.Log(tutoHandClock);
            }
            else
            {
                //on reprend le fade in 
                tutoHandClock = 0;
                hasBreakClockStarted = false;
                hasVanishClockStarted = true;
            }
        }
    }

    public void SavingStarsData()
    {
        //tout ça ça marche pas donc tant pis on va faire sans save
        
        string levelName = currentLevel.name; 
        string levelString = levelName.Substring(3); 

        int levelNumber;
        if (int.TryParse(levelString, out levelNumber))
        {
            levelNumber -= 1;
            
            //GetComponent<SavingData>().levelData.stars[levelNumber] = MenuManager.instance.starsNumber;
            
           // Debug.Log(levelNumber + " = " +  GetComponent<SavingData>().levelData.stars[levelNumber]);
           // GetComponent<SavingData>().SaveGameData();
        }
        /*else
        {
            Debug.LogError("Save failed");
        }*/


       
        if (PlayerPrefs.GetInt(levelNumber.ToString()) < MenuManager.instance.starsNumber)
        {
            PlayerPrefs.SetInt(levelNumber.ToString(), MenuManager.instance.starsNumber);
        }
        else
        {
            Debug.Log("pas d'amélioration");
        }
        
        
        
        
    }

}
