﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabView : MonoBehaviour
{
    public TabButton[] tabButtons;
    public GameObject[] tabPages;

    public int index = -1;

    public Action<int> OnTabSelect;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        for(int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].tabView = this;
            tabButtons[i].tabIndex = i;
        }
        yield return new WaitForEndOfFrame();
        SelectTab(1);
    }

    public void SelectTab(int index)
    {
        if(this.index != index)
        {
            for(int i = 0; i < tabButtons.Length; i++)
            {
                tabButtons[i].Select(i == index);
                if (tabPages.Length <= 1) continue;
                if(tabPages[i]!=null)
                    tabPages[i].SetActive(i == index);
                
            }
        }
        if (OnTabSelect != null)
            OnTabSelect.Invoke(index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
