//
// Auto Generated Code By excel2csharp
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是解释说明不为空，第二行是变量类型 第三行是变量名称

// Generate From EquipDefine.xlsx

using SkillBridge.Message;

namespace Common.Data
{
	public class EquipDefine
	{
		public int ID { get; set;} 	// Column1
		public string Name { get; set;} 	// Column2
		public EquipSlot Slot { get; set;} 	// 类型
		public string Category { get; set;} 	// 类别
		public float MaxHP { get; set;} 	// 生命
		public float MaxMP { get; set;} 	// 法力
		public float STR { get; set;} 	// 力量
		public float INT { get; set;} 	// 智力
		public float DEX { get; set;} 	// 敏捷
		public float AD { get; set;} 	// 物理攻击
		public float AP { get; set;} 	// 法术攻击
		public float DEF { get; set;} 	// 物理防御
		public float MDEF { get; set;} 	// 法术防御
		public float SPD { get; set;} 	// 攻击速度
		public float CRI { get; set;} 	// 暴击概率
	}
}


// End of Auto Generated Code
