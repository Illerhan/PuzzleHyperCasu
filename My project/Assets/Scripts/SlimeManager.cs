using System;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{

    public static SlimeManager Instance;
    [SerializeField] public List<SlimeController> slimesList = new List<SlimeController>();
    private bool hasTriggeredWin = false;
    private void Awake()
    {
        
        if (Instance == null)
            Instance = this;
    }

    public void RegisterSlime(SlimeController slime)
    {
        slimesList.Add(slime);

    }
    
    public SlimeController[] GetSlimes()
    {
        return slimesList.ToArray();
    }
    public void UnregisterSlime(SlimeController slime)
    {
        if(slimesList.Contains(slime))
        {
            slimesList.Remove(slime);
        }
            
    }

    public void CheckSlimes()
    {
        if (hasTriggeredWin)
            return;
        foreach (var slime in slimesList)
        {
            if (slime.currentSize < slime.maxSize)
            {
                return;
            }
        }
        hasTriggeredWin = true;
        StartCoroutine(MenuManager.instance.WinUI());
        MenuManager.instance.levelLoader.SavingStarsData();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
