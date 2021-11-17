﻿using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    class Battle
    {
        public Map Map;
        Dictionary<int, Creature> AllUnits = new Dictionary<int, Creature>();
        Queue<NSkillCastInfo> Actions = new Queue<NSkillCastInfo>();
        List<Creature> DeathPool = new List<Creature>();

        public Battle(Map map)
        {
            this.Map = map;
        }

        public void ProcessBattleMessage(NetConnection<NetSession> sender,SkillCastRequest request)
        {
            Character character = sender.Session.Character;
            if (request.castInfo != null)
            {
                if (character.entityId != request.castInfo.casterId)
                    return;
                this.Actions.Enqueue(request.castInfo);
            }
        }

        public void Update()
        {
            if (this.Actions.Count > 0)
            {
                NSkillCastInfo skillCast = this.Actions.Dequeue();
                this.ExecuteAction(skillCast);
            }
            this.UpdateUnits();
        }
        public void JoinBattle(Creature unit)
        {
            this.AllUnits[unit.entityId] = unit;
        }
        public void LeaveBattle(Creature unit)
        {
            this.AllUnits.Remove(unit.entityId);
        }
        private void ExecuteAction(NSkillCastInfo cast)
        {
            BattleContext context = new BattleContext(this);
            context.Caster = EntityManager.Instance.GetCreature(cast.casterId);
            context.Target = EntityManager.Instance.GetCreature(cast.targetId);
            context.CastSkill = cast;
            if (context.Caster != null)
                this.JoinBattle(context.Caster);
            if (context.Target != null)
                this.JoinBattle(context.Target);

            context.Caster.CastSkill(context, cast.skillId);

            NetMessageResponse message = new NetMessageResponse();
            message.skillCast = new SkillCastResponse();
            message.skillCast.castInfo = context.CastSkill;
            message.skillCast.Damage = context.Damage;
            message.skillCast.Result = context.Result == SkillResult.Ok ? Result.Success : Result.Failed;
            message.skillCast.Errormsg = context.Result.ToString();
            this.Map.BroadcastBattleResponse(message);
        }
        private void UpdateUnits()
        {
            this.DeathPool.Clear();
            foreach(var kv in this.AllUnits)
            {
                kv.Value.Update();
                if (kv.Value.IsDeath)
                    this.DeathPool.Add(kv.Value);
            }
            foreach(var unit in this.DeathPool)
            {
                this.LeaveBattle(unit);
            }
        }

        
    }
}
