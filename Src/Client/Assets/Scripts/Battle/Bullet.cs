using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle
{
    class Bullet
    {
        internal bool Stoped=false;
        private Skill skill;
        int hit = 0;
        float flyTime = 0;
        float duration = 0;

        public Bullet(Skill skill)
        {
            this.skill = skill;
            var target = skill.Target;
            this.hit = skill.Hit;
            int distance = skill.Owner.Distance(target);
            duration = distance / this.skill.Define.BulletSpeed;
        }

        public void Update()
        {
            if (this.Stoped) return;
            this.flyTime += TimeUtil.deltaTime;
            if (this.flyTime > duration)
            {
                this.skill.DoHitDamages(this.hit);
                this.Stoped = true;
            }
        }
    }
}
