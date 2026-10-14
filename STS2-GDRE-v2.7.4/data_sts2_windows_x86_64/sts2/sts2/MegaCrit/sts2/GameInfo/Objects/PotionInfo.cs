using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x0200002E RID: 46
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	[Serializable]
	public class PotionInfo : IGameInfo
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00011222 File Offset: 0x0000F422
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x0001122A File Offset: 0x0000F42A
		[RequiredMember]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00011233 File Offset: 0x0000F433
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x0001123B File Offset: 0x0000F43B
		[RequiredMember]
		[JsonPropertyName("bot_keyword")]
		public string BotKeyword { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00011244 File Offset: 0x0000F444
		// (set) Token: 0x060001FA RID: 506 RVA: 0x0001124C File Offset: 0x0000F44C
		[RequiredMember]
		[JsonPropertyName("bot_text")]
		public string BotText { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00011255 File Offset: 0x0000F455
		// (set) Token: 0x060001FC RID: 508 RVA: 0x0001125D File Offset: 0x0000F45D
		[RequiredMember]
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00011266 File Offset: 0x0000F466
		// (set) Token: 0x060001FE RID: 510 RVA: 0x0001126E File Offset: 0x0000F46E
		[RequiredMember]
		[JsonPropertyName("rarity")]
		public string Rarity { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00011277 File Offset: 0x0000F477
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0001127F File Offset: 0x0000F47F
		[RequiredMember]
		[JsonPropertyName("text")]
		public string Text { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00011288 File Offset: 0x0000F488
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00011290 File Offset: 0x0000F490
		[RequiredMember]
		[JsonPropertyName("color")]
		public string Color { get; set; }

		// Token: 0x06000203 RID: 515 RVA: 0x00011299 File Offset: 0x0000F499
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public PotionInfo()
		{
		}
	}
}
