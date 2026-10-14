using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x0200002C RID: 44
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	[Serializable]
	public class Keywords : IGameInfo
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0001118A File Offset: 0x0000F38A
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00011192 File Offset: 0x0000F392
		[RequiredMember]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0001119B File Offset: 0x0000F39B
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x000111A3 File Offset: 0x0000F3A3
		[RequiredMember]
		[JsonPropertyName("bot_keyword")]
		public string BotKeyword { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x000111AC File Offset: 0x0000F3AC
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x000111B4 File Offset: 0x0000F3B4
		[RequiredMember]
		[JsonPropertyName("bot_text")]
		public string BotText { get; set; }

		// Token: 0x060001E9 RID: 489 RVA: 0x000111BD File Offset: 0x0000F3BD
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public Keywords()
		{
		}
	}
}
