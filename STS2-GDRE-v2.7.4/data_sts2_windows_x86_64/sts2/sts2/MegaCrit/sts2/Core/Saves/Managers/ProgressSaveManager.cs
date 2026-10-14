using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Migrations;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Saves.Validation;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using Sentry;

namespace MegaCrit.Sts2.Core.Saves.Managers
{
	// Token: 0x020000FA RID: 250
	[NullableContext(1)]
	[Nullable(0)]
	public class ProgressSaveManager
	{
		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00049AC3 File Offset: 0x00047CC3
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x00049ACB File Offset: 0x00047CCB
		public ProgressState Progress { get; set; } = ProgressState.CreateDefault();

		// Token: 0x06000ACF RID: 2767 RVA: 0x00049AD4 File Offset: 0x00047CD4
		public ProgressSaveManager(int profileId, ISaveStore saveStore, MigrationManager migrationManager)
			: this(saveStore, migrationManager, new StaticProfileIdProvider(profileId))
		{
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00049AE4 File Offset: 0x00047CE4
		public ProgressSaveManager(ISaveStore saveStore, MigrationManager migrationManager, IProfileIdProvider profileIdProvider)
		{
			this._saveStore = saveStore;
			this._migrationManager = migrationManager;
			this._profileIdProvider = profileIdProvider;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00049B0C File Offset: 0x00047D0C
		public static string GetProgressPathForProfile(int profileId, bool? forceModState = null)
		{
			return StringExtensions.PathJoin(StringExtensions.PathJoin(UserDataPathProvider.GetProfileDir(profileId, forceModState), UserDataPathProvider.SavesDir), "progress.save");
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00049B2C File Offset: 0x00047D2C
		public void SaveProgress()
		{
			try
			{
				SerializableProgress serializableProgress = this.Progress.ToSerializable();
				serializableProgress.SchemaVersion = this._migrationManager.GetLatestVersion<SerializableProgress>();
				string text = JsonSerializationUtility.ToJson<SerializableProgress>(serializableProgress);
				this._saveStore.WriteFile(ProgressSaveManager.GetProgressPathForProfile(this._profileIdProvider.CurrentProfileId, null), text);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to save progress: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SentryService.CaptureException(ex);
			}
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00049BC8 File Offset: 0x00047DC8
		public ReadSaveResult<SerializableProgress> LoadProgress()
		{
			ReadSaveResult<SerializableProgress> readSaveResult = this._migrationManager.LoadSave<SerializableProgress>(ProgressSaveManager.GetProgressPathForProfile(this._profileIdProvider.CurrentProfileId, null));
			if (!readSaveResult.Success || readSaveResult.SaveData == null)
			{
				this.Progress = ProgressState.CreateDefault();
				Ironclad ironclad = ModelDb.Character<Ironclad>();
				foreach (CardModel cardModel in ironclad.StartingDeck)
				{
					this.Progress.MarkCardAsSeen(cardModel.Id);
				}
				foreach (RelicModel relicModel in ironclad.StartingRelics)
				{
					this.Progress.MarkRelicAsSeen(relicModel.Id);
				}
				this.SaveProgress();
			}
			else
			{
				DeserializationContext deserializationContext = new DeserializationContext();
				this.Progress = ProgressState.FromSerializable(readSaveResult.SaveData, deserializationContext);
				foreach (ValidationError validationError in deserializationContext.Errors)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Progress parse: ");
					defaultInterpolatedStringHandler.AppendFormatted<ValidationError>(validationError);
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
			}
			return readSaveResult;
		}

		/// <summary>
		/// Generate an unlock state from the current progress save.
		/// </summary>
		// Token: 0x06000AD4 RID: 2772 RVA: 0x00049D44 File Offset: 0x00047F44
		public UnlockState GenerateUnlockState()
		{
			return new UnlockState(this.Progress);
		}

		/// <summary>
		/// Called after a loss. Marks the player as having lost to an encounter (not a monster).
		/// </summary>
		// Token: 0x06000AD5 RID: 2773 RVA: 0x00049D54 File Offset: 0x00047F54
		private void IncrementEncounterLoss(ModelId characterId, ModelId encounterId)
		{
			EncounterStats orCreateEncounterStats = this.Progress.GetOrCreateEncounterStats(encounterId);
			if (orCreateEncounterStats.FightStats.FirstOrDefault<FightStats>((FightStats f) => f.Character == characterId) == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
				defaultInterpolatedStringHandler.AppendLiteral(" fought ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(encounterId);
				defaultInterpolatedStringHandler.AppendLiteral(" for the first time and LOST :(");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				FightStats fightStats = new FightStats
				{
					Character = characterId,
					Wins = 0,
					Losses = 1
				};
				orCreateEncounterStats.FightStats.Add(fightStats);
				return;
			}
			orCreateEncounterStats.IncrementLoss(characterId);
		}

		/// <summary>
		/// Called after a loss. Marks the player as having lost to a specific monster in an encounter.
		/// </summary>
		// Token: 0x06000AD6 RID: 2774 RVA: 0x00049E14 File Offset: 0x00048014
		private void IncrementEnemyFightLoss(ModelId characterId, ModelId monster)
		{
			EnemyStats orCreateEnemyStats = this.Progress.GetOrCreateEnemyStats(monster);
			if (orCreateEnemyStats.FightStats.FirstOrDefault<FightStats>((FightStats f) => f.Character == characterId) == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 2);
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
				defaultInterpolatedStringHandler.AppendLiteral(" fought ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(monster);
				defaultInterpolatedStringHandler.AppendLiteral(" for the first time and LOST >:(");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				FightStats fightStats = new FightStats
				{
					Character = characterId,
					Wins = 0,
					Losses = 1
				};
				orCreateEnemyStats.FightStats.Add(fightStats);
				return;
			}
			orCreateEnemyStats.IncrementLoss(characterId);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00049ED3 File Offset: 0x000480D3
		public void MarkPotionAsSeen(PotionModel potion)
		{
			if (!this.Progress.MarkPotionAsSeen(potion.Id))
			{
				return;
			}
			if (LocalContext.IsMine(potion))
			{
				potion.Owner.DiscoveredPotions.Add(potion.Id);
			}
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00049F07 File Offset: 0x00048107
		public void MarkCardAsSeen(CardModel card)
		{
			if (!this.Progress.MarkCardAsSeen(card.Id))
			{
				return;
			}
			if (LocalContext.IsMine(card))
			{
				card.Owner.DiscoveredCards.Add(card.Id);
			}
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00049F3B File Offset: 0x0004813B
		public void MarkRelicAsSeen(RelicModel relic)
		{
			if (!this.Progress.MarkRelicAsSeen(relic.Id))
			{
				return;
			}
			if (LocalContext.IsMine(relic))
			{
				relic.Owner.DiscoveredRelics.Add(relic.Id);
			}
		}

		/// <summary>
		/// Called whenever a run is completed. This could be Death, Victory, or Abandon.
		/// </summary>
		// Token: 0x06000ADA RID: 2778 RVA: 0x00049F70 File Offset: 0x00048170
		public void UpdateWithRunData(SerializableRun serializableRun, bool victory)
		{
			bool flag = serializableRun.Players.Count == 1;
			bool flag2 = serializableRun.GameMode == GameMode.Daily;
			bool flag3 = serializableRun.GameMode == GameMode.Custom;
			SerializablePlayer serializablePlayer;
			if (flag)
			{
				serializablePlayer = serializableRun.Players.First<SerializablePlayer>();
			}
			else
			{
				ulong playerId = PlatformUtil.GetLocalPlayerId(serializableRun.PlatformType);
				serializablePlayer = serializableRun.Players.FirstOrDefault<SerializablePlayer>((SerializablePlayer p) => p.NetId == playerId);
				if (serializablePlayer == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Local player with net id ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
					defaultInterpolatedStringHandler.AppendLiteral(" not found in run! Progress will not be updated");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					return;
				}
			}
			for (int i = 0; i < serializableRun.MapPointHistory.Count; i++)
			{
				if (i >= serializableRun.Acts.Count)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(88, 2);
					defaultInterpolatedStringHandler.AppendLiteral("There are ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(serializableRun.MapPointHistory.Count);
					defaultInterpolatedStringHandler.AppendLiteral(" acts in the map point history, but ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(serializableRun.Acts.Count);
					defaultInterpolatedStringHandler.AppendLiteral(" acts in the act array! This is unexpected");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				else
				{
					ModelId id = serializableRun.Acts[i].Id;
					this.Progress.MarkActAsSeen(id);
				}
			}
			List<MapPointHistoryEntry> list = serializableRun.MapPointHistory.SelectMany<List<MapPointHistoryEntry>, MapPointHistoryEntry>((List<MapPointHistoryEntry> act) => act).ToList<MapPointHistoryEntry>();
			this.Progress.TotalPlaytime += ((serializableRun.WinTime > 0L) ? serializableRun.WinTime : serializableRun.RunTime);
			this.Progress.WongoPoints += serializablePlayer.ExtraFields.WongoPoints;
			this.Progress.TestSubjectKills += serializableRun.ExtraFields.TestSubjectKills;
			this.Progress.FloorsClimbed += (long)list.Count;
			CharacterStats orCreateCharacterStats = this.Progress.GetOrCreateCharacterStats(serializablePlayer.CharacterId);
			orCreateCharacterStats.Playtime += ((serializableRun.WinTime > 0L) ? serializableRun.WinTime : serializableRun.RunTime);
			if (victory)
			{
				int num = ScoreUtility.CalculateScore(serializableRun, victory);
				this.Progress.ArchitectDamage += (long)num;
				if (!flag2 && !flag3)
				{
					if (flag)
					{
						ProgressSaveManager.IncrementSingleplayerAscension(serializableRun, orCreateCharacterStats);
					}
					else
					{
						this.IncrementMultiplayerAscension(serializableRun);
					}
					CharacterStats characterStats = orCreateCharacterStats;
					int num2 = characterStats.TotalWins;
					characterStats.TotalWins = num2 + 1;
					CharacterStats characterStats2 = orCreateCharacterStats;
					long num3 = characterStats2.CurrentWinStreak;
					characterStats2.CurrentWinStreak = num3 + 1L;
					orCreateCharacterStats.BestWinStreak = Math.Max(orCreateCharacterStats.BestWinStreak, orCreateCharacterStats.CurrentWinStreak);
					if (orCreateCharacterStats.FastestWinTime < 0L || orCreateCharacterStats.FastestWinTime > serializableRun.RunTime)
					{
						orCreateCharacterStats.FastestWinTime = serializableRun.RunTime;
					}
				}
			}
			else
			{
				if (!flag2 && !flag3)
				{
					orCreateCharacterStats.CurrentWinStreak = 0L;
					CharacterStats characterStats3 = orCreateCharacterStats;
					int num2 = characterStats3.TotalLosses;
					characterStats3.TotalLosses = num2 + 1;
				}
				List<MapPointHistoryEntry> list2 = serializableRun.MapPointHistory.LastOrDefault<List<MapPointHistoryEntry>>();
				MapPointHistoryEntry mapPointHistoryEntry = ((list2 != null) ? list2.LastOrDefault<MapPointHistoryEntry>() : null);
				if (mapPointHistoryEntry != null)
				{
					RoomType roomType = mapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().RoomType;
					if (roomType.IsCombatRoom())
					{
						ModelId modelId = mapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().ModelId;
						ModelId characterId = serializablePlayer.CharacterId;
						this.IncrementEncounterLoss(characterId, modelId);
						foreach (ModelId modelId2 in mapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().MonsterIds)
						{
							this.IncrementEnemyFightLoss(characterId, modelId2);
						}
					}
				}
			}
			foreach (MapPointHistoryEntry mapPointHistoryEntry2 in list)
			{
				MapPointRoomHistoryEntry mapPointRoomHistoryEntry = mapPointHistoryEntry2.FirstRoomOfType(RoomType.Event);
				if (mapPointRoomHistoryEntry != null)
				{
					this.Progress.MarkEventAsSeen(mapPointRoomHistoryEntry.ModelId);
				}
			}
			if (!flag2 && !flag3)
			{
				HashSet<ModelId> hashSet = serializablePlayer.Deck.Select<SerializableCard, ModelId>((SerializableCard c) => c.Id).ToHashSet<ModelId>();
				foreach (ModelId modelId3 in hashSet)
				{
					CardStats orCreateCardStats = this.Progress.GetOrCreateCardStats(modelId3);
					if (victory)
					{
						CardStats cardStats = orCreateCardStats;
						long num3 = cardStats.TimesWon;
						cardStats.TimesWon = num3 + 1L;
					}
					else
					{
						CardStats cardStats2 = orCreateCardStats;
						long num3 = cardStats2.TimesLost;
						cardStats2.TimesLost = num3 + 1L;
					}
				}
				foreach (MapPointHistoryEntry mapPointHistoryEntry3 in list)
				{
					PlayerMapPointHistoryEntry entry = mapPointHistoryEntry3.GetEntry(serializablePlayer.NetId);
					foreach (CardChoiceHistoryEntry cardChoiceHistoryEntry in entry.CardChoices)
					{
						CardStats orCreateCardStats2 = this.Progress.GetOrCreateCardStats(cardChoiceHistoryEntry.Card.Id);
						if (cardChoiceHistoryEntry.wasPicked)
						{
							CardStats cardStats3 = orCreateCardStats2;
							long num3 = cardStats3.TimesPicked;
							cardStats3.TimesPicked = num3 + 1L;
						}
						else
						{
							CardStats cardStats4 = orCreateCardStats2;
							long num3 = cardStats4.TimesSkipped;
							cardStats4.TimesSkipped = num3 + 1L;
						}
					}
					if (mapPointHistoryEntry3.MapPointType == MapPointType.Ancient)
					{
						MapPointRoomHistoryEntry mapPointRoomHistoryEntry2 = mapPointHistoryEntry3.FirstRoomOfType(RoomType.Event);
						AncientStats orCreateAncientStats = this.Progress.GetOrCreateAncientStats(mapPointRoomHistoryEntry2.ModelId);
						if (victory)
						{
							orCreateAncientStats.IncrementWin(serializablePlayer.CharacterId);
						}
						else
						{
							orCreateAncientStats.IncrementLoss(serializablePlayer.CharacterId);
						}
					}
				}
			}
			this.UpdateEpochsPostRun(serializablePlayer, serializableRun, victory);
			this.SaveProgress();
		}

		/// <summary>
		/// Check for Epoch-related unlocks at the end of each run.
		/// Singleplayer only.
		/// </summary>
		// Token: 0x06000ADB RID: 2779 RVA: 0x0004A588 File Offset: 0x00048788
		private void UpdateEpochsPostRun(SerializablePlayer serializablePlayer, SerializableRun serializableRun, bool victory)
		{
			this.TryObtainEpochPostRun(EpochModel.Get<NeowEpoch>(), serializablePlayer, serializableRun);
			this.PostRunUnlockCharacterEpochCheck(serializablePlayer, serializableRun);
			this.PostRunCharacterEpochChecks(serializablePlayer, serializableRun, victory);
			int num = ModelDb.AllCharacters.Count<CharacterModel>();
			if (victory && this.Progress.CharacterStats.Count >= num)
			{
				this.TryObtainEpochPostRun(EpochModel.Get<DailyRunEpoch>(), serializablePlayer, serializableRun);
			}
			if (this.Progress.CharacterStats.Count >= num)
			{
				this.TryObtainEpochPostRun(EpochModel.Get<OrobasEpoch>(), serializablePlayer, serializableRun);
			}
			if (ModelDb.AllAncients.All<AncientEventModel>((AncientEventModel a) => this.Progress.AncientStats.ContainsKey(a.Id) || a is Darv))
			{
				this.TryObtainEpochPostRun(EpochModel.Get<DarvEpoch>(), serializablePlayer, serializableRun);
			}
		}

		/// <summary>
		/// If you complete a run with a character for the first time,
		/// there's generally an unlock for another character.
		/// </summary>
		// Token: 0x06000ADC RID: 2780 RVA: 0x0004A62C File Offset: 0x0004882C
		private void PostRunUnlockCharacterEpochCheck(SerializablePlayer serializablePlayer, SerializableRun serializableRun)
		{
			CharacterModel byId = ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId);
			string text;
			if (!(byId is Silent))
			{
				if (!(byId is Regent))
				{
					if (!(byId is Necrobinder))
					{
						text = null;
					}
					else
					{
						text = EpochModel.GetId<Defect1Epoch>();
					}
				}
				else
				{
					text = EpochModel.GetId<Necrobinder1Epoch>();
				}
			}
			else
			{
				text = EpochModel.GetId<Regent1Epoch>();
			}
			string text2 = text;
			if (text2 != null && this.TryObtainEpochPostRun(EpochModel.Get(text2), serializablePlayer, serializableRun))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Epoch obtained for playing a run as ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(serializablePlayer.CharacterId);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0004A6C0 File Offset: 0x000488C0
		private void PostRunCharacterEpochChecks(SerializablePlayer serializablePlayer, SerializableRun serializableRun, bool victory)
		{
			if (!victory)
			{
				return;
			}
			this.CheckAscensionOneCompleted(serializablePlayer, serializableRun);
			string id = EpochModel.GetId<CustomAndSeedsEpoch>();
			if (this.Progress.Wins >= 3)
			{
				this.TryObtainEpochPostRun(EpochModel.Get(id), serializablePlayer, serializableRun);
			}
		}

		/// <summary>
		/// When the player completes Ascension 1 they unlock Character Epoch 7.
		/// This unlock grants the player the means to Kill the Architect and 3 cards.
		/// </summary>
		// Token: 0x06000ADE RID: 2782 RVA: 0x0004A6FC File Offset: 0x000488FC
		private void CheckAscensionOneCompleted(SerializablePlayer serializablePlayer, SerializableRun serializableRun)
		{
			if (serializableRun.Ascension != 1)
			{
				return;
			}
			CharacterModel byId = ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId);
			string text;
			if (!(byId is Ironclad))
			{
				if (!(byId is Silent))
				{
					if (!(byId is Regent))
					{
						if (!(byId is Defect))
						{
							if (!(byId is Necrobinder))
							{
								text = null;
							}
							else
							{
								text = EpochModel.GetId<Necrobinder7Epoch>();
							}
						}
						else
						{
							text = EpochModel.GetId<Defect7Epoch>();
						}
					}
					else
					{
						text = EpochModel.GetId<Regent7Epoch>();
					}
				}
				else
				{
					text = EpochModel.GetId<Silent7Epoch>();
				}
			}
			else
			{
				text = EpochModel.GetId<Ironclad7Epoch>();
			}
			string text2 = text;
			if (text2 != null)
			{
				this.TryObtainEpochPostRun(EpochModel.Get(text2), serializablePlayer, serializableRun);
			}
		}

		/// <summary>
		/// Checks for Character5Epoch for all characters.
		/// Requires defeating 15 Elites in the game with the character.
		/// </summary>
		// Token: 0x06000ADF RID: 2783 RVA: 0x0004A788 File Offset: 0x00048988
		private void CheckFifteenElitesDefeatedEpoch(Player localPlayer)
		{
			CharacterModel character = localPlayer.Character;
			EpochModel epochModel;
			if (!(character is Ironclad))
			{
				if (!(character is Silent))
				{
					if (!(character is Regent))
					{
						if (!(character is Defect))
						{
							if (!(character is Necrobinder))
							{
								if (!(character is Deprived))
								{
									throw new ArgumentOutOfRangeException("character", character, null);
								}
								epochModel = null;
							}
							else
							{
								epochModel = EpochModel.Get(EpochModel.GetId<Necrobinder5Epoch>());
							}
						}
						else
						{
							epochModel = EpochModel.Get(EpochModel.GetId<Defect5Epoch>());
						}
					}
					else
					{
						epochModel = EpochModel.Get(EpochModel.GetId<Regent5Epoch>());
					}
				}
				else
				{
					epochModel = EpochModel.Get(EpochModel.GetId<Silent5Epoch>());
				}
			}
			else
			{
				epochModel = EpochModel.Get(EpochModel.GetId<Ironclad5Epoch>());
			}
			EpochModel epochModel2 = epochModel;
			if (epochModel2 == null)
			{
				return;
			}
			HashSet<ModelId> eliteEncounters = ProgressSaveManager.GetEliteEncounters();
			int num = 0;
			foreach (EncounterStats encounterStats in this.Progress.EncounterStats.Values)
			{
				if (eliteEncounters.Contains(encounterStats.Id))
				{
					foreach (FightStats fightStats in encounterStats.FightStats)
					{
						if (fightStats.Character == character.Id)
						{
							num += fightStats.Wins;
							break;
						}
					}
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Elites Defeated: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(eliteEncounters.Count);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			if (num >= 15)
			{
				this.TryObtainEpochMidRun(epochModel2, localPlayer);
			}
		}

		/// <summary>
		/// Checks for Character6Epoch for all characters.
		/// Requires defeating 15 Bosses in the game with the character.
		/// </summary>
		// Token: 0x06000AE0 RID: 2784 RVA: 0x0004A940 File Offset: 0x00048B40
		private void CheckFifteenBossesDefeatedEpoch(Player localPlayer)
		{
			CharacterModel character = localPlayer.Character;
			EpochModel epochModel;
			if (!(character is Ironclad))
			{
				if (!(character is Silent))
				{
					if (!(character is Regent))
					{
						if (!(character is Defect))
						{
							if (!(character is Necrobinder))
							{
								if (!(character is Deprived))
								{
									throw new ArgumentOutOfRangeException("character", character, null);
								}
								epochModel = null;
							}
							else
							{
								epochModel = EpochModel.Get(EpochModel.GetId<Necrobinder6Epoch>());
							}
						}
						else
						{
							epochModel = EpochModel.Get(EpochModel.GetId<Defect6Epoch>());
						}
					}
					else
					{
						epochModel = EpochModel.Get(EpochModel.GetId<Regent6Epoch>());
					}
				}
				else
				{
					epochModel = EpochModel.Get(EpochModel.GetId<Silent6Epoch>());
				}
			}
			else
			{
				epochModel = EpochModel.Get(EpochModel.GetId<Ironclad6Epoch>());
			}
			EpochModel epochModel2 = epochModel;
			if (epochModel2 == null)
			{
				return;
			}
			HashSet<ModelId> hashSet = ModelDb.Acts.SelectMany<ActModel, ModelId>((ActModel a) => a.AllBossEncounters.Select<EncounterModel, ModelId>((EncounterModel e) => e.Id)).ToHashSet<ModelId>();
			int num = 0;
			foreach (EncounterStats encounterStats in this.Progress.EncounterStats.Values)
			{
				if (hashSet.Contains(encounterStats.Id))
				{
					foreach (FightStats fightStats in encounterStats.FightStats)
					{
						if (fightStats.Character == character.Id)
						{
							num += fightStats.Wins;
							break;
						}
					}
				}
			}
			if (num >= 15)
			{
				this.TryObtainEpochMidRun(epochModel2, localPlayer);
			}
		}

		/// <summary>
		/// Helper function to obtain character unlocks 1, 2, and 3.
		/// Requires the player to defeat a boss in an act for the first time (for each character!)
		/// </summary>
		// Token: 0x06000AE1 RID: 2785 RVA: 0x0004AADC File Offset: 0x00048CDC
		private void ObtainCharUnlockEpoch(Player localPlayer, int act)
		{
			if (!localPlayer.Character.IsPlayable)
			{
				return;
			}
			string text = localPlayer.Character.Id.Entry.ToUpperInvariant();
			EpochModel epochModel = null;
			switch (act)
			{
			case 0:
				epochModel = EpochModel.Get(text + "2_EPOCH");
				break;
			case 1:
				epochModel = EpochModel.Get(text + "3_EPOCH");
				break;
			case 2:
				epochModel = EpochModel.Get(text + "4_EPOCH");
				break;
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Act ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(act + 1);
				defaultInterpolatedStringHandler.AppendLiteral(" is not yet implemented.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported Act: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(act);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				break;
			}
			}
			if (epochModel == null)
			{
				Log.Error("EpochModel was not found :(", 2);
				return;
			}
			if (this.TryObtainEpochMidRun(epochModel, localPlayer))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Epoch obtained for completing Act ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(act + 1);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		/// <summary>
		/// Called mid-run (like when you kill a boss that unlocks an Epoch).
		/// If this Epoch's slot is available, we obtain it and return true.
		/// We return a bool so callers can log or show VFX accordingly.
		/// </summary>
		/// <param name="epoch">Epoch to try to obtain.</param>
		/// <param name="localPlayer">The local player's Player instance.</param>
		/// <returns>
		/// True if you Obtained a new Epoch, false if not (you already have it OR the slot is unavailable).
		/// </returns>
		// Token: 0x06000AE2 RID: 2786 RVA: 0x0004AC0F File Offset: 0x00048E0F
		private bool TryObtainEpochMidRun(EpochModel epoch, Player localPlayer)
		{
			if (localPlayer.RunState.GameMode.AreAchievementsAndEpochsLocked())
			{
				return false;
			}
			if (!this.TryObtainEpochInternal(epoch))
			{
				return false;
			}
			localPlayer.DiscoveredEpochs.Add(epoch.Id);
			return true;
		}

		/// <summary>
		/// Called post-run (like after abandoning your first run, which unlocks the Silent).
		/// If this Epoch's slot is available, we obtain it and return true.
		/// We return a bool so callers can log or show VFX accordingly.
		/// </summary>
		/// <param name="epoch">Epoch to try to obtain.</param>
		/// <param name="serializablePlayer">The serializable version of the local player's Player instance.</param>
		/// <param name="serializableRun">The serializable version of the run.</param>
		/// <returns>
		/// True if you Obtained a new Epoch, false if not (you already have it OR the slot is unavailable).
		/// </returns>
		// Token: 0x06000AE3 RID: 2787 RVA: 0x0004AC42 File Offset: 0x00048E42
		private bool TryObtainEpochPostRun(EpochModel epoch, SerializablePlayer serializablePlayer, SerializableRun serializableRun)
		{
			if (serializableRun.GameMode.AreAchievementsAndEpochsLocked())
			{
				return false;
			}
			if (!this.TryObtainEpochInternal(epoch))
			{
				return false;
			}
			serializablePlayer.DiscoveredEpochs.Add(epoch.Id);
			return true;
		}

		/// <summary>
		/// WARNING: This should only be called by <see cref="M:MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager.TryObtainEpochMidRun(MegaCrit.Sts2.Core.Timeline.EpochModel,MegaCrit.Sts2.Core.Entities.Players.Player)" /> and
		/// <see cref="M:MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager.TryObtainEpochPostRun(MegaCrit.Sts2.Core.Timeline.EpochModel,MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer,MegaCrit.Sts2.Core.Saves.SerializableRun)" />.
		/// If this Epoch's slot is available, we obtain it and return true.
		/// We return a bool so callers can log or show VFX accordingly.
		/// </summary>
		/// <param name="epoch">Epoch to try to obtain.</param>
		/// <returns>
		/// True if you Obtained a new Epoch, false if not (you already have it OR the slot is unavailable).
		/// </returns>
		// Token: 0x06000AE4 RID: 2788 RVA: 0x0004AC70 File Offset: 0x00048E70
		private bool TryObtainEpochInternal(EpochModel epoch)
		{
			if (this.Progress.IsEpochObtained(epoch.Id))
			{
				Log.Info("Player already has Epoch: " + epoch.Id, 2);
				return false;
			}
			this.Progress.ObtainEpoch(epoch.Id);
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.AddChildSafely(NGainEpochVfx.Create(epoch));
			}
			if (this.GetRevealableEpochs().All<SerializableEpoch>((SerializableEpoch e) => e.Id != epoch.Id))
			{
				string text = "Epoch " + epoch.Id + " was obtained, but is not yet revealable by the player!";
				Log.Warn(text, 2);
				SentryService.CaptureMessage(text, 1, delegate(Scope scope)
				{
					string text2 = JsonSerializer.Serialize(this.Progress.Epochs, JsonSerializationUtility.GetTypeInfo<List<SerializableEpoch>>());
					scope.AddCompressedAttachment(text2, "epochs.txt.gz");
				});
			}
			return true;
		}

		/// <summary>
		/// Returns the set of epochs that the player has both obtained and may reveal.
		/// In certain (buggy) scenarios, the player may end up obtaining epochs that are still hidden. An example scenario
		/// (though this may be fixed in the future) is:
		///  - The player joins a multiplayer daily, which allows them to play a character they have not unlocked
		///  - The player obtains an epoch for that character which they can't see on the timeline
		///
		/// This method skips those sorts of epochs. However, it still returns epochs that are in ObtainedNoSlot state but
		/// are about to be revealed by another epoch the player has obtained.
		/// </summary>
		// Token: 0x06000AE5 RID: 2789 RVA: 0x0004AD48 File Offset: 0x00048F48
		public IEnumerable<SerializableEpoch> GetRevealableEpochs()
		{
			HashSet<string> satisfiedEpochIds = new HashSet<string>(from e in this.Progress.Epochs.Where<SerializableEpoch>(delegate(SerializableEpoch e)
				{
					EpochState state2 = e.State;
					return state2 - EpochState.ObtainedNoSlot <= 1;
				})
				select e.Id);
			HashSet<string> reachableSet = new HashSet<string>();
			Queue<string> queue = new Queue<string>();
			string id = EpochModel.Get<NeowEpoch>().Id;
			reachableSet.Add(id);
			queue.Enqueue(id);
			using (IEnumerator<SerializableEpoch> enumerator = this.Progress.Epochs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SerializableEpoch serializableEpoch = enumerator.Current;
					bool flag = serializableEpoch.Id == id;
					bool flag2 = flag;
					if (!flag2)
					{
						EpochState state = serializableEpoch.State;
						bool flag3 = state == EpochState.None || state - EpochState.NotObtained <= 1;
						flag2 = flag3;
					}
					if (!flag2)
					{
						reachableSet.Add(serializableEpoch.Id);
						queue.Enqueue(serializableEpoch.Id);
					}
				}
				goto IL_0199;
			}
			IL_0121:
			string text = queue.Dequeue();
			foreach (EpochModel epochModel in EpochModel.Get(text).GetTimelineExpansion())
			{
				if (!reachableSet.Contains(epochModel.Id))
				{
					reachableSet.Add(epochModel.Id);
					if (satisfiedEpochIds.Contains(epochModel.Id))
					{
						queue.Enqueue(epochModel.Id);
					}
				}
			}
			IL_0199:
			if (queue.Count <= 0)
			{
				return this.Progress.Epochs.Where<SerializableEpoch>((SerializableEpoch e) => satisfiedEpochIds.Contains(e.Id) && reachableSet.Contains(e.Id));
			}
			goto IL_0121;
		}

		/// <summary>
		/// Lots of checks and update of the progress save file after the end of each combat.
		/// </summary>
		// Token: 0x06000AE6 RID: 2790 RVA: 0x0004AF28 File Offset: 0x00049128
		public void UpdateAfterCombatWon(Player localPlayer, CombatRoom room)
		{
			CombatState combatState = room.CombatState;
			IRunState runState = combatState.RunState;
			CharacterModel character = localPlayer.Character;
			ModelId id = combatState.Encounter.Id;
			EncounterStats orCreateEncounterStats = this.Progress.GetOrCreateEncounterStats(id);
			if (orCreateEncounterStats.FightStats.FirstOrDefault<FightStats>((FightStats f) => f.Character == character.Id) == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(character.Id);
				defaultInterpolatedStringHandler.AppendLiteral(" fought ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(id);
				defaultInterpolatedStringHandler.AppendLiteral(" for the first time and WON >:)");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				FightStats fightStats = new FightStats
				{
					Character = character.Id,
					Wins = 1,
					Losses = 0
				};
				orCreateEncounterStats.FightStats.Add(fightStats);
			}
			else
			{
				orCreateEncounterStats.IncrementWin(character.Id);
			}
			if (room.RoomType == RoomType.Boss)
			{
				this.ObtainCharUnlockEpoch(localPlayer, runState.CurrentActIndex);
				this.CheckFifteenBossesDefeatedEpoch(localPlayer);
			}
			else if (room.RoomType == RoomType.Elite)
			{
				this.CheckFifteenElitesDefeatedEpoch(localPlayer);
			}
			Func<FightStats, bool> <>9__1;
			foreach (MonsterModel monsterModel in room.Encounter.SpawnedEnemies)
			{
				EnemyStats orCreateEnemyStats = this.Progress.GetOrCreateEnemyStats(monsterModel.Id);
				bool flag = orCreateEnemyStats.FightStats.Count == 0;
				IEnumerable<FightStats> fightStats2 = orCreateEnemyStats.FightStats;
				Func<FightStats, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (FightStats f) => f.Character == character.Id);
				}
				if (fightStats2.FirstOrDefault<FightStats>(func) == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(character.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" fought ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(monsterModel.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" for the first time and WON >:(");
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					FightStats fightStats3 = new FightStats
					{
						Character = character.Id,
						Wins = 1,
						Losses = 0
					};
					orCreateEnemyStats.FightStats.Add(fightStats3);
					if (flag)
					{
						localPlayer.DiscoveredEnemies.Add(monsterModel.Id);
					}
				}
				else
				{
					orCreateEnemyStats.IncrementWin(character.Id);
				}
			}
		}

		/// <summary>
		/// If the player wins a singleplayer run, this logic checks if they're playing on Max Ascension.
		/// If so, increment it by 1.
		/// </summary>
		// Token: 0x06000AE7 RID: 2791 RVA: 0x0004B1AC File Offset: 0x000493AC
		private static void IncrementSingleplayerAscension(SerializableRun run, CharacterStats charStats)
		{
			if (run.Ascension == charStats.MaxAscension)
			{
				if (charStats.MaxAscension < 10)
				{
					int maxAscension = charStats.MaxAscension;
					charStats.MaxAscension = maxAscension + 1;
					charStats.PreferredAscension = charStats.MaxAscension;
					return;
				}
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not playing on max singleplayer ascension (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charStats.MaxAscension);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		/// <summary>
		/// If the player wins a multiplayer run, this logic checks if they're playing on Max Ascension.
		/// If so, increment it by 1.
		/// </summary>
		// Token: 0x06000AE8 RID: 2792 RVA: 0x0004B22C File Offset: 0x0004942C
		private void IncrementMultiplayerAscension(SerializableRun run)
		{
			if (run.Ascension == this.Progress.MaxMultiplayerAscension)
			{
				if (this.Progress.MaxMultiplayerAscension < 10)
				{
					ProgressState progress = this.Progress;
					int maxMultiplayerAscension = progress.MaxMultiplayerAscension;
					progress.MaxMultiplayerAscension = maxMultiplayerAscension + 1;
					this.Progress.PreferredMultiplayerAscension = this.Progress.MaxMultiplayerAscension;
					return;
				}
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not playing on max multiplayer ascension (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Progress.MaxMultiplayerAscension);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		/// <summary>
		/// Returns true if all ftues are disabled OR if the given ftue key exists (seen by the player before).
		/// Is also disabled if there's no game (test mode)
		/// </summary>
		// Token: 0x06000AE9 RID: 2793 RVA: 0x0004B2C8 File Offset: 0x000494C8
		public bool SeenFtue(string ftueKey)
		{
			return !this.Progress.EnableFtues || this.Progress.FtueCompleted.Contains(ftueKey);
		}

		/// <summary>
		/// Checks if a one-time popup has been seen before or not.
		/// This is only used by the "Ascension Unlocked" popups for now.
		/// </summary>
		// Token: 0x06000AEA RID: 2794 RVA: 0x0004B2EA File Offset: 0x000494EA
		public bool SeenPopup(string popupKey)
		{
			return TestMode.IsOn || this.Progress.FtueCompleted.Contains(popupKey);
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0004B306 File Offset: 0x00049506
		public void MarkFtueAsComplete(string ftueId)
		{
			if (!this.Progress.MarkFtueAsComplete(ftueId))
			{
				return;
			}
			Log.Info("Player has seen ftue " + ftueId + "!", 2);
			this.SaveProgress();
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0004B334 File Offset: 0x00049534
		public void SetFtuesEnabled(bool enabled)
		{
			if (this.Progress.EnableFtues == enabled)
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player has set FTUEs enabled: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(enabled);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this.Progress.EnableFtues = enabled;
			this.SaveProgress();
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0004B38D File Offset: 0x0004958D
		public void ResetFtues()
		{
			Log.Info("Player has reset FTUEs to enabled", 2);
			this.Progress.ResetFtues();
			this.SaveProgress();
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0004B3AC File Offset: 0x000495AC
		private static HashSet<ModelId> GetEliteEncounters()
		{
			HashSet<ModelId> hashSet;
			if ((hashSet = ProgressSaveManager._eliteEncounters) == null)
			{
				hashSet = (ProgressSaveManager._eliteEncounters = (from e in ModelDb.AllEncounters
					where e.RoomType == RoomType.Elite
					select e.Id).Distinct<ModelId>().ToHashSet<ModelId>());
			}
			return hashSet;
		}

		// Token: 0x040003C1 RID: 961
		public const string fileName = "progress.save";

		// Token: 0x040003C2 RID: 962
		private readonly ISaveStore _saveStore;

		// Token: 0x040003C3 RID: 963
		private readonly MigrationManager _migrationManager;

		// Token: 0x040003C4 RID: 964
		private readonly IProfileIdProvider _profileIdProvider;

		// Token: 0x040003C6 RID: 966
		[Nullable(new byte[] { 2, 1 })]
		private static HashSet<ModelId> _eliteEncounters;
	}
}
