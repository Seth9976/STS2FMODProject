using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent by the host when the modifiers in the custom run menu changes.
	/// </summary>
	// Token: 0x02000468 RID: 1128
	public struct LobbyModifiersChangedMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060045DB RID: 17883 RVA: 0x00217DCC File Offset: 0x00215FCC
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060045DC RID: 17884 RVA: 0x00217DCF File Offset: 0x00215FCF
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060045DD RID: 17885 RVA: 0x00217DD2 File Offset: 0x00215FD2
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060045DE RID: 17886 RVA: 0x00217DD5 File Offset: 0x00215FD5
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x00217DD8 File Offset: 0x00215FD8
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteList<SerializableModifier>(this.modifiers, 32);
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x00217DE8 File Offset: 0x00215FE8
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.modifiers = reader.ReadList<SerializableModifier>(32);
		}

		// Token: 0x04001B6C RID: 7020
		[Nullable(1)]
		public List<SerializableModifier> modifiers;
	}
}
