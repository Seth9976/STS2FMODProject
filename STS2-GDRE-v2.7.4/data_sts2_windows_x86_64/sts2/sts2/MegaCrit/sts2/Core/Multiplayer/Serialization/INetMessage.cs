using System;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.SourceGeneration;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x02000448 RID: 1096
	[GenerateSubtypes]
	public interface INetMessage : IPacketSerializable
	{
		/// <summary>
		/// If set, when this message is sent to the host, it will be echoed to all clients.
		/// </summary>
		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060044D8 RID: 17624
		bool ShouldBroadcast { get; }

		/// <summary>
		/// Whether this message is transferred reliably or unreliably.
		/// </summary>
		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060044D9 RID: 17625
		NetTransferMode Mode { get; }

		/// <summary>
		/// What log level to use when logging info about this message.
		/// Almost all messages should be VeryDebug - only set this to Info for messages that are logged very infrequently.
		/// </summary>
		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060044DA RID: 17626
		LogLevel LogLevel { get; }

		/// <summary>
		/// Determines whether a message is buffered when NetMessageBus.SetBufferMessages is called with true.
		/// Should almost always be true. Only set to false for messages that can always be properly handled when a
		/// connection is valid.
		/// </summary>
		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060044DB RID: 17627
		bool ShouldBuffer { get; }
	}
}
