using Common;
using Common.Data;
using GameServer.Entities;
using SkillBridge.Message;
using System.Collections.Generic;

namespace GameServer.Managers
{
    class ChatManager:Singleton<ChatManager>
    {
        public List<ChatMessage> System = new List<ChatMessage>();
        public List<ChatMessage> World = new List<ChatMessage>();
        public Dictionary<int, List<ChatMessage>> local = new Dictionary<int, List<ChatMessage>>();
        public Dictionary<int, List<ChatMessage>> Team = new Dictionary<int, List<ChatMessage>>();
        public Dictionary<int, List<ChatMessage>> Guild = new Dictionary<int, List<ChatMessage>>();

        public void Init()
        {

        }

        public void AddMessage(Character from,ChatMessage message)
        {
            message.FromId = from.Id;
            message.FromName = from.Name;
            message.Time = TimeUtil.timestamp;
            switch (message.Channel)
            {
                case ChatChannel.Local:
                    this.AddLocalMessage(from.Info.mapId, message);
                    break;
                case ChatChannel.World:
                    this.AddWorldMessage( message);
                    break;
                case ChatChannel.System:
                    this.AddSystemMessage(message);
                    break;
                case ChatChannel.Team:
                    this.AddTeamMessage(from.Team.Id,message);
                    break;
                case ChatChannel.Guild:
                    this.AddGuildMessage(from.Guild.Id, message);
                    break;
            }
        }

        
        private void AddLocalMessage(int mapId, ChatMessage message)
        {
            if(!this.local.TryGetValue(mapId,out List<ChatMessage> messages))
            {
                messages = new List<ChatMessage>();
                this.local[mapId] = messages;
            }
            messages.Add(message);
        }

        private void AddWorldMessage(ChatMessage message)
        {
            this.World.Add(message);
        }

        private void AddSystemMessage(ChatMessage message)
        {
            this.World.Add(message);
        }

        private void AddTeamMessage(int Teamid, ChatMessage message)
        {
            if (!this.Team.TryGetValue(Teamid, out List<ChatMessage> messages))
            {
                messages = new List<ChatMessage>();
                this.Team[Teamid] = messages;
            }
            messages.Add(message);
        }

        private void AddGuildMessage(int guildid, ChatMessage message)
        {
            if(!this.Guild.TryGetValue(guildid,out List<ChatMessage> messages))
            {
                messages = new List<ChatMessage>();
                this.Guild[guildid] = messages;
            }
            messages.Add(message);
        }

        public int GetLocalMessages(int mapId,int idx,List<ChatMessage> result)
        {
            if(!this.local.TryGetValue(mapId,out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetNewMessages(idx, result, messages);
        }  

        public int GetWorldMessages(int idx, List<ChatMessage> result)
        {
            return GetNewMessages(idx, result, this.World);
        }

        public int GetSystemMessages(int idx, List<ChatMessage> result)
        {
            return GetNewMessages(idx, result, this.System);
        }

        public int GetTeamMessages(int teamId, int idx, List<ChatMessage> result)
        {
            if (!this.Team.TryGetValue(teamId, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetNewMessages(idx, result, messages);
        }

        public int GetGuildMessages(int guildId, int idx, List<ChatMessage> result)
        {
            if (!this.Guild.TryGetValue(guildId, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetNewMessages(idx, result, messages);
        }

        private int GetNewMessages(int idx, List<ChatMessage> result, List<ChatMessage> messages)
        {
            if(idx == 0)
            {
                if(messages.Count > GameDefine.MaxChatRecoredNums)
                {
                    idx = messages.Count - GameDefine.MaxChatRecoredNums;
                }
                
            }
            for (; idx < messages.Count; idx++)
            {
                result.Add(messages[idx]);
            }
            return idx;
        }
    }
}
