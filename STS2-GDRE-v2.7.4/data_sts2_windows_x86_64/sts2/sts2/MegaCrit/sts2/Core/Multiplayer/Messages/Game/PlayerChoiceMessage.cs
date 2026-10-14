using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Broadcast from a peer to all other peers when that peer has made a choice in combat.
	/// </summary>
	// Token: 0x02000476 RID: 1142
	public struct PlayerChoiceMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06004639 RID: 17977 RVA: 0x00218301 File Offset: 0x00216501
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x0600463A RID: 17978 RVA: 0x00218304 File Offset: 0x00216504
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x0600463B RID: 17979 RVA: 0x00218307 File Offset: 0x00216507
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x0600463C RID: 17980 RVA: 0x0021830A File Offset: 0x0021650A
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x0021830D File Offset: 0x0021650D
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteUInt(this.choiceId, 32);
			writer.Write<NetPlayerChoiceResult>(this.result);
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x00218329 File Offset: 0x00216529
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.choiceId = reader.ReadUInt(32);
			this.result = reader.Read<NetPlayerChoiceResult>();
		}

		// Token: 0x04001B80 RID: 7040
		public uint choiceId;

		// Token: 0x04001B81 RID: 7041
		public NetPlayerChoiceResult result;
	}
}
