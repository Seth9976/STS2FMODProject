using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves.Validation;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Domain type for player progress. Separates runtime representation from the JSON wire format
	/// (<see cref="T:MegaCrit.Sts2.Core.Saves.SerializableProgress" />). Uses dictionaries for stat lookups and hashsets for deduplication
	/// of discovered content.
	/// </summary>
	// Token: 0x020000A6 RID: 166
	[NullableContext(1)]
	[Nullable(0)]
	public class ProgressState
	{
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0003C37C File Offset: 0x0003A57C
		public IReadOnlyDictionary<ModelId, CharacterStats> CharacterStats
		{
			get
			{
				return this._characterStats;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0003C384 File Offset: 0x0003A584
		public IReadOnlyDictionary<ModelId, CardStats> CardStats
		{
			get
			{
				return this._cardStats;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0003C38C File Offset: 0x0003A58C
		public IReadOnlyDictionary<ModelId, EncounterStats> EncounterStats
		{
			get
			{
				return this._encounterStats;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0003C394 File Offset: 0x0003A594
		public IReadOnlyDictionary<ModelId, EnemyStats> EnemyStats
		{
			get
			{
				return this._enemyStats;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0003C39C File Offset: 0x0003A59C
		public IReadOnlyDictionary<ModelId, AncientStats> AncientStats
		{
			get
			{
				return this._ancientStats;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0003C3A4 File Offset: 0x0003A5A4
		public IReadOnlySet<ModelId> DiscoveredCards
		{
			get
			{
				return this._discoveredCards;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0003C3AC File Offset: 0x0003A5AC
		public IReadOnlySet<ModelId> DiscoveredRelics
		{
			get
			{
				return this._discoveredRelics;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0003C3B4 File Offset: 0x0003A5B4
		public IReadOnlySet<ModelId> DiscoveredPotions
		{
			get
			{
				return this._discoveredPotions;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0003C3BC File Offset: 0x0003A5BC
		public IReadOnlySet<ModelId> DiscoveredEvents
		{
			get
			{
				return this._discoveredEvents;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0003C3C4 File Offset: 0x0003A5C4
		public IReadOnlySet<ModelId> DiscoveredActs
		{
			get
			{
				return this._discoveredActs;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0003C3CC File Offset: 0x0003A5CC
		public IReadOnlyList<SerializableEpoch> Epochs
		{
			get
			{
				return this._epochs;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0003C3D4 File Offset: 0x0003A5D4
		public IReadOnlyDictionary<Achievement, long> UnlockedAchievements
		{
			get
			{
				return this._unlockedAchievements;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0003C3DC File Offset: 0x0003A5DC
		public IReadOnlySet<string> FtueCompleted
		{
			get
			{
				return this._ftueCompleted;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0003C3E4 File Offset: 0x0003A5E4
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0003C3EC File Offset: 0x0003A5EC
		public string UniqueId { get; set; } = "";

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0003C3F5 File Offset: 0x0003A5F5
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0003C3FD File Offset: 0x0003A5FD
		public bool EnableFtues { get; set; } = true;

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0003C406 File Offset: 0x0003A606
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x0003C40E File Offset: 0x0003A60E
		public long TotalPlaytime { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0003C417 File Offset: 0x0003A617
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x0003C41F File Offset: 0x0003A61F
		public int TotalUnlocks { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0003C428 File Offset: 0x0003A628
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x0003C430 File Offset: 0x0003A630
		public int CurrentScore { get; set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0003C439 File Offset: 0x0003A639
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x0003C441 File Offset: 0x0003A641
		public long FloorsClimbed { get; set; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x0003C44A File Offset: 0x0003A64A
		// (set) Token: 0x0600073E RID: 1854 RVA: 0x0003C452 File Offset: 0x0003A652
		public long ArchitectDamage { get; set; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0003C45B File Offset: 0x0003A65B
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x0003C463 File Offset: 0x0003A663
		public int WongoPoints { get; set; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0003C46C File Offset: 0x0003A66C
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x0003C474 File Offset: 0x0003A674
		public int PreferredMultiplayerAscension { get; set; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x0003C47D File Offset: 0x0003A67D
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x0003C485 File Offset: 0x0003A685
		public int MaxMultiplayerAscension { get; set; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x0003C48E File Offset: 0x0003A68E
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x0003C496 File Offset: 0x0003A696
		public int TestSubjectKills { get; set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x0003C49F File Offset: 0x0003A69F
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x0003C4A7 File Offset: 0x0003A6A7
		public ModelId PendingCharacterUnlock { get; set; } = ModelId.none;

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0003C4B0 File Offset: 0x0003A6B0
		public int Wins
		{
			get
			{
				return this._characterStats.Values.Sum<CharacterStats>((CharacterStats c) => c.TotalWins);
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x0003C4E1 File Offset: 0x0003A6E1
		public int Losses
		{
			get
			{
				return this._characterStats.Values.Sum<CharacterStats>((CharacterStats c) => c.TotalLosses);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0003C514 File Offset: 0x0003A714
		public long FastestVictory
		{
			get
			{
				if (this._characterStats.Count == 0)
				{
					return 999999999L;
				}
				return this._characterStats.Values.Min<CharacterStats>(delegate(CharacterStats c)
				{
					if (c.FastestWinTime != -1L)
					{
						return c.FastestWinTime;
					}
					return 999999999L;
				});
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0003C564 File Offset: 0x0003A764
		public long BestWinStreak
		{
			get
			{
				if (this._characterStats.Count == 0)
				{
					return 0L;
				}
				return this._characterStats.Values.Max<CharacterStats>((CharacterStats c) => c.BestWinStreak);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0003C5B0 File Offset: 0x0003A7B0
		public int NumberOfRuns
		{
			get
			{
				return this.Wins + this.Losses;
			}
		}

		/// <summary>
		/// Creates an empty ProgressState. Used for new save files and test setup.
		/// </summary>
		// Token: 0x0600074E RID: 1870 RVA: 0x0003C5BF File Offset: 0x0003A7BF
		public static ProgressState CreateDefault()
		{
			return ProgressState.FromSerializable(new SerializableProgress(), new DeserializationContext());
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0003C5D0 File Offset: 0x0003A7D0
		public static ProgressState FromSerializable(SerializableProgress save, DeserializationContext ctx)
		{
			ArgumentNullException.ThrowIfNull(save, "save");
			ArgumentNullException.ThrowIfNull(ctx, "ctx");
			ProgressState progressState = new ProgressState
			{
				UniqueId = save.UniqueId,
				EnableFtues = save.EnableFtues,
				TotalPlaytime = ProgressState.ClampNonNegative(save.TotalPlaytime, "TotalPlaytime", ctx),
				TotalUnlocks = ProgressState.ClampNonNegativeInt(save.TotalUnlocks, "TotalUnlocks", ctx),
				CurrentScore = ProgressState.ClampNonNegativeInt(save.CurrentScore, "CurrentScore", ctx),
				FloorsClimbed = ProgressState.ClampNonNegative(save.FloorsClimbed, "FloorsClimbed", ctx),
				ArchitectDamage = ProgressState.ClampNonNegative(save.ArchitectDamage, "ArchitectDamage", ctx),
				WongoPoints = ProgressState.ClampNonNegativeInt(save.WongoPoints, "WongoPoints", ctx),
				PreferredMultiplayerAscension = ProgressState.ClampAscension(save.PreferredMultiplayerAscension, "PreferredMultiplayerAscension", ctx),
				MaxMultiplayerAscension = ProgressState.ClampAscension(save.MaxMultiplayerAscension, "MaxMultiplayerAscension", ctx),
				TestSubjectKills = ProgressState.ClampNonNegativeInt(save.TestSubjectKills, "TestSubjectKills", ctx),
				PendingCharacterUnlock = ProgressState.ValidateModelId<CharacterModel>(save.PendingCharacterUnlock, "PendingCharacterUnlock", ctx)
			};
			ProgressState.ParseCharacterStats(save.CharStats, progressState._characterStats, progressState._unknownCharacterStats, ctx);
			ProgressState.ParseCardStats(save.CardStats, progressState._cardStats, progressState._unknownCardStats, ctx);
			ProgressState.ParseEncounterStats(save.EncounterStats, progressState._encounterStats, progressState._unknownEncounterStats, ctx);
			ProgressState.ParseEnemyStats(save.EnemyStats, progressState._enemyStats, progressState._unknownEnemyStats, ctx);
			ProgressState.ParseAncientStats(save.AncientStats, progressState._ancientStats, progressState._unknownAncientStats, ctx);
			ProgressState.ParseDiscoveredSet<CardModel>(save.DiscoveredCards, progressState._discoveredCards, progressState._unknownDiscoveredCards, "DiscoveredCards", ctx);
			ProgressState.ParseDiscoveredSet<RelicModel>(save.DiscoveredRelics, progressState._discoveredRelics, progressState._unknownDiscoveredRelics, "DiscoveredRelics", ctx);
			ProgressState.ParseDiscoveredSet<PotionModel>(save.DiscoveredPotions, progressState._discoveredPotions, progressState._unknownDiscoveredPotions, "DiscoveredPotions", ctx);
			ProgressState.ParseDiscoveredSet<EventModel>(save.DiscoveredEvents, progressState._discoveredEvents, progressState._unknownDiscoveredEvents, "DiscoveredEvents", ctx);
			ProgressState.ParseDiscoveredSet<ActModel>(save.DiscoveredActs, progressState._discoveredActs, progressState._unknownDiscoveredActs, "DiscoveredActs", ctx);
			ProgressState.ParseEpochs(save.Epochs, progressState._epochs, progressState._unknownEpochs, ctx);
			ProgressState.FixMissingSlots(progressState._epochs, ctx);
			ProgressState.ParseFtues(save.FtueCompleted, progressState._ftueCompleted, ctx);
			ProgressState.ParseAchievements(save.UnlockedAchievements, progressState._unlockedAchievements, progressState._unknownUnlockedAchievements, ctx);
			progressState.FilterAndSortEpochs();
			return progressState;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0003C858 File Offset: 0x0003AA58
		public SerializableProgress ToSerializable()
		{
			SerializableProgress serializableProgress = new SerializableProgress();
			serializableProgress.UniqueId = this.UniqueId;
			serializableProgress.SchemaVersion = 0;
			serializableProgress.EnableFtues = this.EnableFtues;
			serializableProgress.TotalPlaytime = this.TotalPlaytime;
			serializableProgress.TotalUnlocks = this.TotalUnlocks;
			serializableProgress.CurrentScore = this.CurrentScore;
			serializableProgress.FloorsClimbed = this.FloorsClimbed;
			serializableProgress.ArchitectDamage = this.ArchitectDamage;
			serializableProgress.WongoPoints = this.WongoPoints;
			serializableProgress.PreferredMultiplayerAscension = this.PreferredMultiplayerAscension;
			serializableProgress.MaxMultiplayerAscension = this.MaxMultiplayerAscension;
			serializableProgress.TestSubjectKills = this.TestSubjectKills;
			serializableProgress.PendingCharacterUnlock = this.PendingCharacterUnlock;
			serializableProgress.CharStats = this._characterStats.Values.Concat<CharacterStats>(this._unknownCharacterStats.Values).ToList<CharacterStats>();
			serializableProgress.CardStats = this._cardStats.Values.Concat<CardStats>(this._unknownCardStats.Values).ToList<CardStats>();
			serializableProgress.EncounterStats = this._encounterStats.Values.Concat<EncounterStats>(this._unknownEncounterStats.Values).ToList<EncounterStats>();
			serializableProgress.EnemyStats = this._enemyStats.Values.Concat<EnemyStats>(this._unknownEnemyStats.Values).ToList<EnemyStats>();
			serializableProgress.AncientStats = this._ancientStats.Values.Concat<AncientStats>(this._unknownAncientStats.Values).ToList<AncientStats>();
			serializableProgress.DiscoveredCards = this._discoveredCards.Concat<ModelId>(this._unknownDiscoveredCards).ToList<ModelId>();
			serializableProgress.DiscoveredRelics = this._discoveredRelics.Concat<ModelId>(this._unknownDiscoveredRelics).ToList<ModelId>();
			serializableProgress.DiscoveredPotions = this._discoveredPotions.Concat<ModelId>(this._unknownDiscoveredPotions).ToList<ModelId>();
			serializableProgress.DiscoveredEvents = this._discoveredEvents.Concat<ModelId>(this._unknownDiscoveredEvents).ToList<ModelId>();
			serializableProgress.DiscoveredActs = this._discoveredActs.Concat<ModelId>(this._unknownDiscoveredActs).ToList<ModelId>();
			serializableProgress.Epochs = this._epochs.Concat<SerializableEpoch>(this._unknownEpochs).ToList<SerializableEpoch>();
			serializableProgress.FtueCompleted = this._ftueCompleted.ToList<string>();
			serializableProgress.UnlockedAchievements = this._unlockedAchievements.Select<KeyValuePair<Achievement, long>, SerializableUnlockedAchievement>((KeyValuePair<Achievement, long> kvp) => new SerializableUnlockedAchievement
			{
				Achievement = JsonNamingPolicy.SnakeCaseLower.ConvertName(kvp.Key.ToString()),
				UnlockTime = kvp.Value
			}).Concat<SerializableUnlockedAchievement>(this._unknownUnlockedAchievements).ToList<SerializableUnlockedAchievement>();
			return serializableProgress;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0003CAB8 File Offset: 0x0003ACB8
		public CharacterStats GetOrCreateCharacterStats(ModelId characterId)
		{
			CharacterStats characterStats;
			if (this._characterStats.TryGetValue(characterId, out characterStats))
			{
				return characterStats;
			}
			CharacterStats characterStats2 = new CharacterStats
			{
				Id = characterId
			};
			this._characterStats[characterId] = characterStats2;
			return characterStats2;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0003CAF4 File Offset: 0x0003ACF4
		public CardStats GetOrCreateCardStats(ModelId cardId)
		{
			CardStats cardStats;
			if (this._cardStats.TryGetValue(cardId, out cardStats))
			{
				return cardStats;
			}
			CardStats cardStats2 = new CardStats
			{
				Id = cardId
			};
			this._cardStats[cardId] = cardStats2;
			return cardStats2;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0003CB2E File Offset: 0x0003AD2E
		public bool MarkCardAsSeen(ModelId cardId)
		{
			return this._discoveredCards.Add(cardId);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0003CB3C File Offset: 0x0003AD3C
		public bool MarkRelicAsSeen(ModelId relicId)
		{
			return this._discoveredRelics.Add(relicId);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0003CB4A File Offset: 0x0003AD4A
		public bool MarkPotionAsSeen(ModelId potionId)
		{
			return this._discoveredPotions.Add(potionId);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0003CB58 File Offset: 0x0003AD58
		public bool MarkEventAsSeen(ModelId eventId)
		{
			return this._discoveredEvents.Add(eventId);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0003CB66 File Offset: 0x0003AD66
		public bool MarkActAsSeen(ModelId actId)
		{
			return this._discoveredActs.Add(actId);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0003CB74 File Offset: 0x0003AD74
		public bool MarkFtueAsComplete(string ftueId)
		{
			return this._ftueCompleted.Add(ftueId);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0003CB82 File Offset: 0x0003AD82
		public void AddUnlockedAchievement(Achievement achievement, long unlockTime)
		{
			this._unlockedAchievements[achievement] = unlockTime;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0003CB91 File Offset: 0x0003AD91
		public bool RemoveUnlockedAchievement(Achievement achievement)
		{
			return this._unlockedAchievements.Remove(achievement);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0003CB9F File Offset: 0x0003AD9F
		public bool IsAchievementUnlocked(Achievement achievement)
		{
			return this._unlockedAchievements.ContainsKey(achievement);
		}

		/// <summary>
		/// Sets an epoch to Obtained if its slot exists, or creates a new ObtainedNoSlot entry.
		/// </summary>
		// Token: 0x0600075C RID: 1884 RVA: 0x0003CBB0 File Offset: 0x0003ADB0
		public void ObtainEpoch(string epochId)
		{
			SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((SerializableEpoch e) => e.Id == epochId);
			if (serializableEpoch != null)
			{
				serializableEpoch.SetObtained(EpochState.Obtained);
				return;
			}
			this._epochs.Add(new SerializableEpoch(epochId, EpochState.ObtainedNoSlot));
			this.FilterAndSortEpochs();
		}

		/// <summary>
		/// Sets or creates an epoch at any desired state. Used by timeline expansions.
		/// Bypasses <see cref="M:MegaCrit.Sts2.Core.Saves.SerializableEpoch.SetObtained(MegaCrit.Sts2.Core.Saves.EpochState)" />'s one-way guard so the
		/// state can be moved in any direction (e.g. NotObtained to Revealed).
		/// </summary>
		// Token: 0x0600075D RID: 1885 RVA: 0x0003CC0C File Offset: 0x0003AE0C
		public void ObtainEpochOverride(string epochId, EpochState state)
		{
			SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((SerializableEpoch e) => e.Id == epochId);
			if (serializableEpoch != null)
			{
				if (serializableEpoch.ObtainDate == 0L && state >= EpochState.ObtainedNoSlot)
				{
					serializableEpoch.ObtainDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
				}
				serializableEpoch.State = state;
				return;
			}
			this._epochs.Add(new SerializableEpoch(epochId, state));
			this.FilterAndSortEpochs();
		}

		/// <summary>
		/// Opens a timeline slot for an epoch. If no entry exists, creates a NotObtained entry.
		/// If the epoch was earned before its slot existed (ObtainedNoSlot), promotes it to Obtained.
		/// Does NOT modify ObtainDate (the date was set when the epoch was earned, not when the slot opened).
		/// </summary>
		// Token: 0x0600075E RID: 1886 RVA: 0x0003CC88 File Offset: 0x0003AE88
		public void UnlockSlot(string epochId)
		{
			SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((SerializableEpoch e) => e.Id == epochId);
			if (serializableEpoch == null)
			{
				this._epochs.Add(new SerializableEpoch(epochId, EpochState.NotObtained));
				this.FilterAndSortEpochs();
				return;
			}
			if (serializableEpoch.State == EpochState.ObtainedNoSlot)
			{
				Log.Info("Attempted to get slot " + epochId + " but we already have an Epoch! Set to Obtained", 2);
				serializableEpoch.State = EpochState.Obtained;
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Slot unlocked for ");
			defaultInterpolatedStringHandler.AppendFormatted(epochId);
			defaultInterpolatedStringHandler.AppendLiteral(" but it's in an invalid state: ");
			defaultInterpolatedStringHandler.AppendFormatted<EpochState>(serializableEpoch.State);
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		/// <summary>
		/// Sets an obtained epoch to Revealed state. Pure data mutation only.
		/// The metric upload stays on <see cref="M:MegaCrit.Sts2.Core.Saves.SaveManager.RevealEpoch(System.String,System.Boolean)" />.
		/// </summary>
		// Token: 0x0600075F RID: 1887 RVA: 0x0003CD50 File Offset: 0x0003AF50
		public void RevealEpoch(string epochId)
		{
			SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((SerializableEpoch e) => e.Id == epochId);
			if (serializableEpoch == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid epoch ");
				defaultInterpolatedStringHandler.AppendFormatted(epochId);
				defaultInterpolatedStringHandler.AppendLiteral(" passed to ");
				defaultInterpolatedStringHandler.AppendFormatted("RevealEpoch");
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			serializableEpoch.State = EpochState.Revealed;
		}

		/// <summary>
		/// Clears all epochs. Used by the debug Reset Progress button.
		/// </summary>
		// Token: 0x06000760 RID: 1888 RVA: 0x0003CDE0 File Offset: 0x0003AFE0
		public void ResetEpochs()
		{
			this._epochs.Clear();
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0003CDF0 File Offset: 0x0003AFF0
		[return: Nullable(2)]
		public CharacterStats GetStatsForCharacter(ModelId characterId)
		{
			CharacterStats characterStats;
			this._characterStats.TryGetValue(characterId, out characterStats);
			return characterStats;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0003CE10 File Offset: 0x0003B010
		public EncounterStats GetOrCreateEncounterStats(ModelId encounterId)
		{
			EncounterStats encounterStats;
			if (this._encounterStats.TryGetValue(encounterId, out encounterStats))
			{
				return encounterStats;
			}
			EncounterStats encounterStats2 = new EncounterStats
			{
				Id = encounterId
			};
			this._encounterStats[encounterId] = encounterStats2;
			return encounterStats2;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0003CE4C File Offset: 0x0003B04C
		public EnemyStats GetOrCreateEnemyStats(ModelId enemyId)
		{
			EnemyStats enemyStats;
			if (this._enemyStats.TryGetValue(enemyId, out enemyStats))
			{
				return enemyStats;
			}
			EnemyStats enemyStats2 = new EnemyStats
			{
				Id = enemyId
			};
			this._enemyStats[enemyId] = enemyStats2;
			return enemyStats2;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0003CE88 File Offset: 0x0003B088
		public AncientStats GetOrCreateAncientStats(ModelId ancientId)
		{
			AncientStats ancientStats;
			if (this._ancientStats.TryGetValue(ancientId, out ancientStats))
			{
				return ancientStats;
			}
			AncientStats ancientStats2 = new AncientStats
			{
				Id = ancientId
			};
			this._ancientStats[ancientId] = ancientStats2;
			return ancientStats2;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0003CEC4 File Offset: 0x0003B0C4
		[return: Nullable(2)]
		public AncientStats GetStatsForAncient(ModelId ancientId)
		{
			AncientStats ancientStats;
			this._ancientStats.TryGetValue(ancientId, out ancientStats);
			return ancientStats;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0003CEE1 File Offset: 0x0003B0E1
		public void ResetFtues()
		{
			this.EnableFtues = true;
			this._ftueCompleted.Clear();
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0003CEF8 File Offset: 0x0003B0F8
		public bool HasEpoch(string epochId)
		{
			return this._epochs.Any<SerializableEpoch>((SerializableEpoch e) => e.Id == epochId);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0003CF2C File Offset: 0x0003B12C
		public bool IsEpochObtained(string epochId)
		{
			SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((SerializableEpoch e) => e.Id == epochId);
			return serializableEpoch != null && serializableEpoch.State >= EpochState.ObtainedNoSlot;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0003CF70 File Offset: 0x0003B170
		public bool IsEpochRevealed(string epochId)
		{
			SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((SerializableEpoch e) => e.Id == epochId);
			return serializableEpoch != null && serializableEpoch.State >= EpochState.Revealed;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0003CFB4 File Offset: 0x0003B1B4
		private static void ParseCharacterStats(List<CharacterStats> source, Dictionary<ModelId, CharacterStats> target, Dictionary<ModelId, CharacterStats> unknown, DeserializationContext ctx)
		{
			ctx.PushPath("CharStats");
			for (int i = 0; i < source.Count; i++)
			{
				CharacterStats characterStats = source[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				ctx.PushPath(defaultInterpolatedStringHandler.ToStringAndClear());
				ModelId id = characterStats.Id;
				if (id == null || id == ModelId.none)
				{
					ctx.Warn("Null or none character ID, skipping");
					ctx.PopPath();
				}
				else if (ModelDb.GetByIdOrNull<CharacterModel>(id) == null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown character ID: ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(id);
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					ProgressState.AddCharacterStats(characterStats, unknown, ctx);
					ctx.PopPath();
				}
				else
				{
					ProgressState.AddCharacterStats(characterStats, target, ctx);
					ProgressState.ClampCharacterStatsFields(characterStats, ctx);
					ctx.PopPath();
				}
			}
			ctx.PopPath();
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0003D0AC File Offset: 0x0003B2AC
		private static void AddCharacterStats(CharacterStats entry, Dictionary<ModelId, CharacterStats> target, DeserializationContext ctx)
		{
			if (!target.TryAdd(entry.Id, entry))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Duplicate character stats for ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(entry.Id);
				defaultInterpolatedStringHandler.AppendLiteral(", merging");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				ProgressState.MergeCharacterStats(target[entry.Id], entry);
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0003D118 File Offset: 0x0003B318
		private static void ParseCardStats(List<CardStats> source, Dictionary<ModelId, CardStats> target, Dictionary<ModelId, CardStats> unknown, DeserializationContext ctx)
		{
			ctx.PushPath("CardStats");
			for (int i = 0; i < source.Count; i++)
			{
				CardStats cardStats = source[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				ctx.PushPath(defaultInterpolatedStringHandler.ToStringAndClear());
				ModelId id = cardStats.Id;
				if (id == null || id == ModelId.none)
				{
					ctx.Warn("Null or none card ID, skipping");
					ctx.PopPath();
				}
				else if (ModelDb.GetByIdOrNull<CardModel>(id) == null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown card ID: ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(id);
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					ProgressState.AddCardStats(cardStats, unknown, ctx);
					ctx.PopPath();
				}
				else
				{
					ProgressState.AddCardStats(cardStats, target, ctx);
					ProgressState.ClampCardStatsFields(cardStats, ctx);
					ctx.PopPath();
				}
			}
			ctx.PopPath();
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0003D210 File Offset: 0x0003B410
		private static void AddCardStats(CardStats entry, Dictionary<ModelId, CardStats> target, DeserializationContext ctx)
		{
			if (!target.TryAdd(entry.Id, entry))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Duplicate card stats for ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(entry.Id);
				defaultInterpolatedStringHandler.AppendLiteral(", merging");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				ProgressState.MergeCardStats(target[entry.Id], entry);
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0003D27C File Offset: 0x0003B47C
		private static void ParseEncounterStats(List<EncounterStats> source, Dictionary<ModelId, EncounterStats> target, Dictionary<ModelId, EncounterStats> unknown, DeserializationContext ctx)
		{
			ctx.PushPath("EncounterStats");
			for (int i = 0; i < source.Count; i++)
			{
				EncounterStats encounterStats = source[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				ctx.PushPath(defaultInterpolatedStringHandler.ToStringAndClear());
				if (encounterStats.Id == null || encounterStats.Id == ModelId.none)
				{
					ctx.Warn("Null or none encounter ID, skipping");
					ctx.PopPath();
				}
				else if (ModelDb.GetByIdOrNull<EncounterModel>(encounterStats.Id) == null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown encounter ID: ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(encounterStats.Id);
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					ProgressState.AddEncounterStats(encounterStats, unknown, ctx);
					ctx.PopPath();
				}
				else
				{
					ProgressState.AddEncounterStats(encounterStats, target, ctx);
					ProgressState.ClampFightStatsFields(encounterStats.FightStats, ctx);
					ctx.PopPath();
				}
			}
			ctx.PopPath();
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0003D384 File Offset: 0x0003B584
		private static void AddEncounterStats(EncounterStats entry, Dictionary<ModelId, EncounterStats> target, DeserializationContext ctx)
		{
			if (!target.TryAdd(entry.Id, entry))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Duplicate encounter stats for ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(entry.Id);
				defaultInterpolatedStringHandler.AppendLiteral(", merging");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				ProgressState.MergeFightStatsList(target[entry.Id].FightStats, entry.FightStats);
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0003D3F8 File Offset: 0x0003B5F8
		private static void ParseEnemyStats(List<EnemyStats> source, Dictionary<ModelId, EnemyStats> target, Dictionary<ModelId, EnemyStats> unknown, DeserializationContext ctx)
		{
			ctx.PushPath("EnemyStats");
			for (int i = 0; i < source.Count; i++)
			{
				EnemyStats enemyStats = source[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				ctx.PushPath(defaultInterpolatedStringHandler.ToStringAndClear());
				if (enemyStats.Id == null || enemyStats.Id == ModelId.none)
				{
					ctx.Warn("Null or none enemy ID, skipping");
					ctx.PopPath();
				}
				else if (ModelDb.GetByIdOrNull<MonsterModel>(enemyStats.Id) == null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown enemy ID: ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(enemyStats.Id);
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					ProgressState.AddEnemyStats(enemyStats, unknown, ctx);
					ctx.PopPath();
				}
				else
				{
					ProgressState.AddEnemyStats(enemyStats, target, ctx);
					ProgressState.ClampFightStatsFields(enemyStats.FightStats, ctx);
					ctx.PopPath();
				}
			}
			ctx.PopPath();
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0003D504 File Offset: 0x0003B704
		private static void AddEnemyStats(EnemyStats entry, Dictionary<ModelId, EnemyStats> target, DeserializationContext ctx)
		{
			if (!target.TryAdd(entry.Id, entry))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Duplicate enemy stats for ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(entry.Id);
				defaultInterpolatedStringHandler.AppendLiteral(", merging");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				ProgressState.MergeFightStatsList(target[entry.Id].FightStats, entry.FightStats);
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0003D578 File Offset: 0x0003B778
		private static void ParseAncientStats(List<AncientStats> source, Dictionary<ModelId, AncientStats> target, Dictionary<ModelId, AncientStats> unknown, DeserializationContext ctx)
		{
			ctx.PushPath("AncientStats");
			for (int i = 0; i < source.Count; i++)
			{
				AncientStats ancientStats = source[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				ctx.PushPath(defaultInterpolatedStringHandler.ToStringAndClear());
				if (ancientStats.Id == null || ancientStats.Id == ModelId.none)
				{
					ctx.Warn("Null or none ancient ID, skipping");
					ctx.PopPath();
				}
				else if (ModelDb.GetByIdOrNull<EventModel>(ancientStats.Id) == null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown ancient event ID: ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(ancientStats.Id);
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					ProgressState.AddAncientStats(ancientStats, unknown, ctx);
					ctx.PopPath();
				}
				else
				{
					ProgressState.ClampAncientCharacterStatsFields(ancientStats.CharStats, ctx);
					ProgressState.AddAncientStats(ancientStats, target, ctx);
					ctx.PopPath();
				}
			}
			ctx.PopPath();
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0003D680 File Offset: 0x0003B880
		private static void AddAncientStats(AncientStats entry, Dictionary<ModelId, AncientStats> target, DeserializationContext ctx)
		{
			if (!target.TryAdd(entry.Id, entry))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Duplicate ancient stats for ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(entry.Id);
				defaultInterpolatedStringHandler.AppendLiteral(", merging");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				ProgressState.MergeAncientCharacterStatsList(target[entry.Id].CharStats, entry.CharStats);
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0003D6F4 File Offset: 0x0003B8F4
		private static void ParseDiscoveredSet<[Nullable(0)] TModel>(List<ModelId> source, HashSet<ModelId> target, HashSet<ModelId> unknown, string fieldName, DeserializationContext ctx) where TModel : AbstractModel
		{
			ctx.PushPath(fieldName);
			for (int i = 0; i < source.Count; i++)
			{
				ModelId modelId = source[i];
				if (modelId == null || modelId == ModelId.none)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Null or none ID at index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(", skipping");
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else if (ModelDb.GetByIdOrNull<TModel>(modelId) == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown ");
					defaultInterpolatedStringHandler.AppendFormatted(typeof(TModel).Name);
					defaultInterpolatedStringHandler.AppendLiteral(" ID: ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(modelId);
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					unknown.Add(modelId);
				}
				else if (!target.Add(modelId))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Duplicate ID: ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(modelId);
					defaultInterpolatedStringHandler.AppendLiteral(", skipping");
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			ctx.PopPath();
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0003D824 File Offset: 0x0003BA24
		private static void ParseEpochs(List<SerializableEpoch> source, List<SerializableEpoch> target, List<SerializableEpoch> unknown, DeserializationContext ctx)
		{
			ctx.PushPath("Epochs");
			HashSet<string> hashSet = new HashSet<string>();
			for (int i = 0; i < source.Count; i++)
			{
				SerializableEpoch serializableEpoch = source[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				ctx.PushPath(defaultInterpolatedStringHandler.ToStringAndClear());
				if (!EpochModel.IsValid(serializableEpoch.Id))
				{
					ctx.Warn("Unknown epoch ID: " + serializableEpoch.Id);
					unknown.Add(serializableEpoch);
					ctx.PopPath();
				}
				else if (!Enum.IsDefined<EpochState>(serializableEpoch.State))
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid epoch state ");
					defaultInterpolatedStringHandler.AppendFormatted<int>((int)serializableEpoch.State);
					defaultInterpolatedStringHandler.AppendLiteral(" for ");
					defaultInterpolatedStringHandler.AppendFormatted(serializableEpoch.Id);
					defaultInterpolatedStringHandler.AppendLiteral(", skipping");
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					ctx.PopPath();
				}
				else if (serializableEpoch.State < EpochState.NotObtained)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Epoch ");
					defaultInterpolatedStringHandler.AppendFormatted(serializableEpoch.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" has unused state ");
					defaultInterpolatedStringHandler.AppendFormatted<EpochState>(serializableEpoch.State);
					defaultInterpolatedStringHandler.AppendLiteral(", skipping");
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					ctx.PopPath();
				}
				else if (!hashSet.Add(serializableEpoch.Id))
				{
					ctx.Warn("Duplicate epoch ID: " + serializableEpoch.Id + ", keeping first");
					ctx.PopPath();
				}
				else
				{
					target.Add(serializableEpoch);
					ctx.PopPath();
				}
			}
			ctx.PopPath();
		}

		/// <summary>
		/// For each Revealed epoch, ensures all epochs it would expand via
		/// <see cref="M:MegaCrit.Sts2.Core.Timeline.EpochModel.GetTimelineExpansion" /> have slot entries.
		/// Promotes ObtainedNoSlot to Obtained and creates missing entries as NotObtained.
		/// </summary>
		// Token: 0x06000776 RID: 1910 RVA: 0x0003D9E4 File Offset: 0x0003BBE4
		private static void FixMissingSlots(List<SerializableEpoch> epochs, DeserializationContext ctx)
		{
			Dictionary<string, SerializableEpoch> dictionary = new Dictionary<string, SerializableEpoch>();
			foreach (SerializableEpoch serializableEpoch in epochs)
			{
				dictionary[serializableEpoch.Id] = serializableEpoch;
			}
			List<SerializableEpoch> list = epochs.ToList<SerializableEpoch>();
			foreach (SerializableEpoch serializableEpoch2 in list)
			{
				if (serializableEpoch2.State >= EpochState.Revealed)
				{
					EpochModel epochModel = EpochModel.Get(serializableEpoch2.Id);
					foreach (EpochModel epochModel2 in epochModel.GetTimelineExpansion())
					{
						SerializableEpoch serializableEpoch3;
						if (dictionary.TryGetValue(epochModel2.Id, out serializableEpoch3))
						{
							if (serializableEpoch3.State == EpochState.ObtainedNoSlot)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Epoch ");
								defaultInterpolatedStringHandler.AppendFormatted(epochModel2.Id);
								defaultInterpolatedStringHandler.AppendLiteral(" was ObtainedNoSlot but parent ");
								defaultInterpolatedStringHandler.AppendFormatted(serializableEpoch2.Id);
								defaultInterpolatedStringHandler.AppendLiteral(" is Revealed, promoting to Obtained");
								ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
								serializableEpoch3.State = EpochState.Obtained;
							}
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Epoch ");
							defaultInterpolatedStringHandler.AppendFormatted(epochModel2.Id);
							defaultInterpolatedStringHandler.AppendLiteral(" slot missing but parent ");
							defaultInterpolatedStringHandler.AppendFormatted(serializableEpoch2.Id);
							defaultInterpolatedStringHandler.AppendLiteral(" is Revealed, creating as NotObtained");
							ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							SerializableEpoch serializableEpoch4 = new SerializableEpoch(epochModel2.Id, EpochState.NotObtained);
							epochs.Add(serializableEpoch4);
							dictionary[epochModel2.Id] = serializableEpoch4;
						}
					}
				}
			}
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0003DBDC File Offset: 0x0003BDDC
		private static void ParseFtues(List<string> source, HashSet<string> target, DeserializationContext ctx)
		{
			ctx.PushPath("FtueCompleted");
			for (int i = 0; i < source.Count; i++)
			{
				if (!target.Add(source[i]))
				{
					ctx.Warn("Duplicate FTUE: " + source[i] + ", skipping");
				}
			}
			ctx.PopPath();
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0003DC38 File Offset: 0x0003BE38
		private static void ParseAchievements([Nullable(new byte[] { 2, 1 })] List<SerializableUnlockedAchievement> source, Dictionary<Achievement, long> target, List<SerializableUnlockedAchievement> unknown, DeserializationContext ctx)
		{
			if (source == null)
			{
				return;
			}
			ctx.PushPath("UnlockedAchievements");
			for (int i = 0; i < source.Count; i++)
			{
				SerializableUnlockedAchievement serializableUnlockedAchievement = source[i];
				Achievement achievement;
				if (!ProgressState._achievementsByName.TryGetValue(serializableUnlockedAchievement.Achievement, out achievement))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown achievement \"");
					defaultInterpolatedStringHandler.AppendFormatted(serializableUnlockedAchievement.Achievement);
					defaultInterpolatedStringHandler.AppendLiteral("\" at index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					unknown.Add(serializableUnlockedAchievement);
				}
				else if (!target.TryAdd(achievement, serializableUnlockedAchievement.UnlockTime))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Duplicate achievement ");
					defaultInterpolatedStringHandler.AppendFormatted<Achievement>(achievement);
					defaultInterpolatedStringHandler.AppendLiteral(" at index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(", keeping first");
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			ctx.PopPath();
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0003DD34 File Offset: 0x0003BF34
		private static Dictionary<string, Achievement> BuildAchievementLookup()
		{
			Dictionary<string, Achievement> dictionary = new Dictionary<string, Achievement>();
			foreach (Achievement achievement in Enum.GetValues<Achievement>())
			{
				string text = JsonNamingPolicy.SnakeCaseLower.ConvertName(achievement.ToString());
				dictionary[text] = achievement;
			}
			return dictionary;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0003DD84 File Offset: 0x0003BF84
		private static void ClampCharacterStatsFields(CharacterStats stats, DeserializationContext ctx)
		{
			if (stats.TotalWins < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative TotalWins (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(stats.TotalWins);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.TotalWins = 0;
			}
			if (stats.TotalLosses < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative TotalLosses (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(stats.TotalLosses);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.TotalLosses = 0;
			}
			if (stats.Playtime < 0L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative Playtime (");
				defaultInterpolatedStringHandler.AppendFormatted<long>(stats.Playtime);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.Playtime = 0L;
			}
			if (stats.MaxAscension < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative MaxAscension (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(stats.MaxAscension);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.MaxAscension = 0;
			}
			if (stats.MaxAscension > 10)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
				defaultInterpolatedStringHandler.AppendLiteral("MaxAscension (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(stats.MaxAscension);
				defaultInterpolatedStringHandler.AppendLiteral(") exceeds allowed (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(10);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.MaxAscension = 10;
			}
			if (stats.PreferredAscension < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative PreferredAscension (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(stats.PreferredAscension);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.PreferredAscension = 0;
			}
			if (stats.PreferredAscension > 10)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 2);
				defaultInterpolatedStringHandler.AppendLiteral("PreferredAscension (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(stats.PreferredAscension);
				defaultInterpolatedStringHandler.AppendLiteral(") exceeds allowed (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(10);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.PreferredAscension = 10;
			}
			if (stats.BestWinStreak < 0L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative BestWinStreak (");
				defaultInterpolatedStringHandler.AppendFormatted<long>(stats.BestWinStreak);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.BestWinStreak = 0L;
			}
			if (stats.CurrentWinStreak < 0L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative CurrentWinStreak (");
				defaultInterpolatedStringHandler.AppendFormatted<long>(stats.CurrentWinStreak);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.CurrentWinStreak = 0L;
			}
			if (stats.FastestWinTime < -1L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid FastestWinTime (");
				defaultInterpolatedStringHandler.AppendFormatted<long>(stats.FastestWinTime);
				defaultInterpolatedStringHandler.AppendLiteral("), resetting to -1");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.FastestWinTime = -1L;
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0003E0C0 File Offset: 0x0003C2C0
		private static void ClampCardStatsFields(CardStats stats, DeserializationContext ctx)
		{
			if (stats.TimesPicked < 0L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative TimesPicked (");
				defaultInterpolatedStringHandler.AppendFormatted<long>(stats.TimesPicked);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.TimesPicked = 0L;
			}
			if (stats.TimesSkipped < 0L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative TimesSkipped (");
				defaultInterpolatedStringHandler.AppendFormatted<long>(stats.TimesSkipped);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.TimesSkipped = 0L;
			}
			if (stats.TimesWon < 0L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative TimesWon (");
				defaultInterpolatedStringHandler.AppendFormatted<long>(stats.TimesWon);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.TimesWon = 0L;
			}
			if (stats.TimesLost < 0L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Negative TimesLost (");
				defaultInterpolatedStringHandler.AppendFormatted<long>(stats.TimesLost);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				stats.TimesLost = 0L;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0003E208 File Offset: 0x0003C408
		private static void ClampFightStatsFields([Nullable(new byte[] { 2, 1 })] List<FightStats> fightStats, DeserializationContext ctx)
		{
			if (fightStats == null)
			{
				return;
			}
			for (int i = fightStats.Count - 1; i >= 0; i--)
			{
				FightStats fight = fightStats[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
				defaultInterpolatedStringHandler.AppendLiteral("FightStats[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				ctx.PushPath(defaultInterpolatedStringHandler.ToStringAndClear());
				if (fight.Character == ModelId.none || ModelDb.GetByIdOrNull<CharacterModel>(fight.Character) == null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown character ID: ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(fight.Character);
					defaultInterpolatedStringHandler.AppendLiteral(", removing");
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					fightStats.RemoveAt(i);
					ctx.PopPath();
				}
				else
				{
					FightStats fightStats2 = fightStats.Take<FightStats>(i).FirstOrDefault<FightStats>((FightStats f) => f.Character == fight.Character);
					if (fightStats2 != null)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Duplicate character ");
						defaultInterpolatedStringHandler.AppendFormatted<ModelId>(fight.Character);
						defaultInterpolatedStringHandler.AppendLiteral(", merging into earlier entry");
						ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						fightStats2.Wins = Math.Max(fightStats2.Wins, fight.Wins);
						fightStats2.Losses = Math.Max(fightStats2.Losses, fight.Losses);
						fightStats.RemoveAt(i);
						ctx.PopPath();
					}
					else
					{
						if (fight.Wins < 0)
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Negative Wins (");
							defaultInterpolatedStringHandler.AppendFormatted<int>(fight.Wins);
							defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
							ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							fight.Wins = 0;
						}
						if (fight.Losses < 0)
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Negative Losses (");
							defaultInterpolatedStringHandler.AppendFormatted<int>(fight.Losses);
							defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
							ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							fight.Losses = 0;
						}
						ctx.PopPath();
					}
				}
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0003E460 File Offset: 0x0003C660
		private static void ClampAncientCharacterStatsFields([Nullable(new byte[] { 2, 1 })] List<AncientCharacterStats> charStats, DeserializationContext ctx)
		{
			if (charStats == null)
			{
				return;
			}
			for (int i = charStats.Count - 1; i >= 0; i--)
			{
				AncientCharacterStats stats = charStats[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
				defaultInterpolatedStringHandler.AppendLiteral("CharStats[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				ctx.PushPath(defaultInterpolatedStringHandler.ToStringAndClear());
				if (stats.Character == ModelId.none || ModelDb.GetByIdOrNull<CharacterModel>(stats.Character) == null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown character ID: ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(stats.Character);
					defaultInterpolatedStringHandler.AppendLiteral(", removing");
					ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					charStats.RemoveAt(i);
					ctx.PopPath();
				}
				else
				{
					AncientCharacterStats ancientCharacterStats = charStats.Take<AncientCharacterStats>(i).FirstOrDefault<AncientCharacterStats>((AncientCharacterStats c) => c.Character == stats.Character);
					if (ancientCharacterStats != null)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Duplicate character ");
						defaultInterpolatedStringHandler.AppendFormatted<ModelId>(stats.Character);
						defaultInterpolatedStringHandler.AppendLiteral(", merging into earlier entry");
						ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						ancientCharacterStats.Wins = Math.Max(ancientCharacterStats.Wins, stats.Wins);
						ancientCharacterStats.Losses = Math.Max(ancientCharacterStats.Losses, stats.Losses);
						charStats.RemoveAt(i);
						ctx.PopPath();
					}
					else
					{
						if (stats.Wins < 0)
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Negative Wins (");
							defaultInterpolatedStringHandler.AppendFormatted<int>(stats.Wins);
							defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
							ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							stats.Wins = 0;
						}
						if (stats.Losses < 0)
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Negative Losses (");
							defaultInterpolatedStringHandler.AppendFormatted<int>(stats.Losses);
							defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
							ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							stats.Losses = 0;
						}
						ctx.PopPath();
					}
				}
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0003E6B8 File Offset: 0x0003C8B8
		private static void MergeCharacterStats(CharacterStats existing, CharacterStats incoming)
		{
			existing.TotalWins = Math.Max(existing.TotalWins, incoming.TotalWins);
			existing.TotalLosses = Math.Max(existing.TotalLosses, incoming.TotalLosses);
			existing.Playtime = Math.Max(existing.Playtime, incoming.Playtime);
			existing.MaxAscension = Math.Max(existing.MaxAscension, incoming.MaxAscension);
			existing.BestWinStreak = Math.Max(existing.BestWinStreak, incoming.BestWinStreak);
			existing.CurrentWinStreak = Math.Max(existing.CurrentWinStreak, incoming.CurrentWinStreak);
			existing.PreferredAscension = Math.Max(existing.PreferredAscension, incoming.PreferredAscension);
			existing.FastestWinTime = ProgressState.MergeFastestWinTime(existing.FastestWinTime, incoming.FastestWinTime);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0003E77D File Offset: 0x0003C97D
		private static long MergeFastestWinTime(long a, long b)
		{
			if (a == -1L)
			{
				return b;
			}
			if (b == -1L)
			{
				return a;
			}
			return Math.Min(a, b);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0003E794 File Offset: 0x0003C994
		private static void MergeCardStats(CardStats existing, CardStats incoming)
		{
			existing.TimesPicked = Math.Max(existing.TimesPicked, incoming.TimesPicked);
			existing.TimesSkipped = Math.Max(existing.TimesSkipped, incoming.TimesSkipped);
			existing.TimesWon = Math.Max(existing.TimesWon, incoming.TimesWon);
			existing.TimesLost = Math.Max(existing.TimesLost, incoming.TimesLost);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0003E800 File Offset: 0x0003CA00
		private static void MergeFightStatsList([Nullable(new byte[] { 2, 1 })] List<FightStats> existing, [Nullable(new byte[] { 2, 1 })] List<FightStats> incoming)
		{
			if (existing == null || incoming == null)
			{
				return;
			}
			using (List<FightStats>.Enumerator enumerator = incoming.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FightStats incomingFight = enumerator.Current;
					FightStats fightStats = existing.FirstOrDefault<FightStats>((FightStats f) => f.Character == incomingFight.Character);
					if (fightStats != null)
					{
						fightStats.Wins = Math.Max(fightStats.Wins, incomingFight.Wins);
						fightStats.Losses = Math.Max(fightStats.Losses, incomingFight.Losses);
					}
					else
					{
						existing.Add(incomingFight);
					}
				}
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0003E8B8 File Offset: 0x0003CAB8
		private static void MergeAncientCharacterStatsList([Nullable(new byte[] { 2, 1 })] List<AncientCharacterStats> existing, [Nullable(new byte[] { 2, 1 })] List<AncientCharacterStats> incoming)
		{
			if (existing == null || incoming == null)
			{
				return;
			}
			using (List<AncientCharacterStats>.Enumerator enumerator = incoming.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AncientCharacterStats incomingStats = enumerator.Current;
					AncientCharacterStats ancientCharacterStats = existing.FirstOrDefault<AncientCharacterStats>((AncientCharacterStats c) => c.Character == incomingStats.Character);
					if (ancientCharacterStats != null)
					{
						ancientCharacterStats.Wins = Math.Max(ancientCharacterStats.Wins, incomingStats.Wins);
						ancientCharacterStats.Losses = Math.Max(ancientCharacterStats.Losses, incomingStats.Losses);
					}
					else
					{
						existing.Add(incomingStats);
					}
				}
			}
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0003E970 File Offset: 0x0003CB70
		private static ModelId ValidateModelId<[Nullable(0)] TModel>(ModelId id, string fieldName, DeserializationContext ctx) where TModel : AbstractModel
		{
			if (id == ModelId.none)
			{
				return ModelId.none;
			}
			if (ModelDb.GetByIdOrNull<TModel>(id) == null)
			{
				ctx.PushPath(fieldName);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unknown ID: ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(id);
				defaultInterpolatedStringHandler.AppendLiteral(", resetting to none");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				ctx.PopPath();
				return ModelId.none;
			}
			return id;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0003E9E8 File Offset: 0x0003CBE8
		private static long ClampNonNegative(long value, string fieldName, DeserializationContext ctx)
		{
			if (value >= 0L)
			{
				return value;
			}
			ctx.PushPath(fieldName);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Negative value (");
			defaultInterpolatedStringHandler.AppendFormatted<long>(value);
			defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
			ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
			ctx.PopPath();
			return 0L;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0003EA44 File Offset: 0x0003CC44
		private static int ClampNonNegativeInt(int value, string fieldName, DeserializationContext ctx)
		{
			if (value >= 0)
			{
				return value;
			}
			ctx.PushPath(fieldName);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Negative value (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(value);
			defaultInterpolatedStringHandler.AppendLiteral("), clamping to 0");
			ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
			ctx.PopPath();
			return 0;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0003EA9C File Offset: 0x0003CC9C
		private static int ClampAscension(int value, string fieldName, DeserializationContext ctx)
		{
			value = ProgressState.ClampNonNegativeInt(value, fieldName, ctx);
			if (value > 10)
			{
				ctx.PushPath(fieldName);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Value (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value);
				defaultInterpolatedStringHandler.AppendLiteral(") exceeds allowed (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(10);
				defaultInterpolatedStringHandler.AppendLiteral("), clamping");
				ctx.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				ctx.PopPath();
				return 10;
			}
			return value;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0003EB18 File Offset: 0x0003CD18
		private void FilterAndSortEpochs()
		{
			int num = this._epochs.RemoveAll((SerializableEpoch e) => !EpochModel.IsValid(e.Id));
			if (num > 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Removed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" invalid epoch(s) from progress state");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			this._epochs.Sort(new EpochComparer());
		}

		// Token: 0x04000267 RID: 615
		private static readonly Dictionary<string, Achievement> _achievementsByName = ProgressState.BuildAchievementLookup();

		// Token: 0x04000268 RID: 616
		private readonly Dictionary<ModelId, CharacterStats> _characterStats = new Dictionary<ModelId, CharacterStats>();

		// Token: 0x04000269 RID: 617
		private readonly Dictionary<ModelId, CardStats> _cardStats = new Dictionary<ModelId, CardStats>();

		// Token: 0x0400026A RID: 618
		private readonly Dictionary<ModelId, EncounterStats> _encounterStats = new Dictionary<ModelId, EncounterStats>();

		// Token: 0x0400026B RID: 619
		private readonly Dictionary<ModelId, EnemyStats> _enemyStats = new Dictionary<ModelId, EnemyStats>();

		// Token: 0x0400026C RID: 620
		private readonly Dictionary<ModelId, AncientStats> _ancientStats = new Dictionary<ModelId, AncientStats>();

		// Token: 0x0400026D RID: 621
		private readonly HashSet<ModelId> _discoveredCards = new HashSet<ModelId>();

		// Token: 0x0400026E RID: 622
		private readonly HashSet<ModelId> _discoveredRelics = new HashSet<ModelId>();

		// Token: 0x0400026F RID: 623
		private readonly HashSet<ModelId> _discoveredPotions = new HashSet<ModelId>();

		// Token: 0x04000270 RID: 624
		private readonly HashSet<ModelId> _discoveredEvents = new HashSet<ModelId>();

		// Token: 0x04000271 RID: 625
		private readonly HashSet<ModelId> _discoveredActs = new HashSet<ModelId>();

		// Token: 0x04000272 RID: 626
		private readonly List<SerializableEpoch> _epochs = new List<SerializableEpoch>();

		// Token: 0x04000273 RID: 627
		private readonly Dictionary<Achievement, long> _unlockedAchievements = new Dictionary<Achievement, long>();

		// Token: 0x04000274 RID: 628
		private readonly Dictionary<ModelId, CharacterStats> _unknownCharacterStats = new Dictionary<ModelId, CharacterStats>();

		// Token: 0x04000275 RID: 629
		private readonly Dictionary<ModelId, CardStats> _unknownCardStats = new Dictionary<ModelId, CardStats>();

		// Token: 0x04000276 RID: 630
		private readonly Dictionary<ModelId, EncounterStats> _unknownEncounterStats = new Dictionary<ModelId, EncounterStats>();

		// Token: 0x04000277 RID: 631
		private readonly Dictionary<ModelId, EnemyStats> _unknownEnemyStats = new Dictionary<ModelId, EnemyStats>();

		// Token: 0x04000278 RID: 632
		private readonly Dictionary<ModelId, AncientStats> _unknownAncientStats = new Dictionary<ModelId, AncientStats>();

		// Token: 0x04000279 RID: 633
		private readonly HashSet<ModelId> _unknownDiscoveredCards = new HashSet<ModelId>();

		// Token: 0x0400027A RID: 634
		private readonly HashSet<ModelId> _unknownDiscoveredRelics = new HashSet<ModelId>();

		// Token: 0x0400027B RID: 635
		private readonly HashSet<ModelId> _unknownDiscoveredPotions = new HashSet<ModelId>();

		// Token: 0x0400027C RID: 636
		private readonly HashSet<ModelId> _unknownDiscoveredEvents = new HashSet<ModelId>();

		// Token: 0x0400027D RID: 637
		private readonly HashSet<ModelId> _unknownDiscoveredActs = new HashSet<ModelId>();

		// Token: 0x0400027E RID: 638
		private readonly List<SerializableEpoch> _unknownEpochs = new List<SerializableEpoch>();

		// Token: 0x0400027F RID: 639
		private readonly List<SerializableUnlockedAchievement> _unknownUnlockedAchievements = new List<SerializableUnlockedAchievement>();

		// Token: 0x04000280 RID: 640
		private readonly HashSet<string> _ftueCompleted = new HashSet<string>();
	}
}
