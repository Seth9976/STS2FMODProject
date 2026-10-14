using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models.Badges;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Holds progress data on a single Badge.
	/// Represents which badges a character has acquired.
	/// Used by CharacterStats
	/// </summary>
	// Token: 0x0200008E RID: 142
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class BadgeStats
	{
		/// <summary>
		/// The badge this BadgeStat entry is for.
		/// </summary>
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x00016120 File Offset: 0x00014320
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x00016128 File Offset: 0x00014328
		[RequiredMember]
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// How many times this character has acquired this badge.
		/// </summary>
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00016131 File Offset: 0x00014331
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x00016139 File Offset: 0x00014339
		[RequiredMember]
		[JsonPropertyName("count")]
		public int Count { get; set; }

		/// <summary>
		/// The rarity of this badge.
		/// </summary>
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00016142 File Offset: 0x00014342
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x0001614A File Offset: 0x0001434A
		[RequiredMember]
		[JsonPropertyName("rarity")]
		public BadgeRarity Rarity { get; set; }

		// Token: 0x06000491 RID: 1169 RVA: 0x00016153 File Offset: 0x00014353
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public BadgeStats()
		{
		}
	}
}
