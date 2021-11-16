using Common.Battle;
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
        private float cd = 0;
        private float castTime = 0;

        public bool IsCasting = false;

        public float CD
        {
            get { return cd; }
        }

        public Skill(NSkillInfo skillInfo, Creature owner)
        {
            this.skillInfo = skillInfo;
            this.Owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.Owner.Define.TID][this.skillInfo.Id];
            this.cd = 0;
        }

        public SkillResult  CanCast(Creature target)
        {
            if (this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                if (target == null || target == this.Owner)
                    return SkillResult.InvalidTarget;
                int distance =(int) Vector3Int.Distance(this.Owner.position, target.position);
                if(distance > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }
            if (this.Define.CastTarget == Common.Battle.TargetType.Position && BattleManager.Instance.CurrentPosition == null)
            {
                return SkillResult.InvalidTarget;
            }
            if (this.Owner.Attributes.MP < this.Define.MPCost)
            {
                return SkillResult.OutOfMP;
            }
            
            if(this.cd > 0)
            {
                return SkillResult.Cooldown;
            }
            return SkillResult.Ok;
        }

        public void OnUpdate(float deltea)
        {
            if (this.IsCasting)
            {

            }
            UpdateCD(deltea);
        }

        private void UpdateCD(float delta)
        {
            if (this.cd > 0) this.cd -= delta;
            if (this.cd < 0) this.cd = 0;
        }

        //public void Cast()
        //{
            
        //}

        public void BeginCast()
        {
            this.IsCasting = true;
            this.castTime = 0;
            this.cd = this.Define.CD;
            this.Owner.PlayAnim(this.Define.SkillAnim);
        }
    }
}
