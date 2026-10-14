using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.TreasureRelicPicking;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Synchronizes treasure room shared relic picking in multiplayer.
	/// After opening the treasure chest, players are presented with multiple relics. They may vote on any one of the relics
	/// to take. After all players cast a vote:
	/// - If only one player votes for a relic, then that player gets the relic.
	/// - If multiple players have voted for a relic, they play rock-paper-scissors. The winning player gets the relic.
	/// - If no one has voted for a relic, a random loser of a rock-paper-scissors game gets the relic.
	/// </summary>
	// Token: 0x020004A9 RID: 1193
	[NullableContext(1)]
	[Nullable(0)]
	public class TreasureRoomRelicSynchronizer
	{
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x060047E7 RID: 18407 RVA: 0x0021F48E File Offset: 0x0021D68E
		[Nullable(new byte[] { 2, 1 })]
		public IReadOnlyList<RelicModel> CurrentRelics
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._currentRelics;
			}
		}

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x060047E8 RID: 18408 RVA: 0x0021F498 File Offset: 0x0021D698
		// (remove) Token: 0x060047E9 RID: 18409 RVA: 0x0021F4D0 File Offset: 0x0021D6D0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action VotesChanged;

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x060047EA RID: 18410 RVA: 0x0021F508 File Offset: 0x0021D708
		// (remove) Token: 0x060047EB RID: 18411 RVA: 0x0021F540 File Offset: 0x0021D740
		[Nullable(new byte[] { 2, 1, 1 })]
		[field: Nullable(new byte[] { 2, 1, 1 })]
		public event Action<List<RelicPickingResult>> RelicsAwarded;

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x060047EC RID: 18412 RVA: 0x0021F575 File Offset: 0x0021D775
		private Player LocalPlayer
		{
			get
			{
				return this._playerCollection.GetPlayer(this._localPlayerId);
			}
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x0021F588 File Offset: 0x0021D788
		public TreasureRoomRelicSynchronizer(IPlayerCollection playerCollection, ulong localPlayerId, ActionQueueSynchronizer actionQueueSynchronizer, RelicGrabBag sharedGrabBag, Rng rng)
		{
			this._playerCollection = playerCollection;
			this._localPlayerId = localPlayerId;
			this._actionQueueSynchronizer = actionQueueSynchronizer;
			this._sharedGrabBag = sharedGrabBag;
			this._rng = rng;
		}

		/// <summary>
		/// Call this when the treasure room is opened to generate the relics that are selected.
		/// CurrentRelics will be null until this is called.
		/// </summary>
		// Token: 0x060047EE RID: 18414 RVA: 0x0021F5DC File Offset: 0x0021D7DC
		public void BeginRelicPicking()
		{
			if (this.CurrentRelics != null)
			{
				throw new InvalidOperationException("Attempted to start new relic picking session while one was already occurring!");
			}
			this._currentRelics = new List<RelicModel>();
			this._votes.Clear();
			this._predictedVote = null;
			foreach (Player player in this._playerCollection.Players)
			{
				this._votes.Add(new TreasureRoomRelicSynchronizer.PlayerVote
				{
					voteReceived = false
				});
				IRunState runState = player.RunState;
				if (Hook.ShouldGenerateTreasure(runState, player))
				{
					RelicRarity relicRarity = RelicFactory.RollRarity(this._rng);
					RelicModel relicModel;
					if ((relicModel = this.TryGetRelicForTutorial(player)) == null)
					{
						relicModel = this._sharedGrabBag.PullFromFront(relicRarity, runState) ?? RelicFactory.FallbackRelic;
					}
					RelicModel relicModel2 = relicModel;
					this._currentRelics.Add(relicModel2);
				}
			}
			if (this._currentRelics.Count <= 0)
			{
				this.EndRelicVoting();
				return;
			}
			if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer && this._playerCollection.Players.Count > 1)
			{
				foreach (Player player2 in this._playerCollection.Players)
				{
					if (player2 != this.LocalPlayer)
					{
						TreasureRoomRelicSynchronizer.PlayerVote playerVote = this._votes[this._playerCollection.GetPlayerSlotIndex(player2)];
						playerVote.index = new int?(this._rng.NextInt(this._currentRelics.Count));
						playerVote.voteReceived = true;
					}
				}
			}
			Action votesChanged = this.VotesChanged;
			if (votesChanged == null)
			{
				return;
			}
			votesChanged();
		}

		/// <summary>
		/// Little semantic sugar which calls PickRelicLocally with null.
		/// </summary>
		// Token: 0x060047EF RID: 18415 RVA: 0x0021F798 File Offset: 0x0021D998
		public void SkipRelicLocally()
		{
			this.PickRelicLocally(null);
		}

		/// <summary>
		/// Called when the local player chooses a relic to vote for.
		/// Note that this immediately invokes `VotesChanged` with the new vote. However, the vote is not confirmed until
		/// the resulting action round-trips to the host.
		/// </summary>
		/// <param name="index">The index of the relic in CurrentRelics that the player voted for, or null if the player
		/// chose to skip.</param>
		// Token: 0x060047F0 RID: 18416 RVA: 0x0021F7B4 File Offset: 0x0021D9B4
		public void PickRelicLocally(int? index)
		{
			if (index != null)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Relic index ");
				defaultInterpolatedStringHandler.AppendFormatted<int?>(index);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				List<RelicModel> currentRelics = this._currentRelics;
				defaultInterpolatedStringHandler.AppendFormatted<RelicModel>((currentRelics != null) ? currentRelics[index.Value] : null);
				defaultInterpolatedStringHandler.AppendLiteral(") is being picked by local player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.LocalPlayer.NetId);
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			else
			{
				Logger logger2 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Relic has been skipped by local player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.LocalPlayer.NetId);
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			if (this._currentRelics == null)
			{
				throw new InvalidOperationException("Attempted to pick relic while relic picking is not active!");
			}
			this._predictedVote = new TreasureRoomRelicSynchronizer.PlayerVote
			{
				index = index,
				voteReceived = true
			};
			this._actionQueueSynchronizer.RequestEnqueue(new PickRelicAction(this.LocalPlayer, index));
			Action votesChanged = this.VotesChanged;
			if (votesChanged == null)
			{
				return;
			}
			votesChanged();
		}

		/// <summary>
		/// Called when any player (local or remote) has voted for a relic.
		/// </summary>
		/// <param name="player">The player that voted.</param>
		/// <param name="index">The index of the relic in CurrentRelics that the player voted for.</param>
		// Token: 0x060047F1 RID: 18417 RVA: 0x0021F8D8 File Offset: 0x0021DAD8
		public void OnPicked(Player player, int? index)
		{
			if (index != null)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<Player>(player);
				defaultInterpolatedStringHandler.AppendLiteral(" picked relic at index ");
				defaultInterpolatedStringHandler.AppendFormatted<int?>(index);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				List<RelicModel> currentRelics = this._currentRelics;
				defaultInterpolatedStringHandler.AppendFormatted<RelicModel>((currentRelics != null) ? currentRelics[index.Value] : null);
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			else
			{
				Logger logger2 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<Player>(player);
				defaultInterpolatedStringHandler.AppendLiteral(" skipped relic");
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			if (this._currentRelics == null)
			{
				this._logger.Warn("Attempted to pick relic while relic picking is not active!", 1);
				return;
			}
			int? num = index;
			int count = this._currentRelics.Count;
			if ((num.GetValueOrDefault() >= count) & (num != null))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to pick relic at index ");
				defaultInterpolatedStringHandler.AppendFormatted<int?>(index);
				defaultInterpolatedStringHandler.AppendLiteral(", but there are only ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._currentRelics.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" to choose from!");
				throw new IndexOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (index == null && this._playerCollection.Players.Count == 1)
			{
				this._singleplayerSkipped = true;
				return;
			}
			TreasureRoomRelicSynchronizer.PlayerVote playerVote = this._votes[this._playerCollection.GetPlayerSlotIndex(player)];
			playerVote.index = index;
			playerVote.voteReceived = true;
			Action votesChanged = this.VotesChanged;
			if (votesChanged != null)
			{
				votesChanged();
			}
			if (this._votes.All<TreasureRoomRelicSynchronizer.PlayerVote>((TreasureRoomRelicSynchronizer.PlayerVote v) => v.voteReceived))
			{
				if (this._predictedVote != null)
				{
					TreasureRoomRelicSynchronizer.PlayerVote predictedVote = this._predictedVote;
					this._predictedVote = null;
					num = this._votes[this._playerCollection.GetPlayerSlotIndex(this.LocalPlayer)].index;
					int? index2 = predictedVote.index;
					if (!((num.GetValueOrDefault() == index2.GetValueOrDefault()) & (num != null == (index2 != null))))
					{
						Action votesChanged2 = this.VotesChanged;
						if (votesChanged2 != null)
						{
							votesChanged2();
						}
					}
				}
				this.AwardRelics();
				this.EndRelicVoting();
			}
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x0021FB40 File Offset: 0x0021DD40
		private void AwardRelics()
		{
			Dictionary<int, List<Player>> dictionary = new Dictionary<int, List<Player>>();
			for (int i = 0; i < this._currentRelics.Count; i++)
			{
				dictionary[i] = new List<Player>();
			}
			for (int j = 0; j < this._votes.Count; j++)
			{
				Player player = this._playerCollection.Players[j];
				TreasureRoomRelicSynchronizer.PlayerVote playerVote = this._votes[j];
				if (playerVote.index != null)
				{
					List<Player> valueOrDefault = dictionary.GetValueOrDefault(playerVote.index.Value, new List<Player>());
					valueOrDefault.Add(player);
					dictionary[playerVote.index.Value] = valueOrDefault;
				}
			}
			List<RelicPickingResult> results = new List<RelicPickingResult>();
			List<RelicModel> list = new List<RelicModel>();
			foreach (KeyValuePair<int, List<Player>> keyValuePair in dictionary)
			{
				RelicModel relicModel = this._currentRelics[keyValuePair.Key];
				if (keyValuePair.Value.Count == 0)
				{
					list.Add(relicModel);
				}
				else if (keyValuePair.Value.Count == 1)
				{
					results.Add(new RelicPickingResult
					{
						type = RelicPickingResultType.OnlyOnePlayerVoted,
						relic = relicModel,
						player = keyValuePair.Value[0]
					});
				}
				else if (keyValuePair.Value.Count > 1)
				{
					RelicPickingFightMove[] possibleMoves = Enum.GetValues<RelicPickingFightMove>();
					results.Add(RelicPickingResult.GenerateRelicFight(keyValuePair.Value, relicModel, () => this._rng.NextItem<RelicPickingFightMove>(possibleMoves)));
				}
			}
			List<Player> list2 = this._playerCollection.Players.Where<Player>(delegate(Player p)
			{
				bool flag = results.Find((RelicPickingResult r) => r.player == p) != null;
				TreasureRoomRelicSynchronizer.PlayerVote playerVote2 = this._votes[this._playerCollection.GetPlayerSlotIndex(p)];
				bool flag2 = playerVote2.voteReceived && playerVote2.index == null;
				return !flag && !flag2;
			}).ToList<Player>();
			list.StableShuffle<RelicModel>(this._rng);
			for (int k = 0; k < list.Count; k++)
			{
				if (k < list2.Count)
				{
					results.Add(new RelicPickingResult
					{
						type = RelicPickingResultType.ConsolationPrize,
						player = list2[k],
						relic = list[k]
					});
				}
				else
				{
					results.Add(new RelicPickingResult
					{
						type = RelicPickingResultType.Skipped,
						player = null,
						relic = list[k]
					});
				}
			}
			Action<List<RelicPickingResult>> relicsAwarded = this.RelicsAwarded;
			if (relicsAwarded == null)
			{
				return;
			}
			relicsAwarded(results);
		}

		/// <summary>
		/// Called when the room is exited.
		/// Used to avoid emitting an error in the legitimate case where the player never completes relic picking in singleplayer.
		/// </summary>
		// Token: 0x060047F3 RID: 18419 RVA: 0x0021FDE8 File Offset: 0x0021DFE8
		public void OnRoomExited()
		{
			if (this._singleplayerSkipped)
			{
				this.EndRelicVoting();
			}
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0021FDF8 File Offset: 0x0021DFF8
		private void EndRelicVoting()
		{
			this._currentRelics = null;
			this._singleplayerSkipped = false;
		}

		/// <summary>
		/// Returns the relic that the player is currently voting for.
		/// If the player has not yet voted for a relic, then null is returned.
		/// </summary>
		// Token: 0x060047F5 RID: 18421 RVA: 0x0021FE08 File Offset: 0x0021E008
		public TreasureRoomRelicSynchronizer.PlayerVote GetPlayerVote(Player player)
		{
			if (player == this.LocalPlayer && this._predictedVote != null)
			{
				return this._predictedVote;
			}
			return this._votes[this._playerCollection.GetPlayerSlotIndex(player)];
		}

		/// <summary>
		/// Completes relic picking immediately with no relics awarded.
		/// Called when the chest is empty (e.g., due to SilverCrucible).
		/// </summary>
		// Token: 0x060047F6 RID: 18422 RVA: 0x0021FE39 File Offset: 0x0021E039
		public void CompleteWithNoRelics()
		{
			this._logger.Debug("Completing relic picking with no relics (empty chest)", 1);
			Action<List<RelicPickingResult>> relicsAwarded = this.RelicsAwarded;
			if (relicsAwarded != null)
			{
				relicsAwarded(new List<RelicPickingResult>());
			}
			this._currentRelics = null;
		}

		/// <summary>
		/// If treasure room rarity should be forced because the player is currently playing a tutorial run, this returns
		/// a non-null rarity. Otherwise, the rarity is randomized.
		/// </summary>
		// Token: 0x060047F7 RID: 18423 RVA: 0x0021FE6C File Offset: 0x0021E06C
		[return: Nullable(2)]
		private RelicModel TryGetRelicForTutorial(Player player)
		{
			if (this._playerCollection.Players.Count == 1 && player.UnlockState.NumberOfRuns == 0)
			{
				if (player.RunState.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((IReadOnlyList<MapPointHistoryEntry> l) => l).Count<MapPointHistoryEntry>((MapPointHistoryEntry p) => p.HasRoomOfType(RoomType.Treasure)) == 1 && player.RelicGrabBag.Contains(ModelDb.Relic<Gorget>()))
				{
					Log.Info("Forcing specific relic because it's the player's first treasure chest ever", 2);
					player.RelicGrabBag.Remove<Gorget>();
					this._sharedGrabBag.Remove<Gorget>();
					return ModelDb.Relic<Gorget>();
				}
			}
			return null;
		}

		// Token: 0x04001C33 RID: 7219
		private readonly IPlayerCollection _playerCollection;

		// Token: 0x04001C34 RID: 7220
		private readonly ulong _localPlayerId;

		// Token: 0x04001C35 RID: 7221
		private readonly RelicGrabBag _sharedGrabBag;

		// Token: 0x04001C36 RID: 7222
		private readonly ActionQueueSynchronizer _actionQueueSynchronizer;

		// Token: 0x04001C37 RID: 7223
		private readonly Rng _rng;

		// Token: 0x04001C38 RID: 7224
		private readonly Logger _logger = new Logger("TreasureRoomRelicSynchronizer", LogType.GameSync);

		// Token: 0x04001C39 RID: 7225
		[Nullable(new byte[] { 2, 1 })]
		private List<RelicModel> _currentRelics;

		// Token: 0x04001C3A RID: 7226
		private readonly List<TreasureRoomRelicSynchronizer.PlayerVote> _votes = new List<TreasureRoomRelicSynchronizer.PlayerVote>();

		// Token: 0x04001C3B RID: 7227
		[Nullable(2)]
		private TreasureRoomRelicSynchronizer.PlayerVote _predictedVote;

		/// <summary>
		/// Set to true if the player skips the relic in singleplayer.
		/// </summary>
		// Token: 0x04001C3C RID: 7228
		private bool _singleplayerSkipped;

		// Token: 0x020019D9 RID: 6617
		[NullableContext(0)]
		public class PlayerVote
		{
			/// <summary>
			/// Index of relic voted on. If null, then player skipped.
			/// </summary>
			// Token: 0x04006614 RID: 26132
			public int? index;

			/// <summary>
			/// If true, then the player has voted. False if the player has not yet picked an option.
			/// </summary>
			// Token: 0x04006615 RID: 26133
			public bool voteReceived;
		}
	}
}
