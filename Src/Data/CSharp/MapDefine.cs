//
// Auto Generated Code By excel2csharp
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是解释说明不为空，第二行是变量类型 第三行是变量名称

// Generate From MapDefine.xlsx

namespace Common.Data
{
	public class MapDefine
	{
		public int ID { get; set;} 	// Column1
		public string Name { get; set;} 	// Column2
		public string Type { get; set;} 	// 类型
		public string SubType { get; set;} 	// 子类型
		public bool PKMode { get; set;} 	// PK模式
		public string Resource { get; set;} 	// 图像名称
		public string MiniMap { get; set;} 	// 小地图资源
		public string Music { get; set;} 	// 音乐资源
		public string Description { get; set;} 	// 技能描述
	}
}


// End of Auto Generated Code
