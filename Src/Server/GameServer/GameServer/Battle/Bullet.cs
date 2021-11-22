using Common;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    class Bullet
    {
        private Skill skill;
        private Creature target;
        NSkillHitInfo hitInfo;
        bool TimeMode = true;
        float duration = 0;
        float flyTime = 0;

        public bool Stoped=false;

        public Bullet(Skill skill, Creature target, SkillBridge.Message.NSkillHitInfo hitInfo)
        {
            this.skill = skill;
            this.target = target;
            this.hitInfo = hitInfo;
            int distance = skill.Owner.Distance(target);
            if (TimeMode)
            {
                duration = distance / this.skill.Define.BulletSpeed;
            }
            Log.InfoFormat("Bullet[{0}].CasterBullet[{1}] Target:{2} Distance:{3} Time:{4}", this.skill.Define.Name, this.skill.Define.BulletResource,target.Name, target.entityId, distance,duration);
        }

        public void Update()
        {
            if (Stoped) return;
            if (TimeMode)
                this.UpdateTime();
            else
                this.UpdatePos();
        }

        private void UpdateTime()
        {
            this.flyTime += TimeUtil.deltaTime;
            if (this.flyTime > duration)
            {
                this.hitInfo.isBullet = true;
                this.skill.DoHit(hitInfo);
                this.Stoped = true;
            }
        }

        private void UpdatePos()
        {
            //int distance = skill.Owner.Distance(target);
            //if (distance > 50)
            //    UpdatePos += speed * TimeUtil.deltaTime;
            //else
            //{
                
            //}
        }
    }
}
