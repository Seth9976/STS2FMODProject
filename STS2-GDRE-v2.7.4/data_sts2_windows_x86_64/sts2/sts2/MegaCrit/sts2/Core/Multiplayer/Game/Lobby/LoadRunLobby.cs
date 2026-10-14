using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game.Lobby
{
	/// <summary>
	/// Class which handles the connection flow for players resuming a run from a save file.
	/// This exists before Run, giving players an opportunity to all join before the run is resumed.
	/// Only connections from players in the loaded save file will be accepted; all other connections will be denied.
	/// <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.RunLobby" /> handles player connection and disconnection after the run begins.
	/// </summary>
	// Token: 0x020004B6 RID: 1206
	[NullableContext(1)]
	[Nullable(0)]
	public class LoadRunLobby
	{
		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06004851 RID: 18513 RVA: 0x00221781 File Offset: 0x0021F981
		public INetGameService NetService { get; }

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06004852 RID: 18514 RVA: 0x00221789 File Offset: 0x0021F989
		public ILoadRunLobbyListener LobbyListener { get; }

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06004853 RID: 18515 RVA: 0x00221791 File Offset: 0x0021F991
		public PeerInputSynchronizer InputSynchronizer { get; }

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06004854 RID: 18516 RVA: 0x00221799 File Offset: 0x0021F999
		public SerializableRun Run { get; }

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06004855 RID: 18517 RVA: 0x002217A1 File Offset: 0x0021F9A1
		public HashSet<ulong> ConnectedPlayerIds { get; } = new HashSet<ulong>();

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06004856 RID: 18518 RVA: 0x002217A9 File Offset: 0x0021F9A9
		public GameMode GameMode
		{
			get
			{
				return this.Run.GameMode;
			}
		}

		/// <summary>
		/// If we are the host, this is the amount of time we give clients to send the handshake response in milliseconds.
		/// Public for tests.
		/// </summary>
		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06004857 RID: 18519 RVA: 0x002217B6 File Offset: 0x0021F9B6
		// (set) Token: 0x06004858 RID: 18520 RVA: 0x002217BE File Offset: 0x0021F9BE
		public int HandshakeTimeout { get; set; } = 10000;

		// Token: 0x06004859 RID: 18521 RVA: 0x002217C8 File Offset: 0x0021F9C8
		public LoadRunLobby(INetGameService netService, ILoadRunLobbyListener lobbyListener, SerializableRun runSave)
		{
			this.Run = runSave;
			this.NetService = netService;
			this.LobbyListener = lobbyListener;
			this.InputSynchronizer = new PeerInputSynchronizer(this.NetService);
			this._logger = new Logger("LoadRunLobby", LogType.Network);
			this.NetService.RegisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
			this.NetService.RegisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
			this.NetService.RegisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
			this.NetService.RegisterMessageHandler<PlayerReconnectedMessage>(new MessageHandlerDelegate<PlayerReconnectedMessage>(this.HandlePlayerReconnectedMessage));
			this.NetService.RegisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
			this.NetService.RegisterMessageHandler<LobbyPlayerSetReadyMessage>(new MessageHandlerDelegate<LobbyPlayerSetReadyMessage>(this.HandlePlayerReadyMessage));
			this.NetService.RegisterMessageHandler<LobbyBeginLoadedRunMessage>(new MessageHandlerDelegate<LobbyBeginLoadedRunMessage>(this.HandleLobbyBeginRunMessage));
			this.NetService.Disconnected += this.OnDisconnected;
			if (this.NetService.Type == NetGameType.Host)
			{
				INetHostGameService netHostGameService = (INetHostGameService)netService;
				netHostGameService.ClientConnected += this.OnConnectedToClientAsHost;
				netHostGameService.ClientDisconnected += this.OnDisconnectedFromClientAsHost;
			}
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x00221930 File Offset: 0x0021FB30
		public LoadRunLobby(INetGameService netService, ILoadRunLobbyListener lobbyListener, ClientLoadJoinResponseMessage message)
			: this(netService, lobbyListener, message.serializableRun)
		{
			foreach (ulong num in message.playersAlreadyConnected)
			{
				this.ConnectedPlayerIds.Add(num);
			}
		}

		/// <summary>
		/// This should be called to cleanup the lobby before exiting the lobby screen.
		/// </summary>
		/// <param name="disconnectSession">
		/// If true, the net service will be disconnected. Pass true if the lobby is being closed rather than transitioning
		/// to a run.
		/// </param>
		/// <param name="error">If disconnectSession is true, this is the error that is sent to clients.</param>
		// Token: 0x0600485B RID: 18523 RVA: 0x00221998 File Offset: 0x0021FB98
		public void CleanUp(bool disconnectSession, NetError error = NetError.Quit)
		{
			this.NetService.UnregisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
			this.NetService.UnregisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
			this.NetService.UnregisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
			this.NetService.UnregisterMessageHandler<PlayerReconnectedMessage>(new MessageHandlerDelegate<PlayerReconnectedMessage>(this.HandlePlayerReconnectedMessage));
			this.NetService.UnregisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
			this.NetService.UnregisterMessageHandler<LobbyPlayerSetReadyMessage>(new MessageHandlerDelegate<LobbyPlayerSetReadyMessage>(this.HandlePlayerReadyMessage));
			this.NetService.UnregisterMessageHandler<LobbyBeginLoadedRunMessage>(new MessageHandlerDelegate<LobbyBeginLoadedRunMessage>(this.HandleLobbyBeginRunMessage));
			if (disconnectSession)
			{
				if (this.NetService.IsConnected)
				{
					this.NetService.Disconnect(error, false);
				}
				this.InputSynchronizer.Dispose();
			}
			this.NetService.Disconnected -= this.OnDisconnected;
			if (this.NetService.Type == NetGameType.Host)
			{
				INetHostGameService netHostGameService = (INetHostGameService)this.NetService;
				netHostGameService.ClientConnected -= this.OnConnectedToClientAsHost;
				netHostGameService.ClientDisconnected -= this.OnDisconnectedFromClientAsHost;
			}
		}

		/// <summary>
		/// Should be called when the lobby opens on the host player's side to generate the host's lobby player.
		/// </summary>
		// Token: 0x0600485C RID: 18524 RVA: 0x00221AC4 File Offset: 0x0021FCC4
		public void AddLocalHostPlayer()
		{
			if (this.NetService.Type == NetGameType.Client)
			{
				throw new InvalidOperationException("Tried to add local host player as client!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Lobby (");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.NetService.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			logger.Context = defaultInterpolatedStringHandler.ToStringAndClear();
			this.ConnectedPlayerIds.Add(this.NetService.NetId);
			this.LobbyListener.PlayerConnected(this.NetService.NetId);
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x00221B5C File Offset: 0x0021FD5C
		private void HandleClientLoadJoinRequestMessage(ClientLoadJoinRequestMessage message, ulong senderId)
		{
			if (this.NetService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received ClientLoadJoinRequestMessage as non-host!");
			}
			INetHostGameService netHostGameService = (INetHostGameService)this.NetService;
			try
			{
				if (this.Run.Players.FindIndex((SerializablePlayer p) => p.NetId == senderId) < 0)
				{
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Client ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
					defaultInterpolatedStringHandler.AppendLiteral(" sent ClientLoadJoinRequestMessage but they are not in the loaded run!");
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					netHostGameService.DisconnectClient(senderId, NetError.NotInSaveGame, false);
				}
				else
				{
					Logger logger2 = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Received ClientLoadJoinRequestMessage for ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
					logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this.ConnectedPlayerIds.Add(senderId);
					this.LobbyListener.PlayerConnected(senderId);
					ClientLoadJoinResponseMessage clientLoadJoinResponseMessage = default(ClientLoadJoinResponseMessage);
					clientLoadJoinResponseMessage.serializableRun = this.Run;
					clientLoadJoinResponseMessage.playersAlreadyConnected = this.ConnectedPlayerIds.ToList<ulong>();
					Logger logger3 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Sending ClientLoadJoinResponseMessage to ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
					logger3.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					netHostGameService.SendMessage<ClientLoadJoinResponseMessage>(clientLoadJoinResponseMessage, senderId);
					netHostGameService.SetPeerReadyForBroadcasting(senderId);
					PlayerReconnectedMessage playerReconnectedMessage = default(PlayerReconnectedMessage);
					playerReconnectedMessage.playerId = senderId;
					foreach (ulong num in this.ConnectedPlayerIds)
					{
						if (num != senderId && num != this.NetService.NetId)
						{
							Logger logger4 = this._logger;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Sending PlayerReconnectedMessage to ");
							defaultInterpolatedStringHandler.AppendFormatted<ulong>(num);
							logger4.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
							netHostGameService.SendMessage<PlayerReconnectedMessage>(playerReconnectedMessage, num);
						}
					}
					this.RemoveConnectingPlayer(senderId);
				}
			}
			catch
			{
				netHostGameService.DisconnectClient(senderId, NetError.InternalError, false);
				throw;
			}
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x00221DD0 File Offset: 0x0021FFD0
		private void HandleClientLobbyJoinRequestMessage(ClientLobbyJoinRequestMessage _, ulong senderId)
		{
			if (this.NetService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received ClientLobbyJoinRequestMessage as non-host!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received invalid ClientLobbyJoinRequestMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			INetHostGameService netHostGameService = (INetHostGameService)this.NetService;
			netHostGameService.DisconnectClient(senderId, NetError.InvalidJoin, false);
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x00221E3C File Offset: 0x0022003C
		private void HandleClientRejoinRequestMessage(ClientRejoinRequestMessage _, ulong senderId)
		{
			if (this.NetService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received ClientRejoinRequestMessage as non-host!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received invalid ClientRejoinRequestMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			INetHostGameService netHostGameService = (INetHostGameService)this.NetService;
			netHostGameService.DisconnectClient(senderId, NetError.InvalidJoin, false);
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x00221EA8 File Offset: 0x002200A8
		private void HandlePlayerReconnectedMessage(PlayerReconnectedMessage message, ulong _)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received PlayerReconnectedMessage with player ID ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(message.playerId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.ConnectedPlayerIds.Add(message.playerId);
			this.LobbyListener.PlayerConnected(message.playerId);
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x00221F10 File Offset: 0x00220110
		private void HandlePlayerLeftMessage(PlayerLeftMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received PlayerLeftMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(message.playerId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (this.ConnectedPlayerIds.Contains(message.playerId))
			{
				this.ConnectedPlayerIds.Remove(message.playerId);
				this.InputSynchronizer.OnPlayerDisconnected(message.playerId);
				this.LobbyListener.RemotePlayerDisconnected(message.playerId);
			}
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x00221F9C File Offset: 0x0022019C
		private void HandlePlayerReadyMessage(LobbyPlayerSetReadyMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Received ");
			defaultInterpolatedStringHandler.AppendFormatted("LobbyPlayerSetReadyMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(" with value ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(message.ready);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (message.ready)
			{
				if (this._readyPlayers.Add(senderId))
				{
					this.LobbyListener.PlayerReadyChanged(senderId);
				}
			}
			else if (this._readyPlayers.Remove(senderId))
			{
				this.LobbyListener.PlayerReadyChanged(senderId);
			}
			this.BeginRunForAllPlayersIfAllReady();
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x0022204F File Offset: 0x0022024F
		private void HandleLobbyBeginRunMessage(LobbyBeginLoadedRunMessage message, ulong senderId)
		{
			this._logger.Debug("Received LobbyBeginLoadedRunMessage", 1);
			this._isBeginningRun = true;
			this.BeginRunLocally();
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x00222070 File Offset: 0x00220270
		private async Task TryBeginRunForAllPlayers()
		{
			if (this.NetService.Type == NetGameType.Client)
			{
				throw new InvalidOperationException("Can only begin run for all peers as host!");
			}
			if (this._isBeginningRun)
			{
				this._logger.Warn("Tried to begin run twice, ignoring second one!", 1);
			}
			else
			{
				this._isBeginningRun = true;
				TaskAwaiter<bool> taskAwaiter = this.LobbyListener.ShouldAllowRunToBegin().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					this.SetReady(false);
					this._isBeginningRun = false;
				}
				else
				{
					LobbyBeginLoadedRunMessage lobbyBeginLoadedRunMessage = default(LobbyBeginLoadedRunMessage);
					this.NetService.SendMessage<LobbyBeginLoadedRunMessage>(lobbyBeginLoadedRunMessage);
					this.BeginRunLocally();
					if (this.NetService.Type == NetGameType.Host)
					{
						NetHost netHost = ((INetHostGameService)this.NetService).NetHost;
						if (netHost != null)
						{
							netHost.SetHostIsClosed(true);
						}
					}
				}
			}
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x002220B3 File Offset: 0x002202B3
		private void BeginRunLocally()
		{
			this.NetService.SetBufferMessages(true);
			this.LobbyListener.BeginRun();
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x002220CC File Offset: 0x002202CC
		public void SetReady(bool ready)
		{
			if (ready)
			{
				this._readyPlayers.Add(this.NetService.NetId);
			}
			else
			{
				this._readyPlayers.Remove(this.NetService.NetId);
			}
			LobbyPlayerSetReadyMessage lobbyPlayerSetReadyMessage = default(LobbyPlayerSetReadyMessage);
			lobbyPlayerSetReadyMessage.ready = ready;
			this.NetService.SendMessage<LobbyPlayerSetReadyMessage>(lobbyPlayerSetReadyMessage);
			this.LobbyListener.PlayerReadyChanged(this.NetService.NetId);
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Local player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.NetService.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" is ready");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.BeginRunForAllPlayersIfAllReady();
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x0022218B File Offset: 0x0022038B
		public bool IsPlayerReady(ulong playerId)
		{
			return this._readyPlayers.Contains(playerId);
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x0022219C File Offset: 0x0022039C
		public bool IsAboutToBeginGame()
		{
			return this._connectingPlayers.Count <= 0 && (!this.NetService.Type.IsMultiplayer() || this.ConnectedPlayerIds.Count != 1) && !this.ConnectedPlayerIds.Except<ulong>(this._readyPlayers).Any<ulong>();
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x002221F6 File Offset: 0x002203F6
		private void BeginRunForAllPlayersIfAllReady()
		{
			if (this.NetService.Type != NetGameType.Host && this.NetService.Type != NetGameType.Singleplayer)
			{
				return;
			}
			if (this.IsAboutToBeginGame())
			{
				TaskHelper.RunSafely(this.TryBeginRunForAllPlayers());
			}
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x0022222C File Offset: 0x0022042C
		private void OnConnectedToClientAsHost(ulong playerId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Client ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			defaultInterpolatedStringHandler.AppendLiteral(" connected. Sending initial game info message");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			InitialGameInfoMessage initialGameInfoMessage = InitialGameInfoMessage.Basic();
			initialGameInfoMessage.sessionState = RunSessionState.InLoadedLobby;
			initialGameInfoMessage.gameMode = this.GameMode;
			if (this._isBeginningRun)
			{
				initialGameInfoMessage.connectionFailureReason = new ConnectionFailureReason?(ConnectionFailureReason.RunInProgress);
				this.NetService.SendMessage<InitialGameInfoMessage>(initialGameInfoMessage, playerId);
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Client ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" connected but we are already beginning the run!");
				logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				((INetHostGameService)this.NetService).DisconnectClient(playerId, NetError.RunInProgress, false);
				return;
			}
			if (this.Run.Players.FindIndex((SerializablePlayer p) => p.NetId == playerId) < 0)
			{
				initialGameInfoMessage.connectionFailureReason = new ConnectionFailureReason?(ConnectionFailureReason.NotInSaveGame);
				this.NetService.SendMessage<InitialGameInfoMessage>(initialGameInfoMessage, playerId);
				Logger logger3 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Client ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" connected but they were not in the loaded game!");
				logger3.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				((INetHostGameService)this.NetService).DisconnectClient(playerId, NetError.NotInSaveGame, false);
				return;
			}
			LoadRunLobby.ConnectingPlayer connectingPlayer = new LoadRunLobby.ConnectingPlayer
			{
				id = playerId,
				timeoutCancelToken = new CancellationTokenSource()
			};
			this._connectingPlayers.Add(connectingPlayer);
			this.NetService.SendMessage<InitialGameInfoMessage>(initialGameInfoMessage, playerId);
			TaskHelper.RunSafely(this.BeginHandshakeTimeout(connectingPlayer));
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x00222410 File Offset: 0x00220610
		private async Task BeginHandshakeTimeout(LoadRunLobby.ConnectingPlayer connectingPlayer)
		{
			await Task.Delay(this.HandshakeTimeout, connectingPlayer.timeoutCancelToken.Token);
			if (!connectingPlayer.timeoutCancelToken.IsCancellationRequested)
			{
				if (this._connectingPlayers.IndexOf(connectingPlayer) >= 0)
				{
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(95, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Disconnecting player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(connectingPlayer.id);
					defaultInterpolatedStringHandler.AppendLiteral(" because they did not respond to the initial game join handshake within ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.HandshakeTimeout);
					defaultInterpolatedStringHandler.AppendLiteral("ms");
					logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					((INetHostGameService)this.NetService).DisconnectClient(connectingPlayer.id, NetError.HandshakeTimeout, false);
				}
			}
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x0022245C File Offset: 0x0022065C
		private void OnDisconnectedFromClientAsHost(ulong playerId, NetErrorInfo info)
		{
			if (!this.ConnectedPlayerIds.Contains(playerId))
			{
				return;
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Client ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			defaultInterpolatedStringHandler.AppendLiteral(" disconnected, reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(info.GetReason());
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			PlayerLeftMessage playerLeftMessage = new PlayerLeftMessage
			{
				playerId = playerId
			};
			this.NetService.SendMessage<PlayerLeftMessage>(playerLeftMessage);
			this.ConnectedPlayerIds.Remove(playerId);
			this._readyPlayers.Remove(playerId);
			this.RemoveConnectingPlayer(playerId);
			this.InputSynchronizer.OnPlayerDisconnected(playerLeftMessage.playerId);
			this.LobbyListener.RemotePlayerDisconnected(playerId);
			this.BeginRunForAllPlayersIfAllReady();
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x00222528 File Offset: 0x00220728
		private void RemoveConnectingPlayer(ulong playerId)
		{
			for (int i = 0; i < this._connectingPlayers.Count; i++)
			{
				if (this._connectingPlayers[i].id == playerId)
				{
					this._connectingPlayers[i].timeoutCancelToken.Cancel();
					this._connectingPlayers.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x00222588 File Offset: 0x00220788
		private void OnDisconnected(NetErrorInfo info)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Disconnected from host, reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(info.GetReason());
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.ConnectedPlayerIds.Clear();
			this.LobbyListener.LocalPlayerDisconnected(info);
		}

		// Token: 0x04001C71 RID: 7281
		private readonly Logger _logger;

		// Token: 0x04001C72 RID: 7282
		private readonly List<LoadRunLobby.ConnectingPlayer> _connectingPlayers = new List<LoadRunLobby.ConnectingPlayer>();

		// Token: 0x04001C73 RID: 7283
		private bool _isBeginningRun;

		// Token: 0x04001C74 RID: 7284
		private readonly HashSet<ulong> _readyPlayers = new HashSet<ulong>();

		// Token: 0x020019E3 RID: 6627
		[NullableContext(0)]
		private struct ConnectingPlayer : IEquatable<LoadRunLobby.ConnectingPlayer>
		{
			// Token: 0x0600ABBE RID: 43966 RVA: 0x003639B2 File Offset: 0x00361BB2
			public bool Equals(LoadRunLobby.ConnectingPlayer other)
			{
				return this.id == other.id && this.timeoutCancelToken.Equals(other.timeoutCancelToken);
			}

			// Token: 0x0600ABBF RID: 43967 RVA: 0x003639D8 File Offset: 0x00361BD8
			[NullableContext(2)]
			public override bool Equals(object obj)
			{
				if (obj is LoadRunLobby.ConnectingPlayer)
				{
					LoadRunLobby.ConnectingPlayer connectingPlayer = (LoadRunLobby.ConnectingPlayer)obj;
					return this.Equals(connectingPlayer);
				}
				return false;
			}

			// Token: 0x0600ABC0 RID: 43968 RVA: 0x003639FD File Offset: 0x00361BFD
			public override int GetHashCode()
			{
				return HashCode.Combine<ulong, CancellationTokenSource>(this.id, this.timeoutCancelToken);
			}

			// Token: 0x04006633 RID: 26163
			public ulong id;

			// Token: 0x04006634 RID: 26164
			[Nullable(1)]
			public CancellationTokenSource timeoutCancelToken;
		}
	}
}
