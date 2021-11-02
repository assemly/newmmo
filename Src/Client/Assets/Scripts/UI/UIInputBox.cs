﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInputBox : MonoBehaviour
{
    public Text title;
    public Text message;
    public Text tips;
    public Button buttonYes;
    public Button buttonNo;
    public InputField input;

    public Text buttonYesTitle;
    public Text buttonNoTitle;

    public delegate bool SubmitHandler(string inputText,out string tips);
    public event SubmitHandler OnSubmit;
    public UnityAction OnCancel;

    public string emptyTips;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string title,string message,string btnOk = "",string btnCancel = "",string emptyTips = "")
    {
        if (!string.IsNullOrEmpty(title)) this.title.text = title;
        this.message.text = message;
        this.tips.text = null;
        this.OnSubmit = null;
        this.emptyTips = emptyTips;

        if (!string.IsNullOrEmpty(btnOk)) this.buttonYesTitle.text = btnOk;
        if (!string.IsNullOrEmpty(btnCancel)) this.buttonNoTitle.text = btnCancel;

        this.buttonYes.onClick.AddListener(OnClickYes);
        this.buttonNo.onClick.AddListener(OnClickNo);
    }

    private void OnClickYes()
    {
        this.tips.text = "";
        if(string.IsNullOrEmpty(input.text))
        {
            this.tips.text = this.emptyTips;
            return;
        }
        if (OnSubmit != null)
        {
            string tips;
            if (!OnSubmit(this.input.text, out tips))
            {
                this.tips.text = tips;
                return;
            }
        }
        Destroy(this.gameObject);
    }

    private void OnClickNo()
    {
        Destroy(this.gameObject);
        if (this.OnCancel != null)
            this.OnCancel();
    }
}
