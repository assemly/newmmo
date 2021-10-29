//
// Auto Generated Code By excel2csharp
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是解释说明不为空，第二行是变量类型 第三行是变量名称

// Generate From .\CSharp\ItemDefine.xlsx

namespace Common.Data
{
	public class NPC
	{
		public int ID { get; set;}// Column1
		public string Name { get; set;}// Column2
		public string Description { get; set;}// 描述
		public string Type { get; set;}// 类型
		public string Category { get; set;}// 类别
		public int Level { get; set;}// 道具等级
		public string LimitClass { get; set;}// 限制职业
		public bool CanUse { get; set;}// Column8
		public Float UseCD { get; set;}// 使用CD(秒)
		public int Price { get; set;}// 购买价格
		public int SellPrice { get; set;}// 销售价格
		public int StackLimit { get; set;}// 堆叠限制
		public string Icon { get; set;}// 图标
		public string Function { get; set;}// 功能
		public int Param { get; set;}// 参数
		public list<int> Params { get; set;}// Column16
	}
}


// End of Auto Generated Code
