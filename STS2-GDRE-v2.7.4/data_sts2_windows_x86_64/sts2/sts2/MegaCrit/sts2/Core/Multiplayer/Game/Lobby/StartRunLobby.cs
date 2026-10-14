using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Multiplayer.Game.Lobby
{
	/// <summary>
	/// Class which handles the connection flow for players beginning a new run.
	/// Exists before Run does, and provides player data to start a multiplayer run.
	/// <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.RunLobby" /> handles player connection and disconnection after the run begins.
	/// </summary>
	// Token: 0x020004B8 RID: 1208
	[NullableContext(1)]
	[Nullable(0)]
	public class StartRunLobby
	{
		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06004883 RID: 18563 RVA: 0x00222F2F File Offset: 0x0022112F
		public INetGameService NetService { get; }

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06004884 RID: 18564 RVA: 0x00222F37 File Offset: 0x00221137
		public IStartRunLobbyListener LobbyListener { get; }

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06004885 RID: 18565 RVA: 0x00222F3F File Offset: 0x0022113F
		public PeerInputSynchronizer InputSynchronizer { get; }

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06004886 RID: 18566 RVA: 0x00222F47 File Offset: 0x00221147
		// (set) Token: 0x06004887 RID: 18567 RVA: 0x00222F4F File Offset: 0x0022114F
		public int MaxPlayers { get; private set; }

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06004888 RID: 18568 RVA: 0x00222F58 File Offset: 0x00221158
		// (set) Token: 0x06004889 RID: 18569 RVA: 0x00222F60 File Offset: 0x00221160
		public int Ascension { get; private set; }

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x0600488A RID: 18570 RVA: 0x00222F69 File Offset: 0x00221169
		// (set) Token: 0x0600488B RID: 18571 RVA: 0x00222F71 File Offset: 0x00221171
		public int MaxAscension { get; private set; }

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x0600488C RID: 18572 RVA: 0x00222F7A File Offset: 0x0022117A
		// (set) Token: 0x0600488D RID: 18573 RVA: 0x00222F82 File Offset: 0x00221182
		[Nullable(2)]
		public string Seed
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x0600488E RID: 18574 RVA: 0x00222F8B File Offset: 0x0022118B
		// (set) Token: 0x0600488F RID: 18575 RVA: 0x00222F93 File Offset: 0x00221193
		public TimeServerResult? DailyTime { get; private set; }

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06004890 RID: 18576 RVA: 0x00222F9C File Offset: 0x0022119C
		// (set) Token: 0x06004891 RID: 18577 RVA: 0x00222FA4 File Offset: 0x002211A4
		public GameMode GameMode { get; private set; }

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06004892 RID: 18578 RVA: 0x00222FAD File Offset: 0x002211AD
		public IReadOnlyList<ModifierModel> Modifiers
		{
			get
			{
				return this._modifiers;
			}
		}

		/// <summary>
		/// If we are the host, this is the amount of time we give clients to send the handshake response in milliseconds.
		/// Public for tests.
		/// </summary>
		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06004893 RID: 18579 RVA: 0x00222FB5 File Offset: 0x002211B5
		// (set) Token: 0x06004894 RID: 18580 RVA: 0x00222FBD File Offset: 0x002211BD
		public int HandshakeTimeout { get; set; } = 10000;

		/// <summary>
		/// TEMPORARY way for the host to manually specify which ActModel they want for act 1.
		/// </summary>
		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06004895 RID: 18581 RVA: 0x00222FC6 File Offset: 0x002211C6
		// (set) Token: 0x06004896 RID: 18582 RVA: 0x00222FCE File Offset: 0x002211CE
		public string Act1 { get; set; } = "random";

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06004897 RID: 18583 RVA: 0x00222FD7 File Offset: 0x002211D7
		public List<LobbyPlayer> Players { get; } = new List<LobbyPlayer>();

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06004898 RID: 18584 RVA: 0x00222FDF File Offset: 0x002211DF
		public LobbyPlayer LocalPlayer
		{
			get
			{
				return this.Players.Find((LobbyPlayer p) => p.id == this.NetService.NetId);
			}
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06004899 RID: 18585 RVA: 0x00222FF8 File Offset: 0x002211F8
		// (remove) Token: 0x0600489A RID: 18586 RVA: 0x00223030 File Offset: 0x00221230
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<LobbyPlayer> PlayerConnected;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x0600489B RID: 18587 RVA: 0x00223068 File Offset: 0x00221268
		// (remove) Token: 0x0600489C RID: 18588 RVA: 0x002230A0 File Offset: 0x002212A0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<LobbyPlayer> PlayerDisconnected;

		// Token: 0x0600489D RID: 18589 RVA: 0x002230D8 File Offset: 0x002212D8
		public StartRunLobby(GameMode gameMode, INetGameService netService, IStartRunLobbyListener lobbyListener, int maxPlayers)
		{
			this.GameMode = gameMode;
			this.NetService = netService;
			this.LobbyListener = lobbyListener;
			this.MaxPlayers = maxPlayers;
			this.InputSynchronizer = new PeerInputSynchronizer(netService);
			this._logger = new Logger("StartRunLobby", LogType.Network);
			this.NetService.RegisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
			this.NetService.RegisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
			this.NetService.RegisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
			this.NetService.RegisterMessageHandler<PlayerJoinedMessage>(new MessageHandlerDelegate<PlayerJoinedMessage>(this.HandlePlayerJoinedMessage));
			this.NetService.RegisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
			this.NetService.RegisterMessageHandler<LobbyPlayerChangedCharacterMessage>(new MessageHandlerDelegate<LobbyPlayerChangedCharacterMessage>(this.HandleLobbyPlayerChangedCharacterMessage));
			this.NetService.RegisterMessageHandler<LobbyAscensionChangedMessage>(new MessageHandlerDelegate<LobbyAscensionChangedMessage>(this.HandleAscensionChangedMessage));
			this.NetService.RegisterMessageHandler<LobbySeedChangedMessage>(new MessageHandlerDelegate<LobbySeedChangedMessage>(this.HandleSeedChangedMessage));
			this.NetService.RegisterMessageHandler<LobbyModifiersChangedMessage>(new MessageHandlerDelegate<LobbyModifiersChangedMessage>(this.HandleModifiersChangedMessage));
			this.NetService.RegisterMessageHandler<LobbyPlayerSetReadyMessage>(new MessageHandlerDelegate<LobbyPlayerSetReadyMessage>(this.HandlePlayerReadyMessage));
			this.NetService.RegisterMessageHandler<LobbyBeginRunMessage>(new MessageHandlerDelegate<LobbyBeginRunMessage>(this.HandleLobbyBeginRunMessage));
			this.NetService.Disconnected += this.OnDisconnected;
			if (this.NetService.Type == NetGameType.Host)
			{
				INetHostGameService netHostGameService = (INetHostGameService)netService;
				netHostGameService.ClientConnected += this.OnConnectedToClientAsHost;
				netHostGameService.ClientDisconnected += this.OnDisconnectedFromClientAsHost;
				foreach (NetClientData netClientData in netHostGameService.ConnectedPeers)
				{
					this.OnConnectedToClientAsHost(netClientData.peerId);
				}
			}
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x002232F0 File Offset: 0x002214F0
		public StartRunLobby(GameMode gameMode, INetGameService netService, IStartRunLobbyListener lobbyListener, TimeServerResult timeServerResult, int maxPlayers)
			: this(gameMode, netService, lobbyListener, maxPlayers)
		{
			this.DailyTime = new TimeServerResult?(timeServerResult);
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x0022330C File Offset: 0x0022150C
		public void InitializeFromMessage(ClientLobbyJoinResponseMessage message)
		{
			foreach (LobbyPlayer lobbyPlayer in message.playersInLobby)
			{
				this.Players.Add(lobbyPlayer);
			}
			this._modifiers.Clear();
			List<ModifierModel> modifiers = this._modifiers;
			IEnumerable<SerializableModifier> modifiers2 = message.modifiers;
			Func<SerializableModifier, ModifierModel> func;
			if ((func = StartRunLobby.<>O.<0>__FromSerializable) == null)
			{
				func = (StartRunLobby.<>O.<0>__FromSerializable = new Func<SerializableModifier, ModifierModel>(ModifierModel.FromSerializable));
			}
			modifiers.AddRange(modifiers2.Select<SerializableModifier, ModifierModel>(func));
			this.Ascension = message.ascension;
			this.UpdateMaxMultiplayerAscension();
			this.Seed = message.seed;
			this.LobbyListener.PlayerConnected(this.LocalPlayer);
			Action<LobbyPlayer> playerConnected = this.PlayerConnected;
			if (playerConnected == null)
			{
				return;
			}
			playerConnected(this.LocalPlayer);
		}

		/// <summary>
		/// This should be called to cleanup the lobby before exiting the lobby screen.
		/// </summary>
		/// <param name="disconnectSession">
		/// If true, the net service will be disconnected. Pass true if the lobby is being closed rather than transitioning
		/// to a run.
		/// </param>
		/// <param name="error">If disconnectSession is true, this is the error that is sent to clients.</param>
		// Token: 0x060048A0 RID: 18592 RVA: 0x002233E8 File Offset: 0x002215E8
		public void CleanUp(bool disconnectSession, NetError error = NetError.Quit)
		{
			this.NetService.UnregisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
			this.NetService.UnregisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
			this.NetService.UnregisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
			this.NetService.UnregisterMessageHandler<PlayerJoinedMessage>(new MessageHandlerDelegate<PlayerJoinedMessage>(this.HandlePlayerJoinedMessage));
			this.NetService.UnregisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
			this.NetService.UnregisterMessageHandler<LobbyPlayerChangedCharacterMessage>(new MessageHandlerDelegate<LobbyPlayerChangedCharacterMessage>(this.HandleLobbyPlayerChangedCharacterMessage));
			this.NetService.UnregisterMessageHandler<LobbyAscensionChangedMessage>(new MessageHandlerDelegate<LobbyAscensionChangedMessage>(this.HandleAscensionChangedMessage));
			this.NetService.UnregisterMessageHandler<LobbySeedChangedMessage>(new MessageHandlerDelegate<LobbySeedChangedMessage>(this.HandleSeedChangedMessage));
			this.NetService.UnregisterMessageHandler<LobbyModifiersChangedMessage>(new MessageHandlerDelegate<LobbyModifiersChangedMessage>(this.HandleModifiersChangedMessage));
			this.NetService.UnregisterMessageHandler<LobbyPlayerSetReadyMessage>(new MessageHandlerDelegate<LobbyPlayerSetReadyMessage>(this.HandlePlayerReadyMessage));
			this.NetService.UnregisterMessageHandler<LobbyBeginRunMessage>(new MessageHandlerDelegate<LobbyBeginRunMessage>(this.HandleLobbyBeginRunMessage));
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
		// Token: 0x060048A1 RID: 18593 RVA: 0x00223570 File Offset: 0x00221770
		public LobbyPlayer? AddLocalHostPlayer(UnlockState unlocks, int maxMultiplayerAscension)
		{
			if (this.NetService.Type == NetGameType.Client)
			{
				throw new InvalidOperationException("Tried to add local host player as client!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler.AppendFormatted("StartRunLobby");
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.NetService.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			logger.Context = defaultInterpolatedStringHandler.ToStringAndClear();
			SerializableUnlockState serializableUnlockState = unlocks.ToSerializable();
			return this.AddLocalHostPlayerInternal(serializableUnlockState, maxMultiplayerAscension);
		}

		/// <summary>
		/// For use in tests and internally in this class.
		/// </summary>
		// Token: 0x060048A2 RID: 18594 RVA: 0x002235F8 File Offset: 0x002217F8
		public LobbyPlayer? AddLocalHostPlayerInternal(SerializableUnlockState unlockState, int maxMultiplayerAscension)
		{
			LobbyPlayer? lobbyPlayer = this.TryAddPlayerInFirstAvailableSlot(unlockState, maxMultiplayerAscension, this.NetService.NetId);
			if (lobbyPlayer != null)
			{
				this.LobbyListener.PlayerConnected(lobbyPlayer.Value);
				Action<LobbyPlayer> playerConnected = this.PlayerConnected;
				if (playerConnected != null)
				{
					playerConnected(lobbyPlayer.Value);
				}
			}
			this.UpdateMaxMultiplayerAscension();
			return lobbyPlayer;
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x00223654 File Offset: 0x00221854
		private void HandleClientLobbyJoinRequestMessage(ClientLobbyJoinRequestMessage message, ulong senderId)
		{
			if (this.NetService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received ClientLobbyJoinRequestMessage as non-host!");
			}
			INetHostGameService netHostGameService = (INetHostGameService)this.NetService;
			try
			{
				if (this.Players.Count >= this.MaxPlayers)
				{
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Client ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
					defaultInterpolatedStringHandler.AppendLiteral(" sent ClientLobbyJoinRequestMessage but we are at maximum players!");
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					netHostGameService.DisconnectClient(senderId, NetError.LobbyFull, false);
				}
				else
				{
					Logger logger2 = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Received ClientLobbyJoinRequestMessage for ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
					logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					LobbyPlayer? lobbyPlayer = this.TryAddPlayerInFirstAvailableSlot(message.unlockState, message.maxAscensionUnlocked, senderId);
					if (lobbyPlayer != null)
					{
						this.UpdateMaxMultiplayerAscension();
						ClientLobbyJoinResponseMessage clientLobbyJoinResponseMessage = default(ClientLobbyJoinResponseMessage);
						clientLobbyJoinResponseMessage.playersInLobby = this.Players;
						clientLobbyJoinResponseMessage.ascension = this.Ascension;
						clientLobbyJoinResponseMessage.dailyTime = this.DailyTime;
						clientLobbyJoinResponseMessage.seed = this.Seed;
						clientLobbyJoinResponseMessage.modifiers = this.Modifiers.Select<ModifierModel, SerializableModifier>((ModifierModel m) => m.ToSerializable()).ToList<SerializableModifier>();
						Logger logger3 = this._logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Sending ClientLobbyJoinResponseMessage length (");
						defaultInterpolatedStringHandler.AppendFormatted<int>(clientLobbyJoinResponseMessage.playersInLobby.Count);
						defaultInterpolatedStringHandler.AppendLiteral(") to (");
						defaultInterpolatedStringHandler.AppendFormatted<ulong>(lobbyPlayer.Value.id);
						defaultInterpolatedStringHandler.AppendLiteral(")");
						logger3.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						netHostGameService.SendMessage<ClientLobbyJoinResponseMessage>(clientLobbyJoinResponseMessage, senderId);
						netHostGameService.SetPeerReadyForBroadcasting(senderId);
						PlayerJoinedMessage playerJoinedMessage = default(PlayerJoinedMessage);
						playerJoinedMessage.lobbyPlayer = lobbyPlayer.Value;
						foreach (LobbyPlayer lobbyPlayer2 in this.Players)
						{
							if (lobbyPlayer2.id != this.NetService.NetId && lobbyPlayer2.id != lobbyPlayer.Value.id)
							{
								this.NetService.SendMessage<PlayerJoinedMessage>(playerJoinedMessage, lobbyPlayer2.id);
							}
						}
						this.RemoveConnectingPlayer(lobbyPlayer.Value.id);
						this.LobbyListener.PlayerConnected(lobbyPlayer.Value);
						Action<LobbyPlayer> playerConnected = this.PlayerConnected;
						if (playerConnected != null)
						{
							playerConnected(lobbyPlayer.Value);
						}
					}
				}
			}
			catch
			{
				netHostGameService.DisconnectClient(senderId, NetError.InternalError, false);
				throw;
			}
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x00223924 File Offset: 0x00221B24
		private void UpdateMaxMultiplayerAscension()
		{
			int num = this.Players.Min<LobbyPlayer>((LobbyPlayer p) => p.maxMultiplayerAscensionUnlocked);
			if (num != this.MaxAscension)
			{
				this.MaxAscension = num;
				this.LobbyListener.MaxAscensionChanged();
				if (this.Ascension > this.MaxAscension && this.NetService.Type == NetGameType.Host)
				{
					this.SyncAscensionChange(this.MaxAscension);
				}
			}
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x002239A0 File Offset: 0x00221BA0
		private void HandleClientLoadJoinRequestMessage(ClientLoadJoinRequestMessage _, ulong senderId)
		{
			if (this.NetService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received ClientLoadJoinRequestMessage as non-host!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received invalid ClientLoadJoinRequestMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			NetHostGameService netHostGameService = (NetHostGameService)this.NetService;
			netHostGameService.DisconnectClient(senderId, NetError.InvalidJoin, false);
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x00223A0C File Offset: 0x00221C0C
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
			NetHostGameService netHostGameService = (NetHostGameService)this.NetService;
			netHostGameService.DisconnectClient(senderId, NetError.InvalidJoin, false);
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x00223A78 File Offset: 0x00221C78
		private void HandlePlayerJoinedMessage(PlayerJoinedMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received PlayerJoinedMessage with (");
			defaultInterpolatedStringHandler.AppendFormatted<LobbyPlayer>(message.lobbyPlayer);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.Players.Add(message.lobbyPlayer);
			this.LobbyListener.PlayerConnected(message.lobbyPlayer);
			Action<LobbyPlayer> playerConnected = this.PlayerConnected;
			if (playerConnected != null)
			{
				playerConnected(message.lobbyPlayer);
			}
			this.UpdateMaxMultiplayerAscension();
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x00223B08 File Offset: 0x00221D08
		private void HandlePlayerLeftMessage(PlayerLeftMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received PlayerLeftMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(message.playerId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			int num = this.Players.FindIndex((LobbyPlayer p) => p.id == message.playerId);
			if (num >= 0)
			{
				LobbyPlayer lobbyPlayer = this.Players[num];
				this.Players.RemoveAt(num);
				this.InputSynchronizer.OnPlayerDisconnected(lobbyPlayer.id);
				this.LobbyListener.RemotePlayerDisconnected(lobbyPlayer);
				Action<LobbyPlayer> playerDisconnected = this.PlayerDisconnected;
				if (playerDisconnected == null)
				{
					return;
				}
				playerDisconnected(lobbyPlayer);
			}
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x00223BC0 File Offset: 0x00221DC0
		private void HandleLobbyPlayerChangedCharacterMessage(LobbyPlayerChangedCharacterMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Received LobbyPlayerChangedCharacterMessage for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<CharacterModel>(message.character);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.ChangeCharacter(senderId, message.character, false);
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x00223C28 File Offset: 0x00221E28
		private void HandleAscensionChangedMessage(LobbyAscensionChangedMessage message, ulong _)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this._isBeginningRun)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(89, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received AscensionChangedMessage with ascension ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(message.ascension);
				defaultInterpolatedStringHandler.AppendLiteral(" while run was already starting! Ignoring");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			Logger logger = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received AscensionChangedMessage, new ascension: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(message.ascension);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.Ascension = message.ascension;
			this.LobbyListener.AscensionChanged();
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x00223CC8 File Offset: 0x00221EC8
		private void HandleSeedChangedMessage(LobbySeedChangedMessage message, ulong _)
		{
			if (this._isBeginningRun)
			{
				Log.Warn("Received SeedChangedMessage with seed " + message.seed + " while run was already starting! Ignoring", 2);
				return;
			}
			this._logger.Debug("Received SeedChangedMessage, new seed: " + message.seed, 1);
			this.Seed = message.seed;
			this.LobbyListener.SeedChanged();
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x00223D2C File Offset: 0x00221F2C
		private void HandleModifiersChangedMessage(LobbyModifiersChangedMessage message, ulong _)
		{
			this._logger.Debug("Received ModifiersChangedMessage, new modifiers: " + string.Join<ModelId>(",", message.modifiers.Select<SerializableModifier, ModelId>((SerializableModifier m) => m.Id)), 1);
			if (this._isBeginningRun)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(79, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received ModifiersChangedMessage with ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(message.modifiers.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" while run was already starting! Ignoring");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			this._modifiers.Clear();
			List<ModifierModel> modifiers = this._modifiers;
			IEnumerable<SerializableModifier> modifiers2 = message.modifiers;
			Func<SerializableModifier, ModifierModel> func;
			if ((func = StartRunLobby.<>O.<0>__FromSerializable) == null)
			{
				func = (StartRunLobby.<>O.<0>__FromSerializable = new Func<SerializableModifier, ModifierModel>(ModifierModel.FromSerializable));
			}
			modifiers.AddRange(modifiers2.Select<SerializableModifier, ModifierModel>(func));
			this.LobbyListener.ModifiersChanged();
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x00223E14 File Offset: 0x00222014
		private void HandlePlayerReadyMessage(LobbyPlayerSetReadyMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Received LobbyPlayerSetReadyMessage for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(" with value ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(message.ready);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			int num = this.Players.FindIndex((LobbyPlayer p) => p.id == senderId);
			if (num >= 0)
			{
				LobbyPlayer lobbyPlayer = this.Players[num];
				lobbyPlayer.isReady = message.ready;
				this.Players[num] = lobbyPlayer;
				this.LobbyListener.PlayerChanged(lobbyPlayer, false);
				this.BeginRunForAllPlayersIfAllReady();
			}
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x00223ED4 File Offset: 0x002220D4
		private void HandleLobbyBeginRunMessage(LobbyBeginRunMessage message, ulong senderId)
		{
			this._logger.Debug("Received LobbyBeginRunMessage", 1);
			this.Players.Clear();
			this.Players.AddRange(message.playersInLobby);
			this.Act1 = message.act1;
			string seed = message.seed;
			IEnumerable<SerializableModifier> modifiers = message.modifiers;
			Func<SerializableModifier, ModifierModel> func;
			if ((func = StartRunLobby.<>O.<0>__FromSerializable) == null)
			{
				func = (StartRunLobby.<>O.<0>__FromSerializable = new Func<SerializableModifier, ModifierModel>(ModifierModel.FromSerializable));
			}
			this.BeginRunLocally(seed, modifiers.Select<SerializableModifier, ModifierModel>(func).ToList<ModifierModel>());
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x00223F54 File Offset: 0x00222154
		private void ChangeCharacter(ulong playerId, CharacterModel character, bool isRandomCharacterResolution = false)
		{
			if (this._isBeginningRun)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" tried to change character while run was already starting! Ignoring");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			int num = this.Players.FindIndex((LobbyPlayer p) => p.id == playerId);
			if (num >= 0)
			{
				LobbyPlayer lobbyPlayer = this.Players[num];
				lobbyPlayer.character = character;
				this.Players[num] = lobbyPlayer;
				this.LobbyListener.PlayerChanged(lobbyPlayer, isRandomCharacterResolution);
			}
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x00224000 File Offset: 0x00222200
		private void BeginRunForAllPlayers(string seed, List<ModifierModel> modifiers)
		{
			if (this.NetService.Type == NetGameType.Client)
			{
				throw new InvalidOperationException("Can only begin run as host!");
			}
			if (this._isBeginningRun)
			{
				this._logger.Warn("Tried to begin run twice, ignoring second one!", 1);
				return;
			}
			this.UpdatePreferredAscension();
			LobbyBeginRunMessage lobbyBeginRunMessage = default(LobbyBeginRunMessage);
			lobbyBeginRunMessage.playersInLobby = this.Players;
			lobbyBeginRunMessage.seed = seed;
			lobbyBeginRunMessage.modifiers = modifiers.Select<ModifierModel, SerializableModifier>((ModifierModel m) => m.ToSerializable()).ToList<SerializableModifier>();
			lobbyBeginRunMessage.act1 = this.Act1;
			this.NetService.SendMessage<LobbyBeginRunMessage>(lobbyBeginRunMessage);
			this.BeginRunLocally(seed, modifiers);
			if (this.NetService.Type == NetGameType.Host)
			{
				INetHostGameService netHostGameService = (INetHostGameService)this.NetService;
				NetHost netHost = netHostGameService.NetHost;
				if (netHost == null)
				{
					return;
				}
				netHost.SetHostIsClosed(true);
			}
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x002240E0 File Offset: 0x002222E0
		private void BeginRunLocally(string seed, List<ModifierModel> modifiers)
		{
			Rng rng = new Rng(StringHelper.GetDeterministicHashCode(seed), "act_selection");
			List<ActModel> list = ActModel.GetRandomList(rng, this.GetUnlockState(), this.NetService.Type.IsMultiplayer()).ToList<ActModel>();
			list[0] = StartRunLobby.GetAct(this.Act1) ?? list[0];
			for (int i = 0; i < this.Players.Count; i++)
			{
				LobbyPlayer lobbyPlayer = this.Players[i];
				if (lobbyPlayer.character is RandomCharacter)
				{
					CharacterModel characterModel = rng.NextItem<CharacterModel>(ModelDb.AllCharacters);
					this.ChangeCharacter(lobbyPlayer.id, characterModel, true);
				}
			}
			if (this.NetService.Type == NetGameType.Singleplayer)
			{
				CharacterStats orCreateCharacterStats = SaveManager.Instance.Progress.GetOrCreateCharacterStats(this.Players[0].character.Id);
				int num = Math.Min(this.Ascension, orCreateCharacterStats.MaxAscension);
				if (this.Ascension != num)
				{
					this.Ascension = num;
					this.LobbyListener.AscensionChanged();
				}
				if (this.MaxAscension != orCreateCharacterStats.MaxAscension)
				{
					this.MaxAscension = orCreateCharacterStats.MaxAscension;
					this.LobbyListener.MaxAscensionChanged();
				}
			}
			this.NetService.SetBufferMessages(true);
			this._isBeginningRun = true;
			this.LobbyListener.BeginRun(seed, list, modifiers);
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x00224238 File Offset: 0x00222438
		[return: Nullable(2)]
		private static ActModel GetAct(string act1Key)
		{
			ActModel actModel;
			if (!(act1Key == "overgrowth"))
			{
				if (!(act1Key == "underdocks"))
				{
					actModel = null;
				}
				else
				{
					actModel = ModelDb.Act<Underdocks>();
				}
			}
			else
			{
				actModel = ModelDb.Act<Overgrowth>();
			}
			return actModel;
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x00224274 File Offset: 0x00222474
		private void SetSingleplayerAscensionAfterCharacterChanged(ModelId characterId)
		{
			if (this.NetService.Type.IsMultiplayer())
			{
				return;
			}
			CharacterStats orCreateCharacterStats = SaveManager.Instance.Progress.GetOrCreateCharacterStats(characterId);
			bool flag = this.IsAscensionEpochRevealed(characterId);
			if (characterId == ModelDb.GetId<RandomCharacter>())
			{
				this.MaxAscension = this.GetMaxAscensionAcrossAllCharacters();
				this.SyncAscensionChange(Math.Min(orCreateCharacterStats.PreferredAscension, this.MaxAscension));
				this.LobbyListener.MaxAscensionChanged();
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 2);
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
				defaultInterpolatedStringHandler.AppendLiteral(" ascension set to Max: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Ascension);
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			else if (orCreateCharacterStats == null || orCreateCharacterStats.MaxAscension <= 0 || !flag)
			{
				this.MaxAscension = 0;
				this.SyncAscensionChange(0);
				this.LobbyListener.MaxAscensionChanged();
				if (!flag)
				{
					Logger logger2 = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
					defaultInterpolatedStringHandler.AppendLiteral(" has not revealed the Ascension Epoch, disabling Ascension.");
					logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				}
				else
				{
					Logger logger3 = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
					defaultInterpolatedStringHandler.AppendLiteral(" has no progress, disabling Ascension.");
					logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				}
			}
			else
			{
				this.MaxAscension = orCreateCharacterStats.MaxAscension;
				this.SyncAscensionChange(Math.Min(orCreateCharacterStats.PreferredAscension, orCreateCharacterStats.MaxAscension));
				this.LobbyListener.MaxAscensionChanged();
				Logger logger4 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(characterId);
				defaultInterpolatedStringHandler.AppendLiteral(" ascension set to preferred: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Ascension);
				logger4.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			if (this.GameMode == GameMode.Standard && this.GetMaxAscensionAcrossAllCharacters() > 0 && !SaveManager.Instance.SeenPopup("ascension_singleplayer_ftue"))
			{
				NAscensionSingleplayerFtue nascensionSingleplayerFtue = NAscensionSingleplayerFtue.Create();
				if (nascensionSingleplayerFtue != null)
				{
					NModalContainer.Instance.Add(nascensionSingleplayerFtue, true);
				}
			}
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x00224468 File Offset: 0x00222668
		private int GetMaxAscensionAcrossAllCharacters()
		{
			int num = 0;
			foreach (CharacterStats characterStats in SaveManager.Instance.Progress.CharacterStats.Values)
			{
				num = Math.Max(num, characterStats.MaxAscension);
			}
			return num;
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x002244CC File Offset: 0x002226CC
		private bool IsAscensionEpochRevealed(ModelId characterId)
		{
			if (characterId == ModelDb.GetId<Ironclad>())
			{
				return SaveManager.Instance.IsEpochRevealed<Ironclad4Epoch>();
			}
			if (characterId == ModelDb.GetId<Silent>())
			{
				return SaveManager.Instance.IsEpochRevealed<Silent4Epoch>();
			}
			if (characterId == ModelDb.GetId<Regent>())
			{
				return SaveManager.Instance.IsEpochRevealed<Regent4Epoch>();
			}
			if (characterId == ModelDb.GetId<Defect>())
			{
				return SaveManager.Instance.IsEpochRevealed<Defect4Epoch>();
			}
			return !(characterId == ModelDb.GetId<Necrobinder>()) || SaveManager.Instance.IsEpochRevealed<Necrobinder4Epoch>();
		}

		/// <summary>
		/// Updates the preferred Ascension.
		/// </summary>
		// Token: 0x060048B6 RID: 18614 RVA: 0x00224554 File Offset: 0x00222754
		private void UpdatePreferredAscension()
		{
			if (this.GameMode == GameMode.Daily)
			{
				return;
			}
			if (this.NetService.Type == NetGameType.Singleplayer)
			{
				if (this.Players.Count == 0)
				{
					return;
				}
				CharacterStats orCreateCharacterStats = SaveManager.Instance.Progress.GetOrCreateCharacterStats(this.LocalPlayer.character.Id);
				if (orCreateCharacterStats.MaxAscension == 0 && orCreateCharacterStats.Id != ModelDb.Character<RandomCharacter>().Id)
				{
					return;
				}
				if (orCreateCharacterStats.PreferredAscension != this.Ascension)
				{
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Setting preferred Ascension for ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this.LocalPlayer.character.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" to ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.Ascension);
					logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					orCreateCharacterStats.PreferredAscension = this.Ascension;
					SaveManager.Instance.SaveProgressFile();
					return;
				}
			}
			else if (this.NetService.Type == NetGameType.Host)
			{
				ProgressState progress = SaveManager.Instance.Progress;
				if (progress.PreferredMultiplayerAscension != this.Ascension)
				{
					Logger logger2 = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Setting preferred multiplayer ascension to ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.Ascension);
					logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					progress.PreferredMultiplayerAscension = this.Ascension;
					SaveManager.Instance.SaveProgressFile();
				}
			}
		}

		/// <summary>
		/// Sets the character chosen by the local player and replicates that to all peers.
		/// </summary>
		/// <param name="character"></param>
		// Token: 0x060048B7 RID: 18615 RVA: 0x002246C0 File Offset: 0x002228C0
		public void SetLocalCharacter(CharacterModel character)
		{
			this.ChangeCharacter(this.NetService.NetId, character, false);
			LobbyPlayerChangedCharacterMessage lobbyPlayerChangedCharacterMessage = default(LobbyPlayerChangedCharacterMessage);
			lobbyPlayerChangedCharacterMessage.character = character;
			this.NetService.SendMessage<LobbyPlayerChangedCharacterMessage>(lobbyPlayerChangedCharacterMessage);
			this.SetSingleplayerAscensionAfterCharacterChanged(character.Id);
		}

		/// <summary>
		/// Sets the seed to use for the run.
		/// This can only be called as host or singleplayer. Calling it on the client will throw an exception and have no
		/// effect. The seed is synced to the clients for display use, but the final seed that is used is sent in the
		/// <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyBeginRunMessage" />.
		/// </summary>
		// Token: 0x060048B8 RID: 18616 RVA: 0x00224708 File Offset: 0x00222908
		[NullableContext(2)]
		public void SetSeed(string seed)
		{
			NetGameType type = this.NetService.Type;
			if (type - NetGameType.Singleplayer > 1)
			{
				throw new InvalidOperationException("Can only be called on host or singleplayer");
			}
			this.Seed = seed;
			LobbySeedChangedMessage lobbySeedChangedMessage = default(LobbySeedChangedMessage);
			lobbySeedChangedMessage.seed = seed;
			this.NetService.SendMessage<LobbySeedChangedMessage>(lobbySeedChangedMessage);
			this.LobbyListener.SeedChanged();
		}

		/// <summary>
		/// Sets the modifiers to use for the run.
		/// This can only be called as host or singleplayer. Calling it on the client will throw an exception and have no
		/// effect. The seed is synced to the clients for display use, but the final seed that is used is sent in the
		/// <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyBeginRunMessage" />.
		/// </summary>
		// Token: 0x060048B9 RID: 18617 RVA: 0x0022476C File Offset: 0x0022296C
		public void SetModifiers(IReadOnlyCollection<ModifierModel> modifiers)
		{
			NetGameType type = this.NetService.Type;
			if (type - NetGameType.Singleplayer > 1)
			{
				throw new InvalidOperationException("Can only be called on host or singleplayer");
			}
			if (this._isBeginningRun)
			{
				Log.Warn("Tried to change modifiers while run was already starting! Ignoring", 2);
				return;
			}
			this._modifiers.Clear();
			this._modifiers.AddRange(modifiers);
			LobbyModifiersChangedMessage lobbyModifiersChangedMessage = default(LobbyModifiersChangedMessage);
			lobbyModifiersChangedMessage.modifiers = modifiers.Select<ModifierModel, SerializableModifier>((ModifierModel m) => m.ToSerializable()).ToList<SerializableModifier>();
			this.NetService.SendMessage<LobbyModifiersChangedMessage>(lobbyModifiersChangedMessage);
			this.LobbyListener.ModifiersChanged();
		}

		/// <summary>
		/// Sets the local player to be ready or unready and syncs the state to all peers.
		/// This is called for both Singleplayer and Multiplayer (singleplayer uses a local lobby).
		/// </summary>
		/// <param name="ready"></param>
		// Token: 0x060048BA RID: 18618 RVA: 0x0022481C File Offset: 0x00222A1C
		public void SetReady(bool ready)
		{
			int num = this.Players.FindIndex((LobbyPlayer p) => p.id == this.NetService.NetId);
			if (num < 0)
			{
				throw new InvalidOperationException("Tried to set local player ready, but they are not in the list of players in the lobby!");
			}
			if (this._isBeginningRun)
			{
				Log.Warn("Tried to set ready while run was already starting! Ignoring", 2);
				return;
			}
			LobbyPlayer lobbyPlayer = this.Players[num];
			lobbyPlayer.isReady = ready;
			this.Players[num] = lobbyPlayer;
			LobbyPlayerSetReadyMessage lobbyPlayerSetReadyMessage = default(LobbyPlayerSetReadyMessage);
			lobbyPlayerSetReadyMessage.ready = ready;
			this.NetService.SendMessage<LobbyPlayerSetReadyMessage>(lobbyPlayerSetReadyMessage);
			this.LobbyListener.PlayerChanged(this.LocalPlayer, false);
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Local player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.LocalPlayer.id);
			defaultInterpolatedStringHandler.AppendLiteral(" is ready");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.BeginRunForAllPlayersIfAllReady();
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x00224904 File Offset: 0x00222B04
		private void BeginRunForAllPlayersIfAllReady()
		{
			if (!this.IsAboutToBeginGame())
			{
				return;
			}
			NetGameType type = this.NetService.Type;
			if (type - NetGameType.Singleplayer > 1)
			{
				return;
			}
			NGame instance = NGame.Instance;
			string text;
			if (((instance != null) ? instance.DebugSeedOverride : null) != null)
			{
				text = NGame.Instance.DebugSeedOverride;
			}
			else if (this.Seed != null)
			{
				text = SeedHelper.CanonicalizeSeed(this.Seed);
			}
			else
			{
				text = SeedHelper.GetRandomSeed(null, 12);
			}
			this.BeginRunForAllPlayers(text, this._modifiers);
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x00224984 File Offset: 0x00222B84
		public bool IsAboutToBeginGame()
		{
			if (this._connectingPlayers.Count > 0)
			{
				return false;
			}
			if (this.NetService.Type.IsMultiplayer() && this.Players.Count == 1)
			{
				return false;
			}
			return this.Players.All<LobbyPlayer>((LobbyPlayer p) => p.isReady);
		}

		/// <summary>
		/// Sets the ascension level. Should only be called on the host.
		/// </summary>
		// Token: 0x060048BD RID: 18621 RVA: 0x002249F4 File Offset: 0x00222BF4
		public void SyncAscensionChange(int ascension)
		{
			if (this.NetService.Type == NetGameType.Client)
			{
				throw new InvalidOperationException("Client attempted to change ascension level!");
			}
			if (this.Ascension == ascension)
			{
				return;
			}
			if (this._isBeginningRun)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to set ascension to ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(ascension);
				defaultInterpolatedStringHandler.AppendLiteral(" while run was already starting! Ignoring");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			this.Ascension = ascension;
			LobbyAscensionChangedMessage lobbyAscensionChangedMessage = default(LobbyAscensionChangedMessage);
			lobbyAscensionChangedMessage.ascension = ascension;
			this.NetService.SendMessage<LobbyAscensionChangedMessage>(lobbyAscensionChangedMessage);
			this.UpdatePreferredAscension();
			this.LobbyListener.AscensionChanged();
		}

		// Token: 0x060048BE RID: 18622 RVA: 0x00224A98 File Offset: 0x00222C98
		private LobbyPlayer? TryAddPlayerInFirstAvailableSlot(SerializableUnlockState unlockState, int maxAscensionUnlocked, ulong playerId)
		{
			int num = -1;
			int i;
			int j;
			for (i = 0; i < this.MaxPlayers; i = j + 1)
			{
				int num2 = this.Players.FindIndex((LobbyPlayer p) => p.slotId == i);
				if (num2 < 0)
				{
					num = i;
					break;
				}
				j = i;
			}
			if (num < 0)
			{
				return null;
			}
			LobbyPlayer lobbyPlayer = new LobbyPlayer
			{
				character = ModelDb.Character<Ironclad>(),
				id = playerId,
				slotId = num,
				maxMultiplayerAscensionUnlocked = maxAscensionUnlocked,
				unlockState = unlockState
			};
			this.Players.Add(lobbyPlayer);
			return new LobbyPlayer?(lobbyPlayer);
		}

		// Token: 0x060048BF RID: 18623 RVA: 0x00224B54 File Offset: 0x00222D54
		private void OnConnectedToClientAsHost(ulong playerId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Client ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			defaultInterpolatedStringHandler.AppendLiteral(" connected. Sending initial game info message");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			InitialGameInfoMessage initialGameInfoMessage = InitialGameInfoMessage.Basic();
			initialGameInfoMessage.sessionState = RunSessionState.InLobby;
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
				((NetHostGameService)this.NetService).DisconnectClient(playerId, NetError.RunInProgress, false);
				return;
			}
			if (this.Players.Count >= this.MaxPlayers)
			{
				initialGameInfoMessage.connectionFailureReason = new ConnectionFailureReason?(ConnectionFailureReason.LobbyFull);
				this.NetService.SendMessage<InitialGameInfoMessage>(initialGameInfoMessage, playerId);
				Logger logger3 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Client ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" connected but we are at maximum players!");
				logger3.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				((NetHostGameService)this.NetService).DisconnectClient(playerId, NetError.LobbyFull, false);
				return;
			}
			StartRunLobby.ConnectingPlayer connectingPlayer = new StartRunLobby.ConnectingPlayer
			{
				id = playerId,
				timeoutCancelToken = new CancellationTokenSource()
			};
			this._connectingPlayers.Add(connectingPlayer);
			this.NetService.SendMessage<InitialGameInfoMessage>(initialGameInfoMessage, playerId);
			TaskHelper.RunSafely(this.BeginHandshakeTimeout(connectingPlayer));
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x00224CF0 File Offset: 0x00222EF0
		private async Task BeginHandshakeTimeout(StartRunLobby.ConnectingPlayer connectingPlayer)
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

		// Token: 0x060048C1 RID: 18625 RVA: 0x00224D3C File Offset: 0x00222F3C
		private void OnDisconnectedFromClientAsHost(ulong playerId, NetErrorInfo info)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Client ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			defaultInterpolatedStringHandler.AppendLiteral(" disconnected, reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(info.GetReason());
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.RemoveConnectingPlayer(playerId);
			int num = this.Players.FindIndex((LobbyPlayer p) => p.id == playerId);
			if (num < 0)
			{
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(82, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" not found in players list. Assuming they disconnected during the handshake");
				logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			LobbyPlayer lobbyPlayer = this.Players[num];
			PlayerLeftMessage playerLeftMessage = new PlayerLeftMessage
			{
				playerId = playerId
			};
			this.NetService.SendMessage<PlayerLeftMessage>(playerLeftMessage);
			this.Players.RemoveAt(num);
			this.InputSynchronizer.OnPlayerDisconnected(lobbyPlayer.id);
			this.LobbyListener.RemotePlayerDisconnected(lobbyPlayer);
			Action<LobbyPlayer> playerDisconnected = this.PlayerDisconnected;
			if (playerDisconnected != null)
			{
				playerDisconnected(lobbyPlayer);
			}
			this.UpdateMaxMultiplayerAscension();
			this.BeginRunForAllPlayersIfAllReady();
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x00224E89 File Offset: 0x00223089
		private UnlockState GetUnlockState()
		{
			if (this.GameMode == GameMode.Daily)
			{
				return UnlockState.all;
			}
			return new UnlockState(this.Players.Select<LobbyPlayer, UnlockState>((LobbyPlayer p) => UnlockState.FromSerializable(p.unlockState)));
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x00224ECC File Offset: 0x002230CC
		private void RemoveConnectingPlayer(ulong playerId)
		{
			for (int i = 0; i < this._connectingPlayers.Count; i++)
			{
				if (this._connectingPlayers[i].id == playerId)
				{
					this._connectingPlayers[i].timeoutCancelToken.Cancel();
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Cancel handshake timeout for ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
					logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this._connectingPlayers.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x00224F5C File Offset: 0x0022315C
		private void OnDisconnected(NetErrorInfo info)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Disconnected from host, reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(info.GetReason());
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.LobbyListener.LocalPlayerDisconnected(info);
		}

		// Token: 0x04001C84 RID: 7300
		private readonly Logger _logger;

		// Token: 0x04001C85 RID: 7301
		private readonly List<StartRunLobby.ConnectingPlayer> _connectingPlayers = new List<StartRunLobby.ConnectingPlayer>();

		// Token: 0x04001C86 RID: 7302
		private bool _isBeginningRun;

		// Token: 0x04001C90 RID: 7312
		private readonly List<ModifierModel> _modifiers = new List<ModifierModel>();

		// Token: 0x020019E8 RID: 6632
		[NullableContext(0)]
		private struct ConnectingPlayer : IEquatable<StartRunLobby.ConnectingPlayer>
		{
			// Token: 0x0600ABC9 RID: 43977 RVA: 0x00363D52 File Offset: 0x00361F52
			public bool Equals(StartRunLobby.ConnectingPlayer other)
			{
				return this.id == other.id && this.timeoutCancelToken.Equals(other.timeoutCancelToken);
			}

			// Token: 0x0600ABCA RID: 43978 RVA: 0x00363D78 File Offset: 0x00361F78
			[NullableContext(2)]
			public override bool Equals(object obj)
			{
				if (obj is StartRunLobby.ConnectingPlayer)
				{
					StartRunLobby.ConnectingPlayer connectingPlayer = (StartRunLobby.ConnectingPlayer)obj;
					return this.Equals(connectingPlayer);
				}
				return false;
			}

			// Token: 0x0600ABCB RID: 43979 RVA: 0x00363D9D File Offset: 0x00361F9D
			public override int GetHashCode()
			{
				return HashCode.Combine<ulong, CancellationTokenSource>(this.id, this.timeoutCancelToken);
			}

			// Token: 0x04006640 RID: 26176
			public ulong id;

			// Token: 0x04006641 RID: 26177
			[Nullable(1)]
			public CancellationTokenSource timeoutCancelToken;
		}

		// Token: 0x020019E9 RID: 6633
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04006642 RID: 26178
			[Nullable(0)]
			public static Func<SerializableModifier, ModifierModel> <0>__FromSerializable;
		}
	}
}
