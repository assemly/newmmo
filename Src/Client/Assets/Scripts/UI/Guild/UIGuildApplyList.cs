using Managers;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuildApplyList : UIWindow
{
    public GameObject ItemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    // Start is called before the first frame update
    void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateList;
        GuildService.Instance.SendGuildListRequest();
        this.UpdateList();
    }
    public void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateList;
    }
    private void UpdateList()
    {
        ClearList();
        InitItems();
    }

    private void ClearList()
    {
        this.listMain.RemoveAll();
    }

    private void InitItems()
    {
        foreach(var item in GuildManager.Instance.guildInfo.Applies)
        {
            GameObject go = Instantiate(ItemPrefab, this.listMain.transform);
            UIGuildApplyItem ui = go.GetComponent<UIGuildApplyItem>();
            ui.SetItemInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    
}
