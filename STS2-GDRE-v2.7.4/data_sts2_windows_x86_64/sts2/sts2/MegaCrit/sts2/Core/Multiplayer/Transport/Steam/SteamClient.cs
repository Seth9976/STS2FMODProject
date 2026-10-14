using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.Steam
{
	// Token: 0x02000437 RID: 1079
	[NullableContext(1)]
	[Nullable(0)]
	public class SteamClient : NetClient
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x0600447B RID: 17531 RVA: 0x00211105 File Offset: 0x0020F305
		public override bool IsConnected
		{
			get
			{
				return this._isConnected;
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600447C RID: 17532 RVA: 0x0021110D File Offset: 0x0020F30D
		public override ulong NetId
		{
			get
			{
				return SteamUser.GetSteamID().m_SteamID;
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x0600447D RID: 17533 RVA: 0x00211119 File Offset: 0x0020F319
		public override ulong HostNetId
		{
			get
			{
				return this._hostNetId.m_SteamID;
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600447E RID: 17534 RVA: 0x00211126 File Offset: 0x0020F326
		public CSteamID? LobbyId
		{
			get
			{
				return this._lobbyId;
			}
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x0021112E File Offset: 0x0020F32E
		public SteamClient(INetClientHandler handler)
			: base(handler)
		{
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x00211148 File Offset: 0x0020F348
		public Task<NetErrorInfo?> ConnectToLobbyOwnedByFriend(ulong steamPlayerId, CancellationToken cancelToken = default(CancellationToken))
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Attempting to connect to lobby. Our player id: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(". Other player's id: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(steamPlayerId);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			FriendGameInfo_t friendGameInfo_t;
			if (!SteamFriends.GetFriendGamePlayed(new CSteamID(steamPlayerId), ref friendGameInfo_t))
			{
				Log.Warn("Tried to join a friend's Steam game, but they're not playing any game. Likely a stale invite", 2);
				return Task.FromResult<NetErrorInfo?>(new NetErrorInfo?(new NetErrorInfo(NetError.InvalidJoin, false)));
			}
			if (friendGameInfo_t.m_gameID != new CGameID(2868840UL) || friendGameInfo_t.m_steamIDLobby.m_SteamID == 0UL)
			{
				Log.Warn("Tried to join a friend's Steam game, but they are not playing STS2 or they are not in a lobby. Likely a stale invite", 2);
				return Task.FromResult<NetErrorInfo?>(new NetErrorInfo?(new NetErrorInfo(NetError.InvalidJoin, false)));
			}
			return this.ConnectToLobby(friendGameInfo_t.m_steamIDLobby.m_SteamID, cancelToken);
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x00211230 File Offset: 0x0020F430
		public Task<NetErrorInfo?> ConnectToLobby(ulong lobbyId, CancellationToken cancelToken = default(CancellationToken))
		{
			SteamClient.<ConnectToLobby>d__18 <ConnectToLobby>d__;
			<ConnectToLobby>d__.<>t__builder = AsyncTaskMethodBuilder<NetErrorInfo?>.Create();
			<ConnectToLobby>d__.<>4__this = this;
			<ConnectToLobby>d__.lobbyId = lobbyId;
			<ConnectToLobby>d__.cancelToken = cancelToken;
			<ConnectToLobby>d__.<>1__state = -1;
			<ConnectToLobby>d__.<>t__builder.Start<SteamClient.<ConnectToLobby>d__18>(ref <ConnectToLobby>d__);
			return <ConnectToLobby>d__.<>t__builder.Task;
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x00211284 File Offset: 0x0020F484
		private void OnNetStatusChanged(SteamNetConnectionStatusChangedCallback_t data)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Connection status changed: ");
			defaultInterpolatedStringHandler.AppendFormatted<ESteamNetworkingConnectionState>(data.m_eOldState);
			defaultInterpolatedStringHandler.AppendLiteral(" -> ");
			defaultInterpolatedStringHandler.AppendFormatted<ESteamNetworkingConnectionState>(data.m_info.m_eState);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (data.m_info.m_eState == 3)
			{
				this._logger.Debug("Steam connection accepted.", 1);
				if (this._connectingTaskCompletionSource == null)
				{
					this._logger.Error("Connection was accepted while we were not waiting for it!", 1);
					this.DisconnectFromHostInternal(SteamDisconnectionReason.InternalError, "Not Connecting", true, false);
					return;
				}
				this._connectingTaskCompletionSource.SetResult(new SteamClient.ConnectionResult
				{
					connection = new HSteamNetConnection?(data.m_hConn)
				});
				return;
			}
			else
			{
				if (data.m_info.m_eState == 5)
				{
					Logger logger2 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Steam connection closed because of problem. Reason: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(data.m_info.m_eEndReason);
					defaultInterpolatedStringHandler.AppendLiteral(", ");
					defaultInterpolatedStringHandler.AppendFormatted(data.m_info.m_szEndDebug);
					logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this.HandleDisconnection((SteamDisconnectionReason)data.m_info.m_eEndReason, data.m_info.m_szEndDebug);
					return;
				}
				if (data.m_info.m_eState == 4)
				{
					Logger logger3 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Steam connection closed by host. Reason: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(data.m_info.m_eEndReason);
					defaultInterpolatedStringHandler.AppendLiteral(", ");
					defaultInterpolatedStringHandler.AppendFormatted(data.m_info.m_szEndDebug);
					logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this.HandleDisconnection((SteamDisconnectionReason)data.m_info.m_eEndReason, data.m_info.m_szEndDebug);
				}
				return;
			}
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x00211464 File Offset: 0x0020F664
		private void HandleDisconnection(SteamDisconnectionReason reason, string debugReason)
		{
			if (this._connectingTaskCompletionSource != null)
			{
				this._connectingTaskCompletionSource.SetResult(new SteamClient.ConnectionResult
				{
					disconnectionReason = new SteamDisconnectionReason?(reason),
					debugReason = debugReason
				});
				return;
			}
			this.DisconnectFromHostInternal(reason, debugReason, true, false);
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x002114AD File Offset: 0x0020F6AD
		public override void Update()
		{
			SteamUtil.ProcessMessages(this._conn.Value, this._handler, this._logger);
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x002114CC File Offset: 0x0020F6CC
		public override void SendMessageToHost(byte[] bytes, int length, NetTransferMode mode, int channel = 0)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Sending ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(length);
			defaultInterpolatedStringHandler.AppendLiteral(" bytes to host");
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			GCHandle gchandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			try
			{
				long num;
				EResult eresult = SteamNetworkingSockets.SendMessageToConnection(this._conn.Value, gchandle.AddrOfPinnedObject(), (uint)length, SteamUtil.FlagsFromMode(mode), ref num);
				if (eresult != 1)
				{
					Logger logger2 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to send message length ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(length);
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

		// Token: 0x06004486 RID: 17542 RVA: 0x002115A4 File Offset: 0x0020F7A4
		public override void DisconnectFromHost(NetError reason, bool now = false)
		{
			this.DisconnectFromHostInternal(reason.ToSteam(), string.Empty, now, true);
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x002115B9 File Offset: 0x0020F7B9
		private void CleanupLobby()
		{
			SteamMatchmaking.LeaveLobby(this._lobbyId.Value);
			this._lobbyId = null;
			Callback<SteamNetConnectionStatusChangedCallback_t> netStatusChangedCallback = this._netStatusChangedCallback;
			if (netStatusChangedCallback != null)
			{
				netStatusChangedCallback.Dispose();
			}
			this._netStatusChangedCallback = null;
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x002115EF File Offset: 0x0020F7EF
		[NullableContext(2)]
		private void CleanupConnection(int closeReason, string debugReason)
		{
			SteamNetworkingSockets.CloseConnection(this._conn.Value, closeReason, debugReason, false);
			this._conn = null;
			this._connectingTaskCompletionSource = null;
			this.CleanupLobby();
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x00211620 File Offset: 0x0020F820
		[NullableContext(2)]
		private void DisconnectFromHostInternal(SteamDisconnectionReason reason, string debugReason, bool now, bool selfInitiated)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Disconnecting from host (now: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(now);
			defaultInterpolatedStringHandler.AppendLiteral(" reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<SteamDisconnectionReason>(reason);
			defaultInterpolatedStringHandler.AppendLiteral(" debug: ");
			defaultInterpolatedStringHandler.AppendFormatted(debugReason);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			SteamNetworkingSockets.CloseConnection(this._conn.Value, (int)reason, debugReason, !now);
			SteamMatchmaking.LeaveLobby(this._lobbyId.Value);
			ulong steamID = this._hostNetId.m_SteamID;
			this._conn = null;
			this._lobbyId = null;
			this._connectingTaskCompletionSource = null;
			Callback<SteamNetConnectionStatusChangedCallback_t> netStatusChangedCallback = this._netStatusChangedCallback;
			if (netStatusChangedCallback != null)
			{
				netStatusChangedCallback.Dispose();
			}
			this._netStatusChangedCallback = null;
			this._isConnected = false;
			this._hostNetId = CSteamID.Nil;
			this._handler.OnDisconnectedFromHost(steamID, new NetErrorInfo(reason, debugReason, selfInitiated));
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x00211723 File Offset: 0x0020F923
		private void CancelConnection()
		{
			TaskCompletionSource<SteamClient.ConnectionResult> connectingTaskCompletionSource = this._connectingTaskCompletionSource;
			if (connectingTaskCompletionSource != null)
			{
				connectingTaskCompletionSource.SetCanceled();
			}
			if (this._conn != null)
			{
				this.DisconnectFromHost(NetError.Quit, false);
				return;
			}
			if (this._lobbyId != null)
			{
				this.CleanupLobby();
			}
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x00211760 File Offset: 0x0020F960
		[NullableContext(2)]
		public override string GetRawLobbyIdentifier()
		{
			if (this._lobbyId == null)
			{
				return null;
			}
			return this._lobbyId.GetValueOrDefault().m_SteamID.ToString();
		}

		// Token: 0x04001A8C RID: 6796
		private CSteamID? _lobbyId;

		// Token: 0x04001A8D RID: 6797
		private CSteamID _hostNetId;

		// Token: 0x04001A8E RID: 6798
		private HSteamNetConnection? _conn;

		// Token: 0x04001A8F RID: 6799
		private bool _isConnected;

		// Token: 0x04001A90 RID: 6800
		[Nullable(2)]
		private TaskCompletionSource<SteamClient.ConnectionResult> _connectingTaskCompletionSource;

		// Token: 0x04001A91 RID: 6801
		[Nullable(2)]
		private Callback<SteamNetConnectionStatusChangedCallback_t> _netStatusChangedCallback;

		// Token: 0x04001A92 RID: 6802
		private readonly Logger _logger = new Logger("SteamClient", LogType.Network);

		// Token: 0x02001996 RID: 6550
		[NullableContext(0)]
		private struct ConnectionResult
		{
			// Token: 0x040064FA RID: 25850
			public HSteamNetConnection? connection;

			// Token: 0x040064FB RID: 25851
			public SteamDisconnectionReason? disconnectionReason;

			// Token: 0x040064FC RID: 25852
			[Nullable(2)]
			public string debugReason;
		}
	}
}
