using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer
{
	/// <summary>
	/// Responsible for synchronizing all players' combat states before combat begins.
	/// </summary>
	// Token: 0x02000427 RID: 1063
	[NullableContext(1)]
	[Nullable(0)]
	public class CombatStateSynchronizer : IDisposable
	{
		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060043E0 RID: 17376 RVA: 0x0020F60A File Offset: 0x0020D80A
		// (set) Token: 0x060043E1 RID: 17377 RVA: 0x0020F612 File Offset: 0x0020D812
		public bool IsDisabled { get; set; }

		// Token: 0x060043E2 RID: 17378 RVA: 0x0020F61C File Offset: 0x0020D81C
		public CombatStateSynchronizer(INetGameService netService, [Nullable(2)] RunLobby runLobby, RunState runState)
		{
			this._netService = netService;
			this._runState = runState;
			this._runLobby = runLobby;
			this._netService.RegisterMessageHandler<SyncPlayerDataMessage>(new MessageHandlerDelegate<SyncPlayerDataMessage>(this.OnSyncPlayerMessageReceived));
			this._netService.RegisterMessageHandler<SyncRngMessage>(new MessageHandlerDelegate<SyncRngMessage>(this.OnSyncRngMessageReceived));
			if (this._runLobby != null)
			{
				this._runLobby.RemotePlayerDisconnected += this.OnPeerDisconnected;
			}
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x0020F6B0 File Offset: 0x0020D8B0
		public void Dispose()
		{
			this._netService.UnregisterMessageHandler<SyncPlayerDataMessage>(new MessageHandlerDelegate<SyncPlayerDataMessage>(this.OnSyncPlayerMessageReceived));
			this._netService.UnregisterMessageHandler<SyncRngMessage>(new MessageHandlerDelegate<SyncRngMessage>(this.OnSyncRngMessageReceived));
			if (this._runLobby != null)
			{
				this._runLobby.RemotePlayerDisconnected -= this.OnPeerDisconnected;
			}
		}

		/// <summary>
		/// Handles a combat synchronization message from another peer.
		/// Remember that this can be called even before StartSync and must queue the data for the next time we sync.
		/// </summary>
		// Token: 0x060043E4 RID: 17380 RVA: 0x0020F70C File Offset: 0x0020D90C
		private void OnSyncPlayerMessageReceived(SyncPlayerDataMessage syncMessage, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received sync player message from ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (this._syncData.ContainsKey(senderId))
			{
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received two player sync messages from ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral("! Ignoring the second one");
				logger2.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			this._syncData[senderId] = syncMessage.player;
			this.CheckSyncCompleted();
		}

		/// <summary>
		/// Handles a RNG synchronization message from the host.
		/// Remember that this can be called even before StartSync and must queue the data for the next time we sync.
		/// </summary>
		// Token: 0x060043E5 RID: 17381 RVA: 0x0020F7B0 File Offset: 0x0020D9B0
		private void OnSyncRngMessageReceived(SyncRngMessage syncMessage, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received sync RNG message from ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (this._rngSet != null)
			{
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received two RNG sync messages from ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral("! Ignoring the second one");
				logger2.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			this._rngSet = syncMessage.rng;
			this._sharedRelicGrabBag = syncMessage.sharedRelicGrabBag;
			this.CheckSyncCompleted();
		}

		/// <summary>
		/// Called when a peer disconnects from the lobby.
		/// Used to check if we should complete the sync now that they've been disconnected.
		/// </summary>
		// Token: 0x060043E6 RID: 17382 RVA: 0x0020F854 File Offset: 0x0020DA54
		private void OnPeerDisconnected(ulong peerId)
		{
			TaskCompletionSource syncCompletionSource = this._syncCompletionSource;
			if (syncCompletionSource == null || syncCompletionSource.Task.IsCompleted)
			{
				return;
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Peer ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(peerId);
			defaultInterpolatedStringHandler.AppendLiteral(" disconnected, checking if sync is complete");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.CheckSyncCompleted();
		}

		/// <summary>
		/// Sends a message syncing our combat state to all other players.
		/// This should be called early before beginning animations for entering a combat room. Then, WaitForSync should be
		/// called just before entering the combat room.
		/// Between this and WaitForSync, no player or RNG state should be modified.
		/// </summary>
		// Token: 0x060043E7 RID: 17383 RVA: 0x0020F8C0 File Offset: 0x0020DAC0
		public void StartSync()
		{
			this._logger.Debug("Broadcasting combat sync message to all peers", 1);
			if (this._netService.Type == NetGameType.Singleplayer || this.IsDisabled)
			{
				return;
			}
			if (this._syncCompletionSource != null)
			{
				throw new InvalidOperationException("StartSync called twice before WaitForSync!");
			}
			SyncPlayerDataMessage syncPlayerDataMessage = default(SyncPlayerDataMessage);
			Player me = LocalContext.GetMe(this._runState);
			syncPlayerDataMessage.player = me.ToSerializable();
			this._netService.SendMessage<SyncPlayerDataMessage>(syncPlayerDataMessage);
			this._syncData[syncPlayerDataMessage.player.NetId] = syncPlayerDataMessage.player;
			this._syncCompletionSource = new TaskCompletionSource();
			if (this._netService.Type == NetGameType.Host)
			{
				this._rngSet = me.RunState.Rng.ToSerializable();
				this._sharedRelicGrabBag = me.RunState.SharedRelicGrabBag.ToSerializable();
				SyncRngMessage syncRngMessage = default(SyncRngMessage);
				syncRngMessage.rng = this._rngSet;
				syncRngMessage.sharedRelicGrabBag = this._sharedRelicGrabBag;
				this._netService.SendMessage<SyncRngMessage>(syncRngMessage);
			}
			this.CheckSyncCompleted();
		}

		/// <summary>
		/// Waits for all other players' combat data to be received. StartSync must be called exactly once before this is called.
		/// </summary>
		// Token: 0x060043E8 RID: 17384 RVA: 0x0020F9CC File Offset: 0x0020DBCC
		public async Task WaitForSync()
		{
			this._logger.Debug("Waiting to receive all sync messages from all clients", 1);
			if (this._netService.Type != NetGameType.Singleplayer && !this.IsDisabled)
			{
				if (this._syncCompletionSource == null)
				{
					throw new InvalidOperationException("StartSync must be called before WaitForSync!");
				}
				await this._syncCompletionSource.Task;
				foreach (KeyValuePair<ulong, SerializablePlayer> keyValuePair in this._syncData)
				{
					if (this._runLobby != null && !this._runLobby.ConnectedPlayerIds.Contains(keyValuePair.Key))
					{
						Logger logger = this._logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Skipping sync for disconnected player ");
						defaultInterpolatedStringHandler.AppendFormatted<ulong>(keyValuePair.Key);
						logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					}
					else
					{
						Player player = this._runState.GetPlayer(keyValuePair.Key);
						if (!LocalContext.IsMe(player))
						{
							player.SyncWithSerializedPlayer(keyValuePair.Value);
						}
					}
				}
				if (this._netService.Type != NetGameType.Host)
				{
					if (this._rngSet != null)
					{
						this._runState.Rng.LoadFromSerializable(this._rngSet);
					}
					else if (this._runState.Players.Count > 1)
					{
						this._logger.Error("There are two or more players and we are a client, but we never received the RNG set!", 1);
					}
					if (this._sharedRelicGrabBag != null)
					{
						this._runState.SharedRelicGrabBag.LoadFromSerializable(this._sharedRelicGrabBag);
					}
					else if (this._runState.Players.Count > 1)
					{
						this._logger.Error("There are two or more players and we are a client, but we never received the shared relic grab bag!", 1);
					}
				}
				this._syncData.Clear();
				this._rngSet = null;
				this._sharedRelicGrabBag = null;
				this._syncCompletionSource = null;
			}
		}

		/// <summary>
		/// Checks if synchronization is complete and sets the result on the sync completion source if we are all done.
		/// This should be called every time we receive some sync data from a peer.
		/// </summary>
		// Token: 0x060043E9 RID: 17385 RVA: 0x0020FA10 File Offset: 0x0020DC10
		private void CheckSyncCompleted()
		{
			if (this._syncCompletionSource == null)
			{
				return;
			}
			if (this._runLobby == null)
			{
				throw new InvalidOperationException("Tried to combat sync without a valid run lobby!");
			}
			if (this._netService.Type == NetGameType.Singleplayer || this.IsDisabled)
			{
				this._syncCompletionSource.SetResult();
				return;
			}
			foreach (ulong num in this._runLobby.ConnectedPlayerIds)
			{
				if (!this._syncData.ContainsKey(num))
				{
					return;
				}
			}
			if (this._rngSet == null)
			{
				return;
			}
			this._syncCompletionSource.SetResult();
		}

		// Token: 0x04001A5D RID: 6749
		private readonly INetGameService _netService;

		// Token: 0x04001A5E RID: 6750
		private readonly RunState _runState;

		// Token: 0x04001A5F RID: 6751
		[Nullable(2)]
		private readonly RunLobby _runLobby;

		// Token: 0x04001A60 RID: 6752
		private readonly Dictionary<ulong, SerializablePlayer> _syncData = new Dictionary<ulong, SerializablePlayer>();

		// Token: 0x04001A61 RID: 6753
		[Nullable(2)]
		private SerializableRunRngSet _rngSet;

		// Token: 0x04001A62 RID: 6754
		[Nullable(2)]
		private SerializableRelicGrabBag _sharedRelicGrabBag;

		// Token: 0x04001A63 RID: 6755
		private readonly Logger _logger = new Logger("CombatStateSynchronizer", LogType.GameSync);

		// Token: 0x04001A64 RID: 6756
		[Nullable(2)]
		private TaskCompletionSource _syncCompletionSource;
	}
}
