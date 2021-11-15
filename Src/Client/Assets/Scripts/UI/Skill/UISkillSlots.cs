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
        var Skills = User.Instance.CurrentCharacter.SkillMgr.Skills;//DataManager.Instance.Skills[(int)User.Instance.CurrentCharacterInfo.Class];
        int skillIdx = 0;
        foreach(var skill in Skills)
        {
            slots[skillIdx].SetSkill(skill);
            skillIdx++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
