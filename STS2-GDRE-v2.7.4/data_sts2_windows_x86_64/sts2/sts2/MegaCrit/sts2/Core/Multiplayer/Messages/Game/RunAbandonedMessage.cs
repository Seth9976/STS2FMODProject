using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Sent by the host to all clients when the host abandons the run.
	/// The clients should also abandon the run.
	/// </summary>
	// Token: 0x0200047B RID: 1147
	public struct RunAbandonedMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x0600465C RID: 18012 RVA: 0x00218560 File Offset: 0x00216760
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x0600465D RID: 18013 RVA: 0x00218563 File Offset: 0x00216763
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x0600465E RID: 18014 RVA: 0x00218566 File Offset: 0x00216766
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Debug;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x0600465F RID: 18015 RVA: 0x00218569 File Offset: 0x00216769
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x0021856C File Offset: 0x0021676C
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x0021856E File Offset: 0x0021676E
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
		}
	}
}
