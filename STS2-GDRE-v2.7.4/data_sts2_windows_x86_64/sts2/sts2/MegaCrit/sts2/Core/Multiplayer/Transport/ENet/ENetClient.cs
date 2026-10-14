using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet
{
	// Token: 0x0200043B RID: 1083
	[NullableContext(1)]
	[Nullable(0)]
	public class ENetClient : NetClient
	{
		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060044A5 RID: 17573 RVA: 0x002123E4 File Offset: 0x002105E4
		public override bool IsConnected
		{
			get
			{
				return this._isConnected;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060044A6 RID: 17574 RVA: 0x002123EC File Offset: 0x002105EC
		public override ulong NetId
		{
			get
			{
				return this._netId;
			}
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060044A7 RID: 17575 RVA: 0x002123F4 File Offset: 0x002105F4
		public override ulong HostNetId
		{
			get
			{
				return 1UL;
			}
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x002123F8 File Offset: 0x002105F8
		public ENetClient(INetClientHandler handler)
			: base(handler)
		{
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x00212414 File Offset: 0x00210614
		public async Task<NetErrorInfo?> ConnectToHost(ulong netId, string ip, ushort port, CancellationToken cancelToken = default(CancellationToken))
		{
			this._connection = new ENetConnection();
			this._connection.CreateHost(32, 0, 0, 0);
			this._peer = this._connection.ConnectToHost(ip, (int)port, 0, 0);
			int timeoutTimer = 0;
			ENetServiceData? enetServiceData;
			while (!this._connection.TryService(out enetServiceData) || enetServiceData.Value.type != 1L)
			{
				await Task.Delay(100, cancelToken);
				NetErrorInfo? netErrorInfo;
				if (cancelToken.IsCancellationRequested)
				{
					this.DisconnectFromHost(NetError.CancelledJoin, false);
					this._logger.Warn("User cancelled join flow", 1);
					netErrorInfo = null;
				}
				else
				{
					timeoutTimer += 100;
					if (timeoutTimer <= 10000)
					{
						continue;
					}
					this._peer.Reset();
					this._logger.Error("Connection timed out!", 1);
					netErrorInfo = new NetErrorInfo?(new NetErrorInfo(NetError.Timeout, false));
				}
				return netErrorInfo;
			}
			if (this._peer.GetState() != 5L)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Connection to ");
				defaultInterpolatedStringHandler.AppendFormatted(ip);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(port);
				defaultInterpolatedStringHandler.AppendLiteral(" failed!");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return new NetErrorInfo?(new NetErrorInfo(NetError.UnknownNetworkError, false));
			}
			List<ENetServiceData> bufferedPackets = new List<ENetServiceData>();
			NetErrorInfo? netErrorInfo2 = await this.SendAndWaitForNetIdAck(netId, bufferedPackets, cancelToken);
			if (netErrorInfo2 != null)
			{
				this._peer.PeerDisconnect(0);
				return netErrorInfo2;
			}
			this._netId = netId;
			this._isConnected = true;
			this._handler.OnConnectedToHost();
			foreach (ENetServiceData enetServiceData2 in bufferedPackets)
			{
				this.HandleMessageReceived(enetServiceData2);
			}
			return null;
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x00212478 File Offset: 0x00210678
		private async Task<NetErrorInfo?> SendAndWaitForNetIdAck(ulong netId, List<ENetServiceData> bufferedPackets, CancellationToken cancelToken = default(CancellationToken))
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Sending handshake with net ID ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(netId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			ENetPacket enetPacket = ENetPacket.FromHandshakeRequest(new ENetHandshakeRequest
			{
				netId = netId
			});
			this._peer.Send(0, enetPacket.AllBytes, 1);
			bool receivedAck = false;
			int timeoutTimer = 0;
			while (!receivedAck)
			{
				await Task.Delay(100, cancelToken);
				NetErrorInfo? netErrorInfo;
				if (cancelToken.IsCancellationRequested)
				{
					this._logger.Warn("User cancelled join flow", 1);
					this.DisconnectFromHost(NetError.CancelledJoin, false);
					netErrorInfo = null;
				}
				else
				{
					ENetServiceData? enetServiceData;
					if (this._connection.TryService(out enetServiceData) && enetServiceData.Value.type == 3L)
					{
						ENetPacket enetPacket2 = new ENetPacket(enetServiceData.Value.packetData);
						if (enetPacket2.PacketType == ENetPacketType.ApplicationMessage)
						{
							bufferedPackets.Add(enetServiceData.Value);
							continue;
						}
						ENetHandshakeResponse enetHandshakeResponse = enetPacket2.AsHandshakeResponse();
						if (enetHandshakeResponse.netId != netId)
						{
							Logger logger2 = this._logger;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Received net ID (");
							defaultInterpolatedStringHandler.AppendFormatted<ulong>(enetHandshakeResponse.netId);
							defaultInterpolatedStringHandler.AppendLiteral(") during handshake that did not match ours!");
							logger2.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
							return new NetErrorInfo?(new NetErrorInfo(NetError.InternalError, false));
						}
						if (enetHandshakeResponse.status != ENetHandshakeStatus.Success)
						{
							Logger logger3 = this._logger;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Received non-success code during handshake (");
							defaultInterpolatedStringHandler.AppendFormatted<ENetHandshakeStatus>(enetHandshakeResponse.status);
							defaultInterpolatedStringHandler.AppendLiteral(")!");
							logger3.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
							return new NetErrorInfo?(new NetErrorInfo(NetError.Kicked, false));
						}
						receivedAck = true;
					}
					timeoutTimer += 100;
					if (timeoutTimer <= 10000)
					{
						continue;
					}
					this._logger.Error("Timed out waiting for handshake ack!", 1);
					this.DisconnectFromHost(NetError.Timeout, false);
					netErrorInfo = new NetErrorInfo?(new NetErrorInfo(NetError.Timeout, false));
				}
				return netErrorInfo;
			}
			return null;
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x002124D4 File Offset: 0x002106D4
		public override void Update()
		{
			this.AssertClientStarted();
			for (;;)
			{
				ENetPacketPeer peer = this._peer;
				if (peer == null || !peer.IsActive())
				{
					return;
				}
				ENetConnection connection = this._connection;
				ENetServiceData? enetServiceData;
				if (connection == null || !connection.TryService(out enetServiceData))
				{
					return;
				}
				ENetConnection.EventType type = enetServiceData.Value.type;
				ENetConnection.EventType eventType = type - -1L;
				if (eventType > 4L)
				{
					break;
				}
				switch (eventType)
				{
				case 0U:
				{
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Got error from ENetConnection! Error: ");
					defaultInterpolatedStringHandler.AppendFormatted<Error>(enetServiceData.Value.error);
					defaultInterpolatedStringHandler.AppendLiteral(" TODO: Expand me");
					logger.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					continue;
				}
				case 2U:
					this._logger.Debug("Received connect on client", 1);
					continue;
				case 3U:
				{
					Logger logger2 = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Received disconnect on client. Already disconnected: ");
					defaultInterpolatedStringHandler.AppendFormatted<bool>(!this._isConnected);
					logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					if (this._isConnected)
					{
						this._isConnected = false;
						this._handler.OnDisconnectedFromHost(this.HostNetId, new NetErrorInfo(NetError.UnknownNetworkError, false));
						continue;
					}
					continue;
				}
				case 4U:
					this.HandleMessageReceived(enetServiceData.Value);
					continue;
				}
				break;
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x00212638 File Offset: 0x00210838
		private void HandleMessageReceived(ENetServiceData data)
		{
			if (!this._isConnected)
			{
				return;
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received packet of length ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(data.packetData.Length);
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			ENetPacket enetPacket = new ENetPacket(data.packetData);
			if (enetPacket.PacketType == ENetPacketType.ApplicationMessage)
			{
				this._handler.OnPacketReceived(1UL, enetPacket.AsAppMessage(), data.mode, data.channel);
				return;
			}
			if (enetPacket.PacketType == ENetPacketType.Disconnection)
			{
				ENetDisconnection enetDisconnection = enetPacket.AsDisconnection();
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received disconnection packet with reason: ");
				defaultInterpolatedStringHandler.AppendFormatted<NetError>(enetDisconnection.reason);
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this._handler.OnDisconnectedFromHost(this.HostNetId, new NetErrorInfo(enetDisconnection.reason, false));
				this._isConnected = false;
				return;
			}
			Logger logger3 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Got unexpected packet of type ");
			defaultInterpolatedStringHandler.AppendFormatted<ENetPacketType>(enetPacket.PacketType);
			defaultInterpolatedStringHandler.AppendLiteral(" while we were connected to the host!");
			logger3.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x00212768 File Offset: 0x00210968
		public override void DisconnectFromHost(NetError reason, bool now = false)
		{
			if (now)
			{
				ENetPacketPeer peer = this._peer;
				if (peer != null)
				{
					peer.PeerDisconnectNow(0);
				}
			}
			else
			{
				ENetPacket enetPacket = ENetPacket.FromDisconnection(new ENetDisconnection
				{
					reason = reason
				});
				ENetPacketPeer peer2 = this._peer;
				if (peer2 != null)
				{
					peer2.Send(0, enetPacket.AllBytes, 8);
				}
				ENetPacketPeer peer3 = this._peer;
				if (peer3 != null)
				{
					peer3.PeerDisconnectLater(0);
				}
			}
			ENetConnection connection = this._connection;
			if (connection != null)
			{
				connection.Flush();
			}
			if (this._isConnected)
			{
				this._isConnected = false;
				this._handler.OnDisconnectedFromHost(this.HostNetId, new NetErrorInfo(reason, true));
				ENetConnection connection2 = this._connection;
				if (connection2 == null)
				{
					return;
				}
				connection2.Destroy();
			}
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x00212818 File Offset: 0x00210A18
		public override void SendMessageToHost(byte[] bytes, int length, NetTransferMode mode, int channel = 0)
		{
			if (!this._isConnected)
			{
				throw new InvalidOperationException("Tried to send message to host while disconnected!");
			}
			this.AssertClientStarted();
			ENetPacket enetPacket = ENetPacket.FromAppMessage(bytes, length);
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Sending packet of length ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(enetPacket.AllBytes.Length);
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			ENetPacketPeer peer = this._peer;
			if (peer == null)
			{
				return;
			}
			peer.Send(channel, enetPacket.AllBytes, ENetUtil.FlagsFromMode(mode));
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x0021289D File Offset: 0x00210A9D
		private void AssertClientStarted()
		{
			if (this._connection == null)
			{
				throw new InvalidOperationException("Must call ConnectToHost first and wait for connection!");
			}
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x002128B2 File Offset: 0x00210AB2
		[NullableContext(2)]
		public override string GetRawLobbyIdentifier()
		{
			return null;
		}

		// Token: 0x04001AA1 RID: 6817
		private const int _handshakeTimeoutMsec = 10000;

		// Token: 0x04001AA2 RID: 6818
		private const int _handshakeUpdateRateMsec = 100;

		// Token: 0x04001AA3 RID: 6819
		private readonly Logger _logger = new Logger("ENetClient", LogType.Network);

		// Token: 0x04001AA4 RID: 6820
		[Nullable(2)]
		private ENetConnection _connection;

		// Token: 0x04001AA5 RID: 6821
		[Nullable(2)]
		private ENetPacketPeer _peer;

		// Token: 0x04001AA6 RID: 6822
		private bool _isConnected;

		// Token: 0x04001AA7 RID: 6823
		private ulong _netId;
	}
}
