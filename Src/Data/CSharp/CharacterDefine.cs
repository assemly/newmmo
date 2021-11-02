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
		public float Speed { get; set;} 	// 速度
	}
}


// End of Auto Generated Code
