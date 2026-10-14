using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x0200002D RID: 45
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	[Serializable]
	public class NeowBonusInfo : IGameInfo
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001EA RID: 490 RVA: 0x000111C5 File Offset: 0x0000F3C5
		// (set) Token: 0x060001EB RID: 491 RVA: 0x000111CD File Offset: 0x0000F3CD
		[RequiredMember]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000111D6 File Offset: 0x0000F3D6
		// (set) Token: 0x060001ED RID: 493 RVA: 0x000111DE File Offset: 0x0000F3DE
		[RequiredMember]
		[JsonPropertyName("bot_keyword")]
		public string BotKeyword { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000111E7 File Offset: 0x0000F3E7
		// (set) Token: 0x060001EF RID: 495 RVA: 0x000111EF File Offset: 0x0000F3EF
		[RequiredMember]
		[JsonPropertyName("bot_text")]
		public string BotText { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x000111F8 File Offset: 0x0000F3F8
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x00011200 File Offset: 0x0000F400
		[RequiredMember]
		[JsonPropertyName("id")]
		public string Id { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00011209 File Offset: 0x0000F409
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x00011211 File Offset: 0x0000F411
		[RequiredMember]
		[JsonPropertyName("text")]
		public string Text { get; set; }

		// Token: 0x060001F4 RID: 500 RVA: 0x0001121A File Offset: 0x0000F41A
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public NeowBonusInfo()
		{
		}
	}
}
