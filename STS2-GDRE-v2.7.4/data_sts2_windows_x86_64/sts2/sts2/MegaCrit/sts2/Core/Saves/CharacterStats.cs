using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Holds progress data on a per character basis.
	/// Used by SerializableProgress
	/// </summary>
	// Token: 0x02000090 RID: 144
	[NullableContext(1)]
	[Nullable(0)]
	public class CharacterStats
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x000161B8 File Offset: 0x000143B8
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x000161C0 File Offset: 0x000143C0
		[Nullable(2)]
		[JsonPropertyName("id")]
		public ModelId Id
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x000161C9 File Offset: 0x000143C9
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x000161D1 File Offset: 0x000143D1
		[JsonPropertyName("max_ascension")]
		public int MaxAscension { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x000161DA File Offset: 0x000143DA
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x000161E2 File Offset: 0x000143E2
		[JsonPropertyName("preferred_ascension")]
		public int PreferredAscension { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x000161EB File Offset: 0x000143EB
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x000161F3 File Offset: 0x000143F3
		[JsonPropertyName("total_wins")]
		public int TotalWins { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x000161FC File Offset: 0x000143FC
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x00016204 File Offset: 0x00014404
		[JsonPropertyName("total_losses")]
		public int TotalLosses { get; set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0001620D File Offset: 0x0001440D
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x00016215 File Offset: 0x00014415
		[JsonPropertyName("fastest_win_time")]
		public long FastestWinTime { get; set; } = -1L;

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0001621E File Offset: 0x0001441E
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x00016226 File Offset: 0x00014426
		[JsonPropertyName("best_win_streak")]
		public long BestWinStreak { get; set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001622F File Offset: 0x0001442F
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x00016237 File Offset: 0x00014437
		[JsonPropertyName("current_streak")]
		public long CurrentWinStreak { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00016240 File Offset: 0x00014440
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x00016248 File Offset: 0x00014448
		[JsonPropertyName("playtime")]
		public long Playtime { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00016251 File Offset: 0x00014451
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x00016259 File Offset: 0x00014459
		[JsonPropertyName("badges")]
		public List<BadgeStats> Badges { get; set; } = new List<BadgeStats>();
	}
}
