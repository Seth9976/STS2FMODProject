using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport
{
	// Token: 0x02000433 RID: 1075
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class NetHost
	{
		/// <summary>
		/// The IDs of currently connected clients.
		/// </summary>
		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x0600446A RID: 17514
		public abstract IEnumerable<ulong> ConnectedPeerIds { get; }

		/// <summary>
		/// True if clients are able to connect and receive messages, false otherwise.
		/// </summary>
		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x0600446B RID: 17515
		public abstract bool IsConnected { get; }

		/// <summary>
		/// The network ID of the host.
		/// </summary>
		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x0600446C RID: 17516
		public abstract ulong NetId { get; }

		// Token: 0x0600446D RID: 17517 RVA: 0x00210F49 File Offset: 0x0020F149
		protected NetHost(INetHostHandler handler)
		{
			this._handler = handler;
		}

		/// <summary>
		/// Should be called at regular intervals to send and receive messages.
		/// </summary>
		// Token: 0x0600446E RID: 17518
		public abstract void Update();

		/// <summary>
		/// If called with true, the host will be considered closed.
		/// What this means is platform-dependent. On Steam, this prevents other people from seeing the lobby, but existing
		/// clients are unaffected.
		/// </summary>
		// Token: 0x0600446F RID: 17519
		public abstract void SetHostIsClosed(bool isClosed);

		/// <summary>
		/// Sends a message to a specific client.
		/// </summary>
		/// <param name="peerId">The ID of the client to send to.</param>
		/// <param name="bytes">The byte content of the message.</param>
		/// <param name="length">The amount of bytes, starting from index 0, to send from bytes.</param>
		/// <param name="mode">Method of transfer (reliable or unreliable).</param>
		/// <param name="channel">The channel to use to send the message.</param>
		// Token: 0x06004470 RID: 17520
		public abstract void SendMessageToClient(ulong peerId, byte[] bytes, int length, NetTransferMode mode, int channel = 0);

		/// <summary>
		/// Broadcasts a message to all clients.
		/// </summary>
		/// <param name="bytes">The byte content of the message.</param>
		/// <param name="length">The amount of bytes, starting from index 0, to send from bytes.</param>
		/// <param name="mode">Method of transfer (reliable or unreliable).</param>
		/// <param name="channel">The channel to use to send the message.</param>
		// Token: 0x06004471 RID: 17521
		public abstract void SendMessageToAll(byte[] bytes, int length, NetTransferMode mode, int channel = 0);

		/// <summary>
		/// Disconnects a client.
		/// </summary>
		/// <param name="peerId">The ID of the client to disconnect.</param>
		/// <param name="reason">THe reason to send to the client accompanying the disconnection.</param>
		/// <param name="now">If true, the client will be forcibly and immediately disconnected. If false, the client is
		/// disconnected at some point in the near future.</param>
		// Token: 0x06004472 RID: 17522
		public abstract void DisconnectClient(ulong peerId, NetError reason, bool now = false);

		/// <summary>
		/// Disconnecst all clients and stops listening for new connections.
		/// </summary>
		/// <param name="reason">The reason that the host is shutting down. Sent to connected clients.</param>
		/// <param name="now">If true, all clients will be forcibly and immediately disconnected. If false, clients are
		/// disconnected at some point in the near future.</param>
		// Token: 0x06004473 RID: 17523
		public abstract void StopHost(NetError reason, bool now = false);

		/// <summary>
		/// Returns a unique identifier for the lobby that this host is advertising.
		/// Returns null if the host has stopped or not yet started hosting.
		/// This returns a platform-specific identifier for use with rich presence. Feel free to refactor later if more
		/// info or more specific info is needed.
		/// </summary>
		// Token: 0x06004474 RID: 17524
		[NullableContext(2)]
		public abstract string GetRawLobbyIdentifier();

		/// <summary>
		/// Delegate to notify about events.
		/// </summary>
		// Token: 0x04001A84 RID: 6788
		protected INetHostHandler _handler;
	}
}
