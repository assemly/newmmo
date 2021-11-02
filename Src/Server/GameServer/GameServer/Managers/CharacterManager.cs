using System.Collections.Generic;
using Common;
using GameServer.Entities;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class CharacterManager:Singleton<CharacterManager>
    {
        public Dictionary<int, Character> Charaters = new Dictionary<int, Character>();

        public CharacterManager()
        {

        }

        public void Dispose()
        {

        }

        public void Init()
        {

        }

        public void Clear()
        {
            this.Charaters.Clear();
        }

        public Character AddCharacter(TCharacter cha)
        {
            Character character = new Character(CharacterType.Player, cha);
            EntityManager.Instance.AddEntity(cha.MapID, character);
            character.Info.EntityId = character.entityId;
            this.Charaters[character.Id] = character;
            return character;
        }

        public void RemoveCharacter(int characterId)
        {
            if (this.Charaters.ContainsKey(characterId))
            {
                var cha = this.Charaters[characterId];
                EntityManager.Instance.RemoveEntity(cha.Data.MapID, cha);
                this.Charaters.Remove(characterId);
            }
        }

        public Character GetCharacter(int characterId)
        {
            Character character = null;
            this.Charaters.TryGetValue(characterId, out character);
            return character;
        }
    }
}
