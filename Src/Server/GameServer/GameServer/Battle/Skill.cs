﻿using Common;
using Common.Data;
using Common.Utils;
using GameServer.Core;
using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;

namespace GameServer.Battle
{
    class Skill
    {
        public NSkillInfo skillInfo;
        public Creature Owner;
        public SkillDefine Define;

        private float cd = 0;
        public float CD
        {
            get { return cd; }
        }

        public SkillStatus Status;
        private int Hit=0;
        private float castingTime = 0;
        private float skillTime = 0;
        BattleContext Context;
        NSkillHitInfo HitInfo;

        public bool Instant { 
            get 
            {
                if (this.Define.CastTime > 0) return false;
                if (this.Define.Bullet) return false;
                if (this.Define.Duration > 0) return false;
                if (this.Define.HitTimes != null && this.Define.HitTimes.Count > 0) return false;
                return true; 
            }  
        }

        public Skill(NSkillInfo skillInfo, Creature owner)
        {
            this.skillInfo = skillInfo;
            this.Owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.Owner.Define.TID][this.skillInfo.Id];
        }
        public SkillResult CanCast(BattleContext context)
        {
            if (this.Status != SkillStatus.None)
            {
                return SkillResult.Casting;
            }
            if(this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                if (context.Target == null || context.Target == this.Owner)
                    return SkillResult.InvalidTarget;
                int distance = this.Owner.Distance(context.Target);
                if(distance > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }
            if(this.Define.CastTarget == Common.Battle.TargetType.Position)
            {
                if (context.CastSkill.Position == null)
                    return SkillResult.InvalidTarget;
                if(this.Owner.Distance(context.Position) > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }

            if(this.Owner.Attributes.MP < this.Define.MPCost)
            {
                return SkillResult.Cooldown;
            }
            return SkillResult.Ok;
        }
        public SkillResult Cast(BattleContext context)
        {
            SkillResult result = this.CanCast(context);
            if(result == SkillResult.Ok)
            {
                this.castingTime = 0;
                this.skillTime = 0;
                this.cd = this.Define.CD;
                this.Context = context;
                this.Hit = 0;
                if (this.Instant)
                {
                    this.DoHit();
                }
                else
                {
                    if (this.Define.CastTime > 0)
                    {
                        this.Status = SkillStatus.Casting;
                    }
                    else
                        this.Status = SkillStatus.Running;
                }
                //if (context.Target != null)
                //{
                //    this.DoSkillDamage(context);
                //}
                //this.cd = this.Define.CD;
            }

            Log.InfoFormat("Skill[{0}].Cast Result:[{1}] Status:{2}", this.Define.Name, result, this.Status);
            return result;
        }

        private void DoHit()
        {
            this.InitHitInfo();
            
            Log.InfoFormat("Skill[{0}].DoHit[{1}]", this.Define.Name,this.Hit);
            this.Hit++;
            if (this.Define.Bullet)
            {
                CastBullet();
                return;
            }
            if(this.Define.AOERange > 0)
            {
                this.HitRange();
            }
            if(this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                this.HitTarget(Context.Target);
            }
        }

        private void CastBullet()
        {
            Log.InfoFormat("Skill[{0}].CastBullet", this.Define.Name);
        }

        private void HitRange()
        {
            Vector3Int pos;
            if (this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                pos = Context.Target.Position;
            }
            else if (this.Define.CastTarget == Common.Battle.TargetType.Position)
            {
                pos = Context.Position;
            }
            else
            {
                pos = this.Owner.Position;
            }
            List<Creature> units = this.Context.Battle.FindUnitsInRange(pos, this.Define.AOERange);
            foreach(var target in units)
            {
                this.HitTarget(target);
            }
        }

        private void HitTarget(Creature target)
        {
            if (this.Define.CastTarget == Common.Battle.TargetType.Self && (target != Context.Caster)) return;
            else if (target == Context.Caster) return;

            NDamageInfo damge = this.CalcSkillDamage(Context.Caster, target);
            Log.InfoFormat("Skill[{0}].HitTarget Result:[{1}] Damage:{2} Crit:{3}", this.Define.Name, target.Name, damge.Damage,damge.Crit);
            target.DoDamage(damge);
            this.HitInfo.Damages.Add(damge);

        }

        private NDamageInfo CalcSkillDamage(Creature caster, Creature target)
        {
            float ad = this.Define.AD + caster.Attributes.AD * this.Define.ADFator;
            float ap = this.Define.AP + caster.Attributes.AP * this.Define.APFator;

            float addmg = ad * (1 - target.Attributes.DEF / (target.Attributes.DEF + 100));
            float apdmg = ap * (1 - target.Attributes.MDEF / (target.Attributes.MDEF + 100));

            float final = addmg + apdmg;
            bool isCrit = IsCrit(caster.Attributes.CRI);
            if (isCrit)
                final = final * 2f;
            final += final * (float)MathUtil.Random.NextDouble() * 0.1f - 0.05f;
            NDamageInfo damage = new NDamageInfo();
            damage.entityId = target.entityId;
            damage.Damage = Math.Max(1, (int)final);
            damage.Crit = isCrit;
            return damage;
        }

        private bool IsCrit(float crit)
        {
            return MathUtil.Random.NextDouble() < crit;
        }

        private void InitHitInfo()
        {
            this.HitInfo = new NSkillHitInfo();
            this.HitInfo.casterId = this.Context.Caster.entityId;
            this.HitInfo.skillId = this.skillInfo.Id;
            this.HitInfo.hitId = this.Hit;
            Context.Battle.AddHitInfo(this.HitInfo);
        }

        //private void DoSkillDamage(BattleContext context)
        //{
        //    context.Damage = new NDamageInfo();
        //    context.Damage.entityId = context.Target.entityId;
        //    context.Damage.Damage = 100;
        //    context.Target.DoDamage(context.Damage);
        //}

        public void Update()
        {
            UpdateCD();
            if (this.Status == SkillStatus.Casting)
            {
                this.UpdateCasting();
            }
            else if(this.Status == SkillStatus.Running)
            {
                this.UpdateSkill();
            }
        }

        private void UpdateCasting()
        {
            if(this.castingTime < this.Define.CastTime)
            {
                this.castingTime += TimeUtil.deltaTime;
            }
            else
            {
                this.castingTime = 0;
                this.Status = SkillStatus.Running;
                Log.InfoFormat("Skill[{0}].UpdateCasting Finish", this.Define.Name);
            }
        }

        private void UpdateSkill()
        {
            this.skillTime += TimeUtil.deltaTime;
            if(this.Define.Duration > 0)
            {
                //持续技能
                if (this.skillTime > this.Define.Interval * (this.Hit + 1))
                {
                    this.DoHit();
                }
                if(this.skillTime > this.Define.Duration)
                {
                    this.Status = SkillStatus.None;
                    Log.InfoFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                }
            }else if (this.Define.HitTimes != null && this.Define.HitTimes.Count > 0)
            {
                if (this.Hit < this.Define.HitTimes.Count)
                {
                    if(this.skillTime > this.Define.HitTimes[this.Hit])
                    {
                        this.DoHit();
                    }
                }
                else
                {
                    this.Status = SkillStatus.None;
                    Log.InfoFormat("Skill[{0}].UpdateSkill Finish", this.Define.Name);
                }
            }
        }

        private void UpdateCD()
        {
            if (this.cd > 0) this.cd -= TimeUtil.deltaTime;
            if (this.cd < 0) this.cd = 0;
        }
    }
}
