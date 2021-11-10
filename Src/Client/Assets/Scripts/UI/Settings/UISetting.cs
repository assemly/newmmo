using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UIWindow
{
    public void ReturnGame()
    {
        Close();
    }
    public void ExitToCharaSelect()
    {

        SceneManager.Instance.LoadScene("CharSelect");
        AudioManager.Instance.PlayMusic(SoundDefine.Music_Select);
        UserService.Instance.SendGameLeave();
    }

    public void ExitGame()
    {
        UserService.Instance.SendGameLeave(true);
    }

    public void AudioSetting()
    {
        UIManager.Instance.Show<UIAduioSetting>();
        this.Close();
    }
}
