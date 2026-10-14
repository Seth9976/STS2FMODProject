using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent by the host when the chosen ascension level changes.
	/// </summary>
	// Token: 0x02000465 RID: 1125
	public struct LobbyAscensionChangedMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060045C9 RID: 17865 RVA: 0x00217CFA File Offset: 0x00215EFA
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060045CA RID: 17866 RVA: 0x00217CFD File Offset: 0x00215EFD
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060045CB RID: 17867 RVA: 0x00217D00 File Offset: 0x00215F00
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060045CC RID: 17868 RVA: 0x00217D03 File Offset: 0x00215F03
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x00217D06 File Offset: 0x00215F06
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.ascension, 32);
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x00217D16 File Offset: 0x00215F16
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.ascension = reader.ReadInt(32);
		}

		// Token: 0x04001B67 RID: 7015
		public int ascension;
	}
}
