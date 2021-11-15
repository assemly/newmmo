using Battle;
using Common.Data;
using Managers;
using Models;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UISkillItem : ListView.ListViewItem
{
    public Image icon;
    public Text title;
    public Text level;
   

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;
   
    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }


    public Skill item;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetEquipItem(Skill item, UISkill owner,bool equiped)
    {
        this.item = item;
        
        if (this.title != null) this.title.text = this.item.Define.Name;
        if (this.level != null) this.level.text = item.skillInfo.Level.ToString();
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }

}
