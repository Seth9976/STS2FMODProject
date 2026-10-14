using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	// Token: 0x02000472 RID: 1138
	public class CrystalSphereRewardsMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x0600461E RID: 17950 RVA: 0x0021816B File Offset: 0x0021636B
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x0600461F RID: 17951 RVA: 0x0021816E File Offset: 0x0021636E
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06004620 RID: 17952 RVA: 0x00218171 File Offset: 0x00216371
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06004621 RID: 17953 RVA: 0x00218174 File Offset: 0x00216374
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06004622 RID: 17954 RVA: 0x00218177 File Offset: 0x00216377
		// (set) Token: 0x06004623 RID: 17955 RVA: 0x0021817F File Offset: 0x0021637F
		public RunLocation Location { get; set; }

		// Token: 0x06004624 RID: 17956 RVA: 0x00218188 File Offset: 0x00216388
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<RunLocation>(this.Location);
			writer.WriteList<SerializableCrystalSphereItem>(this.rewards, 32);
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x002181A4 File Offset: 0x002163A4
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Location = reader.Read<RunLocation>();
			this.rewards = reader.ReadList<SerializableCrystalSphereItem>(32);
		}

		// Token: 0x04001B79 RID: 7033
		[Nullable(1)]
		public List<SerializableCrystalSphereItem> rewards = new List<SerializableCrystalSphereItem>();
	}
}
