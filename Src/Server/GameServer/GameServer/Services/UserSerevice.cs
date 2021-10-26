﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {

        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(this.OnGameLeave);
        }

        

        public void Init()
        {
            
        }

        void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            //Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            //NetMessage message = new NetMessage();
            //message.Response = new NetMessageResponse();
            //message.Response.userLogin = new UserLoginResponse();


            //TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            //if (user == null)
            //{
            //    message.Response.userLogin.Result = Result.Failed;
            //    message.Response.userLogin.Errormsg = "用户不存在";
            //}
            //else if (user.Password != request.Passward)
            //{
            //    message.Response.userLogin.Result = Result.Failed;
            //    message.Response.userLogin.Errormsg = "密码错误";
            //}
            //else
            //{
            //    sender.Session.User = user;

            //    message.Response.userLogin.Result = Result.Success;
            //    message.Response.userLogin.Errormsg = "None";
            //    message.Response.userLogin.Userinfo = new NUserInfo();
            //    message.Response.userLogin.Userinfo.Id = 1;
            //    message.Response.userLogin.Userinfo.Player = new NPlayerInfo();
            //    message.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
            //    foreach (var c in user.Player.Characters)
            //    {
            //        NCharacterInfo info = new NCharacterInfo();
            //        info.Id = c.ID;
            //        info.Name = c.Name;
            //        info.Class = (CharacterClass)c.Class;
            //        message.Response.userLogin.Userinfo.Player.Characters.Add(info);
            //    }

            //}
            //byte[] data = PackageHandler.PackMessage(message);
            //sender.SendData(data, 0, data.Length);

            Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            sender.Session.Response.userLogin = new UserLoginResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user == null)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "用户不存在";
            }
            else if (user.Password != request.Passward)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "密码错误";
            }
            else
            {
                sender.Session.User = user;

                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "None";
                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id = (int)user.ID;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach (var c in user.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Type = CharacterType.Player;
                    info.Class = (CharacterClass)c.Class;
                    info.ConfigId = c.ID;
                    sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(info);
                }

            }
            sender.SendResponse();
        }

        void OnRegister(NetConnection<NetSession> conn, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            conn.Session.Response.userRegister = new UserRegisterResponse();

            //NetMessage message = new NetMessage();
            //message.Response = new NetMessageResponse();
            //message.Response.userRegister = new UserRegisterResponse();


            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                //message.Response.userRegister.Result = Result.Failed;
                //message.Response.userRegister.Errormsg = "用户已存在.";
                conn.Session.Response.userRegister.Result = Result.Failed;
                conn.Session.Response.userRegister.Errormsg = "用户已存在.";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                //message.Response.userRegister.Result = Result.Success;
                //message.Response.userRegister.Errormsg = "None";
                conn.Session.Response.userRegister.Result = Result.Success;
                conn.Session.Response.userRegister.Errormsg = "None";
            }

            //byte[] data = PackageHandler.PackMessage(message);
            //sender.SendData(data, 0, data.Length);
            conn.SendResponse();
        }

        private void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreateCharacterRequest: Name:{0}  Class:{1}", request.Name, request.Class);

            TCharacter character = new TCharacter()
            {
                Name = request.Name,
                Class = (int)request.Class,
                TID = (int)request.Class,
                MapID = 1,
                MapPosX = 5000,
                MapPosY = 4000,
                MapPosZ = 820,
            };


            DBService.Instance.Entities.Characters.Add(character);
            sender.Session.User.Player.Characters.Add(character);
            DBService.Instance.Entities.SaveChanges();

            sender.Session.Response.createChar = new UserCreateCharacterResponse();
            sender.Session.Response.createChar.Result = Result.Success;
            sender.Session.Response.createChar.Errormsg = "None";
            //NetMessage message = new NetMessage();
            //message.Response = new NetMessageResponse();
            //message.Response.createChar = new UserCreateCharacterResponse();
            //message.Response.createChar.Result = Result.Success;

            foreach (var c in sender.Session.User.Player.Characters)
            {
                NCharacterInfo info = new NCharacterInfo();
                info.Id = c.ID;
                info.Name = c.Name;
                info.Type = CharacterType.Player;
                info.Class = (CharacterClass)c.Class;
                info.ConfigId = c.TID;
                sender.Session.Response.createChar.Characters.Add(info);
            }
            sender.SendResponse();
            //message.Response.createChar.Errormsg = "None";

            //byte[] data = PackageHandler.PackMessage(message);
            //sender.SendData(data, 0, data.Length);
        }

        private void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat("UserGameRequest: characterID:{0}:{1} Map:{2}", dbchar.ID, dbchar.Name, dbchar.MapID);
            Character character = CharacterManager.Instance.AddCharacter(dbchar);
            Log.InfoFormat("OnGameEnter Character EntityId{0}:", character.entityId);
            //NetMessage message = new NetMessage();
            //message.Response = new NetMessageResponse();
            //message.Response.gameEnter = new UserGameEnterResponse();
            //message.Response.gameEnter.Result = Result.Success;
            //message.Response.gameEnter.Errormsg = "None";
            //message.Response.gameEnter.Character = character.Info;
            //byte[] data = PackageHandler.PackMessage(message);
            //sender.SendData(data, 0, data.Length);
            //sender.Session.Character = character;
            //SessionManager.Instance.AddSession(character.Id, sender);

            sender.Session.Response.gameEnter = new UserGameEnterResponse();
            sender.Session.Response.gameEnter.Result = Result.Success;
            sender.Session.Response.gameEnter.Errormsg = "None";
            sender.Session.Response.gameEnter.Character = character.Info;

            //道具系统测试
            int itemId = 2;
            bool hasItem = character.ItemManager.HasItem(itemId);
            Log.InfoFormat("HasItem:[{0}]{1}", itemId, hasItem);
            if (hasItem)
            {
                character.ItemManager.RemoveItem(itemId, 1);
            }
            else
            {
                character.ItemManager.AddItem(itemId, 5);
            }
            Models.Item item = character.ItemManager.GetItem(itemId);
            Log.InfoFormat("Item:[{0}][{1}]", itemId, item);

            //进入成功，发送初始角色信息
            sender.Session.Character = character;
            //sender.Session.PostResponser = character;

            sender.Session.Response.gameEnter.Character = character.Info;
            sender.SendResponse();

            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character);
        }

        void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("UserGameLeaveRequest: characterID:{0}:{1} Map{2}", character.Id, character.Info.Name, character.Info.mapId);
            this.CharacterLeave(character);
           // CharacterManager.Instance.RemoveCharacter(character.entityId);
            sender.Session.Response.gameLeave = new UserGameLeaveResponse();
            sender.Session.Response.gameLeave.Result = Result.Success;
            sender.Session.Response.gameLeave.Errormsg = "None";

            sender.SendResponse();
        }

        public void CharacterLeave(Character character)
        {
            Log.InfoFormat("CharacterLeave： characterID:{0}:{1}", character.entityId, character.Info.Name);
            CharacterManager.Instance.RemoveCharacter(character.Id);
            //character.Clear();
            MapManager.Instance[character.Info.mapId].CharacterLeave(character);
        }
    }
}
