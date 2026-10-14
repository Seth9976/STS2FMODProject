using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Provides additional host-related methods on top of the default game service.
	/// </summary>
	// Token: 0x0200049A RID: 1178
	[NullableContext(2)]
	public interface INetHostGameService : INetGameService
	{
		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06004759 RID: 18265
		[Nullable(1)]
		IReadOnlyList<NetClientData> ConnectedPeers
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x0600475A RID: 18266
		NetHost NetHost { get; }

		/// <summary>
		/// Event called when a new client connects to the host.
		/// </summary>
		// Token: 0x14000069 RID: 105
		// (add) Token: 0x0600475B RID: 18267
		// (remove) Token: 0x0600475C RID: 18268
		event Action<ulong> ClientConnected;

		/// <summary>
		/// Event called when a client disconnects from the host, either voluntarily or because we disconnected them.
		/// </summary>
		// Token: 0x1400006A RID: 106
		// (add) Token: 0x0600475D RID: 18269
		// (remove) Token: 0x0600475E RID: 18270
		event Action<ulong, NetErrorInfo> ClientDisconnected;

		/// <summary>
		/// Disconnects a client from the host.
		/// </summary>
		/// <param name="peerId">The client to disconnect.</param>
		/// <param name="reason">Why the client is being disconnected.</param>
		/// <param name="now">If true, the client will be forcibly disconnected. The reason may not make it to the client.</param>
		// Token: 0x0600475F RID: 18271
		void DisconnectClient(ulong peerId, NetError reason, bool now = false);

		/// <summary>
		/// Marks a client as ready to receive broadcasted messages.
		/// During the initial setup process, we don't want clients to receive broadcast messages as they may not be ready
		/// for them. Call this when initial setup is done.
		/// </summary>
		// Token: 0x06004760 RID: 18272
		void SetPeerReadyForBroadcasting(ulong peerId);
	}
}
