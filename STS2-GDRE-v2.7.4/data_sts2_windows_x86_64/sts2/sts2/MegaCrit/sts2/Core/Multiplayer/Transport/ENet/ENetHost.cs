using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet
{
	// Token: 0x02000442 RID: 1090
	[NullableContext(1)]
	[Nullable(0)]
	public class ENetHost : NetHost
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060044B2 RID: 17586 RVA: 0x00212973 File Offset: 0x00210B73
		public override bool IsConnected
		{
			get
			{
				return this._isConnected;
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060044B3 RID: 17587 RVA: 0x0021297B File Offset: 0x00210B7B
		public override IEnumerable<ulong> ConnectedPeerIds
		{
			get
			{
				return this._connectedPeers.Select<ENetHost.ClientConnection, ulong>((ENetHost.ClientConnection c) => c.netId);
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060044B4 RID: 17588 RVA: 0x002129A7 File Offset: 0x00210BA7
		public override ulong NetId
		{
			get
			{
				return 1UL;
			}
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x002129AB File Offset: 0x00210BAB
		public ENetHost(INetHostHandler handler)
			: base(handler)
		{
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x002129DC File Offset: 0x00210BDC
		public NetErrorInfo? StartHost(ushort port, int maxClients)
		{
			this._connection = new ENetConnection();
			Error error = this._connection.CreateHostBound("0.0.0.0", (int)port, maxClients, 0, 0, 0);
			if (error != null)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to create host! ");
				defaultInterpolatedStringHandler.AppendFormatted<Error>(error);
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return new NetErrorInfo?(new NetErrorInfo(error));
			}
			this._isConnected = true;
			return null;
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x00212A5C File Offset: 0x00210C5C
		private Task DoClientHandshake(ENetPacketPeer peer)
		{
			ENetHost.<DoClientHandshake>d__17 <DoClientHandshake>d__;
			<DoClientHandshake>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DoClientHandshake>d__.<>4__this = this;
			<DoClientHandshake>d__.peer = peer;
			<DoClientHandshake>d__.<>1__state = -1;
			<DoClientHandshake>d__.<>t__builder.Start<ENetHost.<DoClientHandshake>d__17>(ref <DoClientHandshake>d__);
			return <DoClientHandshake>d__.<>t__builder.Task;
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x00212AA8 File Offset: 0x00210CA8
		private void HandleClientDisconnection(ENetHost.ClientConnection conn, NetError reason, bool notifyHandler = true)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Peer ID ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(conn.netId);
			defaultInterpolatedStringHandler.AppendLiteral(" disconnected, reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(reason);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this._connectedPeers.Remove(conn);
			if (notifyHandler)
			{
				this._handler.OnPeerDisconnected(conn.netId, new NetErrorInfo(reason, false));
			}
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x00212B28 File Offset: 0x00210D28
		private ENetHost.ClientConnection? GetConnectionByPeer(ENetPacketPeer peer)
		{
			foreach (ENetHost.ClientConnection clientConnection in this._connectedPeers)
			{
				if (clientConnection.peer == peer)
				{
					return new ENetHost.ClientConnection?(clientConnection);
				}
			}
			return null;
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x00212B94 File Offset: 0x00210D94
		private ENetHost.ClientConnection? GetConnectionById(ulong id)
		{
			foreach (ENetHost.ClientConnection clientConnection in this._connectedPeers)
			{
				if (clientConnection.netId == id)
				{
					return new ENetHost.ClientConnection?(clientConnection);
				}
			}
			return null;
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x00212C00 File Offset: 0x00210E00
		public override void Update()
		{
			this.AssertHostStarted();
			ENetServiceData? enetServiceData;
			while (this._connection.TryService(out enetServiceData))
			{
				ENetConnection.EventType type = enetServiceData.Value.type;
				ENetConnection.EventType eventType = type - -1L;
				if (eventType <= 4L)
				{
					switch (eventType)
					{
					case 0U:
						this._logger.Error("Got error from ENetConnection! TODO: Expand me", 1);
						continue;
					case 2U:
						TaskHelper.RunSafely(this.DoClientHandshake(enetServiceData.Value.peer));
						continue;
					case 3U:
						continue;
					case 4U:
						this.HandlePacketReceived(enetServiceData.Value);
						continue;
					}
				}
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x00212C9C File Offset: 0x00210E9C
		private void HandlePacketReceived(ENetServiceData data)
		{
			byte[] packetData = data.packetData;
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received packet of length ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(packetData.Length);
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			ENetPacketPeer peer = data.peer;
			ENetPacket enetPacket = new ENetPacket(packetData);
			if (enetPacket.PacketType == ENetPacketType.HandshakeRequest)
			{
				ENetHandshakeRequest enetHandshakeRequest = enetPacket.AsHandshakeRequest();
				ENetHost.ClientConnection clientConnection = default(ENetHost.ClientConnection);
				clientConnection.peer = peer;
				clientConnection.netId = enetHandshakeRequest.netId;
				ENetHost.HandshakeAwaitingResponse handshakeAwaitingResponse = default(ENetHost.HandshakeAwaitingResponse);
				handshakeAwaitingResponse.conn = clientConnection;
				handshakeAwaitingResponse.receivedMsec = Time.GetTicksMsec();
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received handshake packet containing peer ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(clientConnection.netId);
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this._receivedHandshakes.Add(handshakeAwaitingResponse);
				return;
			}
			if (enetPacket.PacketType == ENetPacketType.Disconnection)
			{
				ENetDisconnection enetDisconnection = enetPacket.AsDisconnection();
				ENetHost.ClientConnection? connectionByPeer = this.GetConnectionByPeer(peer);
				if (connectionByPeer != null)
				{
					this.HandleClientDisconnection(connectionByPeer.Value, enetDisconnection.reason, true);
					return;
				}
			}
			else
			{
				ENetHost.ClientConnection? connectionByPeer2 = this.GetConnectionByPeer(peer);
				if (connectionByPeer2 == null)
				{
					Logger logger3 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Received a non-handshake packet length ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(packetData.Length);
					defaultInterpolatedStringHandler.AppendLiteral(" for a peer, but no connection exists!");
					logger3.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					return;
				}
				this._handler.OnPacketReceived(connectionByPeer2.Value.netId, enetPacket.AsAppMessage(), data.mode, data.channel);
			}
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x00212E3C File Offset: 0x0021103C
		public override void SendMessageToClient(ulong peerId, byte[] bytes, int length, NetTransferMode mode, int channel = 0)
		{
			this.AssertHostStarted();
			ENetHost.ClientConnection? connectionById = this.GetConnectionById(peerId);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (connectionById == null)
			{
				Logger logger = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(82, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to send message to client with ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(peerId);
				defaultInterpolatedStringHandler.AppendLiteral(", but no client with that ID is connected!");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			ENetPacket enetPacket = ENetPacket.FromAppMessage(bytes, length);
			Logger logger2 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Sending packet of length ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(enetPacket.AllBytes.Length);
			logger2.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			connectionById.Value.peer.Send(channel, enetPacket.AllBytes, ENetUtil.FlagsFromMode(mode));
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x00212EFF File Offset: 0x002110FF
		public override void DisconnectClient(ulong peerId, NetError reason, bool now = false)
		{
			this.DisconnectClientInternal(peerId, reason, now, true);
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x00212F0C File Offset: 0x0021110C
		private void DisconnectClientInternal(ulong peerId, NetError reason, bool now = false, bool notifyHandler = true)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Disconnecting client ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(peerId);
			defaultInterpolatedStringHandler.AppendLiteral(", reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(reason);
			defaultInterpolatedStringHandler.AppendLiteral(". Now: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(now);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.AssertHostStarted();
			ENetHost.ClientConnection? connectionById = this.GetConnectionById(peerId);
			if (connectionById == null)
			{
				return;
			}
			if (now)
			{
				connectionById.Value.peer.PeerDisconnectNow(0);
			}
			else
			{
				ENetPacket enetPacket = ENetPacket.FromDisconnection(new ENetDisconnection
				{
					reason = reason
				});
				connectionById.Value.peer.Send(0, enetPacket.AllBytes, 1);
				connectionById.Value.peer.PeerDisconnectLater(0);
			}
			this.HandleClientDisconnection(connectionById.Value, reason, notifyHandler);
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x00212FF4 File Offset: 0x002111F4
		public override void SendMessageToAll(byte[] bytes, int length, NetTransferMode mode, int channel = 0)
		{
			this.AssertHostStarted();
			foreach (ENetHost.ClientConnection clientConnection in this._connectedPeers)
			{
				this.SendMessageToClient(clientConnection.netId, bytes, length, mode, channel);
			}
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x00213058 File Offset: 0x00211258
		public override void SetHostIsClosed(bool isClosed)
		{
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x0021305C File Offset: 0x0021125C
		public override void StopHost(NetError reason, bool now = false)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Stopping host. Reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(reason);
			defaultInterpolatedStringHandler.AppendLiteral(". Now: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(now);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			foreach (ENetHost.ClientConnection clientConnection in this._connectedPeers.ToList<ENetHost.ClientConnection>())
			{
				this.DisconnectClientInternal(clientConnection.netId, reason, now, false);
			}
			this._connectedPeers.Clear();
			this._connection.Flush();
			this._connection.Destroy();
			this._isConnected = false;
			this._handler.OnDisconnected(new NetErrorInfo(reason, true));
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x00213138 File Offset: 0x00211338
		private void AssertHostStarted()
		{
			if (this._connection == null)
			{
				throw new InvalidOperationException("Must call StartHost first!");
			}
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x0021314D File Offset: 0x0021134D
		[NullableContext(2)]
		public override string GetRawLobbyIdentifier()
		{
			return null;
		}

		// Token: 0x04001AB6 RID: 6838
		private const int _handshakeTimeoutMsec = 10000;

		// Token: 0x04001AB7 RID: 6839
		private const int _handshakeUpdateRateMsec = 100;

		// Token: 0x04001AB8 RID: 6840
		private readonly List<ENetHost.ClientConnection> _connectedPeers = new List<ENetHost.ClientConnection>();

		// Token: 0x04001AB9 RID: 6841
		[Nullable(2)]
		private ENetConnection _connection;

		// Token: 0x04001ABA RID: 6842
		private bool _isConnected;

		// Token: 0x04001ABB RID: 6843
		private readonly List<ENetHost.HandshakeAwaitingResponse> _receivedHandshakes = new List<ENetHost.HandshakeAwaitingResponse>();

		// Token: 0x04001ABC RID: 6844
		private readonly Logger _logger = new Logger("ENetHost", LogType.Network);

		// Token: 0x0200199F RID: 6559
		[NullableContext(0)]
		private struct ClientConnection : IEquatable<ENetHost.ClientConnection>
		{
			// Token: 0x0600AB2B RID: 43819 RVA: 0x0035FD54 File Offset: 0x0035DF54
			[CompilerGenerated]
			public override readonly string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ClientConnection");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600AB2C RID: 43820 RVA: 0x0035FDA0 File Offset: 0x0035DFA0
			[CompilerGenerated]
			private readonly bool PrintMembers(StringBuilder builder)
			{
				builder.Append("netId = ");
				ulong num = this.netId;
				builder.Append(num.ToString());
				builder.Append(", peer = ");
				builder.Append(this.peer);
				return true;
			}

			// Token: 0x0600AB2D RID: 43821 RVA: 0x0035FDEE File Offset: 0x0035DFEE
			[CompilerGenerated]
			public static bool operator !=(ENetHost.ClientConnection left, ENetHost.ClientConnection right)
			{
				return !(left == right);
			}

			// Token: 0x0600AB2E RID: 43822 RVA: 0x0035FDFA File Offset: 0x0035DFFA
			[CompilerGenerated]
			public static bool operator ==(ENetHost.ClientConnection left, ENetHost.ClientConnection right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600AB2F RID: 43823 RVA: 0x0035FE04 File Offset: 0x0035E004
			[CompilerGenerated]
			public override readonly int GetHashCode()
			{
				return EqualityComparer<ulong>.Default.GetHashCode(this.netId) * -1521134295 + EqualityComparer<ENetPacketPeer>.Default.GetHashCode(this.peer);
			}

			// Token: 0x0600AB30 RID: 43824 RVA: 0x0035FE2D File Offset: 0x0035E02D
			[CompilerGenerated]
			public override readonly bool Equals(object obj)
			{
				return obj is ENetHost.ClientConnection && this.Equals((ENetHost.ClientConnection)obj);
			}

			// Token: 0x0600AB31 RID: 43825 RVA: 0x0035FE45 File Offset: 0x0035E045
			[CompilerGenerated]
			public readonly bool Equals(ENetHost.ClientConnection other)
			{
				return EqualityComparer<ulong>.Default.Equals(this.netId, other.netId) && EqualityComparer<ENetPacketPeer>.Default.Equals(this.peer, other.peer);
			}

			// Token: 0x0400652F RID: 25903
			public ulong netId;

			// Token: 0x04006530 RID: 25904
			[Nullable(1)]
			public ENetPacketPeer peer;
		}

		// Token: 0x020019A0 RID: 6560
		[NullableContext(0)]
		private struct HandshakeAwaitingResponse
		{
			// Token: 0x04006531 RID: 25905
			public ulong receivedMsec;

			// Token: 0x04006532 RID: 25906
			public ENetHost.ClientConnection conn;
		}
	}
}
