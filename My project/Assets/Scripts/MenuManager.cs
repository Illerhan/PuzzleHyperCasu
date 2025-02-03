using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public LevelLoader levelLoader;
    

    public void Onback()
    {
        SceneManager.LoadScene("LevelMap");
    }

    public void Retry()
    {
        levelLoader.LoadLevel();
    }
}
