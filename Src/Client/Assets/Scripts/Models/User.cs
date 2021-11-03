using Common.Data;
using SkillBridge.Message;


namespace Models
{
    class User : Singleton<User>
    {
        NUserInfo userInfo;
        public NUserInfo Info
        {
            get { return userInfo; }
        }
        public void SetupUserInfo(NUserInfo info)
        {
            this.userInfo = info;
        }
        public NCharacterInfo CurrentCharacter { get; set; }
        public PlayerInputController CurrentCharacterObject { get; set; }
        public NTeamInfo TeamInfo { get; set; }
        public MapDefine CurrentMapData { get; set; }
        internal void AddGold(int gold)
        {
            this.CurrentCharacter.Gold += gold;
        }
    }
}
