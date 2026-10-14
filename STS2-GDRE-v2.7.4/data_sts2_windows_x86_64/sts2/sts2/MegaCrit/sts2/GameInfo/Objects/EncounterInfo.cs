using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x02000029 RID: 41
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	[Serializable]
	public class EncounterInfo : IGameInfo
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x000110AE File Offset: 0x0000F2AE
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x000110B6 File Offset: 0x0000F2B6
		[RequiredMember]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x000110BF File Offset: 0x0000F2BF
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x000110C7 File Offset: 0x0000F2C7
		[RequiredMember]
		[JsonPropertyName("bot_keyword")]
		public string BotKeyword { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001CA RID: 458 RVA: 0x000110D0 File Offset: 0x0000F2D0
		// (set) Token: 0x060001CB RID: 459 RVA: 0x000110D8 File Offset: 0x0000F2D8
		[RequiredMember]
		[JsonPropertyName("bot_text")]
		public string BotText { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001CC RID: 460 RVA: 0x000110E1 File Offset: 0x0000F2E1
		// (set) Token: 0x060001CD RID: 461 RVA: 0x000110E9 File Offset: 0x0000F2E9
		[RequiredMember]
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001CE RID: 462 RVA: 0x000110F2 File Offset: 0x0000F2F2
		// (set) Token: 0x060001CF RID: 463 RVA: 0x000110FA File Offset: 0x0000F2FA
		[RequiredMember]
		[JsonPropertyName("act")]
		public string Act { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00011103 File Offset: 0x0000F303
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x0001110B File Offset: 0x0000F30B
		[RequiredMember]
		[JsonPropertyName("tier")]
		public string Tier { get; set; }

		// Token: 0x060001D2 RID: 466 RVA: 0x00011114 File Offset: 0x0000F314
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public EncounterInfo()
		{
		}
	}
}
