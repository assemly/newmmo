﻿using Entities;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
    public GameObject nameBarPrefab;
    public GameObject npcStatusPrefab;


    private Dictionary<Transform, GameObject> elementNames = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> elementStatus = new Dictionary<Transform, GameObject>();
    // Start is called before the first frame update
    protected  override void OnStart()
    {
        if (nameBarPrefab != null)
            nameBarPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCharacterNameBar(Transform owner, Entities.Creature character)
    {
        GameObject goNameBar = Instantiate(nameBarPrefab, this.transform);
        goNameBar.name = "NameBar" + character.entityId;
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        this.elementNames[owner] = goNameBar;
    }

    public void RemoveCharacterNameBar(Transform owner)
    {
        if (this.elementNames.ContainsKey(owner))
        {
            Destroy(this.elementNames[owner]);
            this.elementNames.Remove(owner);
        }
    }

    public void Clear()
    {
        
        foreach (var ele in elementNames)
        {
            this.RemoveCharacterNameBar(ele.Key);
        }

        //foreach(var ele in elementStatus)
       
    }

    public void AddNpcQuestStatus(Transform owner,NpcQuestStatus status)
    {
        if (this.elementStatus.ContainsKey(owner))
        {
            elementStatus[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
        }
        else
        {
            GameObject go = Instantiate(npcStatusPrefab, this.transform);
            go.name = "NpcQuestStatus" + owner.name;
            go.GetComponent<UIWorldElement>().owner = owner;
            go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
            go.SetActive(true);
            this.elementStatus[owner] = go;
        }
    }

    public void RemoveNpcQuestStatus(Transform owner)
    {
        if (this.elementStatus.ContainsKey(owner))
        {
            Destroy(this.elementStatus[owner]);
            this.elementStatus.Remove(owner);
        }
    }
}
