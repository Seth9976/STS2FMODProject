using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Checksums
{
	/// <summary>
	/// Sent when a state divergence is detected due to the host and client checksums mismatching.
	/// First this is sent from host to client, then it is sent from client to host.
	/// </summary>
	// Token: 0x02000491 RID: 1169
	public struct StateDivergenceMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x060046F6 RID: 18166 RVA: 0x002190F0 File Offset: 0x002172F0
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x060046F7 RID: 18167 RVA: 0x002190F3 File Offset: 0x002172F3
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x060046F8 RID: 18168 RVA: 0x002190F6 File Offset: 0x002172F6
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Info;
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x060046F9 RID: 18169 RVA: 0x002190F9 File Offset: 0x002172F9
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x002190FC File Offset: 0x002172FC
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<NetChecksumData>(this.senderChecksum);
			writer.Write<NetFullCombatState>(this.senderCombatState);
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x00219116 File Offset: 0x00217316
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.senderChecksum = reader.Read<NetChecksumData>();
			this.senderCombatState = reader.Read<NetFullCombatState>();
		}

		// Token: 0x04001BBE RID: 7102
		public NetChecksumData senderChecksum;

		// Token: 0x04001BBF RID: 7103
		[Nullable(1)]
		public NetFullCombatState senderCombatState;
	}
}
