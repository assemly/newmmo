//
// Auto Generated Code By excel2csharp
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是解释说明不为空，第二行是变量类型 第三行是变量名称

// Generate From SkillDefine.xlsx

using SkillBridge.Message;
using Common.Battle;
using System.Collections.Generic;

namespace Common.Data
{
	public class SkillDefine
	{
		public int ID { get; set;} 	// SkillID
		public string Name { get; set;} 	// 技能名称
		public string Description { get; set;} 	// 描述
		public string Icon { get; set;} 	// 图标
		public SkillType Type { get; set;} 	// 类型
		public int UnlockLevel { get; set;} 	// 解锁等级
		public TargetType CastTarget { get; set;} 	// 施法目标
		public int CastRange { get; set;} 	// 释放范围
		public float CastTime { get; set;} 	// 施法时间
		public float CD { get; set;} 	// 技能CD
		public int MPCost { get; set;} 	// MP消耗
		public bool Bullet { get; set;} 	// 是否为子弹
		public float BulletSpeed { get; set;} 	// 子弹速度
		public string BulletResource { get; set;} 	// 子弹资源
		public int AOERange { get; set;} 	// 区域范围
		public string AOEEffect { get; set;} 	// 区域效果
		public string SkillAnim { get; set;} 	// 技能动作
		public float Duration { get; set;} 	// 持续时间
		public float Interval  { get; set;} 	// 间隔时间
		public List<float> HitTimes { get; set;} 	// 击中时间
		public string HitEffect { get; set;} 	// 击中效果
		public List<int> Buff { get; set;} 	// BUFF
		public float AD { get; set;} 	// 物理攻击
		public float AP { get; set;} 	// 法术攻击
		public float ADFator { get; set;} 	// 物理攻击系数
		public float APFator { get; set;} 	// 法术攻击系数
	}
}


// End of Auto Generated Code
