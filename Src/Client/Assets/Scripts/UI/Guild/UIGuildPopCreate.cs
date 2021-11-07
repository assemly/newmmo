using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildPopCreate : UIWindow
{
    public InputField guildName;
    public InputField guildNotice;

    // Start is called before the first frame update
    void Start()
    {
        GuildService.Instance.OnGuildCreateResult = OnGuildCreated;
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildCreateResult = null;
    }

    public override void OnYesClick()
    {
        if (string.IsNullOrEmpty(guildName.text))
        {
            MessageBox.Show("请输入公会名称", "错误", MessageBoxType.Error);
            return;
        }
        if(guildName.text.Length <4 || guildName.text.Length > 10)
        {
            MessageBox.Show("公会名称为4-10个字符", "错误", MessageBoxType.Error);
            return;
        }
        if (string.IsNullOrEmpty(guildNotice.text))
        {
            MessageBox.Show("请输入公会宣言", "错误", MessageBoxType.Error);
            return;
        }

        if (guildNotice.text.Length < 3 || guildNotice.text.Length > 50)
        {
            MessageBox.Show("公会宣言需要3-50个字符", "错误", MessageBoxType.Error);
            return;
        }

        GuildService.Instance.SendGuildCreate(guildName.text, guildNotice.text);
    }
    private void OnGuildCreated(bool result)
    {
        if (result)
            this.Close(WindowResult.Yes);
    }

}
