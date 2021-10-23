using System;
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

        internal Map(MapDefine define)
        {
            this.Define = define;
           
        }

        internal void Update()
        {
            
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        //internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        //{
        //    Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);
        //    character.Info.mapId = this.ID;

        //    NetMessage message = new NetMessage();
        //    message.Response = new NetMessageResponse();

        //    message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
        //    message.Response.mapCharacterEnter.mapId = this.Define.ID;
        //    character.Info.EntityId = character.entityId;
        //    message.Response.mapCharacterEnter.Characters.Add(character.Info);

        //    this.MapCharacters[character.Id] = new MapCharacter(conn, character);

        //    foreach (var kv in this.MapCharacters)
        //    {
        //        message.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
        //        this.SendCharaterEnterMap(kv.Value.connection, character.Info);
        //    }

        //    this.MapCharacters[character.Id] = new MapCharacter(conn, character);

        //    byte[] data = PackageHandler.PackMessage(message);
        //    conn.SendData(data, 0, data.Length);

        //}

        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharaterEnter: Map{0} characterId:{1}", this.Define.ID, character.Id);
            character.Info.mapId = this.ID;
            character.Info.EntityId = character.entityId;
            this.MapCharacters[character.Id] = new MapCharacter(conn, character);
            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            conn.Session.Response.mapCharacterEnter.Characters.Add(character.Info);
            conn.SendResponse();

            foreach (var kv in this.MapCharacters)
            {
                //加入entityID；
                //kv.Value.character.Info.EntityId = character.entityId;
                //conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                //conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
                //conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                //if (kv.Value.character != character)
                  this.AddCharacterEnterMap(kv.Value.connection, this.ReturnMapCharacters(character));
                //this.AddCharacterEnterMap(kv.Value.connection, character.Info);
            }
            
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
        private void AddCharacterEnterMap(NetConnection<NetSession> conn, List<NCharacterInfo> character)
        {
            if(conn.Session.Response.mapCharacterEnter == null)
            {
                conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            }


            conn.Session.Response.mapCharacterEnter.Characters.AddRange(character);
            
            conn.SendResponse();
        }

        private List<NCharacterInfo> ReturnMapCharacters(Character character) 
        {
            List<NCharacterInfo> NMapCharacter = new List<NCharacterInfo>();
            foreach(var kv in this.MapCharacters)
            {
                //if(character!= kv.Value.character)
                //{
                    NMapCharacter.Add(kv.Value.character.Info);
                //}
            }
            return NMapCharacter;
        }
            

        private void SendCharaterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            message.Response.mapCharacterEnter.mapId = this.Define.ID;
            message.Response.mapCharacterEnter.Characters.Add(character);

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        void  SendCharacterLeaveMap(NetConnection<NetSession> conn,Character character)
        {
            // 早期方案
            //NetMessage message = new NetMessage();
            //message.Response = new NetMessageResponse();
            //message.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            //message.Response.mapCharacterLeave.entityId = character.entityId;
            
            //byte[] data = PackageHandler.PackMessage(message);
            //conn.SendData(data, 0, data.Length);

            // 这个或许改成ID；
            Log.InfoFormat("SendCharacterLeaveMap To {0}:{1} : Map:{2} Character:{3}:{4}", conn.Session.Character.Id, conn.Session.Character.Info.Name, this.Define.ID, character.entityId, character.Info.Name);
            conn.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            conn.Session.Response.mapCharacterLeave.entityId = character.entityId;
            //conn.Session.Response.mapCharacterEnter.Characters.Add(character);
            //conn.Session.Response.mapCharacterEnter.Characters.Remove(character);
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
    
    }
}
