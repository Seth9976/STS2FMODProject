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
	/// Holds progress data on a per enemy creature basis.
	/// Used by SerializableProgress. Useful for the Bestiary, Statistics, Achievements, and Epochs.
	/// </summary>
	// Token: 0x02000095 RID: 149
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class EnemyStats
	{
		/// <summary>
		/// The Id of this enemy!
		/// </summary>
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00016ED3 File Offset: 0x000150D3
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x00016EDB File Offset: 0x000150DB
		[RequiredMember]
		[JsonPropertyName("enemy_id")]
		public ModelId Id { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00016EE4 File Offset: 0x000150E4
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x00016EEC File Offset: 0x000150EC
		[JsonPropertyName("fight_stats")]
		public List<FightStats> FightStats { get; set; } = new List<FightStats>();

		/// <summary>
		/// How many times the player has won against this enemy.
		/// </summary>
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00016EF5 File Offset: 0x000150F5
		[JsonIgnore]
		public int TotalWins
		{
			get
			{
				if (this.FightStats.Count == 0)
				{
					return 0;
				}
				return this.FightStats.Sum<FightStats>((FightStats f) => f.Wins);
			}
		}

		/// <summary>
		/// How many times the player has died to this enemy.
		/// </summary>
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00016F30 File Offset: 0x00015130
		[JsonIgnore]
		public int TotalLosses
		{
			get
			{
				if (this.FightStats.Count == 0)
				{
					return 0;
				}
				return this.FightStats.Sum<FightStats>((FightStats f) => f.Losses);
			}
		}

		/// <summary>
		/// Increments a victory against this enemy.
		/// </summary>
		// Token: 0x060004EC RID: 1260 RVA: 0x00016F6C File Offset: 0x0001516C
		public void IncrementWin(ModelId characterId)
		{
			FightStats fightStats = this.FightStats.First<FightStats>((FightStats fight) => fight.Character == characterId);
			FightStats fightStats2 = fightStats;
			int wins = fightStats2.Wins;
			fightStats2.Wins = wins + 1;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 3);
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
			defaultInterpolatedStringHandler.AppendLiteral(" has killed a ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral(". That's ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(fightStats.Wins);
			defaultInterpolatedStringHandler.AppendLiteral(" kills");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		/// <summary>
		/// Increments a loss against this enemy.
		/// </summary>
		// Token: 0x060004ED RID: 1261 RVA: 0x00017010 File Offset: 0x00015210
		public void IncrementLoss(ModelId characterId)
		{
			FightStats fightStats = this.FightStats.First<FightStats>((FightStats fight) => fight.Character == characterId);
			FightStats fightStats2 = fightStats;
			int losses = fightStats2.Losses;
			fightStats2.Losses = losses + 1;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 3);
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
			defaultInterpolatedStringHandler.AppendLiteral(" has died to a ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral(". That's ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(fightStats.Losses);
			defaultInterpolatedStringHandler.AppendLiteral(" losses");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000170B4 File Offset: 0x000152B4
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public EnemyStats()
		{
		}
	}
}
