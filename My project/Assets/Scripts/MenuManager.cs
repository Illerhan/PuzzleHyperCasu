using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Onback()
    {
        SceneManager.LoadScene("LevelMap");
    }
}
