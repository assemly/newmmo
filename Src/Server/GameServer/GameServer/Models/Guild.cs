﻿using Common;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Guild
    {
        public TGuild Data;
        public int Id { get { return this.Data.Id; } }

        //private Character Leader;
        public string Name { get { return this.Data.Name; } }
      //  public List<Character> Members = new List<Character>();
        public double timestamp;
        public Guild(TGuild guild)
        {
            this.Data = guild;
        }

        internal bool JoinApply(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if (oldApply != null)
            {
                return false;
            }
            var dbApply = DBService.Instance.Entities.GuildApplies.Create();
            dbApply.GuildId = apply.GuildId;
            dbApply.CharacterId = apply.characterId;
            dbApply.Name = apply.Name;
            dbApply.Class = apply.Class;
            dbApply.Level = apply.Level;
            dbApply.ApplyTime = DateTime.Now;

            DBService.Instance.Entities.GuildApplies.Add(dbApply);
            this.Data.Applies.Add(dbApply);

            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        public bool JoinAppove(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result == 0);
            if (oldApply == null)
                return false;
            oldApply.Result = (int)apply.Result;
            if (apply.Result == ApplyResult.Accept)
            {
                this.AddMember(apply.characterId, apply.Name, apply.Class, apply.Level, GuildTitle.None);
                TCharacter character = DBService.Instance.Entities.Characters.Where(u => u.ID == apply.characterId).FirstOrDefault();
                character.GuildId = apply.GuildId;
            }
                        
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        public void AddMember(int characterId, string name, int @class, int level, GuildTitle title)
        {
            
            DateTime now = DateTime.Now;
            TGuildMember dbMember = new TGuildMember()
            {
                CharacterId = characterId,
                Name = name,
                Class = @class,
                Level = level,
                Title = (int)title,
                JoinTime = now,
                LastTime = now
            };
            var character = CharacterManager.Instance.GetCharacter(characterId);
            if (character != null)
                character.Data.GuildId = this.Id;
            else
            {
                //DBService.Instance.Entities.Database.ExecuteSqlCommand("UPDATE Characters SET GuildId = @p0 WHERE CharacterId = @p1", this.Id, characterId);
                TCharacter dbchar = DBService.Instance.Entities.Characters.SingleOrDefault(c => c.ID == characterId);
                dbchar.GuildId = this.Id;
            }
            this.Data.Members.Add(dbMember);
            timestamp = TimeUtil.timestamp;
        }

        //逻辑有问题
        public void Leave(Character member)
        {
            //Log.InfoFormat("Leave Guild: {0}:{1}", member.Id, member.Info.Name);
            //this.Members.Remove(member);
            //if(member == this.Leader)
            //{
            //    if (this.Members.Count > 0)
            //        this.Leader = this.Members[0];
            //    else
            //        this.Leader = null;
            //}
            //member.Guild = null;
            timestamp = TimeUtil.timestamp;
        }

        public void PostProcess(Character from, NetMessageResponse message)
        {
            if(message.Guild == null)
            {
                message.Guild = new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.guildInfo = this.GuildInfo(from);
            }
        }


        internal NGuildInfo GuildInfo(Character from)
        {
            NGuildInfo info = new NGuildInfo()
            {
                Id = this.Id,
                GuildName = this.Name,
                Notice = this.Data.Notice,
                leaderId = this.Data.LeaderID,
                leaderName = this.Data.LeaderName,
                createTime = (long)TimeUtil.GetTimestamp(this.Data.CreateTime),
                memberCount = this.Data.Members.Count
            };

            if(from != null)
            {
                info.Members.AddRange(GetMemberInfos());
                if (from.Id == this.Data.LeaderID)
                    info.Applies.AddRange(GetApplyInfos());
            }
            return info;
        }

       
        private List<NGuildMemberInfo> GetMemberInfos()
        {
            List<NGuildMemberInfo> members = new List<NGuildMemberInfo>();
            foreach(var member in this.Data.Members)
            {
                var memberInfo = new NGuildMemberInfo()
                {
                    Id = member.Id,
                    characterId = member.CharacterId,
                    Title = (GuildTitle)member.Title,
                    joinTime = (long)TimeUtil.GetTimestamp(member.JoinTime),
                    lastTime = (long)TimeUtil.GetTimestamp(member.LastTime),
                };
                //TODO
                var character = CharacterManager.Instance.GetCharacter(member.CharacterId);
                if (character != null)
                {
                    memberInfo.Info = character.GetBasicInfo();
                    memberInfo.Status = 1;
                    member.Level = character.Data.Level;
                    member.Name = character.Data.Name;
                    member.LastTime = DateTime.Now;
                    //if (member.Id == this.Data.LeaderID)
                    //    this.Leader = character;
                }
                else
                {
                    memberInfo.Info = this.GetMemberInfo(member);
                    memberInfo.Status = 0;
                    //if (member.Id == this.Data.LeaderID)
                    //    this.Leader = null;
                }
                members.Add(memberInfo);
            }
            return members;
        }

        private NCharacterInfo GetMemberInfo(TGuildMember member)
        {
            return new NCharacterInfo()
            {
                Id = member.CharacterId,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level,
            };
        }

        private List<NGuildApplyInfo> GetApplyInfos()
        {
            List<NGuildApplyInfo> applies = new List<NGuildApplyInfo>();
            foreach(var apply in this.Data.Applies)
            {
                if (apply.Result != (int)ApplyResult.None) continue;
                applies.Add(new NGuildApplyInfo()
                {
                    characterId = apply.CharacterId,
                    GuildId = apply.GuildId,
                    Class = apply.Class,
                    Level = apply.Level,
                    Name = apply.Name,
                    Result = (ApplyResult)apply.Result
                });
            }
            return applies;
        }

        TGuildMember GetDBMember(int characterId)
        {
            foreach(var member in this.Data.Members)
            {
                if (member.CharacterId == characterId)
                    return member;
            }
            return null;
        }
        public void ExecuteAdmin(GuildAdminCommand command, int targetId, int sourceid)
        {
            var target = GetDBMember(targetId);
            var source = GetDBMember(sourceid);
            switch (command)
            {
                case GuildAdminCommand.Promote:
                    target.Title = (int)GuildTitle.VicePresident;
                    break;
                case GuildAdminCommand.Depost:
                    target.Title = (int)GuildTitle.None;
                    break;
                case GuildAdminCommand.Transfer:
                    target.Title = (int)GuildTitle.President;
                    source.Title = (int)GuildTitle.None;
                    this.Data.LeaderID = targetId;
                    this.Data.LeaderName = target.Name;
                    break;
                case GuildAdminCommand.Kickout:
                    
                    break;
            }
            DBService.Instance.Save();
            timestamp = TimeUtil.timestamp;
         }
    }
}
