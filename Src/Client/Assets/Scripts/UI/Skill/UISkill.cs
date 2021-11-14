using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkill : UIWindow
{
    public Text descript;
    public GameObject itemPreafab;
    public ListView listMain;
    private UISkillItem selecetedItem;

    void Start()
    {
        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSelected;
    }
    private void OnDestroy()
    {
        this.listMain.onItemSelected -= this.OnItemSelected;
    }

    private void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    private void ClearItems()
    {
        this.listMain.RemoveAll();
    }

    private void InitItems()
    {
        var Skills = DataManager.Instance.Skills[(int)User.Instance.CurrentCharacterInfo.Class];
        foreach(var kv in Skills)
        {
            if(kv.Value.Type == Common.Battle.SkillType.Skill)
            {
                GameObject go = Instantiate(itemPreafab, this.listMain.transform);
                UISkillItem ui = go.GetComponent<UISkillItem>();
                ui.SetEquipItem(kv.Value,this,false);
                this.listMain.AddItem(ui);
            }
        }
    }

    public void OnItemSelected(ListView.ListViewItem item)
    {
        this.selecetedItem = item as UISkillItem;
        this.descript.text = this.selecetedItem.item.Description;
    }

}
