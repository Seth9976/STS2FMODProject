using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport
{
	// Token: 0x02000432 RID: 1074
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class NetClient
	{
		/// <summary>
		/// True this client is connected to a host, false otherwise.
		/// </summary>
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06004462 RID: 17506
		public abstract bool IsConnected { get; }

		/// <summary>
		/// The network ID of the client.
		/// </summary>
		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06004463 RID: 17507
		public abstract ulong NetId { get; }

		/// <summary>
		/// The network ID of the host we are connected to.
		/// </summary>
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06004464 RID: 17508
		public abstract ulong HostNetId { get; }

		// Token: 0x06004465 RID: 17509 RVA: 0x00210F3A File Offset: 0x0020F13A
		protected NetClient(INetClientHandler handler)
		{
			this._handler = handler;
		}

		/// <summary>
		/// Should be called at regular intervals to send and receive messages.
		/// </summary>
		// Token: 0x06004466 RID: 17510
		public abstract void Update();

		/// <summary>
		/// Sends a message to the host.
		/// </summary>
		/// <param name="bytes">The byte content of the message.</param>
		/// <param name="length">The amount of bytes, starting from index 0, to send from bytes.</param>
		/// <param name="mode">Method of transfer (reliable or unreliable).</param>
		/// <param name="channel">The channel to use to send the message.</param>
		// Token: 0x06004467 RID: 17511
		public abstract void SendMessageToHost(byte[] bytes, int length, NetTransferMode mode, int channel = 0);

		/// <summary>
		/// Disconnects from the host.
		/// </summary>
		/// <param name="reason">THe reason to send to the client accompanying the disconnection.</param>
		/// <param name="now">If true, we will be forcibly and immediately disconnected. If false, we are disconnected at
		/// some point in the near future.</param>
		// Token: 0x06004468 RID: 17512
		public abstract void DisconnectFromHost(NetError reason, bool now = false);

		/// <summary>
		/// Returns a unique identifier for the lobby that the client is connected to.
		/// Returns null if the client is not yet connected or has been disconnected.
		/// This returns a platform-specific identifier for use with rich presence. Feel free to refactor later if more
		/// info or more specific info is needed.
		/// </summary>
		// Token: 0x06004469 RID: 17513
		[NullableContext(2)]
		public abstract string GetRawLobbyIdentifier();

		/// <summary>
		/// Delegate to notify about events.
		/// </summary>
		// Token: 0x04001A83 RID: 6787
		protected INetClientHandler _handler;
	}
}
