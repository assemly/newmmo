//
// Auto Generated Code By excel2csharp
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是解释说明不为空，第二行是变量类型 第三行是变量名称

// Generate From RideDefine.xlsx

using SkillBridge.Message;

namespace Common.Data
{
	public class RideDefine
	{
		public int ID { get; set;} 	// Column1
		public string Name { get; set;} 	// Column2
		public string Description { get; set;} 	// 描述
		public int Level { get; set;} 	// 道具等级
		public CharacterClass LimitClass { get; set;} 	// 限制职业
		public string Resource { get; set;} 	// 资源
		public string Icon { get; set;} 	// 图标
	}
}


// End of Auto Generated Code
