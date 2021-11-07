using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem
{
    public Text ID;
    public Text Name;
    public Text Leader;
    public Text createTime;
    //public Text joinTime;
    //public Text status;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }
    
    public NGuildInfo Info;

    

    public void SetGuildInfo(NGuildInfo item)
    {
        this.Info = item;
        if (this.ID != null) this.ID.text = this.Info.Id.ToString();
        if (this.Name != null) this.Name.text = this.Info.GuildName;
        if (this.Leader != null) this.Leader.text = this.Info.leaderName;
        if (this.createTime != null) this.createTime.text = TimeUtil.GetTime(this.Info.createTime).ToShortDateString();
    }
}
