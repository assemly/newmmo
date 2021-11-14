//
// Auto Generated Code By excel2csharp
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是解释说明不为空，第二行是变量类型 第三行是变量名称

// Generate From BuffDefine.xlsx

using SkillBridge.Message;

namespace Common.Data
{
	public class BuffDefine
	{
		public int ID { get; set;} 	// BuffID
		public string Name { get; set;} 	// Column2
		public string Description { get; set;} 	// 描述
		public string Icon { get; set;} 	// 图标
		public TargetType Target { get; set;} 	// 目标类型
		public string Trigger { get; set;} 	// 触发时机
		public BuffEffect Effect { get; set;} 	// Column7
		public int CD { get; set;} 	// CD
		public float Duration { get; set;} 	// 持续时间
		public float Interval { get; set;} 	// 间隔时间
		public float AD { get; set;} 	// 物理攻击
		public float AP { get; set;} 	// 法术攻击
		public float ADFator { get; set;} 	// 物理攻击系数
		public float APFator { get; set;} 	// 法术攻击系数
		public float DEFRatio { get; set;} 	// 防御加成
	}
}


// End of Auto Generated Code
