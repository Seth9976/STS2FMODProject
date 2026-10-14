using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Unlocks
{
	// Token: 0x02000034 RID: 52
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableUnlockState : IPacketSerializable
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00011389 File Offset: 0x0000F589
		// (set) Token: 0x06000217 RID: 535 RVA: 0x00011391 File Offset: 0x0000F591
		[JsonPropertyName("unlocked_epochs")]
		public List<string> UnlockedEpochs { get; set; } = new List<string>();

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0001139A File Offset: 0x0000F59A
		// (set) Token: 0x06000219 RID: 537 RVA: 0x000113A2 File Offset: 0x0000F5A2
		[JsonPropertyName("encounters_seen")]
		public List<ModelId> EncountersSeen { get; set; } = new List<ModelId>();

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600021A RID: 538 RVA: 0x000113AB File Offset: 0x0000F5AB
		// (set) Token: 0x0600021B RID: 539 RVA: 0x000113B3 File Offset: 0x0000F5B3
		[JsonPropertyName("number_of_runs")]
		public int NumberOfRuns { get; set; }

		// Token: 0x0600021C RID: 540 RVA: 0x000113BC File Offset: 0x0000F5BC
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.UnlockedEpochs.Count, 32);
			foreach (string text in this.UnlockedEpochs)
			{
				writer.WriteEpochId(text);
			}
			writer.WriteModelEntriesInList(this.EncountersSeen);
			writer.WriteInt(this.NumberOfRuns, 16);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0001143C File Offset: 0x0000F63C
		public void Deserialize(PacketReader reader)
		{
			int num = reader.ReadInt(32);
			for (int i = 0; i < num; i++)
			{
				this.UnlockedEpochs.Add(reader.ReadEpochId());
			}
			this.EncountersSeen = reader.ReadModelIdListAssumingType<EncounterModel>();
			this.NumberOfRuns = reader.ReadInt(16);
		}
	}
}
