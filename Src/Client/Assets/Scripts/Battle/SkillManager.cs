﻿using Battle;
using Entities;
using System;
using System.Collections.Generic;

namespace Managers
{
    public class SkillManager
    {
        Creature Owner;
        public List<Skill> Skills { get; private set; }

        public SkillManager(Creature owner)
        {
            this.Owner = owner;
            this.Skills = new List<Skill>();
            this.InitSkills();
        }

        private void InitSkills()
        {
            this.Skills.Clear();
            foreach(var skillInfo in this.Owner.Info.Skills)
            {
                Skill skill = new Skill(skillInfo, this.Owner);
                this.AddSkill(skill);
            }
        }

        public void AddSkill(Skill skill)
        {
            this.Skills.Add(skill);
        }

        public Skill GetSkill(int skillId)
        {
            for(int i = 0; i < this.Skills.Count; i++)
            {
                if (this.Skills[i].Define.ID == skillId)
                    return this.Skills[i];
            }
            return null;
        }

        public void OnUpdate(float deltea)
        {
            for(int i = 0; i < this.Skills.Count; i++)
            {
                this.Skills[i].OnUpdate(deltea);
            }
        }
    }
}
