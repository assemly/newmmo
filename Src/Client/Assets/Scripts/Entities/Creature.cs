using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Common.Battle;
using Managers;
using Battle;

namespace Entities
{
    public class Creature : Entity
    {
        public NCharacterInfo Info;
        public CharacterDefine Define;

        public Attributes Attributes;

        public SkillManager SkillMgr;
        bool battleState = false;
        public bool BattleStates
        {
            get { return battleState; }
            set
            {
                if (battleState != value)
                {
                    battleState = value;
                    this.SetStandby(value);
                }
            }
        }
        public int Id
        {
            get { return this.Info.Id; }
        }
        public string Name
        {
            get
            {
                if (this.Info.Type == CharacterType.Player)
                    return this.Info.Name;
                else
                    return this.Define.Name;
            }

        }
        public Skill CastringSkill = null;

        public bool IsPlayer
        {
            get
            {
                return this.Info.Type == CharacterType.Player;
            }
        }

        public bool IsCurrentPlayer
        {
            get
            {
                if (!IsPlayer) return false;
                return this.Info.Id == Models.User.Instance.CurrentCharacterInfo.Id;
            }
        }

        public int Distance(Creature target)
        {
            return (int)Vector3Int.Distance(this.position, target.position);
        }

        public Creature(NCharacterInfo info) : base(info.Entity)
        {
            this.Info = info;
            this.Define = DataManager.Instance.Characters[info.ConfigId];
            this.Attributes = new Attributes();
            //var equips = EquipManager.Instance.GetEquipDefines();
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic);
            this.SkillMgr = new SkillManager(this);
        }

        public void UpdateInfo(NCharacterInfo info)
        {
            this.SetEntityData(info.Entity);
            this.Info = info;
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic);
            this.SkillMgr.UpdateSkills();
        }

        public virtual List<EquipDefine> GetEquips()
        {
            return null;
        }
        public void MoveForward()
        {
            Debug.LogFormat("MoveForward");
            this.speed = this.Define.Speed*1.5f;
        }

        public void MoveBack()
        {
            Debug.LogFormat("MoveBack");
            this.speed = -this.Define.Speed;
        }

        public void Stop()
        {
            Debug.LogFormat("Stop");
            this.speed = 0;
        }

        public void SetDirection(Vector3Int direction)
        {
            Debug.LogFormat("SetDirection:{0}", direction);
            this.direction = direction;
        }

        public void SetPosition(Vector3Int position)
        {
            Debug.LogFormat("SetPostion:{0}", position);
            this.position = position;
        }
        public void CastSkill(int skillId, Creature target, NVector3 position,NDamageInfo damage)
        {
            this.SetStandby(true);
            var skill = this.SkillMgr.GetSkill(skillId);
            skill.BeginCast(damage);
        }
        public void PlayAnim(string name)
        {
            if (this.Controller != null)
                this.Controller.PlayAnim(name);
        }
        public void SetStandby(bool standby)
        {
            if (this.Controller != null)
                this.Controller.SetStandby(standby);
        }

        public override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
            this.SkillMgr.OnUpdate(delta);
        }

        public void DoDamage(NDamageInfo damage)
        {
            Debug.LogFormat("Creature.DoDamage:{0}", damage);
            this.Attributes.HP -= damage.Damage;
            this.PlayAnim("Hurt");
        }
        public void DoSkillHit(int skillId, int hitId, List<NDamageInfo> damages)
        {
            Debug.LogFormat("Creature.DoSkillHit: count{0} ", damages.Count);
            var skill = this.SkillMgr.GetSkill(skillId);
            skill.DoHit(hitId, damages);
        }

    }
}
