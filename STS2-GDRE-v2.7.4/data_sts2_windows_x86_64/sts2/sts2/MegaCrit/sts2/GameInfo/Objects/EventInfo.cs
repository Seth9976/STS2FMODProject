using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x0200002A RID: 42
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	[Serializable]
	public class EventInfo : IGameInfo
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0001111C File Offset: 0x0000F31C
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00011124 File Offset: 0x0000F324
		[RequiredMember]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0001112D File Offset: 0x0000F32D
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00011135 File Offset: 0x0000F335
		[RequiredMember]
		[JsonPropertyName("bot_keyword")]
		public string BotKeyword { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0001113E File Offset: 0x0000F33E
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00011146 File Offset: 0x0000F346
		[RequiredMember]
		[JsonPropertyName("bot_text")]
		public string BotText { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0001114F File Offset: 0x0000F34F
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00011157 File Offset: 0x0000F357
		[RequiredMember]
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00011160 File Offset: 0x0000F360
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00011168 File Offset: 0x0000F368
		[RequiredMember]
		[JsonPropertyName("act")]
		public string Act { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00011171 File Offset: 0x0000F371
		// (set) Token: 0x060001DE RID: 478 RVA: 0x00011179 File Offset: 0x0000F379
		[RequiredMember]
		[JsonPropertyName("options")]
		public List<string> Options { get; set; }

		// Token: 0x060001DF RID: 479 RVA: 0x00011182 File Offset: 0x0000F382
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public EventInfo()
		{
		}
	}
}
