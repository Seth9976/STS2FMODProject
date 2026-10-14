using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x02000027 RID: 39
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	[Serializable]
	public class DailyMods : IGameInfo
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00011005 File Offset: 0x0000F205
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x0001100D File Offset: 0x0000F20D
		[RequiredMember]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00011016 File Offset: 0x0000F216
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x0001101E File Offset: 0x0000F21E
		[RequiredMember]
		[JsonPropertyName("bot_keyword")]
		public string BotKeyword { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00011027 File Offset: 0x0000F227
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x0001102F File Offset: 0x0000F22F
		[RequiredMember]
		[JsonPropertyName("bot_text")]
		public string BotText { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00011038 File Offset: 0x0000F238
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00011040 File Offset: 0x0000F240
		[RequiredMember]
		[JsonPropertyName("text")]
		public string Text { get; set; }

		// Token: 0x060001BA RID: 442 RVA: 0x00011049 File Offset: 0x0000F249
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public DailyMods()
		{
		}
	}
}
