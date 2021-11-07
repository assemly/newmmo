using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildMemberItem : ListView.ListViewItem
{
    public Text Name;
    public Text Level;
    public Text @class;
    public Text Title;
    public Text JoinTime;
    public Text Status;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }
    
    public NGuildMemberInfo Info;

    

    public void SetGuildInfo(NGuildMemberInfo item)
    {
        this.Info = item;
        if (this.Name != null) this.Name.text = this.Info.Info.Name;
        if (this.Level != null) this.Level.text = this.Info.Info.Level.ToString(); ;
        if (this.@class != null) this.@class.text = this.Info.Info.Class.ToString();
        if (this.Title != null) this.Title.text = this.Info.Title.ToString();
        if (this.Status != null) this.Status.text = this.Info.Status == 1 ? "在线" : TimeUtil.GetTime(this.Info.lastTime).ToShortDateString();
        if (this.JoinTime != null) this.JoinTime.text = TimeUtil.GetTime(this.Info.joinTime).ToShortDateString();
    }
}
