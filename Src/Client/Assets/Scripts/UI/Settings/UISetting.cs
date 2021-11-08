using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UIWindow
{
    public void ReturnGame()
    {
        UIManager.Instance.Show<UIPopCharMenu>();
    }
    public void ExitToCharaSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();
    }

    public void ExitGame()
    {
        UserService.Instance.SendGameLeave(true);
    }
}
