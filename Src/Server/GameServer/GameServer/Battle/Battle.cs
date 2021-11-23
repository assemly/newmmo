﻿using GameServer.Core;
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

namespace GameServer.Battle
{
    class Battle
    {
        public Map Map;
        Dictionary<int, Creature> AllUnits = new Dictionary<int, Creature>();
        Queue<NSkillCastInfo> Actions = new Queue<NSkillCastInfo>();
        List<NSkillHitInfo> Hits = new List<NSkillHitInfo>();
        List<NBuffInfo> BuffActions = new List<NBuffInfo>();
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
            this.Hits.Clear();
            this.BuffActions.Clear();
            if (this.Actions.Count > 0)
            {
                NSkillCastInfo skillCast = this.Actions.Dequeue();
                this.ExecuteAction(skillCast);
            }
            this.UpdateUnits();
            this.BoradcastHitMessage();
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
            context.Position = cast.Position;
            context.CastSkill = cast;
            if (context.Caster != null)
                this.JoinBattle(context.Caster);
            if (context.Target != null)
                this.JoinBattle(context.Target);

            context.Caster.CastSkill(context, cast.skillId);

            NetMessageResponse message = new NetMessageResponse();
            message.skillCast = new SkillCastResponse();
            message.skillCast.castInfo = context.CastSkill;
           // message.skillCast.Damage = context.Damage;
            message.skillCast.Result = context.Result == SkillResult.Ok ? Result.Success : Result.Failed;
            message.skillCast.Errormsg = context.Result.ToString();
            this.Map.BroadcastBattleResponse(message);
        }


        private void BoradcastHitMessage()
        {
            if (this.Hits.Count == 0 &&this.BuffActions.Count==0) return;
            NetMessageResponse message = new NetMessageResponse();
            if (this.Hits.Count > 0)
            {
                message.skillHits = new SkillHitResponse();
                message.skillHits.Hits.AddRange(this.Hits);
                message.skillHits.Result = Result.Success;
                message.skillHits.Errormsg = "";
            }
            if (this.BuffActions.Count > 0)
            {
                message.buffRes = new BuffResponse();
                message.buffRes.Buffs.AddRange(this.BuffActions);
                message.buffRes.Result = Result.Success;
                message.buffRes.Errormsg = "";
            }
          
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

        public List<Creature> FindUnitsInRange(Vector3Int pos, int range)
        {
            List<Creature> result = new List<Creature>();
            foreach(var unit in this.AllUnits)
            {
                if (unit.Value.Distance(pos) < range)
                {
                    result.Add(unit.Value);
                }
            }
            return result;
        }

        public List<Creature> FindUnitsInMapRange(Vector3Int pos, int range)
        {
           return EntityManager.Instance.GetMapEntitiesInRange<Creature>(this.Map.ID, pos,range);
        }

        public void AddHitInfo(NSkillHitInfo hit)
        {
            this.Hits.Add(hit);
        }

        internal void AddBuffAction(NBuffInfo buff)
        {
            this.BuffActions.Add(buff);
        }
    }
}
