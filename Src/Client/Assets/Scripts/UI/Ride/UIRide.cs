using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindow
{
    public Text descript;
    public GameObject itemPreafab;
    public ListView listMain;
    public UIRideItem selecetedItem;

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
        foreach(var kv in ItemManager.Instance.Items)
        {
            if(kv.Value.Define.Type == ItemType.Ride && (kv.Value.Define.LimitClass == CharacterClass.None || kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class))
            {
                GameObject go = Instantiate(itemPreafab, this.listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetEquipItem(kv.Key, kv.Value, this);
                this.listMain.AddItem(ui);
            }
        }
    }

    public void OnItemSelected(ListView.ListViewItem item)
    {
        this.selecetedItem = item as UIRideItem;
        this.descript.text = this.selecetedItem.item.Define.Description;
    }

    public void DoRide()
    {
        if(this.selecetedItem == null)
        {
            MessageBox.Show("请选择要召回的坐骑", "提示");
            return;
        }
        User.Instance.Ride(this.selecetedItem.item.Id);
    }
}
