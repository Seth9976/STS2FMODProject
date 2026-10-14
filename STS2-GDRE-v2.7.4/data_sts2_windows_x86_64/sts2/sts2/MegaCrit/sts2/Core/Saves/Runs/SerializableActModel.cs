using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000C3 RID: 195
	[NullableContext(2)]
	[Nullable(0)]
	public class SerializableActModel : IPacketSerializable
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00043F27 File Offset: 0x00042127
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x00043F2F File Offset: 0x0004212F
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00043F38 File Offset: 0x00042138
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x00043F40 File Offset: 0x00042140
		[Nullable(1)]
		[JsonPropertyName("rooms")]
		public SerializableRoomSet SerializableRooms
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00043F49 File Offset: 0x00042149
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x00043F51 File Offset: 0x00042151
		[JsonPropertyName("saved_map")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public SerializableActMap SavedMap { get; set; }

		// Token: 0x06000955 RID: 2389 RVA: 0x00043F5A File Offset: 0x0004215A
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteModelEntry(this.Id);
			writer.Write<SerializableRoomSet>(this.SerializableRooms);
			writer.WriteBool(this.SavedMap != null);
			if (this.SavedMap != null)
			{
				writer.Write<SerializableActMap>(this.SavedMap);
			}
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00043F98 File Offset: 0x00042198
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Id = reader.ReadModelIdAssumingType<ActModel>();
			this.SerializableRooms = reader.Read<SerializableRoomSet>();
			bool flag = reader.ReadBool();
			if (flag)
			{
				this.SavedMap = reader.Read<SerializableActMap>();
			}
		}
	}
}
