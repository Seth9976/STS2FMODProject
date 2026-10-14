using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;

namespace MegaCrit.Sts2.Core.Multiplayer
{
	// Token: 0x0200042E RID: 1070
	[NullableContext(1)]
	[Nullable(0)]
	public class NetSingleplayerGameService : INetGameService
	{
		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06004449 RID: 17481 RVA: 0x00210E86 File Offset: 0x0020F086
		public bool IsConnected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x0600444A RID: 17482 RVA: 0x00210E89 File Offset: 0x0020F089
		public bool IsGameLoading
		{
			get
			{
				return this._isLoading;
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x0600444B RID: 17483 RVA: 0x00210E91 File Offset: 0x0020F091
		public ulong NetId
		{
			get
			{
				return 1UL;
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x0600444C RID: 17484 RVA: 0x00210E95 File Offset: 0x0020F095
		public NetGameType Type
		{
			get
			{
				return NetGameType.Singleplayer;
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x0600444D RID: 17485 RVA: 0x00210E98 File Offset: 0x0020F098
		public PlatformType Platform
		{
			get
			{
				if (!SteamInitializer.Initialized)
				{
					return PlatformType.None;
				}
				return PlatformType.Steam;
			}
		}

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x0600444E RID: 17486 RVA: 0x00210EA4 File Offset: 0x0020F0A4
		// (remove) Token: 0x0600444F RID: 17487 RVA: 0x00210EDC File Offset: 0x0020F0DC
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<NetErrorInfo> Disconnected;

		// Token: 0x06004450 RID: 17488 RVA: 0x00210F11 File Offset: 0x0020F111
		public void SendMessage<[Nullable(0)] T>(T message, ulong playerId) where T : INetMessage
		{
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x00210F13 File Offset: 0x0020F113
		public void SendMessage<[Nullable(0)] T>(T message) where T : INetMessage
		{
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x00210F15 File Offset: 0x0020F115
		public void RegisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage
		{
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x00210F17 File Offset: 0x0020F117
		public void UnregisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage
		{
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x00210F19 File Offset: 0x0020F119
		public void Update()
		{
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x00210F1B File Offset: 0x0020F11B
		public void Disconnect(NetError reason, bool now = false)
		{
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x00210F1D File Offset: 0x0020F11D
		public ConnectionStats GetStatsForPeer(ulong peerId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x00210F24 File Offset: 0x0020F124
		public void SetGameLoading(bool isLoading)
		{
			this._isLoading = isLoading;
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x00210F2D File Offset: 0x0020F12D
		public void SetBufferMessages(bool bufferMessages)
		{
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x00210F2F File Offset: 0x0020F12F
		[NullableContext(2)]
		public string GetRawLobbyIdentifier()
		{
			return null;
		}

		// Token: 0x04001A80 RID: 6784
		public const int defaultNetId = 1;

		// Token: 0x04001A81 RID: 6785
		private bool _isLoading;
	}
}
