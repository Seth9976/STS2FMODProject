using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync
{
	/// <summary>
	/// Message that is sent when a reward is selected from a rewards screen.
	/// This is sent BEFORE obtaining a reward. For instance, when a player selects a card reward, this message is sent
	/// before the player finishes selecting the card they wish to choose. The card selection occurs as a separate process.
	/// </summary>
	// Token: 0x02000485 RID: 1157
	public class RewardSelectedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x0600469F RID: 18079 RVA: 0x00218C3F File Offset: 0x00216E3F
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x060046A0 RID: 18080 RVA: 0x00218C42 File Offset: 0x00216E42
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x060046A1 RID: 18081 RVA: 0x00218C45 File Offset: 0x00216E45
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Debug;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x060046A2 RID: 18082 RVA: 0x00218C48 File Offset: 0x00216E48
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x060046A3 RID: 18083 RVA: 0x00218C4B File Offset: 0x00216E4B
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x00218C53 File Offset: 0x00216E53
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<RunLocation>(this.location);
			writer.WriteInt(this.setId, 32);
			writer.WriteInt(this.rewardIndex, 8);
		}

		// Token: 0x060046A5 RID: 18085 RVA: 0x00218C7C File Offset: 0x00216E7C
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.location = reader.Read<RunLocation>();
			this.setId = reader.ReadInt(32);
			this.rewardIndex = reader.ReadInt(8);
		}

		// Token: 0x04001BA7 RID: 7079
		public RunLocation location;

		// Token: 0x04001BA8 RID: 7080
		public int setId;

		// Token: 0x04001BA9 RID: 7081
		public int rewardIndex;
	}
}
