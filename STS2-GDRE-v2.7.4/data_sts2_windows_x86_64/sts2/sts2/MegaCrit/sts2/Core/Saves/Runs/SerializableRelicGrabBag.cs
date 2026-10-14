using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000CE RID: 206
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableRelicGrabBag : IPacketSerializable
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x00044EE7 File Offset: 0x000430E7
		// (set) Token: 0x060009DD RID: 2525 RVA: 0x00044EEF File Offset: 0x000430EF
		[JsonPropertyName("relic_id_lists")]
		public Dictionary<RelicRarity, List<ModelId>> RelicIdLists { get; set; } = new Dictionary<RelicRarity, List<ModelId>>();

		// Token: 0x060009DE RID: 2526 RVA: 0x00044EF8 File Offset: 0x000430F8
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.RelicIdLists.Count, 32);
			foreach (RelicRarity relicRarity in Enum.GetValues<RelicRarity>())
			{
				List<ModelId> list;
				if (this.RelicIdLists.TryGetValue(relicRarity, out list))
				{
					writer.WriteEnum<RelicRarity>(relicRarity);
					writer.WriteModelEntriesInList(list);
				}
			}
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00044F50 File Offset: 0x00043150
		public void Deserialize(PacketReader reader)
		{
			int num = reader.ReadInt(32);
			for (int i = 0; i < num; i++)
			{
				RelicRarity relicRarity = reader.ReadEnum<RelicRarity>();
				List<ModelId> list = reader.ReadModelIdListAssumingType<RelicModel>();
				this.RelicIdLists[relicRarity] = list;
			}
		}
	}
}
