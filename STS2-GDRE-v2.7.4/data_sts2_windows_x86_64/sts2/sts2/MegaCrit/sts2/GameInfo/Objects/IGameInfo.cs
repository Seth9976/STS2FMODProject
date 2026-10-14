using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	[JsonDerivedType(typeof(AncientChoiceInfo))]
	[JsonDerivedType(typeof(CardInfo))]
	[JsonDerivedType(typeof(DailyMods))]
	[JsonDerivedType(typeof(EncounterInfo))]
	[JsonDerivedType(typeof(EventInfo))]
	[JsonDerivedType(typeof(Keywords))]
	[JsonDerivedType(typeof(NeowBonusInfo))]
	[JsonDerivedType(typeof(PotionInfo))]
	[JsonDerivedType(typeof(RelicInfo))]
	[JsonDerivedType(typeof(EnchantmentInfo))]
	public interface IGameInfo
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001E0 RID: 480
		[JsonPropertyName("name")]
		string Name { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001E1 RID: 481
		[JsonPropertyName("bot_keyword")]
		string BotKeyword { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001E2 RID: 482
		[JsonPropertyName("bot_text")]
		string BotText { get; }
	}
}
