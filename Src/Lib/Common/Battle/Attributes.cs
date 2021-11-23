using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Battle
{
    public class Attributes
    {
       public AttributeData Initial = new AttributeData();
       public AttributeData Growth = new AttributeData();
       public AttributeData Equip = new AttributeData();
       public AttributeData Basic = new AttributeData();
       public AttributeData Buff = new AttributeData();

        public AttributeData Final = new AttributeData();

        int level;

        public NAttributeDynamic DynamicAttr;

        public float HP
        { 
            get { return DynamicAttr.Hp; }
            set { DynamicAttr.Hp = (int)Math.Min(MaxHP, value); }
        }
        public float MP
        {
            get { return DynamicAttr.Mp; }
            set { DynamicAttr.Mp = (int)Math.Min(MaxMP, value); }
        }
        public float MaxHP { get { return this.Final.MaxHP; } }
        public float MaxMP { get { return this.Final.MaxMP; } }
        public float STR
        {
            get { return this.Final.STR; }
        }
        public float INT
        {
            get { return this.Final.INT; }
        }
        public float DEX
        {
            get { return this.Final.DEX; }          
        }
        public float AD
        {
            get { return this.Final.AD; }      
        }
        public float AP
        {
            get { return this.Final.AP; }   
        }
        public float DEF
        {
            get { return this.Final.DEF; }
        }
        public float MDEF
        {
            get { return this.Final.MDEF; }     
        }
        public float SPD
        {
            get { return this.Final.SPD; }           
        }
        public float CRI
        {
            get { return this.Final.CRI; }         
        }
        public void Init(CharacterDefine define,int level,List<EquipDefine> equips,NAttributeDynamic dynamicAttr)
        {
            this.DynamicAttr = dynamicAttr;
            this.LoadInitAttribute(this.Initial, define);
            this.LoadGrowthAttribute(this.Growth, define);
            this.LoadEquipAttributes(this.Equip, equips);
            this.level = level;
            this.InitBasicAttributes();
            this.InitSecondaryAttributes();

            this.InitFinalAttributes();
            if (this.DynamicAttr == null)
            {
                this.DynamicAttr = new NAttributeDynamic();
                this.HP = this.MaxHP;
                this.MP = this.MaxMP;
            }
            else
            {
                this.HP = this.DynamicAttr.Hp;
                this.MP = this.DynamicAttr.Mp;
            }    
            
        }

        private void LoadInitAttribute(AttributeData initial, CharacterDefine define)
        {
            initial.MaxHP = define.MaxHP;
            initial.MaxMP = define.MaxMP;

            initial.STR = define.STR;
            initial.INT = define.INT;
            initial.DEX = define.DEX;
            initial.AD = define.AD;
            initial.AP = define.AP;
            initial.DEF = define.DEF;
            initial.MDEF = define.MDEF;
            initial.SPD = define.SPD;
            initial.CRI = define.CRI;
        }

        private void LoadGrowthAttribute(AttributeData growth, CharacterDefine define)
        {
            growth.STR = define.GrowthSTR;
            growth.INT = define.GrowthINT;
            growth.DEX = define.GrowthDEX;
        }

        private void LoadEquipAttributes(AttributeData equipAttr,List<EquipDefine> equips)
        {
            equipAttr.Reset();
            if (equips == null) return;
            foreach(var define in equips)
            {
                equipAttr.MaxHP += define.MaxHP;
                equipAttr.MaxMP += define.MaxMP;
                equipAttr.STR += define.STR;
                equipAttr.INT += define.INT;
                equipAttr.DEX += define.DEX;
                equipAttr.AD += define.AD;
                equipAttr.AP += define.AP;
                equipAttr.DEF += define.DEF;
                equipAttr.MDEF += define.MDEF;
                equipAttr.SPD += define.SPD;
                equipAttr.CRI += define.CRI;
            }
        }


        private void InitBasicAttributes()
        {
            for(int i = (int)AttributeType.MaxHP; i < (int)AttributeType.MAX; i++)
            {
                this.Basic.Data[i] = this.Initial.Data[i];
            }
            for(int i = (int)AttributeType.STR; i <= (int)AttributeType.DEX; i++)
            {
                this.Basic.Data[i] = this.Initial.Data[i] + this.Growth.Data[i] * (this.level - 1); //一级属性成长
                this.Basic.Data[i] += this.Equip.Data[i]; // 装备一级属性加成在计算属性前
            }
        }

        private void InitSecondaryAttributes()
        {
            this.Basic.MaxHP = this.Basic.STR * 10 + this.Initial.MaxHP + this.Equip.MaxHP;
            this.Basic.MaxMP = this.Basic.INT * 10 + this.Initial.MaxMP + this.Equip.MaxMP;

            this.Basic.AD = this.Basic.STR * 5 + this.Initial.AD + this.Equip.AD;
            this.Basic.AP = this.Basic.INT * 5 + this.Initial.AP + this.Equip.AP;
            this.Basic.DEF = this.Basic.STR * 2 + this.Initial.DEX*1 + this.Initial.DEF + this.Equip.DEF;
            this.Basic.MDEF = this.Basic.INT * 2 + this.Initial.DEX * 1 + this.Initial.MDEF + this.Equip.MDEF;

            this.Basic.SPD = this.Basic.DEX * 0.2f + this.Initial.SPD + this.Equip.SPD;
            this.Basic.CRI = this.Basic.DEX * 0.0002f + this.Initial.CRI + this.Equip.CRI;
        }

        public void InitFinalAttributes()
        {
            for(int i = (int)AttributeType.MaxHP; i < (int)AttributeType.MAX; i++)
            {
                this.Final.Data[i] = this.Basic.Data[i] + this.Buff.Data[i];
            }
        }
    }
}
