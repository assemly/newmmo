using Managers;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIGuild:UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;
    public GameObject panelLeader;

    private void OnEnable()
    {
        GuildService.Instance.OnGuildUpdate = UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }
    private void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }
    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

    void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();

        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }
    private void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
  
    }
    

    void UpdateGuildList(List<NGuildInfo> guilds)
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
         foreach(var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    public void OnClickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }

    public void OnClickLeave()
    {
        GuildService.Instance.SendGuildLeaveRequest();
    }

    public void OnClickChat()
    {

    }

    public void OnClickKickOut()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员");
            return;
        }
        MessageBox.Show(string.Format("要踢【{0}】出公会吗？", this.selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要晋升的成员");
            return;
        }
        if (selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方已经是高管");
            return;
        }
        MessageBox.Show(string.Format("要晋升为【{0}】公会副会长？", this.selectedItem.Info.Info.Name), "晋升", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要降职的成员");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方已经普通成员");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("无权操作");
            return;
        }
        MessageBox.Show(string.Format("要降职为【{0}】普通成员？", this.selectedItem.Info.Info.Name), "降职", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickTransfer()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要转让的成员");
            return;
        }
        
        MessageBox.Show(string.Format("要把会长转给【{0}】吗？", this.selectedItem.Info.Info.Name), "转移会长", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickSetNotice()
    {

    }
}

