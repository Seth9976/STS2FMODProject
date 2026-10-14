using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Multiplayer
{
	// Token: 0x0200042D RID: 1069
	[NullableContext(1)]
	[Nullable(0)]
	public class NetReplayGameService : INetGameService
	{
		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06004436 RID: 17462 RVA: 0x00210DC6 File Offset: 0x0020EFC6
		public bool IsGameLoading
		{
			get
			{
				return this._isLoading;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06004437 RID: 17463 RVA: 0x00210DCE File Offset: 0x0020EFCE
		public bool IsConnected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06004438 RID: 17464 RVA: 0x00210DD1 File Offset: 0x0020EFD1
		// (set) Token: 0x06004439 RID: 17465 RVA: 0x00210DD9 File Offset: 0x0020EFD9
		public ulong NetId { get; set; }

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x0600443A RID: 17466 RVA: 0x00210DE2 File Offset: 0x0020EFE2
		public NetGameType Type
		{
			get
			{
				return NetGameType.Replay;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x0600443B RID: 17467 RVA: 0x00210DE5 File Offset: 0x0020EFE5
		public PlatformType Platform
		{
			get
			{
				return PlatformType.None;
			}
		}

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x0600443C RID: 17468 RVA: 0x00210DE8 File Offset: 0x0020EFE8
		// (remove) Token: 0x0600443D RID: 17469 RVA: 0x00210E20 File Offset: 0x0020F020
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<NetErrorInfo> Disconnected;

		// Token: 0x0600443E RID: 17470 RVA: 0x00210E55 File Offset: 0x0020F055
		public NetReplayGameService(ulong netId)
		{
			this.NetId = netId;
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x00210E64 File Offset: 0x0020F064
		public void SendMessage<[Nullable(0)] T>(T message, ulong playerId) where T : INetMessage
		{
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x00210E66 File Offset: 0x0020F066
		public void SendMessage<[Nullable(0)] T>(T message) where T : INetMessage
		{
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x00210E68 File Offset: 0x0020F068
		public void RegisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage
		{
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x00210E6A File Offset: 0x0020F06A
		public void UnregisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage
		{
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x00210E6C File Offset: 0x0020F06C
		public void Update()
		{
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x00210E6E File Offset: 0x0020F06E
		public void Disconnect(NetError reason, bool now = false)
		{
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x00210E70 File Offset: 0x0020F070
		public ConnectionStats GetStatsForPeer(ulong peerId)
		{
			return new ConnectionStats(peerId);
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x00210E78 File Offset: 0x0020F078
		public void SetGameLoading(bool isLoading)
		{
			this._isLoading = isLoading;
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x00210E81 File Offset: 0x0020F081
		public void SetBufferMessages(bool bufferMessages)
		{
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x00210E83 File Offset: 0x0020F083
		[NullableContext(2)]
		public string GetRawLobbyIdentifier()
		{
			return null;
		}

		// Token: 0x04001A7D RID: 6781
		private bool _isLoading;
	}
}
