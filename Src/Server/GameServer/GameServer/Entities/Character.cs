using Common;
using Common.Data;
using GameServer.Core;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    class Character : Creature, IPostResponser
    {

        public TCharacter Data;

        public ItemManager ItemManager;
        public StatusManager StatusManager;
        public QuestManager QuestManager;
        public FriendManager FriendManager;
        public Team Team;
        public double TeamUpdateTS;

        public Guild Guild;
        public double GuildUpdateTS;

        public Chat Chat;
       // public EquipSlot Slot;
        public Character(CharacterType type, TCharacter cha) :
            base(type,cha.TID,cha.Level,new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ), new Core.Vector3Int(100, 0, 0))
        {
            
            this.Id = cha.ID;
            this.Data = cha;
            //this.Info = new NCharacterInfo();
            //this.Info.EntityId = this.entityId;
            //this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.Name = cha.Name;
            //this.Info.Level = cha.Level;
            this.Info.Exp = cha.Exp;
            //this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Gold = cha.Gold;
            this.Info.Ride = 0;
            this.Info.Equips = cha.Equips;
            //this.Info.Entity = this.EntityData;
            //this.Define = DataManager.Instance.Characters[this.Info.ConfigId];
            
            this.ItemManager = new ItemManager(this);
            this.ItemManager.GetItemInofs(this.Info.Items);
            this.Info.Bag = new NBagInfo();
            this.Info.Bag.Unlocked = this.Data.Bag.Unlocked;
            this.Info.Bag.Items = this.Data.Bag.Items;
            this.QuestManager = new QuestManager(this);
            this.QuestManager.GetQuestInfos(this.Info.Quests);

            this.StatusManager = new StatusManager(this);
            this.FriendManager = new FriendManager(this);
            this.FriendManager.GetFriendInfos(this.Info.Friends);

            this.Guild = GuildManager.Instance.GetGuild(this.Data.GuildId);
            this.Chat = new Chat(this);

            this.Info.attrDynamic = new NAttributeDynamic();
            this.Info.attrDynamic.Hp = cha.HP;
            this.Info.attrDynamic.Mp = cha.MP;
        }

        public void AddExp(int exp)
        {
            this.Exp += exp;
            this.CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            long needExp = (long)Math.Pow(this.Level, 3) * 10 + this.Level * 40 + 50;
            if(this.Exp > needExp)
            {
                this.LevelUp();
            }
        }

        private void LevelUp()
        {
            this.Level += 1;
            Log.InfoFormat("Character[{0}:{1}] LevelUp:{2}", this.Id, this.Info.Name, this.Level);
            CheckLevelUp();
        }
        public long Exp
        {
            get { return this.Data.Exp; }
            private set
            {
                if (this.Data.Exp == value)
                    return;
                this.StatusManager.AddExpChange((int)(value - this.Data.Exp));
                this.Data.Exp = value;
                this.Info.Exp = value;
            }
        }

        public int Level
        {
            get { return this.Data.Level; }
            private set 
            {
                if (this.Data.Level == value)
                    return;
                this.StatusManager.AddLevelUp((int)(value - this.Data.Level));
                this.Data.Level = value;
                this.Info.Exp = value;
            }
        }

        public long Gold
        {
            get { return this.Data.Gold; }
            set
            {
                if (this.Data.Gold == value)
                    return;
                this.StatusManager.AddGoldChange((int)(value - this.Data.Gold));
                this.Data.Gold = value;
                this.Info.Gold = value;
            }
        }

        public int Ride 
        { 
            get { return this.Info.Ride; }
            set {
                if (this.Info.Ride == value)
                    return;
                this.Info.Ride = value;     
                }
        }
        public NCharacterInfo GetBasicInfo()
        {
            return new NCharacterInfo()
            {
                Id = this.Info.Id,
                Name = this.Info.Name,
                Class = this.Info.Class,
                Level = this.Info.Level
            };
        }
        public void PostProcess(NetMessageResponse message)
        {
            Log.InfoFormat("Character PostProcess > FriendManager :CharacteID:{0}:{1}", this.Id, this.Info.Name);
            this.FriendManager.PostProcess(message);
            
            if (this.Team != null)
            {
                Log.InfoFormat("PostProcess > Team :CharacteID:{0}:{1} {2}<{3}", this.Id, this.Info.Name,TeamUpdateTS,this.Team.timestamp);
                if(TeamUpdateTS < this.Team.timestamp)
                {
                    TeamUpdateTS = Team.timestamp;
                    this.Team.PostProcess(message);
                }
            }
            if (this.StatusManager.HasStatus)
            {
                this.StatusManager.PostProcess(message);
            }

            if(this.Guild != null)
            {
                Log.InfoFormat("PostProcess > Guild :CharacteID:{0}:{1} {2}<{3}", this.Id, this.Info.Name, GuildUpdateTS, this.Guild.timestamp);
                if(this.Info.Guild == null)
                {
                    this.Info.Guild = this.Guild.GuildInfo(this);
                    if (message.mapCharacterEnter != null)
                        GuildUpdateTS = Guild.timestamp;
                }
                if(GuildUpdateTS <this.Guild.timestamp && message.mapCharacterEnter == null)
                {
                    GuildUpdateTS = Guild.timestamp;
                    this.Guild.PostProcess(this, message);
                }
                
            }
            this.Chat.PostProcess(message);
        }

        public void Clear()
        {
            //this.FriendManager.UpdateFriendInfo(this.Info, 0);
            this.FriendManager.OfflineNotify();
        }

        //public void LeaveTeam()
        //{
        //    this.Team.LeaveTeamNotify();
        //}
    }
}