using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	// Token: 0x020004A4 RID: 1188
	[NullableContext(1)]
	[Nullable(0)]
	public class RestSiteSynchronizer : IDisposable
	{
		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06004798 RID: 18328 RVA: 0x0021CC96 File Offset: 0x0021AE96
		private Player LocalPlayer
		{
			get
			{
				return this._playerCollection.GetPlayer(this._localPlayerId);
			}
		}

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06004799 RID: 18329 RVA: 0x0021CCAC File Offset: 0x0021AEAC
		// (remove) Token: 0x0600479A RID: 18330 RVA: 0x0021CCE4 File Offset: 0x0021AEE4
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<ulong> PlayerHoverChanged;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x0600479B RID: 18331 RVA: 0x0021CD1C File Offset: 0x0021AF1C
		// (remove) Token: 0x0600479C RID: 18332 RVA: 0x0021CD54 File Offset: 0x0021AF54
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<RestSiteOption, ulong> BeforePlayerOptionChosen;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x0600479D RID: 18333 RVA: 0x0021CD8C File Offset: 0x0021AF8C
		// (remove) Token: 0x0600479E RID: 18334 RVA: 0x0021CDC4 File Offset: 0x0021AFC4
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<RestSiteOption, bool, ulong> AfterPlayerOptionChosen;

		// Token: 0x0600479F RID: 18335 RVA: 0x0021CDFC File Offset: 0x0021AFFC
		public RestSiteSynchronizer(RunLocationTargetedMessageBuffer messageBuffer, INetGameService netService, IPlayerCollection playerCollection, ulong localPlayerId, [Nullable(2)] RunLobby runLobby)
		{
			this._netService = netService;
			this._messageBuffer = messageBuffer;
			this._playerCollection = playerCollection;
			this._localPlayerId = localPlayerId;
			this._runLobby = runLobby;
			this._messageBuffer.RegisterMessageHandler<OptionIndexChosenMessage>(new MessageHandlerDelegate<OptionIndexChosenMessage>(this.HandleRestSiteOptionChosenMessage));
			this._messageBuffer.RegisterMessageHandler<RestSiteOptionHoveredMessage>(new MessageHandlerDelegate<RestSiteOptionHoveredMessage>(this.HandleRestSiteOptionHoveredMessage));
			this._messageBuffer.RegisterMessageHandler<RestSiteSkippedMessage>(new MessageHandlerDelegate<RestSiteSkippedMessage>(this.HandleRestSiteSkippedMessage));
			if (this._runLobby != null)
			{
				this._runLobby.RemotePlayerDisconnected += this.OnPeerDisconnected;
			}
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x0021CEB4 File Offset: 0x0021B0B4
		public void Dispose()
		{
			this._messageBuffer.UnregisterMessageHandler<OptionIndexChosenMessage>(new MessageHandlerDelegate<OptionIndexChosenMessage>(this.HandleRestSiteOptionChosenMessage));
			this._messageBuffer.UnregisterMessageHandler<RestSiteOptionHoveredMessage>(new MessageHandlerDelegate<RestSiteOptionHoveredMessage>(this.HandleRestSiteOptionHoveredMessage));
			this._messageBuffer.UnregisterMessageHandler<RestSiteSkippedMessage>(new MessageHandlerDelegate<RestSiteSkippedMessage>(this.HandleRestSiteSkippedMessage));
			if (this._runLobby != null)
			{
				this._runLobby.RemotePlayerDisconnected -= this.OnPeerDisconnected;
			}
			Task hoverMessageTask = this._hoverMessageTask;
			if (hoverMessageTask != null)
			{
				hoverMessageTask.Dispose();
			}
			this._hoverMessageTask = null;
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x0021CF40 File Offset: 0x0021B140
		public void BeginRestSite()
		{
			this._logger.Debug("Beginning rest site", 1);
			this._restSites.Clear();
			foreach (Player player in this._playerCollection.Players)
			{
				List<RestSiteOption> list = RestSiteOption.Generate(player);
				this._restSites.Add(new RestSiteSynchronizer.PlayerRestSite
				{
					options = list
				});
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Rest site began for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" with options: ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<RestSiteOption>(",", list));
				logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x0021D018 File Offset: 0x0021B218
		private void HandleRestSiteOptionChosenMessage(OptionIndexChosenMessage message, ulong senderId)
		{
			if (message.type != OptionIndexType.RestSite)
			{
				return;
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(" chose rest site option index ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(message.optionIndex);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			Player player = this._playerCollection.GetPlayer(senderId);
			if (player == null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received EventOptionChosenMessage for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral(" that doesn't exist!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			TaskHelper.RunSafely(this.ChooseOption(player, (int)message.optionIndex));
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x0021D0D4 File Offset: 0x0021B2D4
		private void HandleRestSiteOptionHoveredMessage(RestSiteOptionHoveredMessage message, ulong senderId)
		{
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(senderId));
			this._restSites[playerSlotIndex].hoveredOptionIndex = message.optionIndex;
			Action<ulong> playerHoverChanged = this.PlayerHoverChanged;
			if (playerHoverChanged == null)
			{
				return;
			}
			playerHoverChanged(senderId);
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x0021D124 File Offset: 0x0021B324
		private void HandleRestSiteSkippedMessage(RestSiteSkippedMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(" skipped remaining options at rest site");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(senderId));
			RestSiteSynchronizer.PlayerRestSite playerRestSite = this._restSites[playerSlotIndex];
			if (playerRestSite.completionTaskSource.Task.IsCompleted)
			{
				return;
			}
			Logger logger2 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Skipping ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(playerRestSite.options.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" options");
			logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			playerRestSite.options.Clear();
			playerRestSite.completionTaskSource.TrySetResult();
		}

		/// <summary>
		/// Called when a remote player disconnects. If they still had rest site options remaining, they will never send us a
		/// skip or final option-chosen message, so we complete their rest site here. Otherwise a room exit that is waiting
		/// on <see cref="M:MegaCrit.Sts2.Core.Multiplayer.Game.RestSiteSynchronizer.AfterAllRestSitesCompleted" /> would hang forever.
		/// </summary>
		// Token: 0x060047A5 RID: 18341 RVA: 0x0021D204 File Offset: 0x0021B404
		private void OnPeerDisconnected(ulong peerId)
		{
			Player player = this._playerCollection.GetPlayer(peerId);
			if (player == null)
			{
				return;
			}
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(player);
			if (playerSlotIndex >= this._restSites.Count)
			{
				return;
			}
			RestSiteSynchronizer.PlayerRestSite playerRestSite = this._restSites[playerSlotIndex];
			if (playerRestSite.completionTaskSource.Task.IsCompleted)
			{
				return;
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(82, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(peerId);
			defaultInterpolatedStringHandler.AppendLiteral(" disconnected with ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(playerRestSite.options.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" rest site options remaining; completing their rest site");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			playerRestSite.options.Clear();
			playerRestSite.completionTaskSource.SetResult();
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x0021D2D0 File Offset: 0x0021B4D0
		public Task<bool> ChooseLocalOption(int index)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Local player chose rest site option index ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(index);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			OptionIndexChosenMessage optionIndexChosenMessage = default(OptionIndexChosenMessage);
			optionIndexChosenMessage.type = OptionIndexType.RestSite;
			optionIndexChosenMessage.optionIndex = (uint)index;
			optionIndexChosenMessage.location = this._messageBuffer.CurrentLocation;
			this._netService.SendMessage<OptionIndexChosenMessage>(optionIndexChosenMessage);
			return this.ChooseOption(this.LocalPlayer, index);
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x0021D354 File Offset: 0x0021B554
		private async Task<bool> ChooseOption(Player player, int optionIndex)
		{
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(player);
			RestSiteSynchronizer.PlayerRestSite restSite = this._restSites[playerSlotIndex];
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (restSite.completionTaskSource.Task.IsCompleted)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(98, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" attempted to choose rest site option index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(optionIndex);
				defaultInterpolatedStringHandler.AppendLiteral(", but the rest site has already been completed!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (optionIndex >= restSite.options.Count)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(92, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" attempted to choose rest site option index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(optionIndex);
				defaultInterpolatedStringHandler.AppendLiteral(", but there were only ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(restSite.options.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" options available!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			RestSiteOption option = restSite.options[optionIndex];
			Action<RestSiteOption, ulong> beforePlayerOptionChosen = this.BeforePlayerOptionChosen;
			if (beforePlayerOptionChosen != null)
			{
				beforePlayerOptionChosen(option, player.NetId);
			}
			bool flag = await option.OnSelect();
			bool flag2 = flag;
			Logger logger = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Rest site option index ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(optionIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" chosen for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" with success ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(flag2);
			defaultInterpolatedStringHandler.AppendLiteral(". Option: ");
			defaultInterpolatedStringHandler.AppendFormatted(option.OptionId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			restSite.lastChosenOptionIndex = new uint?((uint)optionIndex);
			Action<RestSiteOption, bool, ulong> afterPlayerOptionChosen = this.AfterPlayerOptionChosen;
			if (afterPlayerOptionChosen != null)
			{
				afterPlayerOptionChosen(option, flag2, player.NetId);
			}
			bool flag3;
			if (!flag2)
			{
				flag3 = false;
			}
			else
			{
				MapPointHistoryEntry currentMapPointHistoryEntry = player.RunState.CurrentMapPointHistoryEntry;
				if (currentMapPointHistoryEntry != null)
				{
					currentMapPointHistoryEntry.GetEntry(player.NetId).RestSiteChoices.Add(option.OptionId);
				}
				if (restSite.completionTaskSource.Task.IsCompleted)
				{
					flag3 = true;
				}
				else
				{
					if (Hook.ShouldDisableRemainingRestSiteOptions(player.RunState, player))
					{
						Logger logger2 = this._logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Clearing all remaining rest site options for player ");
						defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
						logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						restSite.options.Clear();
					}
					else
					{
						Logger logger3 = this._logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Leaving remaining rest site options enabled for player ");
						defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
						logger3.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						restSite.options.RemoveAt(optionIndex);
					}
					if (restSite.options.Count == 0)
					{
						Logger logger4 = this._logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Completing rest site for player ");
						defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
						defaultInterpolatedStringHandler.AppendLiteral(" because there are no options left");
						logger4.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						restSite.completionTaskSource.SetResult();
					}
					flag3 = true;
				}
			}
			return flag3;
		}

		/// <summary>
		/// Called by <see cref="T:MegaCrit.Sts2.Core.Rooms.RestSiteRoom" /> when the room is exited.
		/// If there are still options remaining for the local player, those options are cleared and the room is completed.
		/// This completion is synced to other players.
		/// </summary>
		// Token: 0x060047A8 RID: 18344 RVA: 0x0021D3A8 File Offset: 0x0021B5A8
		public void BeforeLocalRestSiteExited()
		{
			Player me = LocalContext.GetMe(this._playerCollection);
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(me);
			RestSiteSynchronizer.PlayerRestSite playerRestSite = this._restSites[playerSlotIndex];
			if (playerRestSite.options.Count > 0)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(96, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Skipping remaining options (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(playerRestSite.options.Count);
				defaultInterpolatedStringHandler.AppendLiteral(") in rest site for local player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(me.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" because we're exiting the rest site");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				playerRestSite.options.Clear();
				playerRestSite.completionTaskSource.SetResult();
				this._netService.SendMessage<RestSiteSkippedMessage>(new RestSiteSkippedMessage
				{
					Location = this._messageBuffer.CurrentLocation
				});
			}
		}

		/// <summary>
		/// Waits for all players' rest sites to be completed.
		/// This must be awaited before the room-exit checksum is generated so that we know that we won't be receiving any
		/// more OptionIndexChosenMessages for this room.
		/// </summary>
		// Token: 0x060047A9 RID: 18345 RVA: 0x0021D48C File Offset: 0x0021B68C
		public async Task AfterAllRestSitesCompleted()
		{
			foreach (RestSiteSynchronizer.PlayerRestSite playerRestSite in this._restSites)
			{
				await playerRestSite.completionTaskSource.Task;
			}
			List<RestSiteSynchronizer.PlayerRestSite>.Enumerator enumerator = default(List<RestSiteSynchronizer.PlayerRestSite>.Enumerator);
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x0021D4D0 File Offset: 0x0021B6D0
		[NullableContext(2)]
		public void LocalOptionHovered(RestSiteOption option)
		{
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(LocalContext.GetMe(this._playerCollection));
			RestSiteSynchronizer.PlayerRestSite playerRestSite = this._restSites[playerSlotIndex];
			if (option != null)
			{
				playerRestSite.hoveredOptionIndex = new uint?((uint)playerRestSite.options.IndexOf(option));
			}
			else
			{
				playerRestSite.hoveredOptionIndex = null;
			}
			RestSiteOptionHoveredMessage restSiteOptionHoveredMessage = this._hoveredMessage.GetValueOrDefault();
			if (this._hoveredMessage == null)
			{
				restSiteOptionHoveredMessage = new RestSiteOptionHoveredMessage
				{
					Location = this._messageBuffer.CurrentLocation,
					optionIndex = playerRestSite.hoveredOptionIndex
				};
				this._hoveredMessage = new RestSiteOptionHoveredMessage?(restSiteOptionHoveredMessage);
			}
			this.TrySendHoverMessage();
			Action<ulong> playerHoverChanged = this.PlayerHoverChanged;
			if (playerHoverChanged == null)
			{
				return;
			}
			playerHoverChanged(LocalContext.NetId.Value);
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x0021D5A4 File Offset: 0x0021B7A4
		public int? GetHoveredOptionIndex(ulong playerId)
		{
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(playerId));
			uint? hoveredOptionIndex = this._restSites[playerSlotIndex].hoveredOptionIndex;
			if (hoveredOptionIndex == null)
			{
				return null;
			}
			return new int?((int)hoveredOptionIndex.GetValueOrDefault());
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x0021D5FC File Offset: 0x0021B7FC
		public int? GetChosenOptionIndex(ulong playerId)
		{
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(playerId));
			uint? lastChosenOptionIndex = this._restSites[playerSlotIndex].lastChosenOptionIndex;
			if (lastChosenOptionIndex == null)
			{
				return null;
			}
			return new int?((int)lastChosenOptionIndex.GetValueOrDefault());
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x0021D652 File Offset: 0x0021B852
		public IReadOnlyList<RestSiteOption> GetLocalOptions()
		{
			return this.GetOptionsForPlayer(this.LocalPlayer);
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x0021D660 File Offset: 0x0021B860
		public IReadOnlyList<RestSiteOption> GetOptionsForPlayer(ulong playerId)
		{
			return this.GetOptionsForPlayer(this._playerCollection.GetPlayer(playerId));
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x0021D674 File Offset: 0x0021B874
		public IReadOnlyList<RestSiteOption> GetOptionsForPlayer(Player player)
		{
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(player);
			return this._restSites[playerSlotIndex].options;
		}

		/// <summary>
		/// Sends a hover message if enough time has passed since the last one, or buffers it to be sent otherwise.
		/// </summary>
		// Token: 0x060047B0 RID: 18352 RVA: 0x0021D6A0 File Offset: 0x0021B8A0
		private void TrySendHoverMessage()
		{
			if (this._hoverMessageTask != null)
			{
				return;
			}
			int num = (int)(this._lastHoverMessageMsec + 50UL - Time.GetTicksMsec());
			if (num <= 0)
			{
				this._hoverMessageTask = TaskHelper.RunSafely(this.SendHoverMessageAfterSmallDelay());
				return;
			}
			this._hoverMessageTask = TaskHelper.RunSafely(this.QueueHoverMessage(num));
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x0021D6F0 File Offset: 0x0021B8F0
		private async Task QueueHoverMessage(int delayMsec)
		{
			await Task.Delay(delayMsec);
			this.SendHoverMessage();
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x0021D73C File Offset: 0x0021B93C
		private async Task SendHoverMessageAfterSmallDelay()
		{
			await Task.Yield();
			this.SendHoverMessage();
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x0021D780 File Offset: 0x0021B980
		private void SendHoverMessage()
		{
			if (!this._netService.IsConnected)
			{
				return;
			}
			this._netService.SendMessage<RestSiteOptionHoveredMessage>(this._hoveredMessage.Value);
			this._lastHoverMessageMsec = Time.GetTicksMsec();
			this._hoveredMessage = null;
			this._hoverMessageTask = null;
		}

		// Token: 0x04001C13 RID: 7187
		public const int minHoverMessageMsec = 50;

		// Token: 0x04001C14 RID: 7188
		private readonly INetGameService _netService;

		// Token: 0x04001C15 RID: 7189
		private readonly RunLocationTargetedMessageBuffer _messageBuffer;

		// Token: 0x04001C16 RID: 7190
		private readonly IPlayerCollection _playerCollection;

		// Token: 0x04001C17 RID: 7191
		private readonly ulong _localPlayerId;

		// Token: 0x04001C18 RID: 7192
		[Nullable(2)]
		private readonly RunLobby _runLobby;

		// Token: 0x04001C19 RID: 7193
		private readonly List<RestSiteSynchronizer.PlayerRestSite> _restSites = new List<RestSiteSynchronizer.PlayerRestSite>();

		// Token: 0x04001C1A RID: 7194
		private readonly Logger _logger = new Logger("RestSiteSynchronizer", LogType.GameSync);

		// Token: 0x04001C1B RID: 7195
		private ulong _lastHoverMessageMsec;

		// Token: 0x04001C1C RID: 7196
		private RestSiteOptionHoveredMessage? _hoveredMessage;

		// Token: 0x04001C1D RID: 7197
		[Nullable(2)]
		private Task _hoverMessageTask;

		// Token: 0x020019C4 RID: 6596
		[Nullable(0)]
		private class PlayerRestSite
		{
			// Token: 0x040065BF RID: 26047
			public List<RestSiteOption> options;

			// Token: 0x040065C0 RID: 26048
			public uint? lastChosenOptionIndex;

			// Token: 0x040065C1 RID: 26049
			public uint? hoveredOptionIndex;

			// Token: 0x040065C2 RID: 26050
			public TaskCompletionSource completionTaskSource = new TaskCompletionSource();
		}
	}
}
