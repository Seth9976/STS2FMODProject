using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	// Token: 0x02000462 RID: 1122
	public struct ClientRejoinRequestMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060045B6 RID: 17846 RVA: 0x002179FD File Offset: 0x00215BFD
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060045B7 RID: 17847 RVA: 0x00217A00 File Offset: 0x00215C00
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060045B8 RID: 17848 RVA: 0x00217A03 File Offset: 0x00215C03
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Info;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060045B9 RID: 17849 RVA: 0x00217A06 File Offset: 0x00215C06
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x00217A09 File Offset: 0x00215C09
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00217A0B File Offset: 0x00215C0B
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
		}
	}
}
