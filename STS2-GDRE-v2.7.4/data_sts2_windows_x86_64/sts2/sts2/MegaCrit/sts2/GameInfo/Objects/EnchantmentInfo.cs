using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x02000028 RID: 40
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	[Serializable]
	public class EnchantmentInfo : IGameInfo
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00011051 File Offset: 0x0000F251
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00011059 File Offset: 0x0000F259
		[RequiredMember]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00011062 File Offset: 0x0000F262
		// (set) Token: 0x060001BE RID: 446 RVA: 0x0001106A File Offset: 0x0000F26A
		[RequiredMember]
		[JsonPropertyName("bot_keyword")]
		public string BotKeyword { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00011073 File Offset: 0x0000F273
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x0001107B File Offset: 0x0000F27B
		[RequiredMember]
		[JsonPropertyName("bot_text")]
		public string BotText { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00011084 File Offset: 0x0000F284
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x0001108C File Offset: 0x0000F28C
		[RequiredMember]
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00011095 File Offset: 0x0000F295
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x0001109D File Offset: 0x0000F29D
		[RequiredMember]
		[JsonPropertyName("text")]
		public string Text { get; set; }

		// Token: 0x060001C5 RID: 453 RVA: 0x000110A6 File Offset: 0x0000F2A6
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public EnchantmentInfo()
		{
		}
	}
}
