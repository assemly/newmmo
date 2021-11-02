using Managers;
using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoSingleton<UIMain>
{
    // Start is called before the first frame update
    public void OnCharacterLeaveMap()
    {
       
        //UIWorldElementManager.Instance.Clear();
        //CharacterManager.Instance.Clear();
        
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();

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
}
