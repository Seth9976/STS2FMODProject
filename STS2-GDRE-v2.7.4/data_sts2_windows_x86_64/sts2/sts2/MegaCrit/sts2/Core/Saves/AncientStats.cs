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
	/// Holds progress data on a per ancient basis.
	/// </summary>
	// Token: 0x0200008D RID: 141
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class AncientStats
	{
		/// <summary>
		/// The Id of this ancient!
		/// </summary>
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00015F0F File Offset: 0x0001410F
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x00015F17 File Offset: 0x00014117
		[RequiredMember]
		[JsonPropertyName("ancient_id")]
		public ModelId Id { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x00015F20 File Offset: 0x00014120
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x00015F28 File Offset: 0x00014128
		[JsonPropertyName("character_stats")]
		public List<AncientCharacterStats> CharStats { get; set; } = new List<AncientCharacterStats>();

		/// <summary>
		/// Total number of visits to this ancient across all characters.
		/// </summary>
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x00015F31 File Offset: 0x00014131
		[JsonIgnore]
		public int TotalVisits
		{
			get
			{
				if (this.CharStats.Count == 0)
				{
					return 0;
				}
				return this.CharStats.Sum<AncientCharacterStats>((AncientCharacterStats c) => c.Visits);
			}
		}

		/// <summary>
		/// How many times the player has won a run after encountering this ancient in it.
		/// </summary>
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00015F6C File Offset: 0x0001416C
		[JsonIgnore]
		public int TotalWins
		{
			get
			{
				if (this.CharStats.Count == 0)
				{
					return 0;
				}
				return this.CharStats.Sum<AncientCharacterStats>((AncientCharacterStats fight) => fight.Wins);
			}
		}

		/// <summary>
		/// How many times the player has lost a run after encountering this ancient in it.
		/// </summary>
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x00015FA7 File Offset: 0x000141A7
		[JsonIgnore]
		public int TotalLosses
		{
			get
			{
				if (this.CharStats.Count == 0)
				{
					return 0;
				}
				return this.CharStats.Sum<AncientCharacterStats>((AncientCharacterStats fight) => fight.Losses);
			}
		}

		/// <summary>
		/// Increments a run victory with this ancient.
		/// </summary>
		// Token: 0x06000486 RID: 1158 RVA: 0x00015FE4 File Offset: 0x000141E4
		public void IncrementWin(ModelId characterId)
		{
			AncientCharacterStats ancientCharacterStats = this.GetStats(characterId);
			if (ancientCharacterStats == null)
			{
				ancientCharacterStats = new AncientCharacterStats
				{
					Character = characterId
				};
				this.CharStats.Add(ancientCharacterStats);
			}
			AncientCharacterStats ancientCharacterStats2 = ancientCharacterStats;
			int wins = ancientCharacterStats2.Wins;
			ancientCharacterStats2.Wins = wins + 1;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 3);
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
			defaultInterpolatedStringHandler.AppendLiteral(" has won a run with ancient ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral(". That's ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(ancientCharacterStats.Wins);
			defaultInterpolatedStringHandler.AppendLiteral(" wins");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		/// <summary>
		/// Increments a run loss with this ancient.
		/// </summary>
		// Token: 0x06000487 RID: 1159 RVA: 0x00016084 File Offset: 0x00014284
		public void IncrementLoss(ModelId characterId)
		{
			AncientCharacterStats ancientCharacterStats = this.GetStats(characterId);
			if (ancientCharacterStats == null)
			{
				ancientCharacterStats = new AncientCharacterStats
				{
					Character = characterId
				};
				this.CharStats.Add(ancientCharacterStats);
			}
			AncientCharacterStats ancientCharacterStats2 = ancientCharacterStats;
			int losses = ancientCharacterStats2.Losses;
			ancientCharacterStats2.Losses = losses + 1;
		}

		/// <summary>
		/// Get the number of times the player has visited this ancient as the specified character.
		/// </summary>
		// Token: 0x06000488 RID: 1160 RVA: 0x000160C5 File Offset: 0x000142C5
		public int GetVisitsAs(ModelId characterId)
		{
			AncientCharacterStats stats = this.GetStats(characterId);
			if (stats == null)
			{
				return 0;
			}
			return stats.Visits;
		}

		/// <summary>
		/// Get the stats for this ancient and the specified character.
		/// </summary>
		// Token: 0x06000489 RID: 1161 RVA: 0x000160DC File Offset: 0x000142DC
		[return: Nullable(2)]
		private AncientCharacterStats GetStats(ModelId characterId)
		{
			return this.CharStats.FirstOrDefault<AncientCharacterStats>((AncientCharacterStats c) => c.Character == characterId);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001610D File Offset: 0x0001430D
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public AncientStats()
		{
		}
	}
}
