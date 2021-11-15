﻿using Entities;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        public Dictionary<int, Entities.Creature> Characters = new Dictionary<int, Entities.Creature>();

        public UnityAction<Entities.Creature> OnCharacterEnter;
        public UnityAction<Entities.Creature> OnCharacterLeave;

        public CharacterManager()
        {

        }

        public void Init()
        {

        }

        public void Dispose()
        {
            
        }

        public void Clear()
        {
            int[] keys = this.Characters.Keys.ToArray();
            foreach(var key in keys)
            {
                this.RemoveCharacter(key);
            }
            this.Characters.Clear();
        }

        public void AddCharacter(NCharacterInfo cha)
        {
            Debug.LogFormat("AddCharacter:{0}:{1} Map:{2} Entity:{3},EntityID：{4}", cha.Id, cha.Name, cha.mapId, cha.Entity.String(),cha.EntityId);
            Entities.Creature character = new Entities.Creature(cha);
            this.Characters[cha.EntityId] = character;
            EntityManager.Instance.AddEntity(character);
            if(OnCharacterEnter!=null)
            {
                OnCharacterEnter(character);
            }
            if (cha.EntityId == User.Instance.CurrentCharacterInfo.EntityId)
            {
                User.Instance.CurrentCharacter = character;
            }
        }


        public void RemoveCharacter(int entityId)
        {
            Debug.LogFormat("RemoveCharacter:{0}", entityId);
            if (this.Characters.ContainsKey(entityId))
            {
                 EntityManager.Instance.RemoveEntity(this.Characters[entityId].Info.Entity);
                if (OnCharacterLeave != null)
                {
                    OnCharacterLeave(this.Characters[entityId]);
                }
                this.Characters.Remove(entityId);
            }
        }

        public Entities.Creature GetCharacter(int id)
        {
            Entities.Creature character;
            this.Characters.TryGetValue(id, out character);
            return character;
        }
    }
}
