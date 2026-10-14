using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Holds progress data on a per encounter basis.
	/// Used by SerializableProgress. Useful for the Bestiary, Statistics, Achievements, and Epochs.
	/// </summary>
	// Token: 0x02000094 RID: 148
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class EncounterStats
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00016CDE File Offset: 0x00014EDE
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x00016CE6 File Offset: 0x00014EE6
		[RequiredMember]
		[JsonPropertyName("encounter_id")]
		public ModelId Id { get; set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00016CEF File Offset: 0x00014EEF
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x00016CF7 File Offset: 0x00014EF7
		[JsonPropertyName("fight_stats")]
		public List<FightStats> FightStats { get; set; } = new List<FightStats>();

		/// <summary>
		/// How many times the player has won against this encounter.
		/// </summary>
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00016D00 File Offset: 0x00014F00
		[JsonIgnore]
		public int TotalWins
		{
			get
			{
				if (this.FightStats.Count == 0)
				{
					return 0;
				}
				return this.FightStats.Sum<FightStats>((FightStats fight) => fight.Wins);
			}
		}

		/// <summary>
		/// How many times the player has died to this encounter.
		/// </summary>
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00016D3B File Offset: 0x00014F3B
		[JsonIgnore]
		public int TotalLosses
		{
			get
			{
				if (this.FightStats.Count == 0)
				{
					return 0;
				}
				return this.FightStats.Sum<FightStats>((FightStats fight) => fight.Losses);
			}
		}

		/// <summary>
		/// Increments a victory against this encounter.
		/// </summary>
		// Token: 0x060004E3 RID: 1251 RVA: 0x00016D78 File Offset: 0x00014F78
		public void IncrementWin(ModelId characterId)
		{
			FightStats fightStats = this.FightStats.First<FightStats>((FightStats fight) => fight.Character == characterId);
			FightStats fightStats2 = fightStats;
			int wins = fightStats2.Wins;
			fightStats2.Wins = wins + 1;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 3);
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
			defaultInterpolatedStringHandler.AppendLiteral(" has won against encounter ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral(". That's ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(fightStats.Wins);
			defaultInterpolatedStringHandler.AppendLiteral(" wins");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		/// <summary>
		/// Increments a loss against this encounter.
		/// </summary>
		// Token: 0x060004E4 RID: 1252 RVA: 0x00016E1C File Offset: 0x0001501C
		public void IncrementLoss(ModelId characterId)
		{
			FightStats fightStats = this.FightStats.First<FightStats>((FightStats fight) => fight.Character == characterId);
			FightStats fightStats2 = fightStats;
			int losses = fightStats2.Losses;
			fightStats2.Losses = losses + 1;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 3);
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
			defaultInterpolatedStringHandler.AppendLiteral(" has lost to encounter ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral(". That's ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(fightStats.Losses);
			defaultInterpolatedStringHandler.AppendLiteral(" losses");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00016EC0 File Offset: 0x000150C0
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public EncounterStats()
		{
		}
	}
}
