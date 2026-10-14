using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Holds progress data on a per card basis.
	/// Used by SerializableProgress
	/// </summary>
	// Token: 0x0200008F RID: 143
	[NullableContext(2)]
	[Nullable(0)]
	public class CardStats
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x0001615B File Offset: 0x0001435B
		// (set) Token: 0x06000493 RID: 1171 RVA: 0x00016163 File Offset: 0x00014363
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x0001616C File Offset: 0x0001436C
		// (set) Token: 0x06000495 RID: 1173 RVA: 0x00016174 File Offset: 0x00014374
		[JsonPropertyName("times_picked")]
		public long TimesPicked { get; set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x0001617D File Offset: 0x0001437D
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x00016185 File Offset: 0x00014385
		[JsonPropertyName("times_skipped")]
		public long TimesSkipped { get; set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0001618E File Offset: 0x0001438E
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x00016196 File Offset: 0x00014396
		[JsonPropertyName("times_won")]
		public long TimesWon { get; set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001619F File Offset: 0x0001439F
		// (set) Token: 0x0600049B RID: 1179 RVA: 0x000161A7 File Offset: 0x000143A7
		[JsonPropertyName("times_lost")]
		public long TimesLost { get; set; }
	}
}
