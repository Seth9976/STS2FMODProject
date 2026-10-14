using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent by a player when that player is ready to begin the run.
	/// </summary>
	// Token: 0x0200046A RID: 1130
	public struct LobbyPlayerSetReadyMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x060045E7 RID: 17895 RVA: 0x00217E20 File Offset: 0x00216020
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x060045E8 RID: 17896 RVA: 0x00217E23 File Offset: 0x00216023
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x060045E9 RID: 17897 RVA: 0x00217E26 File Offset: 0x00216026
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x060045EA RID: 17898 RVA: 0x00217E29 File Offset: 0x00216029
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x00217E2C File Offset: 0x0021602C
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteBool(this.ready);
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x00217E3A File Offset: 0x0021603A
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.ready = reader.ReadBool();
		}

		// Token: 0x04001B6E RID: 7022
		public bool ready;
	}
}
