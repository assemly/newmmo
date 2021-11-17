using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using System;

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
        public Skill(NSkillInfo skillInfo, Creature owner)
        {
            this.skillInfo = skillInfo;
            this.Owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.Owner.Define.TID][this.skillInfo.Id];
        }

        public SkillResult Cast(BattleContext context)
        {
            SkillResult result = SkillResult.Ok;
            if (this.cd > 0)
                return SkillResult.Cooldown;
            if (context.Target != null)
            {
                this.DoSkillDamage(context);
            }
            //else
            //    result = SkillResult.InvalidTarget;
            this.cd = this.Define.CD;
            return result;
        }

        private void DoSkillDamage(BattleContext context)
        {
            context.Damage = new NDamageInfo();
            context.Damage.entityId = context.Target.entityId;
            context.Damage.Damage = 100;
            context.Target.DoDamage(context.Damage);
        }

        public void Update()
        {
            UpdateCD();
        }

        private void UpdateCD()
        {
            if (this.cd > 0) this.cd -= TimeUtil.deltaTime;
            if (this.cd < 0) this.cd = 0;
        }
    }
}
