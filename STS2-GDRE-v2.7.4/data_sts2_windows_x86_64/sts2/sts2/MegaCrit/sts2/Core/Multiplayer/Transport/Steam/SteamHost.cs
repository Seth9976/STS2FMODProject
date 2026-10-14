using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.Steam
{
	// Token: 0x02000438 RID: 1080
	[NullableContext(1)]
	[Nullable(0)]
	public class SteamHost : NetHost
	{
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x0600448C RID: 17548 RVA: 0x00211790 File Offset: 0x0020F990
		public override bool IsConnected
		{
			get
			{
				return this._isConnected;
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600448D RID: 17549 RVA: 0x00211798 File Offset: 0x0020F998
		public override ulong NetId
		{
			get
			{
				return SteamUser.GetSteamID().m_SteamID;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x0600448E RID: 17550 RVA: 0x002117A4 File Offset: 0x0020F9A4
		public override IEnumerable<ulong> ConnectedPeerIds
		{
			get
			{
				return this._connections.Select<SteamHost.ClientConnection, ulong>((SteamHost.ClientConnection c) => c.netId.GetSteamID64());
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x0600448F RID: 17551 RVA: 0x002117D0 File Offset: 0x0020F9D0
		public CSteamID? LobbyId
		{
			get
			{
				return this._lobbyId;
			}
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x002117D8 File Offset: 0x0020F9D8
		public SteamHost(INetHostHandler handler)
			: base(handler)
		{
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x00211800 File Offset: 0x0020FA00
		public async Task<NetErrorInfo?> StartHost(int maxPlayers)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Initializing Steam host. Our player id: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.NetId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			SteamAPICall_t steamAPICall_t = SteamMatchmaking.CreateLobby(1, maxPlayers);
			NetErrorInfo? netErrorInfo;
			using (SteamCallResult<LobbyCreated_t> callResult = new SteamCallResult<LobbyCreated_t>(steamAPICall_t, SteamInitializer.DisconnectToken))
			{
				LobbyCreated_t lobbyCreated_t = await callResult.Task;
				LobbyCreated_t lobbyCreated_t2 = lobbyCreated_t;
				if (lobbyCreated_t2.m_eResult != 1)
				{
					Logger logger2 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Error creating steam lobby! ");
					defaultInterpolatedStringHandler.AppendFormatted<EResult>(lobbyCreated_t2.m_eResult);
					logger2.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					netErrorInfo = new NetErrorInfo?(new NetErrorInfo(lobbyCreated_t2.m_eResult));
				}
				else
				{
					this._lobbyId = new CSteamID?(new CSteamID(lobbyCreated_t2.m_ulSteamIDLobby));
					SteamNetworkingConfigValue_t[] array = new SteamNetworkingConfigValue_t[2];
					SteamNetworkingConfigValue_t[] array2 = array;
					int num = 0;
					SteamNetworkingConfigValue_t steamNetworkingConfigValue_t = default(SteamNetworkingConfigValue_t);
					steamNetworkingConfigValue_t.m_eValue = 24;
					steamNetworkingConfigValue_t.m_eDataType = 1;
					SteamNetworkingConfigValue_t.OptionValue optionValue = default(SteamNetworkingConfigValue_t.OptionValue);
					optionValue.m_int32 = 20000;
					steamNetworkingConfigValue_t.m_val = optionValue;
					array2[num] = steamNetworkingConfigValue_t;
					SteamNetworkingConfigValue_t[] array3 = array;
					int num2 = 1;
					steamNetworkingConfigValue_t = default(SteamNetworkingConfigValue_t);
					steamNetworkingConfigValue_t.m_eValue = 25;
					steamNetworkingConfigValue_t.m_eDataType = 1;
					optionValue = default(SteamNetworkingConfigValue_t.OptionValue);
					optionValue.m_int32 = 20000;
					steamNetworkingConfigValue_t.m_val = optionValue;
					array3[num2] = steamNetworkingConfigValue_t;
					this._socket = SteamNetworkingSockets.CreateListenSocketP2P(0, 0, array);
					this._netStatusChangedCallback = new Callback<SteamNetConnectionStatusChangedCallback_t>(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate(this.OnNetStatusChanged), false);
					this._isConnected = true;
					netErrorInfo = null;
				}
			}
			return netErrorInfo;
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0021184C File Offset: 0x0020FA4C
		public override void Update()
		{
			SteamHost._connectionsCache.Clear();
			SteamHost._connectionsCache.AddRange(this._connections);
			foreach (SteamHost.ClientConnection clientConnection in SteamHost._connectionsCache)
			{
				SteamUtil.ProcessMessages(clientConnection.conn, this._handler, this._logger);
			}
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x002118C8 File Offset: 0x0020FAC8
		public override void SetHostIsClosed(bool isClosed)
		{
			SteamMatchmaking.SetLobbyType(this._lobbyId.Value, isClosed ? 0 : 1);
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x002118E4 File Offset: 0x0020FAE4
		private void OnNetStatusChanged(SteamNetConnectionStatusChangedCallback_t data)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Connection status changed: (");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(data.m_info.m_identityRemote.GetSteamID64());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<ESteamNetworkingConnectionState>(data.m_eOldState);
			defaultInterpolatedStringHandler.AppendLiteral(" -> ");
			defaultInterpolatedStringHandler.AppendFormatted<ESteamNetworkingConnectionState>(data.m_info.m_eState);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (data.m_info.m_eState == 1)
			{
				if (!this.IsInLobby(data.m_info.m_identityRemote))
				{
					Logger logger2 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Player with steam id ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(data.m_info.m_identityRemote.GetSteamID64());
					defaultInterpolatedStringHandler.AppendLiteral(" attempted to join the game, but they are not in the lobby (id ");
					defaultInterpolatedStringHandler.AppendFormatted<CSteamID>(this._lobbyId.Value);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this.CloseConnectionAndRemove(data.m_hConn, NetError.TryAgainLater.ToSteam(), "Player is not in the lobby!", true, false);
					return;
				}
				Logger logger3 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Accepting new connection with user ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(data.m_info.m_identityRemote.GetSteamID64());
				logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				EResult eresult = SteamNetworkingSockets.AcceptConnection(data.m_hConn);
				if (eresult != 1)
				{
					Logger logger4 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to accept connection with user ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(data.m_info.m_identityRemote.GetSteamID64());
					defaultInterpolatedStringHandler.AppendLiteral(" but it returned result ");
					defaultInterpolatedStringHandler.AppendFormatted<EResult>(eresult);
					defaultInterpolatedStringHandler.AppendLiteral("!");
					logger4.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					HSteamNetConnection hConn = data.m_hConn;
					SteamDisconnectionReason steamDisconnectionReason = SteamDisconnectionReason.AppInternalError;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Connection accept failure: ");
					defaultInterpolatedStringHandler.AppendFormatted<EResult>(eresult);
					this.CloseConnectionAndRemove(hConn, steamDisconnectionReason, defaultInterpolatedStringHandler.ToStringAndClear(), true, false);
					return;
				}
			}
			else
			{
				if (data.m_info.m_eState == 3)
				{
					this._connections.Add(new SteamHost.ClientConnection
					{
						conn = data.m_hConn,
						netId = data.m_info.m_identityRemote
					});
					this._handler.OnPeerConnected(data.m_info.m_identityRemote.GetSteamID64());
					return;
				}
				if (data.m_info.m_eState == 5)
				{
					Logger logger5 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Steam connection closed because of problem. Reason: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(data.m_info.m_eEndReason);
					defaultInterpolatedStringHandler.AppendLiteral(", ");
					defaultInterpolatedStringHandler.AppendFormatted(data.m_info.m_szEndDebug);
					logger5.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this.CloseConnectionAndRemove(data.m_hConn, (SteamDisconnectionReason)data.m_info.m_eEndReason, data.m_info.m_szEndDebug, true, false);
					return;
				}
				if (data.m_info.m_eState == 4)
				{
					Logger logger6 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Steam connection closed by peer. Reason: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(data.m_info.m_eEndReason);
					defaultInterpolatedStringHandler.AppendLiteral(", ");
					defaultInterpolatedStringHandler.AppendFormatted(data.m_info.m_szEndDebug);
					logger6.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this.CloseConnectionAndRemove(data.m_hConn, (SteamDisconnectionReason)data.m_info.m_eEndReason, data.m_info.m_szEndDebug, true, false);
				}
			}
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x00211C80 File Offset: 0x0020FE80
		public override void SendMessageToClient(ulong peerId, byte[] bytes, int length, NetTransferMode mode, int channel = 0)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Sending ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(length);
			defaultInterpolatedStringHandler.AppendLiteral(" bytes to client ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(peerId);
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			default(SteamNetworkingIdentity).SetSteamID64(peerId);
			SteamHost.ClientConnection? connectionForNetId = this.GetConnectionForNetId(peerId);
			if (connectionForNetId == null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Could not find connection for peer ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(peerId);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			GCHandle gchandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			try
			{
				long num;
				EResult eresult = SteamNetworkingSockets.SendMessageToConnection(connectionForNetId.Value.conn, gchandle.AddrOfPinnedObject(), (uint)length, SteamUtil.FlagsFromMode(mode), ref num);
				if (eresult != 1)
				{
					Logger logger2 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to send message length ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(length);
					defaultInterpolatedStringHandler.AppendLiteral(" to peer ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(peerId);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<EResult>(eresult);
					logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				}
			}
			finally
			{
				gchandle.Free();
			}
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x00211DD0 File Offset: 0x0020FFD0
		public override void SendMessageToAll(byte[] bytes, int length, NetTransferMode mode, int channel = 0)
		{
			foreach (SteamHost.ClientConnection clientConnection in this._connections)
			{
				SteamNetworkingIdentity netId = clientConnection.netId;
				this.SendMessageToClient(netId.GetSteamID64(), bytes, length, mode, channel);
			}
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x00211E38 File Offset: 0x00210038
		private SteamHost.ClientConnection? GetConnectionForNetId(ulong peerId)
		{
			foreach (SteamHost.ClientConnection clientConnection in this._connections)
			{
				SteamNetworkingIdentity netId = clientConnection.netId;
				if (netId.GetSteamID64() == peerId)
				{
					return new SteamHost.ClientConnection?(clientConnection);
				}
			}
			return null;
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x00211EAC File Offset: 0x002100AC
		public override void DisconnectClient(ulong peerId, NetError reason, bool now = false)
		{
			HSteamNetConnection? hsteamNetConnection = null;
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Disconnecting peer ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(peerId);
			defaultInterpolatedStringHandler.AppendLiteral(", reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(reason);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			foreach (SteamHost.ClientConnection clientConnection in this._connections)
			{
				SteamNetworkingIdentity netId = clientConnection.netId;
				if (netId.GetSteamID64() == peerId)
				{
					hsteamNetConnection = new HSteamNetConnection?(clientConnection.conn);
					break;
				}
			}
			if (hsteamNetConnection != null)
			{
				this.CloseConnectionAndRemove(hsteamNetConnection.Value, reason.ToSteam(), null, now, true);
			}
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x00211F84 File Offset: 0x00210184
		[NullableContext(2)]
		private void CloseConnectionAndRemove(HSteamNetConnection conn, SteamDisconnectionReason reason, string debugReason, bool now, bool selfInitiated)
		{
			SteamNetworkingSockets.CloseConnection(conn, (int)reason, debugReason, !now);
			int num = this._connections.FindIndex((SteamHost.ClientConnection c) => c.conn == conn);
			if (num >= 0)
			{
				SteamHost.ClientConnection clientConnection = this._connections[num];
				this._connections.RemoveAt(num);
				this._handler.OnPeerDisconnected(clientConnection.netId.GetSteamID64(), new NetErrorInfo(reason, debugReason, selfInitiated));
			}
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x00212008 File Offset: 0x00210208
		public override void StopHost(NetError reason, bool now = false)
		{
			this._logger.Debug("Stopping host", 1);
			foreach (SteamHost.ClientConnection clientConnection in this._connections)
			{
				SteamNetworkingSockets.CloseConnection(clientConnection.conn, (int)reason.ToSteam(), null, !now);
			}
			this._connections.Clear();
			SteamMatchmaking.LeaveLobby(this._lobbyId.Value);
			if (now)
			{
				SteamNetworkingSockets.CloseListenSocket(this._socket);
			}
			else
			{
				TaskHelper.RunSafely(this.CloseSocketAfterDelay(this._socket));
			}
			this._lobbyId = null;
			this._isConnected = false;
			Callback<SteamNetConnectionStatusChangedCallback_t> netStatusChangedCallback = this._netStatusChangedCallback;
			if (netStatusChangedCallback != null)
			{
				netStatusChangedCallback.Dispose();
			}
			this._netStatusChangedCallback = null;
			this._handler.OnDisconnected(new NetErrorInfo(reason, true));
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x002120F8 File Offset: 0x002102F8
		private async Task CloseSocketAfterDelay(HSteamListenSocket socket)
		{
			await Task.Delay(1000);
			this._logger.Debug("Closing socket after delay.", 1);
			SteamNetworkingSockets.CloseListenSocket(socket);
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x00212144 File Offset: 0x00210344
		private bool IsInLobby(SteamNetworkingIdentity id)
		{
			CSteamID steamID = id.GetSteamID();
			if (steamID == CSteamID.Nil)
			{
				return false;
			}
			int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(this._lobbyId.Value);
			for (int i = 0; i < numLobbyMembers; i++)
			{
				CSteamID lobbyMemberByIndex = SteamMatchmaking.GetLobbyMemberByIndex(this._lobbyId.Value, i);
				if (steamID == lobbyMemberByIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x002121A4 File Offset: 0x002103A4
		[NullableContext(2)]
		public override string GetRawLobbyIdentifier()
		{
			if (this._lobbyId == null)
			{
				return null;
			}
			return this._lobbyId.GetValueOrDefault().m_SteamID.ToString();
		}

		// Token: 0x04001A93 RID: 6803
		private static readonly List<SteamHost.ClientConnection> _connectionsCache = new List<SteamHost.ClientConnection>();

		// Token: 0x04001A94 RID: 6804
		private readonly Logger _logger = new Logger("SteamHost", LogType.Network);

		// Token: 0x04001A95 RID: 6805
		[Nullable(2)]
		private Callback<SteamNetConnectionStatusChangedCallback_t> _netStatusChangedCallback;

		// Token: 0x04001A96 RID: 6806
		private CSteamID? _lobbyId;

		// Token: 0x04001A97 RID: 6807
		private HSteamListenSocket _socket;

		// Token: 0x04001A98 RID: 6808
		private readonly List<SteamHost.ClientConnection> _connections = new List<SteamHost.ClientConnection>();

		// Token: 0x04001A99 RID: 6809
		private bool _isConnected;

		// Token: 0x02001998 RID: 6552
		[NullableContext(0)]
		private struct ClientConnection
		{
			// Token: 0x0400650B RID: 25867
			public HSteamNetConnection conn;

			// Token: 0x0400650C RID: 25868
			public SteamNetworkingIdentity netId;
		}
	}
}
