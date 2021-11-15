using Battle;
using Managers;
using SkillBridge.Message;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour,IPointerClickHandler
{
    public Image icon;
    public Image overlay;
    public Text cdText;
    Skill skill;

    float overlaySpeed = 0;
    float cdRemian = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!overlay.enabled) overlay.enabled = false;
        if (!cdText.enabled) cdText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.skill.CD>0)
        {
            if (!overlay.enabled) overlay.enabled = true;
            if (!cdText.enabled) cdText.enabled = true;
            overlay.fillAmount = this.skill.CD / this.skill.Define.CD;
            this.cdText.text = ((int)Math.Ceiling(this.skill.CD)).ToString();
            this.cdRemian -= Time.deltaTime;
        }
        else
        {
            if (overlay.enabled) overlay.enabled = false;
            if (this.cdText.enabled) this.cdText.enabled = false;
        }
    }

    public void SetSkill(Skill value)
    {
        this.skill = value;
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.skill.Define.Icon);
        this.SetCD(this.skill.Define.CD);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SkillResult result = this.skill.CanCast(BattleManager.Instance.CurrentTarget);
        switch (result)
        {
            case SkillResult.InvalidTarget:
                MessageBox.Show("技能:" + this.skill.Define.Name + "目标无效");
                return;
            case SkillResult.OutOfMP:
                MessageBox.Show("技能:" + this.skill.Define.Name + "MP 不足");
                return;
            case SkillResult.Cooldown:
                MessageBox.Show("技能:" + this.skill.Define.Name + "正在冷却");
                return;
        }
       
       // MessageBox.Show("释放技能:" + this.skill.Define.Name );
        //this.SetCD(this.skill.Define.CD);
        BattleManager.Instance.CastSkill(this.skill);
    }

    public void SetCD(float cd)
    {
        if (!overlay.enabled) overlay.enabled = true;
        if (!cdText.enabled) cdText.enabled = true;
        this.cdText.text = ((int)Math.Floor(this.cdRemian)).ToString();
        overlay.fillAmount = 1f;
        overlaySpeed = 1f / cd;
        cdRemian = cd;
    }
}
