using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game.Lobby
{
	/// <summary>
	/// Class which handles disconnection and reconnection while a run is in progress.
	/// <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby" /> or <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby" /> are used to initialize the run. Once the run is in
	/// progress, those instances go away and this class takes over their responsibilities.
	/// Connections from players that are not currently in the run will be denied. Only players that existed in the run
	/// may reconnect to the RunLobby.
	/// </summary>
	// Token: 0x020004B7 RID: 1207
	[NullableContext(2)]
	[Nullable(0)]
	public class RunLobby
	{
		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x0600486F RID: 18543 RVA: 0x002225E3 File Offset: 0x002207E3
		[Nullable(1)]
		public IReadOnlyCollection<ulong> ConnectedPlayerIds
		{
			[NullableContext(1)]
			get
			{
				return this._connectedPlayerIds;
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06004870 RID: 18544 RVA: 0x002225EB File Offset: 0x002207EB
		public GameMode GameMode { get; }

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06004871 RID: 18545 RVA: 0x002225F4 File Offset: 0x002207F4
		// (remove) Token: 0x06004872 RID: 18546 RVA: 0x0022262C File Offset: 0x0022082C
		public event Action<ulong> PlayerRejoined;

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06004873 RID: 18547 RVA: 0x00222664 File Offset: 0x00220864
		// (remove) Token: 0x06004874 RID: 18548 RVA: 0x0022269C File Offset: 0x0022089C
		public event Action<ulong> RemotePlayerDisconnected;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06004875 RID: 18549 RVA: 0x002226D4 File Offset: 0x002208D4
		// (remove) Token: 0x06004876 RID: 18550 RVA: 0x0022270C File Offset: 0x0022090C
		public event Action LocalPlayerDisconnected;

		/// <summary>
		/// Creates the RunLobby.
		/// </summary>
		/// <param name="gameMode">The type of run that was started.</param>
		/// <param name="netService">Net service used to send/receive messages.</param>
		/// <param name="lobbyListener">The lobby listener which receives callbacks from this lobby.</param>
		/// <param name="playerCollection">Queryable collection of players.</param>
		/// <param name="connectedPlayerIds">
		/// The connected player IDs. Note that this may be different than the IDs contained
		/// in the player collection if we launched a saved game, but not everyone in the save rejoined.
		/// </param>
		// Token: 0x06004877 RID: 18551 RVA: 0x00222744 File Offset: 0x00220944
		[NullableContext(1)]
		public RunLobby(GameMode gameMode, INetGameService netService, IRunLobbyListener lobbyListener, IPlayerCollection playerCollection, IEnumerable<ulong> connectedPlayerIds)
		{
			this.GameMode = gameMode;
			this._netService = netService;
			this._lobbyListener = lobbyListener;
			this._playerCollection = playerCollection;
			this._logger = new Logger("RunLobby", LogType.Network);
			this._netService.RegisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
			this._netService.RegisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
			this._netService.RegisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
			this._netService.RegisterMessageHandler<PlayerRejoinedMessage>(new MessageHandlerDelegate<PlayerRejoinedMessage>(this.HandlePlayerRejoinedMessage));
			this._netService.RegisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
			this._netService.RegisterMessageHandler<RunAbandonedMessage>(new MessageHandlerDelegate<RunAbandonedMessage>(this.HandleRunAbandonedMessage));
			foreach (ulong num in connectedPlayerIds)
			{
				this._connectedPlayerIds.Add(num);
			}
			this._netService.Disconnected += this.OnDisconnected;
			if (this._netService.Type == NetGameType.Host)
			{
				INetHostGameService netHostGameService = netService as INetHostGameService;
				if (netHostGameService != null)
				{
					netHostGameService.ClientConnected += this.OnConnectedToClientAsHost;
					netHostGameService.ClientDisconnected += this.OnDisconnectedFromClientAsHost;
				}
			}
		}

		/// <summary>
		/// This should be called to cleanup the lobby before exiting the run.
		/// </summary>
		// Token: 0x06004878 RID: 18552 RVA: 0x002228B0 File Offset: 0x00220AB0
		public void Dispose()
		{
			this._netService.UnregisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
			this._netService.UnregisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
			this._netService.UnregisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
			this._netService.UnregisterMessageHandler<PlayerRejoinedMessage>(new MessageHandlerDelegate<PlayerRejoinedMessage>(this.HandlePlayerRejoinedMessage));
			this._netService.UnregisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
			this._netService.UnregisterMessageHandler<RunAbandonedMessage>(new MessageHandlerDelegate<RunAbandonedMessage>(this.HandleRunAbandonedMessage));
			this._netService.Disconnected -= this.OnDisconnected;
			if (this._netService.Type == NetGameType.Host)
			{
				INetHostGameService netHostGameService = this._netService as INetHostGameService;
				if (netHostGameService != null)
				{
					netHostGameService.ClientConnected -= this.OnConnectedToClientAsHost;
					netHostGameService.ClientDisconnected -= this.OnDisconnectedFromClientAsHost;
				}
			}
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x002229A0 File Offset: 0x00220BA0
		private void HandleClientRejoinRequestMessage(ClientRejoinRequestMessage message, ulong senderId)
		{
			if (this._netService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received ClientRejoinRequestMessage as non-host!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received ClientRejoinRequestMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			NetHostGameService netHostGameService = (NetHostGameService)this._netService;
			if (this._playerCollection.GetPlayer(senderId) == null)
			{
				netHostGameService.DisconnectClient(senderId, NetError.RunInProgress, false);
				return;
			}
			ClientRejoinResponseMessage rejoinMessage = this._lobbyListener.GetRejoinMessage();
			netHostGameService.SendMessage<ClientRejoinResponseMessage>(rejoinMessage, senderId);
			netHostGameService.SetPeerReadyForBroadcasting(senderId);
			PlayerRejoinedMessage playerRejoinedMessage = new PlayerRejoinedMessage
			{
				playerId = senderId
			};
			foreach (NetClientData netClientData in netHostGameService.ConnectedPeers)
			{
				if (netClientData.readyForBroadcasting && netClientData.peerId != senderId)
				{
					netHostGameService.SendMessage<PlayerRejoinedMessage>(playerRejoinedMessage, netClientData.peerId);
				}
			}
			this._connectedPlayerIds.Add(senderId);
			Action<ulong> playerRejoined = this.PlayerRejoined;
			if (playerRejoined == null)
			{
				return;
			}
			playerRejoined(senderId);
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x00222AC8 File Offset: 0x00220CC8
		private void HandleClientLobbyJoinRequestMessage(ClientLobbyJoinRequestMessage _, ulong senderId)
		{
			if (this._netService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received ClientLobbyJoinRequestMessage as non-host!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received invalid ClientLobbyJoinRequestMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			NetHostGameService netHostGameService = (NetHostGameService)this._netService;
			netHostGameService.DisconnectClient(senderId, NetError.InvalidJoin, false);
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x00222B34 File Offset: 0x00220D34
		private void HandleClientLoadJoinRequestMessage(ClientLoadJoinRequestMessage _, ulong senderId)
		{
			if (this._netService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received ClientLoadJoinRequestMessage as non-host!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received invalid ClientLoadJoinRequestMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			NetHostGameService netHostGameService = (NetHostGameService)this._netService;
			netHostGameService.DisconnectClient(senderId, NetError.InvalidJoin, false);
		}

		// Token: 0x0600487C RID: 18556 RVA: 0x00222BA0 File Offset: 0x00220DA0
		private void HandlePlayerRejoinedMessage(PlayerRejoinedMessage message, ulong _)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received PlayerRejoinedMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(message.playerId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this._connectedPlayerIds.Add(message.playerId);
			Action<ulong> playerRejoined = this.PlayerRejoined;
			if (playerRejoined == null)
			{
				return;
			}
			playerRejoined(message.playerId);
		}

		// Token: 0x0600487D RID: 18557 RVA: 0x00222C0C File Offset: 0x00220E0C
		private void HandlePlayerLeftMessage(PlayerLeftMessage message, ulong _)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received PlayerLeftMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(message.playerId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this._connectedPlayerIds.Remove(message.playerId);
			Action<ulong> remotePlayerDisconnected = this.RemotePlayerDisconnected;
			if (remotePlayerDisconnected == null)
			{
				return;
			}
			remotePlayerDisconnected(message.playerId);
		}

		// Token: 0x0600487E RID: 18558 RVA: 0x00222C78 File Offset: 0x00220E78
		public void AbandonRun()
		{
			if (this._netService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Abandon run can only be called as host!");
			}
			this._logger.Info("Abandoning run as host", 1);
			NetHostGameService netHostGameService = (NetHostGameService)this._netService;
			netHostGameService.SendMessage<RunAbandonedMessage>(default(RunAbandonedMessage));
			this._lobbyListener.RunAbandoned();
			this._netService.Disconnect(NetError.HostAbandoned, false);
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x00222CE2 File Offset: 0x00220EE2
		private void HandleRunAbandonedMessage(RunAbandonedMessage message, ulong _)
		{
			this._logger.Debug("Received RunAbandonedMessage", 1);
			this._lobbyListener.RunAbandoned();
			this._netService.Disconnect(NetError.HostAbandoned, false);
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x00222D10 File Offset: 0x00220F10
		private void OnConnectedToClientAsHost(ulong playerId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			defaultInterpolatedStringHandler.AppendLiteral(" connected to host.");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			InitialGameInfoMessage initialGameInfoMessage = InitialGameInfoMessage.Basic();
			initialGameInfoMessage.sessionState = RunSessionState.Running;
			initialGameInfoMessage.gameMode = this.GameMode;
			if (this._playerCollection.GetPlayer(playerId) == null)
			{
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Client ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" connected but they are not in the run!");
				logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				initialGameInfoMessage.connectionFailureReason = new ConnectionFailureReason?(ConnectionFailureReason.RunInProgress);
				this._netService.SendMessage<InitialGameInfoMessage>(initialGameInfoMessage, playerId);
				NetHostGameService netHostGameService = (NetHostGameService)this._netService;
				netHostGameService.DisconnectClient(playerId, NetError.RunInProgress, false);
				return;
			}
			this._netService.SendMessage<InitialGameInfoMessage>(initialGameInfoMessage, playerId);
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x00222E00 File Offset: 0x00221000
		private void OnDisconnectedFromClientAsHost(ulong playerId, NetErrorInfo info)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			defaultInterpolatedStringHandler.AppendLiteral(" disconnected from host. Is in connected players: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this._connectedPlayerIds.Contains(playerId));
			defaultInterpolatedStringHandler.AppendLiteral(". Reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(info.GetReason());
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (this._connectedPlayerIds.Contains(playerId))
			{
				PlayerLeftMessage playerLeftMessage = new PlayerLeftMessage
				{
					playerId = playerId
				};
				this._netService.SendMessage<PlayerLeftMessage>(playerLeftMessage);
				this._connectedPlayerIds.Remove(playerId);
				Action<ulong> remotePlayerDisconnected = this.RemotePlayerDisconnected;
				if (remotePlayerDisconnected == null)
				{
					return;
				}
				remotePlayerDisconnected(playerId);
			}
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x00222EC4 File Offset: 0x002210C4
		private void OnDisconnected(NetErrorInfo info)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Disconnected. Reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(info.GetReason());
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this._connectedPlayerIds.Clear();
			this._lobbyListener.LocalPlayerDisconnected(info);
			Action localPlayerDisconnected = this.LocalPlayerDisconnected;
			if (localPlayerDisconnected == null)
			{
				return;
			}
			localPlayerDisconnected();
		}

		// Token: 0x04001C7B RID: 7291
		[Nullable(1)]
		private readonly Logger _logger;

		// Token: 0x04001C7C RID: 7292
		[Nullable(1)]
		private readonly IPlayerCollection _playerCollection;

		// Token: 0x04001C7D RID: 7293
		[Nullable(1)]
		private readonly IRunLobbyListener _lobbyListener;

		// Token: 0x04001C7E RID: 7294
		[Nullable(1)]
		private readonly INetGameService _netService;

		// Token: 0x04001C7F RID: 7295
		[Nullable(1)]
		private readonly HashSet<ulong> _connectedPlayerIds = new HashSet<ulong>();
	}
}
