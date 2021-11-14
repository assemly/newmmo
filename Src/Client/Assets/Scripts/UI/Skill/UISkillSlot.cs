using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour,IPointerClickHandler
{
    public Image icon;
    public Image overlay;
    public Text cdText;
    SkillDefine skill;

    float overlaySpeed = 0;
    float cdRemian = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (overlay.fillAmount > 0)
        {
            overlay.fillAmount = this.cdRemian / this.skill.CD;
            this.cdText.text = ((int)Math.Ceiling(this.cdRemian)).ToString();
            this.cdRemian -= Time.deltaTime;
        }
        else
        {
            if (overlay.enabled) overlay.enabled = false;
            if (this.cdText.enabled) this.cdText.enabled = false;
        }
    }

    public void SetSkill(SkillDefine value)
    {
        this.skill = value;
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.skill.Icon);
        this.SetCD(this.skill.CD);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.overlay.fillAmount > 0)
        {
            MessageBox.Show("技能:" + this.skill.Name + "正在冷却");
        }
        else
        {
            MessageBox.Show("释放技能:" + this.skill.Name );
            this.SetCD(this.skill.CD);
        }
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
