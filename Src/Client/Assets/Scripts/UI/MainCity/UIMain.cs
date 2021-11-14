using Managers;
using Models;
using Services;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    public Text avatarName;
    public Text avatarLevel;
    public UITeam TeamWindow;
    // Start is called before the first frame update
    public void OnCharacterLeaveMap()
    {
       
        //UIWorldElementManager.Instance.Clear();
        //CharacterManager.Instance.Clear();
        
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();

    }

    protected override void OnStart()
    {
        this.UpdateAvatar();
    }

    private void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacterInfo.Name, User.Instance.CurrentCharacterInfo.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacterInfo.Level.ToString();
    }

    public void OnClickTest()
    {
        UITest test =UIManager.Instance.Show<UITest>();
        test.SetTitle("Just for test");
        test.OnClose += Test_OnClose;
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("关闭测试窗口:" + result, "对话框相应结果", MessageBoxType.Information);
    }

    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }

    public void OnClickEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnClickQuest()
    {                             
        UIManager.Instance.Show<UIQuestSystem>();
    }

    public void OnClickFriends()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.Show(show);
    }

    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }

    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }

    public void OnClckRide()
    {
        UIManager.Instance.Show<UIRide>();
    }

    public void OnSkill()
    {
        UIManager.Instance.Show<UISkill>();
    }
}
