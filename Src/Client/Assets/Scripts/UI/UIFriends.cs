using Managers;
using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIFriends : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIFriendItem selectedItem;


    // Start is called before the first frame update
    void Start()
    {
        FriendService.Instance.OnFriendUpdate = RefreshUI;
        this.listMain.onItemSelected += this.OnFriendSelected;
        RefreshUI();
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFriendSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIFriendItem;

    }

    public void OnClickFriendAdd()
    {
        InputBox.Show("请输入要添加的好友名称或ID", "添加好友").OnSubmit += OnFirendAddSubmit;
    }

    private bool OnFirendAddSubmit(string inputText, out string tips)
    {
        tips = "";
        int friendId = 0;
        string friendName = "";
        if (!int.TryParse(inputText, out friendId))
            friendName = inputText;
        if(friendId == User.Instance.CurrentCharacterInfo.Id || friendName == User.Instance.CurrentCharacterInfo.Name)
        {
            tips = "不能添加自己哦";
            return false;
        }
        FriendService.Instance.SendFriendAddRequest(friendId, friendName);
        return true;
    }

    public void OnClickFriendChat()
    {
        MessageBox.Show("暂未开放");
    }

    public void OnClickFriendTeamInvite()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要邀请的好友");
            return;
        }
        if(selectedItem.Info.Status == 0)
        {
            MessageBox.Show("请选择在线好友");
            return;
        }
        MessageBox.Show(string.Format("确认要邀请好友【{0}】加入队伍吗？",selectedItem.Info.friendInfo.Name),"邀请好友组队",MessageBoxType.Confirm,"邀请","取消").OnYes = ()=>{
            TeamService.Instance.SendTeamInviteRequest(this.selectedItem.Info.friendInfo.Id, this.selectedItem.Info.friendInfo.Name);
        };
    }
    public void OnClickFriendRemove()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要删除的好友");
            return;
        }
        MessageBox.Show(string.Format("确定要删除好友【{0}】吗？", selectedItem.Info.friendInfo.Name), "删除好友", MessageBoxType.Confirm, "删除", "取消").OnYes = () =>
        {
            FriendService.Instance.SendFriendRemoveRequest(this.selectedItem.Info.Id, this.selectedItem.Info.friendInfo.Id);
        };
        
    }
    void RefreshUI()
    {
        ClearFriendList();
        InitFriendItems();
    }

    private void InitFriendItems()
    {
        if (this.listMain == null)
            return;
        foreach(var item in FriendManager.Instance.allFriends)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIFriendItem ui = go.GetComponent<UIFriendItem>();
            ui.SetFriendInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    void ClearFriendList()
    {
        if(this.listMain!=null)
            this.listMain.RemoveAll();
    }
}
