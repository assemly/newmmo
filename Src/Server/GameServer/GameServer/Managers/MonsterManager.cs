﻿using GameServer.Entities;
using GameServer.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class MonsterManager
    {
        private Map Map;
        public Dictionary<int,Monster> Monsters = new Dictionary<int, Monster>();
        public void Init(Map map)
        {
            this.Map = map;
        }

        public Monster Create(int spawnMonID, int spwanLevel,NVector3 position,NVector3 direction)
        {
            
            Monster monster = new Monster(spawnMonID, spwanLevel, position, direction);
            EntityManager.Instance.AddEntity(this.Map.ID, monster);

            monster.Info.EntityId = monster.entityId;
            monster.Info.mapId = this.Map.ID;
            Monsters[monster.Id] = monster;

            this.Map.MonsterEnter(monster);
            return monster;
        }
    }
}
