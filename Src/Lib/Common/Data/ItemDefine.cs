using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public enum ItemFunction
    {
        RecoverHP,
        RecoverMP,
        AddBuff,
        AddExp,
        AddMoney,
        AddItem,
        AddSkillPoint,
    }
    public class ItemDefine
    {
        public int ID { get; set; }// 
        public string Name { get; set; }// 
        public string Description { get; set; }// 描述
        public ItemType Type { get; set; }// 类型
        public string Category { get; set; }// 类别
        public int Level { get; set; }// 道具等级
        public string LimitClass { get; set; }// 限制职业
        public bool CanUse { get; set; }// 
        public float UseCD { get; set; }// 使用CD(秒)
        public int Price { get; set; }// 购买价格
        public int SellPrice { get; set; }// 销售价格
        public int StackLimit { get; set; }// 堆叠限制
        public string Icon { get; set; }// 图标
        public ItemFunction Function { get; set; }// 功能
        public int Param { get; set; }// 参数
        public List<int> Params { get; set; }// 
    }
}
