//
// Auto Generated Code By excel2csharp
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是解释说明不为空，第二行是变量类型 第三行是变量名称

// Generate From QuestDefine.xlsx

using SkillBridge.Message;
using System.ComponentModel;

namespace Common.Data
{
	public enum QuestType
    {
		[Description("主线")]
		Main,
		[Description("支线")]
        Branch
    }

    public enum QuestTarget
    {
		None,
		Kill,
		Item
	}

    public class QuestDefine
	{
		public int ID { get; set;} 	// Column1
		public string Name { get; set;} 	// Column2
		public int LimitLevel { get; set;} 	// 任务等级
		public CharacterClass LimitClass { get; set;} 	// 职业限制
		public int PreQuest { get; set;} 	// 前置任务
		public int PostQuest { get; set;} 	// 后置任务
		public QuestType Type { get; set;} 	// 类型
		public int AcceptNPC { get; set;} 	// 任务接取NPC
		public int SubmitNPC { get; set;} 	// 任务提交NPC
		public string Overview { get; set;} 	// 任务概述
		public string Dialog { get; set;} 	// 任务对话
		public string DialogAccept { get; set;} 	// 任务接受对话
		public string DialogDeny { get; set;} 	// 任务拒绝对话
		public string DialogIncomplete { get; set;} 	// 任务未完成对话
		public string DialogFinish { get; set;} 	// 任务完成对话
		public QuestTarget Target1 { get; set;} 	// 目标1
		public int Target1ID { get; set;} 	// 目标1ID
		public int Target1Num { get; set;} 	// 目标1数量
		public QuestTarget Target2 { get; set;} 	// 目标2
		public int Target2ID { get; set;} 	// 目标2ID
		public int Target2Num { get; set;} 	// 目标2数量
		public QuestTarget Target3 { get; set;} 	// 目标3
		public int Target3ID { get; set;} 	// 目标3ID
		public int Target3Num { get; set;} 	// 目标3数量
		public int RewardGold { get; set;} 	// 奖励金币
		public int RewardExp { get; set;} 	// 奖励经验
		public int RewardItem1 { get; set;} 	// 奖励道具1
		public int RewardItem1Count { get; set;} 	// 奖励道具1数量
		public int RewardItem2 { get; set;} 	// 奖励道具2
		public int RewardItem2Count { get; set;} 	// 奖励道具2数量
		public int RewardItem3 { get; set;} 	// 奖励道具3
		public int RewardItem3Count { get; set;} 	// 奖励道具3数量
	}
}


// End of Auto Generated Code
