using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlots : MonoBehaviour
{
    public UISkillSlot[] slots;
    void Start()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        var Skills = DataManager.Instance.Skills[(int)User.Instance.CurrentCharacterInfo.Class];
        int skillIdx = 0;
        foreach(var kv in Skills)
        {
            slots[skillIdx].SetSkill(kv.Value);
            skillIdx++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
