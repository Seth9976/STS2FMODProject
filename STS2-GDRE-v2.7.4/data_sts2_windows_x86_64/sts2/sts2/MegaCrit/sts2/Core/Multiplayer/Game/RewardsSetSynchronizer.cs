using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Synchronizer responsible for executing reward set behavior for each player.
	/// When any player generates a reward from any source (end of combat, relics, events), the RewardsSet calls into this
	/// synchronizer. Then, as the owning player selects or skips rewards, they send messages to all other players
	/// indicating which rewards they took. This synchronizer handles those messages.
	/// </summary>
	// Token: 0x020004A5 RID: 1189
	[NullableContext(1)]
	[Nullable(0)]
	public class RewardsSetSynchronizer : IDisposable
	{
		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x060047B4 RID: 18356 RVA: 0x0021D7CF File Offset: 0x0021B9CF
		private Player LocalPlayer
		{
			get
			{
				return this._playerCollection.GetPlayer(this._localPlayerId);
			}
		}

		/// <summary>
		/// Called when we skip all rewards just before leaving the room.
		/// </summary>
		// Token: 0x14000071 RID: 113
		// (add) Token: 0x060047B5 RID: 18357 RVA: 0x0021D7E4 File Offset: 0x0021B9E4
		// (remove) Token: 0x060047B6 RID: 18358 RVA: 0x0021D81C File Offset: 0x0021BA1C
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action RewardsSkippedDuringRoomExit;

		// Token: 0x060047B7 RID: 18359 RVA: 0x0021D854 File Offset: 0x0021BA54
		public RewardsSetSynchronizer(RunLocationTargetedMessageBuffer messageBuffer, INetGameService netService, IPlayerCollection playerCollection, ulong localPlayerId)
		{
			this._netService = netService;
			this._playerCollection = playerCollection;
			this._localPlayerId = localPlayerId;
			for (int i = 0; i < playerCollection.Players.Count; i++)
			{
				this._rewardStates.Add(new RewardsSetSynchronizer.PlayerRewardState
				{
					rewardsStack = new List<RewardsSetSynchronizer.RewardsSetState>(),
					bufferedMessages = new List<RewardsSetSynchronizer.BufferedMessage>()
				});
			}
			this._messageBuffer = messageBuffer;
			this._messageBuffer.RegisterMessageHandler<RewardSelectedMessage>(new MessageHandlerDelegate<RewardSelectedMessage>(this.HandleRewardSelectedMessage));
			this._messageBuffer.RegisterMessageHandler<RewardSetSkippedMessage>(new MessageHandlerDelegate<RewardSetSkippedMessage>(this.HandleRewardSetSkippedMessage));
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x0021D90A File Offset: 0x0021BB0A
		public void Dispose()
		{
			this._messageBuffer.UnregisterMessageHandler<RewardSelectedMessage>(new MessageHandlerDelegate<RewardSelectedMessage>(this.HandleRewardSelectedMessage));
			this._messageBuffer.UnregisterMessageHandler<RewardSetSkippedMessage>(new MessageHandlerDelegate<RewardSetSkippedMessage>(this.HandleRewardSetSkippedMessage));
		}

		// Token: 0x060047B9 RID: 18361 RVA: 0x0021D93C File Offset: 0x0021BB3C
		private RewardsSetSynchronizer.PlayerRewardState GetRewardStateForPlayer(Player player)
		{
			for (int i = this._rewardStates.Count; i <= this._playerCollection.GetPlayerSlotIndex(player); i++)
			{
				this._rewardStates.Add(new RewardsSetSynchronizer.PlayerRewardState
				{
					rewardsStack = new List<RewardsSetSynchronizer.RewardsSetState>(),
					bufferedMessages = new List<RewardsSetSynchronizer.BufferedMessage>()
				});
			}
			return this._rewardStates[this._playerCollection.GetPlayerSlotIndex(player)];
		}

		/// <summary>
		/// Begin tracking a rewards set.
		/// This is called when a reward set is spawned and offered to the owning player.
		/// </summary>
		/// <param name="set">The rewards set to track.</param>
		/// <returns>A Task which completes when the player is done taking the rewards.</returns>
		// Token: 0x060047BA RID: 18362 RVA: 0x0021D9A8 File Offset: 0x0021BBA8
		public Task BeginRewardsSet(RewardsSet set)
		{
			RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(set.Player);
			set.Id = rewardStateForPlayer.nextId;
			rewardStateForPlayer.nextId++;
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Beginning rewards set ");
			defaultInterpolatedStringHandler.AppendFormatted<RewardsSet>(set);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			TaskCompletionSource taskCompletionSource = new TaskCompletionSource();
			RewardsSetSynchronizer.RewardsSetState rewardsSetState = new RewardsSetSynchronizer.RewardsSetState
			{
				set = set,
				completionSource = taskCompletionSource
			};
			rewardStateForPlayer.rewardsStack.Add(rewardsSetState);
			foreach (RewardsSetSynchronizer.BufferedMessage bufferedMessage in rewardStateForPlayer.bufferedMessages.ToList<RewardsSetSynchronizer.BufferedMessage>())
			{
				if (set.Id == bufferedMessage.SetId)
				{
					if (bufferedMessage.selectedMessage != null)
					{
						this._logger.Debug("Handling buffered RewardSelectedMessage", 1);
						this.HandleRewardSelectedMessage(bufferedMessage.selectedMessage, bufferedMessage.senderId);
					}
					else if (bufferedMessage.skippedMessage != null)
					{
						this._logger.Debug("Handling buffered RewardSetSkippedMessage", 1);
						this.HandleRewardSetSkippedMessage(bufferedMessage.skippedMessage, bufferedMessage.senderId);
					}
					rewardStateForPlayer.bufferedMessages.Remove(bufferedMessage);
				}
			}
			this.CompleteRewardsSetIfNecessary(rewardsSetState);
			return taskCompletionSource.Task;
		}

		/// <summary>
		/// Selects a reward locally.
		/// Throws if the reward is not owned by the local player.
		/// </summary>
		/// <param name="reward">The reward to select.</param>
		/// <returns>A Task which completes when the player finishes with the reward. See <see cref="M:MegaCrit.Sts2.Core.Rewards.Reward.SelectUnsynchronized" />.</returns>
		// Token: 0x060047BB RID: 18363 RVA: 0x0021DB08 File Offset: 0x0021BD08
		public async Task<bool> SelectLocalReward(Reward reward)
		{
			if (reward.Player != this.LocalPlayer)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 3);
				defaultInterpolatedStringHandler.AppendFormatted("SelectLocalReward");
				defaultInterpolatedStringHandler.AppendLiteral(" called for reward ");
				defaultInterpolatedStringHandler.AppendFormatted<Reward>(reward);
				defaultInterpolatedStringHandler.AppendLiteral(" with non-local player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(reward.Player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral("! This is not allowed");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(this.LocalPlayer);
			if (rewardStateForPlayer.rewardsStack.Count <= 0)
			{
				throw new InvalidOperationException("Tried to sync reward for local player, but they are not currently viewing any reward set!");
			}
			RewardsSetSynchronizer.RewardsSetState rewardsSetState = rewardStateForPlayer.rewardsStack.Last<RewardsSetSynchronizer.RewardsSetState>();
			RewardsSet set = rewardsSetState.set;
			int num = set.Rewards.IndexOf(reward);
			RewardSelectedMessage rewardSelectedMessage = new RewardSelectedMessage
			{
				location = this._messageBuffer.CurrentLocation,
				setId = rewardsSetState.set.Id,
				rewardIndex = num
			};
			this._netService.SendMessage<RewardSelectedMessage>(rewardSelectedMessage);
			return await this.SelectRewardForPlayer(rewardsSetState, reward);
		}

		/// <summary>
		/// Skips the current rewards that the local player is looking at.
		/// </summary>
		// Token: 0x060047BC RID: 18364 RVA: 0x0021DB54 File Offset: 0x0021BD54
		public void SkipLocalRewardsSet()
		{
			this._logger.Debug("Skipping local RewardsSet", 1);
			RewardsSetSynchronizer.RewardsSetState rewardsSetState = this.SkipRewardsSetOnStackTopForPlayer(this.LocalPlayer);
			RewardSetSkippedMessage rewardSetSkippedMessage = new RewardSetSkippedMessage
			{
				location = this._messageBuffer.CurrentLocation,
				setId = rewardsSetState.set.Id
			};
			this._netService.SendMessage<RewardSetSkippedMessage>(rewardSetSkippedMessage);
		}

		/// <summary>
		/// Called when a remote player selects a reward.
		/// </summary>
		// Token: 0x060047BD RID: 18365 RVA: 0x0021DBB4 File Offset: 0x0021BDB4
		public void HandleRewardSelectedMessage(RewardSelectedMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Received ");
			defaultInterpolatedStringHandler.AppendFormatted("RewardSelectedMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" from player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(", set id: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(message.setId);
			defaultInterpolatedStringHandler.AppendLiteral(" reward index: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(message.rewardIndex);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			Player player = this._playerCollection.GetPlayer(senderId);
			RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(player);
			if (rewardStateForPlayer.nextId <= message.setId)
			{
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Buffering ");
				defaultInterpolatedStringHandler.AppendFormatted("RewardSelectedMessage");
				defaultInterpolatedStringHandler.AppendLiteral(" because RewardsSet id ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(message.setId);
				defaultInterpolatedStringHandler.AppendLiteral(" hasn't been created yet");
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				rewardStateForPlayer.bufferedMessages.Add(new RewardsSetSynchronizer.BufferedMessage
				{
					selectedMessage = message,
					senderId = senderId
				});
				return;
			}
			TaskHelper.RunSafely(this.SelectRewardForPlayer(player, message.rewardIndex));
		}

		/// <summary>
		/// Called when a remote player skips the remaining the rewards in a set.
		/// Occurs both when leaving a room as a result of <see cref="M:MegaCrit.Sts2.Core.Multiplayer.Game.RewardsSetSynchronizer.BeforeLeavingRoom" /> and pressing the skip button on
		/// non-terminal rewards.
		/// </summary>
		// Token: 0x060047BE RID: 18366 RVA: 0x0021DCF4 File Offset: 0x0021BEF4
		public void HandleRewardSetSkippedMessage(RewardSetSkippedMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Received ");
			defaultInterpolatedStringHandler.AppendFormatted("RewardSetSkippedMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" from player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(", set id: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(message.setId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			Player player = this._playerCollection.GetPlayer(senderId);
			RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(player);
			if (rewardStateForPlayer.nextId <= message.setId)
			{
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Buffering ");
				defaultInterpolatedStringHandler.AppendFormatted("RewardSetSkippedMessage");
				defaultInterpolatedStringHandler.AppendLiteral(" because RewardsSet id ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(message.setId);
				defaultInterpolatedStringHandler.AppendLiteral(" hasn't been created yet");
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				rewardStateForPlayer.bufferedMessages.Add(new RewardsSetSynchronizer.BufferedMessage
				{
					skippedMessage = message,
					senderId = senderId
				});
				return;
			}
			this.SkipRewardsSetOnStackTopForPlayer(this._playerCollection.GetPlayer(senderId));
		}

		/// <summary>
		/// Selects the reward at the specified index for the given player's top-most rewards screen.
		/// </summary>
		// Token: 0x060047BF RID: 18367 RVA: 0x0021DE18 File Offset: 0x0021C018
		private async Task SelectRewardForPlayer(Player player, int rewardIndex)
		{
			RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(player);
			if (rewardStateForPlayer.rewardsStack.Count <= 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(86, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to select reward for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(", but they are not currently viewing any reward set!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			RewardsSetSynchronizer.RewardsSetState rewardsSetState = rewardStateForPlayer.rewardsStack.Last<RewardsSetSynchronizer.RewardsSetState>();
			RewardsSet set = rewardsSetState.set;
			if (rewardIndex < 0 || rewardIndex >= set.Rewards.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(97, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to select reward index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(rewardIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(", but it is out of bounds in their current rewards set ");
				defaultInterpolatedStringHandler.AppendFormatted<RewardsSet>(set);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Reward reward = set.Rewards[rewardIndex];
			await this.SelectRewardForPlayer(rewardsSetState, reward);
		}

		/// <summary>
		/// Selects the passed reward for the given reward set.
		/// Takes care of completing the reward set if all rewards have been obtained.
		/// </summary>
		// Token: 0x060047C0 RID: 18368 RVA: 0x0021DE6C File Offset: 0x0021C06C
		private async Task<bool> SelectRewardForPlayer(RewardsSetSynchronizer.RewardsSetState setState, Reward reward)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Selecting reward ");
			defaultInterpolatedStringHandler.AppendFormatted<Reward>(reward);
			defaultInterpolatedStringHandler.AppendLiteral(" for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(reward.Player.NetId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			bool flag = await reward.SelectUnsynchronized();
			bool flag2 = flag;
			this.CompleteRewardsSetIfNecessary(setState);
			return flag2;
		}

		/// <summary>
		/// If all rewards in the passed state are successfully complete, then the set is marked as complete.
		/// </summary>
		// Token: 0x060047C1 RID: 18369 RVA: 0x0021DEBF File Offset: 0x0021C0BF
		private void CompleteRewardsSetIfNecessary(RewardsSetSynchronizer.RewardsSetState setState)
		{
			if (setState.set.AllRewardsSuccessfullySelected)
			{
				this.CompleteRewardsSet(setState, RewardsSetSynchronizer.RewardSetCompleteState.Completed);
			}
		}

		/// <summary>
		/// Skips all rewards in reward set on the top of the stack for the given player, completing the reward set.
		/// </summary>
		// Token: 0x060047C2 RID: 18370 RVA: 0x0021DED8 File Offset: 0x0021C0D8
		private RewardsSetSynchronizer.RewardsSetState SkipRewardsSetOnStackTopForPlayer(Player player)
		{
			RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(player);
			if (rewardStateForPlayer.rewardsStack.Count <= 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(88, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to skip reward set for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(", but they are not currently viewing any reward set!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<RewardsSetSynchronizer.RewardsSetState> rewardsStack = rewardStateForPlayer.rewardsStack;
			RewardsSetSynchronizer.RewardsSetState rewardsSetState = rewardsStack[rewardsStack.Count - 1];
			this.SkipRewardsSet(rewardsSetState);
			return rewardsSetState;
		}

		/// <summary>
		/// Skips all rewards in the passed reward set, completing the reward set.
		/// </summary>
		// Token: 0x060047C3 RID: 18371 RVA: 0x0021DF54 File Offset: 0x0021C154
		private void SkipRewardsSet(RewardsSetSynchronizer.RewardsSetState setState)
		{
			foreach (Reward reward in setState.set.Rewards)
			{
				if (!reward.SuccessfullySelected)
				{
					reward.OnSkipped();
				}
			}
			this.CompleteRewardsSet(setState, RewardsSetSynchronizer.RewardSetCompleteState.Skipped);
		}

		/// <summary>
		/// Marks a rewards set as complete (or skipped), popping it off the owning player's stack.
		/// </summary>
		// Token: 0x060047C4 RID: 18372 RVA: 0x0021DFBC File Offset: 0x0021C1BC
		private void CompleteRewardsSet(RewardsSetSynchronizer.RewardsSetState setState, RewardsSetSynchronizer.RewardSetCompleteState completeState)
		{
			RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(setState.set.Player);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this.IsRewardsSetCompleted(setState.set))
			{
				Logger logger = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Reward set with id ");
				defaultInterpolatedStringHandler.AppendFormatted<RewardsSet>(setState.set);
				defaultInterpolatedStringHandler.AppendLiteral(" is already finished (state ");
				defaultInterpolatedStringHandler.AppendFormatted<RewardsSetSynchronizer.RewardSetCompleteState>(rewardStateForPlayer.completedRewards[setState.set.Id]);
				defaultInterpolatedStringHandler.AppendLiteral(")!");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			rewardStateForPlayer.rewardsStack.Remove(setState);
			rewardStateForPlayer.completedRewards[setState.set.Id] = completeState;
			setState.completionSource.SetResult();
			Logger logger2 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Reward set ");
			defaultInterpolatedStringHandler.AppendFormatted<RewardsSet>(setState.set);
			defaultInterpolatedStringHandler.AppendLiteral(" completed with state: ");
			defaultInterpolatedStringHandler.AppendFormatted<RewardsSetSynchronizer.RewardSetCompleteState>(completeState);
			logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
		}

		/// <summary>
		/// Returns true if the passed reward set has been completed.
		/// </summary>
		// Token: 0x060047C5 RID: 18373 RVA: 0x0021E0D0 File Offset: 0x0021C2D0
		public bool IsRewardsSetCompleted(RewardsSet set)
		{
			RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(set.Player);
			return set.Id >= 0 && rewardStateForPlayer.completedRewards.ContainsKey(set.Id);
		}

		/// <summary>
		/// Returns true if the passed reward set ID has been completed for the given player.
		/// </summary>
		// Token: 0x060047C6 RID: 18374 RVA: 0x0021E108 File Offset: 0x0021C308
		public bool IsRewardsSetCompleted(Player player, int id)
		{
			RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(player);
			return rewardStateForPlayer.completedRewards.ContainsKey(id);
		}

		/// <summary>
		/// Skips all remaining rewards for the local player.
		/// </summary>
		// Token: 0x060047C7 RID: 18375 RVA: 0x0021E12C File Offset: 0x0021C32C
		public void BeforeLeavingRoom()
		{
			RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(this.LocalPlayer);
			if (rewardStateForPlayer.rewardsStack.Count > 0)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Skipping remaining rewards for local player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.LocalPlayer.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" because we're exiting the room");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			for (int i = rewardStateForPlayer.rewardsStack.Count - 1; i >= 0; i--)
			{
				RewardsSetSynchronizer.RewardsSetState rewardsSetState = rewardStateForPlayer.rewardsStack[i];
				this.SkipRewardsSet(rewardStateForPlayer.rewardsStack[i]);
				RewardSetSkippedMessage rewardSetSkippedMessage = new RewardSetSkippedMessage
				{
					location = this._messageBuffer.CurrentLocation,
					setId = rewardsSetState.set.Id
				};
				this._netService.SendMessage<RewardSetSkippedMessage>(rewardSetSkippedMessage);
			}
			rewardStateForPlayer.rewardsStack.Clear();
			Action rewardsSkippedDuringRoomExit = this.RewardsSkippedDuringRoomExit;
			if (rewardsSkippedDuringRoomExit == null)
			{
				return;
			}
			rewardsSkippedDuringRoomExit();
		}

		/// <summary>
		/// Gets all the next reward IDs in player slot order.
		/// </summary>
		// Token: 0x060047C8 RID: 18376 RVA: 0x0021E223 File Offset: 0x0021C423
		public IEnumerable<int> GetNextRewardIds()
		{
			RewardsSetSynchronizer.<GetNextRewardIds>d__32 <GetNextRewardIds>d__ = new RewardsSetSynchronizer.<GetNextRewardIds>d__32(-2);
			<GetNextRewardIds>d__.<>4__this = this;
			return <GetNextRewardIds>d__;
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x0021E234 File Offset: 0x0021C434
		public void FastForwardRewardIds(List<int> rewardIds)
		{
			for (int i = 0; i < rewardIds.Count; i++)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Fast-forwarded reward set ID for ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral(" to ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(rewardIds[i]);
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this._rewardStates[i].nextId = rewardIds[i];
			}
		}

		// Token: 0x04001C21 RID: 7201
		private readonly RunLocationTargetedMessageBuffer _messageBuffer;

		// Token: 0x04001C22 RID: 7202
		private readonly INetGameService _netService;

		// Token: 0x04001C23 RID: 7203
		private readonly IPlayerCollection _playerCollection;

		// Token: 0x04001C24 RID: 7204
		private readonly ulong _localPlayerId;

		// Token: 0x04001C25 RID: 7205
		private readonly Logger _logger = new Logger("RewardsSetSynchronizer", LogType.GameSync);

		/// <summary>
		/// Reward state for each player.
		/// There is one for each player in _playerCollection, in the same order.
		/// </summary>
		// Token: 0x04001C26 RID: 7206
		private readonly List<RewardsSetSynchronizer.PlayerRewardState> _rewardStates = new List<RewardsSetSynchronizer.PlayerRewardState>();

		/// <summary>
		/// Denotes how a reward set was completed.
		/// </summary>
		// Token: 0x020019C9 RID: 6601
		[NullableContext(0)]
		private enum RewardSetCompleteState
		{
			// Token: 0x040065DA RID: 26074
			None,
			/// <summary>
			/// All the rewards in the set were taken.
			/// </summary>
			// Token: 0x040065DB RID: 26075
			Completed,
			/// <summary>
			/// Some (or all) of the rewards in the set were skipped.
			/// </summary>
			// Token: 0x040065DC RID: 26076
			Skipped
		}

		/// <summary>
		/// Represents a message that was sent for a remote player about a reward set that has not yet spawned for the local
		/// player.
		/// For example, if players have differing combat speeds, one player may reach the end of combat before us, and
		/// begin taking their rewards. This will start sending messages, but our local NCombatUi will not have begun the
		/// rewards process yet, so we need to buffer the remote messages for when it does.
		/// </summary>
		// Token: 0x020019CA RID: 6602
		[NullableContext(0)]
		private struct BufferedMessage : IEquatable<RewardsSetSynchronizer.BufferedMessage>
		{
			// Token: 0x170026C0 RID: 9920
			// (get) Token: 0x0600AB86 RID: 43910 RVA: 0x00362AB2 File Offset: 0x00360CB2
			public int SetId
			{
				get
				{
					RewardSelectedMessage rewardSelectedMessage = this.selectedMessage;
					if (rewardSelectedMessage != null)
					{
						return rewardSelectedMessage.setId;
					}
					RewardSetSkippedMessage rewardSetSkippedMessage = this.skippedMessage;
					if (rewardSetSkippedMessage == null)
					{
						throw new InvalidOperationException();
					}
					return rewardSetSkippedMessage.setId;
				}
			}

			// Token: 0x0600AB87 RID: 43911 RVA: 0x00362ADC File Offset: 0x00360CDC
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("BufferedMessage");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600AB88 RID: 43912 RVA: 0x00362B28 File Offset: 0x00360D28
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("SetId = ");
				builder.Append(this.SetId.ToString());
				builder.Append(", selectedMessage = ");
				builder.Append(this.selectedMessage);
				builder.Append(", skippedMessage = ");
				builder.Append(this.skippedMessage);
				builder.Append(", senderId = ");
				builder.Append(this.senderId.ToString());
				return true;
			}

			// Token: 0x0600AB89 RID: 43913 RVA: 0x00362BB3 File Offset: 0x00360DB3
			[CompilerGenerated]
			public static bool operator !=(RewardsSetSynchronizer.BufferedMessage left, RewardsSetSynchronizer.BufferedMessage right)
			{
				return !(left == right);
			}

			// Token: 0x0600AB8A RID: 43914 RVA: 0x00362BBF File Offset: 0x00360DBF
			[CompilerGenerated]
			public static bool operator ==(RewardsSetSynchronizer.BufferedMessage left, RewardsSetSynchronizer.BufferedMessage right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600AB8B RID: 43915 RVA: 0x00362BC9 File Offset: 0x00360DC9
			[CompilerGenerated]
			public override readonly int GetHashCode()
			{
				return (EqualityComparer<RewardSelectedMessage>.Default.GetHashCode(this.selectedMessage) * -1521134295 + EqualityComparer<RewardSetSkippedMessage>.Default.GetHashCode(this.skippedMessage)) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.senderId);
			}

			// Token: 0x0600AB8C RID: 43916 RVA: 0x00362C09 File Offset: 0x00360E09
			[CompilerGenerated]
			public override readonly bool Equals(object obj)
			{
				return obj is RewardsSetSynchronizer.BufferedMessage && this.Equals((RewardsSetSynchronizer.BufferedMessage)obj);
			}

			// Token: 0x0600AB8D RID: 43917 RVA: 0x00362C24 File Offset: 0x00360E24
			[CompilerGenerated]
			public readonly bool Equals(RewardsSetSynchronizer.BufferedMessage other)
			{
				return EqualityComparer<RewardSelectedMessage>.Default.Equals(this.selectedMessage, other.selectedMessage) && EqualityComparer<RewardSetSkippedMessage>.Default.Equals(this.skippedMessage, other.skippedMessage) && EqualityComparer<ulong>.Default.Equals(this.senderId, other.senderId);
			}

			// Token: 0x040065DD RID: 26077
			[Nullable(2)]
			public RewardSelectedMessage selectedMessage;

			// Token: 0x040065DE RID: 26078
			[Nullable(2)]
			public RewardSetSkippedMessage skippedMessage;

			// Token: 0x040065DF RID: 26079
			public ulong senderId;
		}

		/// <summary>
		/// Reward state for a given player. Contains a stack of reward sets.
		/// </summary>
		// Token: 0x020019CB RID: 6603
		[Nullable(0)]
		[RequiredMember]
		private class PlayerRewardState
		{
			// Token: 0x0600AB8E RID: 43918 RVA: 0x00362C79 File Offset: 0x00360E79
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public PlayerRewardState()
			{
			}

			/// <summary>
			/// The reward sets the player is currently choosing from.
			/// Players can have multiple rewards screens up because relics, when taken from a rewards screen, may themselves
			/// spawn new rewards screens. This acts like a stack.
			/// </summary>
			// Token: 0x040065E0 RID: 26080
			[RequiredMember]
			public List<RewardsSetSynchronizer.RewardsSetState> rewardsStack;

			/// <summary>
			/// If messages are received for a reward before it is started locally, then they will be buffered here until
			/// the reward set is spawned.
			/// </summary>
			// Token: 0x040065E1 RID: 26081
			[RequiredMember]
			public List<RewardsSetSynchronizer.BufferedMessage> bufferedMessages;

			/// <summary>
			/// Completed reward IDs, for bookkeeping purposes.
			/// </summary>
			// Token: 0x040065E2 RID: 26082
			public readonly Dictionary<int, RewardsSetSynchronizer.RewardSetCompleteState> completedRewards = new Dictionary<int, RewardsSetSynchronizer.RewardSetCompleteState>();

			/// <summary>
			/// The next ID to assign the rewards set for the given player.
			/// </summary>
			// Token: 0x040065E3 RID: 26083
			public int nextId;
		}

		/// <summary>
		/// State of a single reward set.
		/// </summary>
		// Token: 0x020019CC RID: 6604
		[Nullable(0)]
		[RequiredMember]
		private class RewardsSetState
		{
			// Token: 0x0600AB8F RID: 43919 RVA: 0x00362C8C File Offset: 0x00360E8C
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public RewardsSetState()
			{
			}

			/// <summary>
			/// The set of rewards.
			/// </summary>
			// Token: 0x040065E4 RID: 26084
			[RequiredMember]
			public RewardsSet set;

			/// <summary>
			/// The completion source to complete when the rewards are all taken, or when they are skipped.
			/// </summary>
			// Token: 0x040065E5 RID: 26085
			[RequiredMember]
			public TaskCompletionSource completionSource;
		}
	}
}
