using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Represents an abstraction of the networking API used to send and receive messages.
	/// Host/join methods are specific to the game manager type on purpose, as those cannot be abstracted. However, once
	/// connected, this interface can be used as a generic way to send/receive messages regardless of whether we are a
	/// host or a client.
	/// </summary>
	// Token: 0x02000498 RID: 1176
	[NullableContext(2)]
	public interface INetGameService
	{
		/// <summary>
		/// The player ID of the local player.
		/// </summary>
		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06004744 RID: 18244
		ulong NetId { get; }

		/// <summary>
		/// True when the net service is in a state where it can send messages.
		/// </summary>
		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06004745 RID: 18245
		bool IsConnected { get; }

		/// <summary>
		/// True if the last call to SetGameLoading was called with true, false otherwise.
		/// </summary>
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06004746 RID: 18246
		bool IsGameLoading { get; }

		/// <summary>
		/// Called when the network connection is disconnected and the sockets are disposed.
		/// On host, this occurs when we stop hosting. On clients, this can occur when we quit, our connection drops, or the
		/// host disconnects from us.
		/// </summary>
		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06004747 RID: 18247
		// (remove) Token: 0x06004748 RID: 18248
		event Action<NetErrorInfo> Disconnected;

		/// <summary>
		/// The type of manager this is (host, client, or singleplayer).
		/// </summary>
		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06004749 RID: 18249
		NetGameType Type { get; }

		/// <summary>
		/// The networking backend we are using to connect to players.
		/// Note that if we ever consider cross-platform, this likely needs to be contained in the player ID, not here.
		/// </summary>
		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x0600474A RID: 18250
		PlatformType Platform { get; }

		/// <summary>
		/// Sends a message to a specific peer. Note that this should only be used as a host.
		/// </summary>
		/// <param name="message">The message to send.</param>
		/// <param name="playerId">The specific player to send to.</param>
		// Token: 0x0600474B RID: 18251
		[NullableContext(1)]
		void SendMessage<[Nullable(0)] T>(T message, ulong playerId) where T : INetMessage;

		/// <summary>
		/// Sends a message to all connected peers.
		/// On the host, this sends a message to all clients. On a client, this sends a message to only the host, but if the
		/// message has the broadcast flag set, the host will replicate it to all other clients.
		/// </summary>
		/// <param name="message">The message to send.</param>
		// Token: 0x0600474C RID: 18252
		[NullableContext(1)]
		void SendMessage<[Nullable(0)] T>(T message) where T : INetMessage;

		/// <summary>
		/// Registers a message handler delegate which will be called when a message of type T is received.
		/// </summary>
		/// <param name="messageHandlerDelegate">The delegate to call.</param>
		// Token: 0x0600474D RID: 18253
		[NullableContext(1)]
		void RegisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> messageHandlerDelegate) where T : INetMessage;

		/// <summary>
		/// Unregisters a message handler delegate that was previously registered. Does nothing if it was not registered.
		/// </summary>
		/// <param name="messageHandlerDelegate">The delegate to unregister.</param>
		// Token: 0x0600474E RID: 18254
		[NullableContext(1)]
		void UnregisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> messageHandlerDelegate) where T : INetMessage;

		/// <summary>
		/// Method which should be called at regular intervals.
		/// Messages, connections, or disconnections will not be processed unless this is called.
		/// </summary>
		// Token: 0x0600474F RID: 18255
		void Update();

		/// <summary>
		/// Disconnects us from all connected peers.
		/// </summary>
		/// <param name="reason">The reason to pass to the remote end to indicate why we quit.</param>
		/// <param name="now">If false, then the connection is allowed to wait until messages are finished sending.
		/// Otherwise, the connection is terminated immediately.</param>
		// Token: 0x06004750 RID: 18256
		void Disconnect(NetError reason, bool now = false);

		/// <summary>
		/// Returns the connection statistics for a given peer.
		/// </summary>
		// Token: 0x06004751 RID: 18257
		ConnectionStats GetStatsForPeer(ulong peerId);

		/// <summary>
		/// DO NOT use this directly in most circumstances! Instead, use NetLoadingHandle.
		/// Tells the net service that the game is in a loading state, and that Update may be called irregularly.
		/// See <see cref="M:MegaCrit.Sts2.Core.Multiplayer.Quality.NetQualityTracker.SetIsLoading(System.Boolean)" /> for why this is needed.
		/// </summary>
		// Token: 0x06004752 RID: 18258
		void SetGameLoading(bool isLoading);

		/// <summary>
		/// Begins buffering all messages.
		/// When the game is transitioning from one state to another, it will unregister and register message handlers. If
		/// this takes time, then there's a chance that a message will be sent during this window that the game doesn't know
		/// how to handle yet. This method should be called with true after a sync point and then false after the game is
		/// ready to handle messages again.
		/// </summary>
		// Token: 0x06004753 RID: 18259
		void SetBufferMessages(bool bufferMessages);

		/// <summary>
		/// Returns a unique identifier for the lobby that this service is connected to.
		/// Returns null if the net service is not yet connected.
		/// This returns a platform-specific identifier for use with rich presence. Feel free to refactor later if more
		/// info or more specific info is needed.
		/// </summary>
		// Token: 0x06004754 RID: 18260
		string GetRawLobbyIdentifier();
	}
}
