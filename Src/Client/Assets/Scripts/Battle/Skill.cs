﻿using Common.Battle;
using Common.Data;
using Entities;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Battle
{
    public class Skill
    {
        public NSkillInfo skillInfo;
        public Creature Owner;
        public SkillDefine Define;
        public float CD;
        public Skill(NSkillInfo skillInfo, Creature owner)
        {
            this.skillInfo = skillInfo;
            this.Owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.Owner.Define.TID][this.skillInfo.Id];
        }

        public SkillResult  CanCast()
        {
            if(this.Define.CastTarget == Common.Battle.TargetType.Target && BattleManager.Instance.Target == null)
            {
                return SkillResult.InvalidTarget;
            }
            if(this.Define.CastTarget == Common.Battle.TargetType.Position && BattleManager.Instance.Position== Vector3.negativeInfinity)
            {
                return SkillResult.InvalidTarget;
            }
            if(this.Owner.Attributes.MP < this.Define.MPCost)
            {
                return SkillResult.OutOfMP;
            }
            
            if(this.CD > 0)
            {
                return SkillResult.Cooldown;
            }
            return SkillResult.OK;
        }

        public void Cast()
        {
            
        }
    }
}
