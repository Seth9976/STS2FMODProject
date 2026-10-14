using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Multiplayer
{
	// Token: 0x02000429 RID: 1065
	[NullableContext(2)]
	[Nullable(0)]
	public class NetClientGameService : INetClientHandler, INetHandler, INetClientGameService, INetGameService
	{
		// Token: 0x1400005D RID: 93
		// (add) Token: 0x060043F0 RID: 17392 RVA: 0x0020FD5C File Offset: 0x0020DF5C
		// (remove) Token: 0x060043F1 RID: 17393 RVA: 0x0020FD94 File Offset: 0x0020DF94
		public event Action ConnectedToHost;

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x060043F2 RID: 17394 RVA: 0x0020FDCC File Offset: 0x0020DFCC
		// (remove) Token: 0x060043F3 RID: 17395 RVA: 0x0020FE04 File Offset: 0x0020E004
		public event Action<NetErrorInfo> Disconnected;

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x060043F4 RID: 17396 RVA: 0x0020FE39 File Offset: 0x0020E039
		// (set) Token: 0x060043F5 RID: 17397 RVA: 0x0020FE41 File Offset: 0x0020E041
		public NetClient NetClient { get; private set; }

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060043F6 RID: 17398 RVA: 0x0020FE4A File Offset: 0x0020E04A
		public bool IsConnected
		{
			get
			{
				NetClient netClient = this.NetClient;
				return netClient != null && netClient.IsConnected;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060043F7 RID: 17399 RVA: 0x0020FE5D File Offset: 0x0020E05D
		public ulong NetId
		{
			get
			{
				NetClient netClient = this.NetClient;
				if (netClient == null)
				{
					throw new InvalidOperationException("Tried to get NetId while not connected!");
				}
				return netClient.NetId;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060043F8 RID: 17400 RVA: 0x0020FE79 File Offset: 0x0020E079
		public ulong HostNetId
		{
			get
			{
				NetClient netClient = this.NetClient;
				if (netClient == null)
				{
					throw new InvalidOperationException("Tried to get HostNetId while not connected!");
				}
				return netClient.HostNetId;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060043F9 RID: 17401 RVA: 0x0020FE95 File Offset: 0x0020E095
		public bool IsGameLoading
		{
			get
			{
				return this._qualityTracker.IsGameLoading;
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060043FA RID: 17402 RVA: 0x0020FEA2 File Offset: 0x0020E0A2
		// (set) Token: 0x060043FB RID: 17403 RVA: 0x0020FEAA File Offset: 0x0020E0AA
		public PlatformType Platform { get; private set; }

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060043FC RID: 17404 RVA: 0x0020FEB3 File Offset: 0x0020E0B3
		public NetGameType Type
		{
			get
			{
				return NetGameType.Client;
			}
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x0020FEB6 File Offset: 0x0020E0B6
		public NetClientGameService()
		{
			this._qualityTracker = new NetQualityTracker(this);
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x0020FED5 File Offset: 0x0020E0D5
		[NullableContext(1)]
		public void Initialize(NetClient client, PlatformType platform)
		{
			this.NetClient = client;
			this.Platform = platform;
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x0020FEE5 File Offset: 0x0020E0E5
		public void Update()
		{
			NetClient netClient = this.NetClient;
			if (netClient != null && netClient.IsConnected)
			{
				this.NetClient.Update();
				this._qualityTracker.Update();
			}
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x0020FF14 File Offset: 0x0020E114
		[NullableContext(1)]
		public void SendMessage<[Nullable(0)] T>(T message, ulong playerId) where T : INetMessage
		{
			NetClient netClient = this.NetClient;
			ulong? num = ((netClient != null) ? new ulong?(netClient.HostNetId) : null);
			if (!((playerId == num.GetValueOrDefault()) & (num != null)))
			{
				throw new NotImplementedException("Cannot send messages to non-host players as client!");
			}
			this.SendMessage<T>(message);
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x0020FF68 File Offset: 0x0020E168
		[NullableContext(1)]
		public void SendMessage<[Nullable(0)] T>(T message) where T : INetMessage
		{
			if (!this.IsConnected)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to send message ");
				defaultInterpolatedStringHandler.AppendFormatted<T>(message);
				defaultInterpolatedStringHandler.AppendLiteral(" while ");
				defaultInterpolatedStringHandler.AppendFormatted<NetClientGameService>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" is not connected!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			int num;
			byte[] array = this._messageBus.SerializeMessage<T>(this.NetClient.NetId, message, out num);
			this.NetClient.SendMessageToHost(array, num, message.Mode, message.Mode.ToChannelId());
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x0021000F File Offset: 0x0020E20F
		[NullableContext(1)]
		public void RegisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage
		{
			this._messageBus.RegisterMessageHandler<T>(handler);
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x0021001D File Offset: 0x0020E21D
		[NullableContext(1)]
		public void UnregisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage
		{
			this._messageBus.UnregisterMessageHandler<T>(handler);
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x0021002C File Offset: 0x0020E22C
		[NullableContext(1)]
		public void OnPacketReceived(ulong senderId, byte[] packetBytes, NetTransferMode mode, int channel)
		{
			INetMessage netMessage;
			ulong? num;
			if (!this._messageBus.TryDeserializeMessage(packetBytes, out netMessage, out num))
			{
				return;
			}
			senderId = num.GetValueOrDefault(senderId);
			this._messageBus.SendMessageToAllHandlers(netMessage, senderId);
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x00210063 File Offset: 0x0020E263
		public void Disconnect(NetError reason, bool now = false)
		{
			NetClient netClient = this.NetClient;
			if (netClient != null && netClient.IsConnected)
			{
				this.NetClient.DisconnectFromHost(reason, now);
			}
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x00210086 File Offset: 0x0020E286
		public void OnConnectedToHost()
		{
			this._qualityTracker.OnPeerConnected(this.NetClient.HostNetId);
			Action connectedToHost = this.ConnectedToHost;
			if (connectedToHost == null)
			{
				return;
			}
			connectedToHost();
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x002100AE File Offset: 0x0020E2AE
		public void OnDisconnectedFromHost(ulong hostNetId, NetErrorInfo info)
		{
			this._qualityTracker.OnPeerDisconnected(hostNetId);
			this._qualityTracker.Dispose();
			Action<NetErrorInfo> disconnected = this.Disconnected;
			if (disconnected == null)
			{
				return;
			}
			disconnected(info);
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x002100D8 File Offset: 0x0020E2D8
		public ConnectionStats GetStatsForPeer(ulong peerId)
		{
			return this._qualityTracker.GetStatsForPeer(peerId);
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x002100E6 File Offset: 0x0020E2E6
		public void SetGameLoading(bool isLoading)
		{
			this._qualityTracker.SetIsLoading(isLoading);
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x002100F4 File Offset: 0x0020E2F4
		public void SetBufferMessages(bool bufferMessages)
		{
			this._messageBus.SetBufferMessages(bufferMessages);
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x00210102 File Offset: 0x0020E302
		public string GetRawLobbyIdentifier()
		{
			NetClient netClient = this.NetClient;
			if (netClient == null)
			{
				return null;
			}
			return netClient.GetRawLobbyIdentifier();
		}

		// Token: 0x04001A66 RID: 6758
		[Nullable(1)]
		private readonly NetMessageBus _messageBus = new NetMessageBus();

		// Token: 0x04001A67 RID: 6759
		[Nullable(1)]
		private readonly NetQualityTracker _qualityTracker;
	}
}
