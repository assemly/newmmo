﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        /// <summary>
        /// 地图中的角色，以CharacterID为Key
        /// </summary>
        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();

        /// <summary>
        /// 刷怪管理器
        /// </summary>
        /// <param name="define"></param>
        SpawnManager SpawnManager = new SpawnManager();

        public MonsterManager MonsterManager = new MonsterManager();

        internal Map(MapDefine define)
        {
            this.Define = define;
            this.SpawnManager.Init(this);
            this.MonsterManager.Init(this);
        }

        internal void Update()
        {
            SpawnManager.Update();
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);
            character.Info.mapId = this.ID;
            this.MapCharacters[character.Id] = new MapCharacter(conn, character);
           
            
            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            


            foreach (var kv in this.MapCharacters)
            {
                
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                if (kv.Value.character != character)
                    this.AddCharacterEnterMap(kv.Value.connection, character.Info);
                
            }
            foreach (var kv in this.MonsterManager.Monsters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.Info);
            }
            conn.SendResponse();


        }


        internal void CharacterLeave(Character cha)
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterId:{1}", this.Define.ID, cha.Id);
            foreach(var kv in this.MapCharacters)
            {
                this.SendCharacterLeaveMap(kv.Value.connection, cha);
            }
            this.MapCharacters.Remove(cha.Id);
        }
        private void AddCharacterEnterMap(NetConnection<NetSession> conn,NCharacterInfo character)
        {
            if(conn.Session.Response.mapCharacterEnter == null)
            {
                conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            }

            conn.Session.Response.mapCharacterEnter.Characters.Add(character);
            

            conn.SendResponse();
        }

        private List<NCharacterInfo> ReturnMapCharacters(Character character) 
        {
            List<NCharacterInfo> NMapCharacter = new List<NCharacterInfo>();
            foreach(var kv in this.MapCharacters)
            {
                if (character != kv.Value.character)
                {
                    NMapCharacter.Add(kv.Value.character.Info);
                }
            }
            return NMapCharacter;
        }
            

        private void SendCharaterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            NetMessage sendOtherCharactermessage = new NetMessage();
            sendOtherCharactermessage.Response = new NetMessageResponse();

            sendOtherCharactermessage.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            sendOtherCharactermessage.Response.mapCharacterEnter.mapId = this.Define.ID;
            sendOtherCharactermessage.Response.mapCharacterEnter.Characters.Add(character);

            byte[] data = PackageHandler.PackMessage(sendOtherCharactermessage);
            conn.SendData(data, 0, data.Length);
        }

        void  SendCharacterLeaveMap(NetConnection<NetSession> conn,Character character)
        {
            
            Log.InfoFormat("SendCharacterLeaveMap To {0}:{1} : Map:{2} CharacterID:{3}:{4}:EntityId:{5}", conn.Session.Character.Id, conn.Session.Character.Info.Name, this.Define.ID, character.Id, character.Info.Name,character.entityId);
            conn.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            conn.Session.Response.mapCharacterLeave.entityId = character.entityId;
            
            conn.SendResponse();
        }

        internal void UpdateEntity(NEntitySync entity)
        {
            foreach (var kv in this.MapCharacters)
            {
                if (kv.Value.character.entityId == entity.Id)
                {
                    kv.Value.character.Position = entity.Entity.Position;
                    kv.Value.character.Direction = entity.Entity.Direction;
                    kv.Value.character.Speed = entity.Entity.Speed;
                    if(entity.Event == EntityEvent.Ride)
                    {
                        kv.Value.character.Ride = entity.Param;
                    }
                    
                }
                else
                {
                   MapService.Instance.SendEntityUpdate(kv.Value.connection, entity);
                }
            }
        }



        /// <summary>
        /// 怪物进入地图
        /// </summary>
        /// <param name="character"></param>
        public void MonsterEnter(Monster monster)
        {
            Log.InfoFormat("MonsterEnter: Map:{0} monsterId:{1}", this.Define.ID, monster.Id);
            foreach(var kv in this.MapCharacters)
            {
                this.AddCharacterEnterMap(kv.Value.connection, monster.Info);
            }
        }

        public void BroadcastBattleResponse(NetMessageResponse response)
        {
            foreach (var kv in this.MapCharacters)
            {
                kv.Value.connection.Session.Response.skillCast = response.skillCast;
                kv.Value.connection.SendResponse();
            }
        }

    }
}
