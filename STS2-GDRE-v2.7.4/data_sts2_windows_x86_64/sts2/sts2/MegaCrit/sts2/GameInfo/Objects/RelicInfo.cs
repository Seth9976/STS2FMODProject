using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x0200002F RID: 47
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	[Serializable]
	public class RelicInfo : IGameInfo
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000112A1 File Offset: 0x0000F4A1
		// (set) Token: 0x06000205 RID: 517 RVA: 0x000112A9 File Offset: 0x0000F4A9
		[RequiredMember]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000206 RID: 518 RVA: 0x000112B2 File Offset: 0x0000F4B2
		// (set) Token: 0x06000207 RID: 519 RVA: 0x000112BA File Offset: 0x0000F4BA
		[RequiredMember]
		[JsonPropertyName("bot_keyword")]
		public string BotKeyword { get; set; }

		/// This is for feedback bot (the discord bot), the text here is what it will display when called.
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000208 RID: 520 RVA: 0x000112C3 File Offset: 0x0000F4C3
		// (set) Token: 0x06000209 RID: 521 RVA: 0x000112CB File Offset: 0x0000F4CB
		[RequiredMember]
		[JsonPropertyName("bot_text")]
		public string BotText { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600020A RID: 522 RVA: 0x000112D4 File Offset: 0x0000F4D4
		// (set) Token: 0x0600020B RID: 523 RVA: 0x000112DC File Offset: 0x0000F4DC
		[RequiredMember]
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600020C RID: 524 RVA: 0x000112E5 File Offset: 0x0000F4E5
		// (set) Token: 0x0600020D RID: 525 RVA: 0x000112ED File Offset: 0x0000F4ED
		[RequiredMember]
		[JsonPropertyName("rarity")]
		public string Rarity { get; set; }

		/// This is for the metrics system, the text here is what will be displayed in the metrics website screen.
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600020E RID: 526 RVA: 0x000112F6 File Offset: 0x0000F4F6
		// (set) Token: 0x0600020F RID: 527 RVA: 0x000112FE File Offset: 0x0000F4FE
		[RequiredMember]
		[JsonPropertyName("text")]
		public string Text { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00011307 File Offset: 0x0000F507
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0001130F File Offset: 0x0000F50F
		[RequiredMember]
		[JsonPropertyName("color")]
		public string Color { get; set; }

		// Token: 0x06000212 RID: 530 RVA: 0x00011318 File Offset: 0x0000F518
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public RelicInfo()
		{
		}
	}
}
