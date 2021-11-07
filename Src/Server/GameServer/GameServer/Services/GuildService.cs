using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class GuildService:Singleton<GuildService>
    {
        public GuildService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildCreateRequest>(this.OnGuildCreate);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildListRequest>(this.OnGuildList);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildLeaveRequest>(this.OnGuildLeave);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildAdminRequest>(this.OnGuildAdmin);
        }

    
        public void Init()
        {
            GuildManager.Instance.Init();
        }
        private void OnGuildCreate(NetConnection<NetSession> sender, GuildCreateRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildCreate::GuildName: {0} character:[{1}]{2}", request.GuildName, character.Id, character.Name);
            sender.Session.Response.guildCreate = new GuildCreateResponse();
            if(character.Guild != null)
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Erromsg = "已有公会";
                sender.SendResponse();
                return;
            }
            if (GuildManager.Instance.CheackNameExisted(request.GuildName))
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Erromsg = "公会名已存在";
                sender.SendResponse();
                return;
            }
            GuildManager.Instance.CreateGuild(request.GuildName, request.GuildNotice, character);
            sender.Session.Response.guildCreate.guildInfo = character.Guild.GuildInfo(character);
            sender.Session.Response.guildCreate.Result = Result.Success;
            sender.SendResponse();
            
        }

        private void OnGuildList(NetConnection<NetSession> sender, GuildListRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildList::[{0}]{1}", character.Id, character.Name);

            sender.Session.Response.guildList = new GuildListResponse();
            sender.Session.Response.guildList.Guilds.AddRange(GuildManager.Instance.GetGuildsInfo());
            sender.Session.Response.guildList.Result = Result.Success;
            sender.SendResponse();
        }

        private void OnGuildJoinRequest(NetConnection<NetSession> sender, GuildJoinRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinRequest::GuildId: {0} character:[{1}]{2}", request.Apply.GuildId, character.Id, character.Name);
            var guild = GuildManager.Instance.GetGuild(request.Apply.GuildId);
            if (guild == null)
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Erromsg = "公会不存在";
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.SendResponse();
                return;
            }
            request.Apply.characterId = character.Data.ID;
            request.Apply.Name = character.Data.Name;
            request.Apply.Class = character.Data.Class;
            request.Apply.Level = character.Data.Level;
            if (guild.JoinApply(request.Apply))
            {
                var leader = SessionManager.Instance.GetSession(guild.Data.LeaderID);
                if (leader != null)
                {
                    leader.Session.Response.guildJoinReq = request;
                    leader.SendResponse();
                }
            }
            else
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Erromsg = "请勿重复申请";
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.SendResponse();
            }
        }

        private void OnGuildJoinResponse(NetConnection<NetSession> sender, GuildJoinResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinRequest::GuildId: {0} character:[{1}]{2}", response.Apply.GuildId, character.Id, character.Name);
            var guild = GuildManager.Instance.GetGuild(response.Apply.GuildId);
            if (response.Result == Result.Success)
            {
                guild.JoinAppove(response.Apply);
            }
            var request = SessionManager.Instance.GetSession(response.Apply.characterId);
            
            if (request!=null)
            {
                request.Session.Character.Guild = guild;
                request.Session.Response.guildJoinRes = response;
                request.Session.Response.guildJoinRes.Erromsg = "加入公会成功";
                request.Session.Response.guildJoinRes.Result = Result.Success;
                request.SendResponse();
            }
        }

        private void OnGuildLeave(NetConnection<NetSession> sender, GuildLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildLeave::character:[{1}]{2}",  character.Id, character.Name);
            sender.Session.Response.guildLeave = new GuildLeaveResponse();

            character.Guild.Leave(character);
            sender.Session.Response.guildLeave.Result = Result.Success;

            DBService.Instance.Save();
            sender.SendResponse();
        }

        private void OnGuildAdmin(NetConnection<NetSession> sender, GuildAdminRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildAdmin::character:[{1}]{2}", character.Id, character.Name);
            sender.Session.Response.guildAdmin = new GuildAdminResponse();
            if(character.Guild == null)
            {
                sender.Session.Response.guildAdmin.Result = Result.Failed;
                sender.Session.Response.guildAdmin.Erromsg = "无权操作";
                sender.SendResponse();
                return;
            }

            character.Guild.ExecuteAdmin(message.Command, message.Target, character.Id);

            var target = SessionManager.Instance.GetSession(message.Target);
            if (target != null)
            {
                target.Session.Response.guildAdmin = new GuildAdminResponse();
                target.Session.Response.guildAdmin.Result = Result.Success;
                target.Session.Response.guildAdmin.Command = message;
                target.SendResponse();
            }

            sender.Session.Response.guildAdmin.Result = Result.Success;
            sender.Session.Response.guildAdmin.Command = message;
            sender.SendResponse();
        }

    }
}
