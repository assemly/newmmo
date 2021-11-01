using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : ListView.ListViewItem
{
    public Text title;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public Quest quest;
    
    public override void onSelected(bool selected)
    {
        if(background!=null)
            this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

   public void SetQuestInfo(Quest item)
    {
        this.quest = item;
        if (this.title != null)
            this.title.text = string.Format("<color=yellow>[{0}]</color>{1}", this.quest.GetTypeName(), this.quest.Define.Name);
        
    }
}
