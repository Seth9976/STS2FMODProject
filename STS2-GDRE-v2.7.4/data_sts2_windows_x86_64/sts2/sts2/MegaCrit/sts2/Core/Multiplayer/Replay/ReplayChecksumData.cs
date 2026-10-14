using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Multiplayer.Replay
{
	// Token: 0x02000458 RID: 1112
	[NullableContext(1)]
	[Nullable(0)]
	public struct ReplayChecksumData : IPacketSerializable
	{
		// Token: 0x06004567 RID: 17767 RVA: 0x00216AA5 File Offset: 0x00214CA5
		public void Serialize(PacketWriter writer)
		{
			writer.Write<NetChecksumData>(this.checksumData);
			writer.WriteString(this.context);
			writer.Write<NetFullCombatState>(this.fullState);
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x00216ACB File Offset: 0x00214CCB
		public void Deserialize(PacketReader reader)
		{
			this.checksumData = reader.Read<NetChecksumData>();
			this.context = reader.ReadString();
			this.fullState = reader.Read<NetFullCombatState>();
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x00216AF4 File Offset: 0x00214CF4
		public ReplayChecksumData Anonymized()
		{
			ReplayChecksumData replayChecksumData = this;
			replayChecksumData.fullState = this.fullState.Anonymized();
			return replayChecksumData;
		}

		// Token: 0x04001B38 RID: 6968
		public NetChecksumData checksumData;

		// Token: 0x04001B39 RID: 6969
		public string context;

		// Token: 0x04001B3A RID: 6970
		public NetFullCombatState fullState;
	}
}
