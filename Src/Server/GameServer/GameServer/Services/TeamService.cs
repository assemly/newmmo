﻿using Common;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class TeamService:Singleton<TeamService>
    {

        public TeamService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamLeaveRequest>(this.OnTeamLeave);
        }

        public void Init()
        {
            TeamManager.Instance.Init();
        }
        private void OnTeamInviteRequest(NetConnection<NetSession> sender, TeamInviteRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteRequest::FromId:{0} FromName:{1} ToID:{2} ToName:{3}", request.FromId, request.FromName, request.ToId, request.ToName);
            //TODO:执行一些前置数据校验

            NetConnection<NetSession> target = SessionManager.Instance.GetSession(request.ToId);
            if(target == null)
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.Session.Response.teamInviteRes.Erromsg = "好友不在线";
                sender.SendResponse();
                return;
            }

            if (target.Session.Character.Team != null)
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.Session.Response.teamInviteRes.Erromsg = "对方已有队伍";
                sender.SendResponse();
            }

            Log.InfoFormat("ForwardTeamInviteRequestRequest:: FromId:{0} FromName:{1} ToID:{2} ToName:{3}", request.FromId, request.FromName, request.ToId, request.ToName);
            target.Session.Response.teamInviteReq = request;
            target.SendResponse();
        }

        private void OnTeamInviteResponse(NetConnection<NetSession> sender, TeamInviteResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteResponse::charcter:{0} Result:{1} FromId:{2} ToID:{3}", character.Id, response.Result, response.Request.FromId, response.Request.ToId);
            
            
            if (response.Result == Result.Success)
            {
                sender.Session.Response.teamInviteRes = response;
                //接受了组队请求
                var requester = SessionManager.Instance.GetSession(response.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.teamInviteRes.Result = Result.Failed;
                    sender.Session.Response.teamInviteRes.Erromsg = "请求者已下线";
                }
                else
                {
                    TeamManager.Instance.AddTeamMember(requester.Session.Character, character);
                    requester.Session.Response.teamInviteRes = response;
                    requester.SendResponse();
                }
                sender.SendResponse();
            }
            else
            {
                var toResponse = SessionManager.Instance.GetSession(response.Request.FromId);
                toResponse.Session.Response.teamInviteRes = response;
                toResponse.Session.Response.teamInviteRes.Result = Result.Failed;
                toResponse.Session.Response.teamInviteRes.Erromsg = "对方拒绝组队";
                toResponse.SendResponse();
                //sender.Session.Response.teamInviteRes.Result = Result.Failed;
                //sender.Session.Response.teamInviteRes.Erromsg = "对方拒绝组队";
                //responseter.Session.Response.teamInviteRes = new TeamInviteResponse();
                //responseter.Session.Response.teamInviteRes.Result = Result.Failed;
                //responseter.Session.Response.teamInviteRes.Erromsg = "对方拒绝组队";
                //responseter.SendResponse();

            }
            
        }

        private void OnTeamLeave(NetConnection<NetSession> sender, TeamLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendRemove::character:{0} TeamID:{1}:{2}", character.Id, request.TeamId,request.characterId);
            sender.Session.Response.teamLeave = new TeamLeaveResponse();
            sender.Session.Response.teamLeave.Result = Result.Success;
            sender.Session.Response.teamLeave.characterId = request.characterId;

            //Team team=TeamManager.Instance.GetTeamByCharacter(request.TeamId);
            //team.Leave(sender.Session.Character);
            character.Team.Leave(character);

            
            sender.SendResponse();
        }
    }
}
