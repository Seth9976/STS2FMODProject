using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	// Token: 0x02000463 RID: 1123
	public struct ClientRejoinResponseMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060045BC RID: 17852 RVA: 0x00217A0D File Offset: 0x00215C0D
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060045BD RID: 17853 RVA: 0x00217A10 File Offset: 0x00215C10
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060045BE RID: 17854 RVA: 0x00217A13 File Offset: 0x00215C13
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Info;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060045BF RID: 17855 RVA: 0x00217A16 File Offset: 0x00215C16
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x00217A19 File Offset: 0x00215C19
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<SerializableRun>(this.serializableRun);
			writer.WriteBool(this.combatState != null);
			if (this.combatState != null)
			{
				writer.Write<NetFullCombatState>(this.combatState);
			}
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x00217A4A File Offset: 0x00215C4A
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.serializableRun = reader.Read<SerializableRun>();
			if (reader.ReadBool())
			{
				this.combatState = reader.Read<NetFullCombatState>();
			}
		}

		// Token: 0x04001B5D RID: 7005
		[Nullable(1)]
		public SerializableRun serializableRun;

		// Token: 0x04001B5E RID: 7006
		[Nullable(2)]
		public NetFullCombatState combatState;
	}
}
