using Candlelight.UI;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChat : MonoBehaviour
{
    public HyperText textArea;
    public TabView channelTab;

    public InputField chatText;
    public Text chatTarget;

    public Dropdown channelSelect;
    // Start is called before the first frame update
    void Start()
    {
        this.channelTab.OnTabSelect += OnDisplayChannelSelected;
        this.channelSelect.onValueChanged.AddListener(OnSendChannelChanged);
        ChatManager.Instance.OnChat += RefreshUI;
    }

    private void OnDestroy()
    {
        this.channelTab.OnTabSelect -= OnDisplayChannelSelected;
        ChatManager.Instance.OnChat -= RefreshUI;
    }

    // Update is called once per frame
    void Update()
    {
        if(chatText!=null)
            InputManager.Instance.IsInputMode = chatText.isFocused;
    }

    private void OnDisplayChannelSelected(int idx)
    {
        ChatManager.Instance.displayChannel = (ChatManager.LocalChannel)idx;
        RefreshUI();
    }
    private void RefreshUI()
    {
        Debug.Log("UIChat RefreshUI");
       
        this.textArea.text = ChatManager.Instance.GetCurrentMessages();
        this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        if(ChatManager.Instance.SendChannel == SkillBridge.Message.ChatChannel.Private)
        {
            this.chatTarget.gameObject.SetActive(true);
            if(ChatManager.Instance.PrivateID != 0)
            {
                this.chatTarget.text = ChatManager.Instance.PrivateName + ":";
            }
            else
            {
                this.chatText.text = "<无>:";
            }
        }
        else
        {
            this.chatTarget.gameObject.SetActive(false);
        }
    }

    public void OnClickChatLink(HyperText text,HyperText.LinkInfo link)
    {
        if (string.IsNullOrEmpty(link.Name)) return;
        //<a name="c:1001:Name" class="player">Name</a>
        //<a name="i:1001:Name" class="item">Name</a>
        if (link.Name.StartsWith("c:"))
        {
            string[] strs = link.Name.Split(":".ToCharArray());
            UIPopCharMenu menu = UIManager.Instance.Show<UIPopCharMenu>();
            menu.targetId = int.Parse(strs[1]);
            menu.targetName = strs[2];
        }
    }

    public void OnClickSend()
    {
        OnEndInput(this.chatText.text);
    }

    public void OnEndInput(string text)
    {
        if (!string.IsNullOrEmpty(text.Trim()))
            this.SendChat(text);

        this.chatText.text = "";
    }

    private void SendChat(string content)
    {
        ChatManager.Instance.SendChat(content, ChatManager.Instance.PrivateID, ChatManager.Instance.PrivateName);
    }

    public void OnSendChannelChanged(int idx)
    {
        Debug.LogFormat("OnSendChannelChanged: {0}", idx);
        if (ChatManager.Instance.sendChannel == (ChatManager.LocalChannel)(idx + 1))
            return;
        if (!ChatManager.Instance.SetSendChannel((ChatManager.LocalChannel)(idx+1)))
        {
            this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        }
        else
        {
            this.RefreshUI();
        }
    }
}
