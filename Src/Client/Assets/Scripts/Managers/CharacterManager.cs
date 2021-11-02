﻿using Entities;
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
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;

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
            Debug.LogFormat("AddCharacter:{0}:{1} Map:{2} Entity:{3},EntityID：{4}", cha.Id, cha.Name, cha.mapId, cha.Entity.String(),cha.Id);
            Character character = new Character(cha);
            this.Characters[cha.Id] = character;
            EntityManager.Instance.AddEntity(character);
            if(OnCharacterEnter!=null)
            {
                OnCharacterEnter(character);
            }
        }


        public void RemoveCharacter(int chaId)
        {
            Debug.LogFormat("RemoveCharacter:{0}", chaId);
            if (this.Characters.ContainsKey(chaId))
            {
                 EntityManager.Instance.RemoveEntity(this.Characters[chaId].Info.Entity);
                if (OnCharacterLeave != null)
                {
                    OnCharacterLeave(this.Characters[chaId]);
                }
                this.Characters.Remove(chaId);
            }
        }

        public Character GetCharacter(int id)
        {
            Character character;
            this.Characters.TryGetValue(id, out character);
            return character;
        }
    }
}
