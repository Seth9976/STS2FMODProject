using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Multiplayer.Replay
{
	// Token: 0x02000454 RID: 1108
	[NullableContext(1)]
	[Nullable(0)]
	public class CombatReplay : IPacketSerializable
	{
		// Token: 0x06004554 RID: 17748 RVA: 0x00215F70 File Offset: 0x00214170
		public void Serialize(PacketWriter writer)
		{
			writer.WriteString(this.version);
			writer.WriteString(this.gitCommit);
			writer.WriteUInt(this.modelIdHash, 32);
			writer.WriteInt(this.choiceIds.Count, 32);
			foreach (uint num in this.choiceIds)
			{
				writer.WriteUInt(num, 32);
			}
			writer.WriteInt(this.rewardIds.Count, 32);
			foreach (int num2 in this.rewardIds)
			{
				writer.WriteInt(num2, 32);
			}
			writer.WriteUInt(this.nextActionId, 32);
			writer.WriteUInt(this.nextChecksumId, 32);
			writer.WriteUInt(this.nextHookId, 32);
			writer.Write<SerializableRun>(this.serializableRun);
			writer.WriteList<CombatReplayEvent>(this.events, 32);
			writer.WriteList<ReplayChecksumData>(this.checksumData, 32);
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x002160A8 File Offset: 0x002142A8
		public void Deserialize(PacketReader reader)
		{
			this.version = reader.ReadString();
			this.gitCommit = reader.ReadString();
			this.modelIdHash = reader.ReadUInt(32);
			int num = reader.ReadInt(32);
			for (int i = 0; i < num; i++)
			{
				this.choiceIds.Add(reader.ReadUInt(32));
			}
			int num2 = reader.ReadInt(32);
			for (int j = 0; j < num2; j++)
			{
				this.rewardIds.Add(reader.ReadInt(32));
			}
			this.nextActionId = reader.ReadUInt(32);
			this.nextChecksumId = reader.ReadUInt(32);
			this.nextHookId = reader.ReadUInt(32);
			this.serializableRun = reader.Read<SerializableRun>();
			this.events = reader.ReadList<CombatReplayEvent>(32);
			this.checksumData = reader.ReadList<ReplayChecksumData>(32);
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x00216180 File Offset: 0x00214380
		public CombatReplay Anonymized()
		{
			CombatReplay combatReplay = new CombatReplay();
			combatReplay.version = this.version;
			combatReplay.gitCommit = this.gitCommit;
			combatReplay.modelIdHash = this.modelIdHash;
			combatReplay.choiceIds = this.choiceIds;
			combatReplay.rewardIds = this.rewardIds;
			combatReplay.nextActionId = this.nextActionId;
			combatReplay.nextChecksumId = this.nextChecksumId;
			combatReplay.nextHookId = this.nextHookId;
			combatReplay.serializableRun = this.serializableRun.Anonymized();
			combatReplay.events = this.events.Select<CombatReplayEvent, CombatReplayEvent>((CombatReplayEvent e) => e.Anonymized()).ToList<CombatReplayEvent>();
			combatReplay.checksumData = this.checksumData.Select<ReplayChecksumData, ReplayChecksumData>((ReplayChecksumData c) => c.Anonymized()).ToList<ReplayChecksumData>();
			return combatReplay;
		}

		// Token: 0x04001B17 RID: 6935
		public string version;

		// Token: 0x04001B18 RID: 6936
		public string gitCommit;

		// Token: 0x04001B19 RID: 6937
		public uint modelIdHash;

		// Token: 0x04001B1A RID: 6938
		public List<uint> choiceIds = new List<uint>();

		// Token: 0x04001B1B RID: 6939
		public List<int> rewardIds = new List<int>();

		// Token: 0x04001B1C RID: 6940
		public uint nextActionId;

		// Token: 0x04001B1D RID: 6941
		public uint nextChecksumId;

		// Token: 0x04001B1E RID: 6942
		public uint nextHookId;

		// Token: 0x04001B1F RID: 6943
		public SerializableRun serializableRun;

		// Token: 0x04001B20 RID: 6944
		public List<CombatReplayEvent> events = new List<CombatReplayEvent>();

		// Token: 0x04001B21 RID: 6945
		public List<ReplayChecksumData> checksumData = new List<ReplayChecksumData>();
	}
}
