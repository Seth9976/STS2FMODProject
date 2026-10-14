using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000CC RID: 204
	[NullableContext(2)]
	[Nullable(0)]
	public class SerializablePotion : IPacketSerializable
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x00044D87 File Offset: 0x00042F87
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x00044D8F File Offset: 0x00042F8F
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x00044D98 File Offset: 0x00042F98
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x00044DA0 File Offset: 0x00042FA0
		[JsonPropertyName("slot_index")]
		public int SlotIndex { get; set; }

		// Token: 0x060009D0 RID: 2512 RVA: 0x00044DA9 File Offset: 0x00042FA9
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteModelEntry(this.Id);
			writer.WriteInt(this.SlotIndex, 4);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00044DC4 File Offset: 0x00042FC4
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Id = reader.ReadModelIdAssumingType<PotionModel>();
			this.SlotIndex = reader.ReadInt(4);
		}
	}
}
