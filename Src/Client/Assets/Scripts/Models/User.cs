using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }

        public PlayerInputController CurrentCharacterObject { get; set; }

        public MapDefine CurrentMapData { get; set; }

        internal void AddGold(int gold)
        {
            this.CurrentCharacter.Gold += gold;
        }
    }
}
