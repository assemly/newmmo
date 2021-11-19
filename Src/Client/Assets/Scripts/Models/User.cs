using Common.Data;
using Entities;
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
        public Entities.Character CurrentCharacter { get; set; }
        public NCharacterInfo CurrentCharacterInfo { get; set; }
        public PlayerInputController CurrentCharacterObject { get; set; }
        public NTeamInfo TeamInfo { get; set; }
        public MapDefine CurrentMapData { get; set; }
        internal void AddGold(int gold)
        {
            this.CurrentCharacterInfo.Gold += gold;
        }

        public int CurrentRide = 0;
        public void Ride(int id)
        {
            if (CurrentRide != id)
            {
                CurrentRide = id;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, CurrentRide);
            }
            else
            {
                CurrentRide = 0;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, 0);
            }
        }

        public delegate void CharacterInitHandle();
        public event CharacterInitHandle OnCharacterInit;

        public void CharacterInited()
        {
            if (OnCharacterInit != null)
                OnCharacterInit();
        }
    }
}
