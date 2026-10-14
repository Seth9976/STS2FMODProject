using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Multiplayer.Transport.ENet;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Multiplayer
{
	// Token: 0x0200042B RID: 1067
	[NullableContext(2)]
	[Nullable(0)]
	public class NetHostGameService : INetHostHandler, INetHandler, INetHostGameService, INetGameService
	{
		// Token: 0x1400005F RID: 95
		// (add) Token: 0x0600440C RID: 17420 RVA: 0x00210118 File Offset: 0x0020E318
		// (remove) Token: 0x0600440D RID: 17421 RVA: 0x00210150 File Offset: 0x0020E350
		public event Action<NetErrorInfo> Disconnected;

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x0600440E RID: 17422 RVA: 0x00210188 File Offset: 0x0020E388
		// (remove) Token: 0x0600440F RID: 17423 RVA: 0x002101C0 File Offset: 0x0020E3C0
		public event Action<ulong> ClientConnected;

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x06004410 RID: 17424 RVA: 0x002101F8 File Offset: 0x0020E3F8
		// (remove) Token: 0x06004411 RID: 17425 RVA: 0x00210230 File Offset: 0x0020E430
		public event Action<ulong, NetErrorInfo> ClientDisconnected;

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06004412 RID: 17426 RVA: 0x00210265 File Offset: 0x0020E465
		public bool IsConnected
		{
			get
			{
				NetHost netHost = this._netHost;
				return netHost != null && netHost.IsConnected;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06004413 RID: 17427 RVA: 0x00210278 File Offset: 0x0020E478
		[Nullable(1)]
		public IReadOnlyList<NetClientData> ConnectedPeers
		{
			[NullableContext(1)]
			get
			{
				return this._connectedPeers;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06004414 RID: 17428 RVA: 0x00210280 File Offset: 0x0020E480
		public ulong NetId
		{
			get
			{
				NetHost netHost = this._netHost;
				if (netHost == null)
				{
					throw new InvalidOperationException("Tried to get NetId while not connected!");
				}
				return netHost.NetId;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06004415 RID: 17429 RVA: 0x0021029C File Offset: 0x0020E49C
		public bool IsGameLoading
		{
			get
			{
				return this._qualityTracker.IsGameLoading;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06004416 RID: 17430 RVA: 0x002102A9 File Offset: 0x0020E4A9
		// (set) Token: 0x06004417 RID: 17431 RVA: 0x002102B1 File Offset: 0x0020E4B1
		public PlatformType Platform { get; private set; }

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06004418 RID: 17432 RVA: 0x002102BA File Offset: 0x0020E4BA
		public NetHost NetHost
		{
			get
			{
				return this._netHost;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06004419 RID: 17433 RVA: 0x002102C2 File Offset: 0x0020E4C2
		public NetGameType Type
		{
			get
			{
				return NetGameType.Host;
			}
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x002102C5 File Offset: 0x0020E4C5
		public NetHostGameService()
		{
			this._qualityTracker = new NetQualityTracker(this);
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x002102F0 File Offset: 0x0020E4F0
		public NetErrorInfo? StartENetHost(ushort port, int maxClients)
		{
			ENetHost enetHost = new ENetHost(this);
			this._netHost = enetHost;
			return enetHost.StartHost(port, maxClients);
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x00210314 File Offset: 0x0020E514
		[NullableContext(1)]
		public Task<NetErrorInfo?> StartSteamHost(int maxClients)
		{
			SteamHost steamHost = new SteamHost(this);
			this._netHost = steamHost;
			this.Platform = PlatformType.Steam;
			return steamHost.StartHost(maxClients);
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x0021033D File Offset: 0x0020E53D
		public void Update()
		{
			NetHost netHost = this._netHost;
			if (netHost != null && netHost.IsConnected)
			{
				this._netHost.Update();
				this._qualityTracker.Update();
			}
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x0021036C File Offset: 0x0020E56C
		[NullableContext(1)]
		public void SendMessage<[Nullable(0)] T>(T message, ulong peerId) where T : INetMessage
		{
			this.SendMessageToClientInternal<T>(message, peerId, message.Mode.ToChannelId(), null);
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x0021039C File Offset: 0x0020E59C
		[NullableContext(1)]
		private void SendMessageToClientInternal<[Nullable(0)] T>(T message, ulong peerId, int channel, ulong? overrideSenderId) where T : INetMessage
		{
			if (!this.IsConnected)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to send message ");
				defaultInterpolatedStringHandler.AppendFormatted<T>(message);
				defaultInterpolatedStringHandler.AppendLiteral(" while ");
				defaultInterpolatedStringHandler.AppendFormatted<NetHostGameService>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" is not connected!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			int num;
			byte[] array = this._messageBus.SerializeMessage<T>(overrideSenderId ?? this._netHost.NetId, message, out num);
			this._netHost.SendMessageToClient(peerId, array, num, message.Mode, channel);
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x00210448 File Offset: 0x0020E648
		[NullableContext(1)]
		public void SendMessage<[Nullable(0)] T>(T message) where T : INetMessage
		{
			if (!this.IsConnected)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to send message ");
				defaultInterpolatedStringHandler.AppendFormatted<T>(message);
				defaultInterpolatedStringHandler.AppendLiteral(" while ");
				defaultInterpolatedStringHandler.AppendFormatted<NetHostGameService>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" is not connected!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			int num;
			byte[] array = this._messageBus.SerializeMessage<T>(this._netHost.NetId, message, out num);
			foreach (NetClientData netClientData in this._connectedPeers)
			{
				if (netClientData.readyForBroadcasting)
				{
					this._netHost.SendMessageToClient(netClientData.peerId, array, num, message.Mode, message.Mode.ToChannelId());
				}
			}
		}

		// Token: 0x06004421 RID: 17441 RVA: 0x00210540 File Offset: 0x0020E740
		[NullableContext(1)]
		public void RegisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage
		{
			this._messageBus.RegisterMessageHandler<T>(handler);
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x0021054E File Offset: 0x0020E74E
		[NullableContext(1)]
		public void UnregisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage
		{
			this._messageBus.UnregisterMessageHandler<T>(handler);
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x0021055C File Offset: 0x0020E75C
		[NullableContext(1)]
		public void OnPacketReceived(ulong senderId, byte[] packetBytes, NetTransferMode mode, int channel)
		{
			INetMessage netMessage;
			ulong? num;
			if (!this._messageBus.TryDeserializeMessage(packetBytes, out netMessage, out num))
			{
				return;
			}
			if (netMessage.ShouldBroadcast)
			{
				this.BroadcastMessage<INetMessage>(netMessage, senderId, channel, num.Value);
			}
			senderId = num.GetValueOrDefault(senderId);
			this._messageBus.SendMessageToAllHandlers(netMessage, senderId);
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x002105AC File Offset: 0x0020E7AC
		[NullableContext(1)]
		private void BroadcastMessage<[Nullable(0)] T>(T message, ulong excludePeerId, int channel, ulong overrideSenderId) where T : INetMessage
		{
			foreach (NetClientData netClientData in this._connectedPeers)
			{
				if (netClientData.readyForBroadcasting && netClientData.peerId != excludePeerId)
				{
					this.SendMessageToClientInternal<T>(message, netClientData.peerId, channel, new ulong?(overrideSenderId));
				}
			}
		}

		/// <summary>
		/// Starts sending broadcasted messages to a peer.
		/// When a peer first connects, messages that have ShouldBroadcast set are not sent to that peer. They are only sent
		/// to the newly connected peer after this method is called, passing the newly connected peer's ID.
		/// This is used to prevent messages from being sent to a peer until the game-level connection flow has been completed.
		/// </summary>
		// Token: 0x06004425 RID: 17445 RVA: 0x00210620 File Offset: 0x0020E820
		public void SetPeerReadyForBroadcasting(ulong peerId)
		{
			for (int i = 0; i < this._connectedPeers.Count; i++)
			{
				if (this._connectedPeers[i].peerId == peerId)
				{
					NetClientData netClientData = this._connectedPeers[i];
					netClientData.readyForBroadcasting = true;
					this._connectedPeers[i] = netClientData;
				}
			}
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x00210679 File Offset: 0x0020E879
		public void DisconnectClient(ulong peerId, NetError reason, bool now = false)
		{
			this._netHost.DisconnectClient(peerId, reason, now);
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x00210689 File Offset: 0x0020E889
		public void Disconnect(NetError reason, bool now = false)
		{
			NetHost netHost = this._netHost;
			if (netHost != null && netHost.IsConnected)
			{
				this._netHost.StopHost(reason, now);
				this._qualityTracker.Dispose();
			}
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x002106B7 File Offset: 0x0020E8B7
		public void OnDisconnected(NetErrorInfo info)
		{
			Action<NetErrorInfo> disconnected = this.Disconnected;
			if (disconnected == null)
			{
				return;
			}
			disconnected(info);
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x002106CC File Offset: 0x0020E8CC
		public void OnPeerConnected(ulong peerId)
		{
			this._connectedPeers.Add(new NetClientData
			{
				peerId = peerId,
				readyForBroadcasting = false
			});
			this._qualityTracker.OnPeerConnected(peerId);
			Action<ulong> clientConnected = this.ClientConnected;
			if (clientConnected == null)
			{
				return;
			}
			clientConnected(peerId);
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x0021071C File Offset: 0x0020E91C
		public void OnPeerDisconnected(ulong peerId, NetErrorInfo info)
		{
			this._connectedPeers.RemoveAll((NetClientData p) => p.peerId == peerId);
			this._qualityTracker.OnPeerDisconnected(peerId);
			Action<ulong, NetErrorInfo> clientDisconnected = this.ClientDisconnected;
			if (clientDisconnected == null)
			{
				return;
			}
			clientDisconnected(peerId, info);
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x00210776 File Offset: 0x0020E976
		public ConnectionStats GetStatsForPeer(ulong peerId)
		{
			return this._qualityTracker.GetStatsForPeer(peerId);
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x00210784 File Offset: 0x0020E984
		public void SetGameLoading(bool isLoading)
		{
			this._qualityTracker.SetIsLoading(isLoading);
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x00210792 File Offset: 0x0020E992
		public void SetBufferMessages(bool bufferMessages)
		{
			this._messageBus.SetBufferMessages(bufferMessages);
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x002107A0 File Offset: 0x0020E9A0
		public string GetRawLobbyIdentifier()
		{
			NetHost netHost = this._netHost;
			if (netHost == null)
			{
				return null;
			}
			return netHost.GetRawLobbyIdentifier();
		}

		// Token: 0x04001A6D RID: 6765
		private NetHost _netHost;

		// Token: 0x04001A6E RID: 6766
		[Nullable(1)]
		private readonly NetMessageBus _messageBus = new NetMessageBus();

		// Token: 0x04001A6F RID: 6767
		[Nullable(1)]
		private readonly NetQualityTracker _qualityTracker;

		// Token: 0x04001A70 RID: 6768
		[Nullable(1)]
		private readonly List<NetClientData> _connectedPeers = new List<NetClientData>();
	}
}
