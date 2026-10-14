using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// The player's gameplay progress for Slay the Spire 2.
	/// This would be considered the most important save file and is synced across devices/platforms.
	/// There would be a progress.save in each respective profile's folder and contains data such as:
	/// - Total wins, losses, playtime, and progress per character
	/// - Which cards/relics/potions/etc have been seen or unlocked
	/// - Which enemies/elites/bosses have been encountered and defeated
	/// - Character data
	/// - Timeline and Unlock progress (meta progression)
	/// - Unlocked achievements
	/// - Which FTUEs this player has seen
	/// </summary>
	// Token: 0x020000B1 RID: 177
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableProgress : ISaveSchema
	{
		// Token: 0x06000820 RID: 2080 RVA: 0x000404F0 File Offset: 0x0003E6F0
		public SerializableProgress()
		{
			this.UniqueId = SerializableProgress.<.ctor>g__GenerateUniqueId|0_0(7);
		}

		/// <summary>
		/// The schema version of this save.
		/// </summary>
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x000405B0 File Offset: 0x0003E7B0
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x000405B8 File Offset: 0x0003E7B8
		[JsonPropertyName("schema_version")]
		public int SchemaVersion { get; set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x000405C1 File Offset: 0x0003E7C1
		// (set) Token: 0x06000824 RID: 2084 RVA: 0x000405C9 File Offset: 0x0003E7C9
		[JsonPropertyName("unique_id")]
		public string UniqueId { get; set; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x000405D2 File Offset: 0x0003E7D2
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x000405DA File Offset: 0x0003E7DA
		[JsonPropertyName("character_stats")]
		public List<CharacterStats> CharStats { get; set; } = new List<CharacterStats>();

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x000405E3 File Offset: 0x0003E7E3
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x000405EB File Offset: 0x0003E7EB
		[JsonPropertyName("card_stats")]
		public List<CardStats> CardStats { get; set; } = new List<CardStats>();

		/// <summary>
		/// Tracks how often each character has encountered, won, and lost against each Encounter.
		/// </summary>
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x000405F4 File Offset: 0x0003E7F4
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x000405FC File Offset: 0x0003E7FC
		[JsonPropertyName("encounter_stats")]
		public List<EncounterStats> EncounterStats { get; set; } = new List<EncounterStats>();

		/// <summary>
		/// Tracks how often each character has encountered, won, and lost against each Enemy Creature.
		/// </summary>
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00040605 File Offset: 0x0003E805
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x0004060D File Offset: 0x0003E80D
		[JsonPropertyName("enemy_stats")]
		public List<EnemyStats> EnemyStats { get; set; } = new List<EnemyStats>();

		/// <summary>
		/// Tracks how often each character has encountered, won, and lost after encountering each ancient.
		/// </summary>
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00040616 File Offset: 0x0003E816
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x0004061E File Offset: 0x0003E81E
		[JsonPropertyName("ancient_stats")]
		public List<AncientStats> AncientStats { get; set; } = new List<AncientStats>();

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x00040627 File Offset: 0x0003E827
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0004062F File Offset: 0x0003E82F
		[JsonPropertyName("enable_ftues")]
		public bool EnableFtues { get; set; } = true;

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00040638 File Offset: 0x0003E838
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x00040640 File Offset: 0x0003E840
		[JsonPropertyName("epochs")]
		public List<SerializableEpoch> Epochs { get; set; } = new List<SerializableEpoch>();

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00040649 File Offset: 0x0003E849
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x00040651 File Offset: 0x0003E851
		[JsonPropertyName("ftue_completed")]
		public List<string> FtueCompleted { get; set; } = new List<string>();

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0004065A File Offset: 0x0003E85A
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x00040662 File Offset: 0x0003E862
		[JsonPropertyName("unlocked_achievements")]
		public List<SerializableUnlockedAchievement> UnlockedAchievements { get; set; } = new List<SerializableUnlockedAchievement>();

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x0004066B File Offset: 0x0003E86B
		// (set) Token: 0x06000838 RID: 2104 RVA: 0x00040673 File Offset: 0x0003E873
		[JsonPropertyName("discovered_cards")]
		public List<ModelId> DiscoveredCards { get; set; } = new List<ModelId>();

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x0004067C File Offset: 0x0003E87C
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x00040684 File Offset: 0x0003E884
		[JsonPropertyName("discovered_relics")]
		public List<ModelId> DiscoveredRelics { get; set; } = new List<ModelId>();

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x0004068D File Offset: 0x0003E88D
		// (set) Token: 0x0600083C RID: 2108 RVA: 0x00040695 File Offset: 0x0003E895
		[JsonPropertyName("discovered_events")]
		public List<ModelId> DiscoveredEvents { get; set; } = new List<ModelId>();

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0004069E File Offset: 0x0003E89E
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x000406A6 File Offset: 0x0003E8A6
		[JsonPropertyName("discovered_potions")]
		public List<ModelId> DiscoveredPotions { get; set; } = new List<ModelId>();

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x000406AF File Offset: 0x0003E8AF
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x000406B7 File Offset: 0x0003E8B7
		[JsonPropertyName("discovered_acts")]
		public List<ModelId> DiscoveredActs { get; set; } = new List<ModelId>();

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x000406C0 File Offset: 0x0003E8C0
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x000406C8 File Offset: 0x0003E8C8
		[JsonPropertyName("total_playtime")]
		public long TotalPlaytime { get; set; }

		/// <summary>
		/// The amount of agnostic unlocks via score system this player has unlocked.
		/// </summary>
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x000406D1 File Offset: 0x0003E8D1
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x000406D9 File Offset: 0x0003E8D9
		[JsonPropertyName("total_unlocks")]
		public int TotalUnlocks { get; set; }

		/// <summary>
		/// How much score this player has (is not cumulative).
		/// </summary>
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x000406E2 File Offset: 0x0003E8E2
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x000406EA File Offset: 0x0003E8EA
		[JsonPropertyName("current_score")]
		public int CurrentScore { get; set; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x000406F3 File Offset: 0x0003E8F3
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x000406FB File Offset: 0x0003E8FB
		[JsonPropertyName("floors_climbed")]
		public long FloorsClimbed { get; set; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x00040704 File Offset: 0x0003E904
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x0004070C File Offset: 0x0003E90C
		[JsonPropertyName("architect_damage")]
		public long ArchitectDamage { get; set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x00040715 File Offset: 0x0003E915
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x0004071D File Offset: 0x0003E91D
		[JsonPropertyName("wongo_points")]
		public int WongoPoints { get; set; }

		/// <summary> Which multiplayer ascension we had selected the last time we started a multiplayer run. </summary>
		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x00040726 File Offset: 0x0003E926
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x0004072E File Offset: 0x0003E92E
		[JsonPropertyName("preferred_multiplayer_ascension")]
		public int PreferredMultiplayerAscension { get; set; }

		/// <summary>
		/// The maximum unlocked ascension in multiplayer.
		/// In singleplayer runs, we use character stats. In multiplayer runs, all characters share an ascension.
		/// </summary>
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x00040737 File Offset: 0x0003E937
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x0004073F File Offset: 0x0003E93F
		[JsonPropertyName("max_multiplayer_ascension")]
		public int MaxMultiplayerAscension { get; set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x00040748 File Offset: 0x0003E948
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x00040750 File Offset: 0x0003E950
		[JsonPropertyName("test_subject_kills")]
		public int TestSubjectKills { get; set; }

		/// <summary>
		/// Characters that have been unlocked between the last run and the next time the player opens the character
		/// select screen.
		/// Used to show an animation on the character select screen.
		/// </summary>
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x00040759 File Offset: 0x0003E959
		// (set) Token: 0x06000854 RID: 2132 RVA: 0x00040761 File Offset: 0x0003E961
		[JsonPropertyName("pending_character_unlock")]
		public ModelId PendingCharacterUnlock { get; set; } = ModelId.none;

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x0004076A File Offset: 0x0003E96A
		[JsonIgnore]
		public int Wins
		{
			get
			{
				return this.CharStats.Sum<CharacterStats>((CharacterStats character) => character.TotalWins);
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x00040796 File Offset: 0x0003E996
		[JsonIgnore]
		public int Losses
		{
			get
			{
				return this.CharStats.Sum<CharacterStats>((CharacterStats character) => character.TotalLosses);
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x000407C2 File Offset: 0x0003E9C2
		[JsonIgnore]
		public long FastestVictory
		{
			get
			{
				if (this.CharStats.Count == 0)
				{
					return 999999999L;
				}
				return this.CharStats.Min<CharacterStats>(delegate(CharacterStats c)
				{
					if (c.FastestWinTime != -1L)
					{
						return c.FastestWinTime;
					}
					return 999999999L;
				});
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x00040802 File Offset: 0x0003EA02
		[JsonIgnore]
		public long BestWinStreak
		{
			get
			{
				if (this.CharStats.Count == 0)
				{
					return 0L;
				}
				return this.CharStats.Max<CharacterStats>((CharacterStats c) => c.BestWinStreak);
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0004083E File Offset: 0x0003EA3E
		[JsonIgnore]
		public int NumberOfRuns
		{
			get
			{
				return this.Wins + this.Losses;
			}
		}

		/// <summary>
		/// Get the stats for the specified character.
		/// </summary>
		// Token: 0x0600085A RID: 2138 RVA: 0x00040850 File Offset: 0x0003EA50
		[return: Nullable(2)]
		public CharacterStats GetStatsForCharacter(ModelId characterId)
		{
			return this.CharStats.FirstOrDefault<CharacterStats>((CharacterStats c) => c.Id == characterId);
		}

		/// <summary>
		/// Get the stats for the specified ancient.
		/// </summary>
		// Token: 0x0600085B RID: 2139 RVA: 0x00040884 File Offset: 0x0003EA84
		[return: Nullable(2)]
		public AncientStats GetStatsForAncient(ModelId ancientId)
		{
			return this.AncientStats.FirstOrDefault<AncientStats>((AncientStats a) => a.Id == ancientId);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x000408B5 File Offset: 0x0003EAB5
		[CompilerGenerated]
		internal static string <.ctor>g__GenerateUniqueId|0_0(int length = 7)
		{
			return new string((from s in Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
				select s[Rng.Chaotic.NextInt(s.Length)]).ToArray<char>());
		}
	}
}
