using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{

    public static SlimeManager Instance;

    [SerializeField] private List<SlimeController> slimesList = new List<SlimeController>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void RegisterSlime(SlimeController slime)
    {
        slimesList.Add(slime);

    }
    public void UnregisterSlime(SlimeController slime)
    {
        if(slimesList.Contains(slime))
        {
            slimesList.Remove(slime);
        }
            
    }

    public bool CheckSlimes()
    {
        int i = 0;
        foreach (var slime in slimesList)
        {
            if (slime.currentSize == slime.maxSize)
            {
                i++;
            }
        }

        if (i == slimesList.Count)
        {
            MenuManager.instance.WinUI();
            return true;
        }
        else
        {
            return false;
        }
            
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
