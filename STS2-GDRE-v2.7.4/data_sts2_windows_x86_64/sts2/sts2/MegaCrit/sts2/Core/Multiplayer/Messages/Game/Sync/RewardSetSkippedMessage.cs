using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync
{
	/// <summary>
	/// Message sent when a player chooses to skip the remaining rewards in a set of rewards.
	/// </summary>
	// Token: 0x02000486 RID: 1158
	public class RewardSetSkippedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x060046A7 RID: 18087 RVA: 0x00218CAD File Offset: 0x00216EAD
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x060046A8 RID: 18088 RVA: 0x00218CB0 File Offset: 0x00216EB0
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x060046A9 RID: 18089 RVA: 0x00218CB3 File Offset: 0x00216EB3
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Debug;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x060046AA RID: 18090 RVA: 0x00218CB6 File Offset: 0x00216EB6
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x060046AB RID: 18091 RVA: 0x00218CB9 File Offset: 0x00216EB9
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x00218CC1 File Offset: 0x00216EC1
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<RunLocation>(this.location);
			writer.WriteInt(this.setId, 32);
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x00218CDD File Offset: 0x00216EDD
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.location = reader.Read<RunLocation>();
			this.setId = reader.ReadInt(32);
		}

		// Token: 0x04001BAA RID: 7082
		public RunLocation location;

		// Token: 0x04001BAB RID: 7083
		public int setId;
	}
}
