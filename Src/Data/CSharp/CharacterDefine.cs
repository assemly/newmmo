//
// Auto Generated Code By excel2csharp
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是解释说明不为空，第二行是变量类型 第三行是变量名称

// Generate From CharacterDefine.xlsx

namespace Common.Data
{
	public class CharacterDefine
	{
		public int TID { get; set;} 	// TypeID
		public string Name { get; set;} 	// Column2
		public string Type { get; set;} 	// 类型
		public string Class { get; set;} 	// 子类型
		public string Resource { get; set;} 	// 模型资源
		public string Description { get; set;} 	// 描述
		public int InitLevel { get; set;} 	// 初始等级
		public float Height { get; set;} 	// 身高(单位:米)
		public int Speed { get; set;} 	// 速度
		public float MaxHP { get; set;} 	// 生命
		public float MaxMP { get; set;} 	// 法力
		public float GrowthSTR { get; set;} 	// 力量成长
		public float GrowthINT { get; set;} 	// 智力成长
		public float GrowthDEX { get; set;} 	// 敏捷成长
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
