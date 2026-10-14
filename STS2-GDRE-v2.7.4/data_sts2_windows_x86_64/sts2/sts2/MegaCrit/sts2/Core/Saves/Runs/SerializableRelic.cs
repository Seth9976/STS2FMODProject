using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000CD RID: 205
	[NullableContext(2)]
	[Nullable(0)]
	public class SerializableRelic : IPacketSerializable
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00044DE7 File Offset: 0x00042FE7
		// (set) Token: 0x060009D4 RID: 2516 RVA: 0x00044DEF File Offset: 0x00042FEF
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x00044DF8 File Offset: 0x00042FF8
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x00044E00 File Offset: 0x00043000
		[JsonPropertyName("props")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public SavedProperties Props { get; set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x00044E09 File Offset: 0x00043009
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x00044E11 File Offset: 0x00043011
		[JsonPropertyName("floor_added_to_deck")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? FloorAddedToDeck { get; set; }

		// Token: 0x060009D9 RID: 2521 RVA: 0x00044E1C File Offset: 0x0004301C
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteModelEntry(this.Id);
			writer.WriteBool(this.Props != null);
			if (this.Props != null)
			{
				writer.Write<SavedProperties>(this.Props);
			}
			writer.WriteBool(this.FloorAddedToDeck != null);
			if (this.FloorAddedToDeck != null)
			{
				writer.WriteInt(this.FloorAddedToDeck.Value, 8);
			}
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00044E94 File Offset: 0x00043094
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Id = reader.ReadModelIdAssumingType<RelicModel>();
			bool flag = reader.ReadBool();
			if (flag)
			{
				this.Props = reader.Read<SavedProperties>();
			}
			bool flag2 = reader.ReadBool();
			if (flag2)
			{
				this.FloorAddedToDeck = new int?(reader.ReadInt(8));
			}
		}
	}
}
