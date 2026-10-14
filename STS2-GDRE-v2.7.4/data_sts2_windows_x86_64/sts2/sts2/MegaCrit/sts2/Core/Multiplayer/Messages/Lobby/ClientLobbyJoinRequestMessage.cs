using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent from a newly connected client to the host as the first message in the joining flow.
	/// </summary>
	// Token: 0x02000460 RID: 1120
	public struct ClientLobbyJoinRequestMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060045A9 RID: 17833 RVA: 0x00217837 File Offset: 0x00215A37
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060045AA RID: 17834 RVA: 0x0021783A File Offset: 0x00215A3A
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060045AB RID: 17835 RVA: 0x0021783D File Offset: 0x00215A3D
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Info;
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060045AC RID: 17836 RVA: 0x00217840 File Offset: 0x00215A40
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x00217843 File Offset: 0x00215A43
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.maxAscensionUnlocked, 32);
			writer.Write<SerializableUnlockState>(this.unlockState);
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x0021785F File Offset: 0x00215A5F
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.maxAscensionUnlocked = reader.ReadInt(32);
			this.unlockState = reader.Read<SerializableUnlockState>();
		}

		// Token: 0x04001B56 RID: 6998
		public int maxAscensionUnlocked;

		// Token: 0x04001B57 RID: 6999
		[Nullable(1)]
		public SerializableUnlockState unlockState;
	}
}
