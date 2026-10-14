using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs.History
{
	/// <summary>
	/// Represents a room in a map point.
	/// Since each map point can contain multiple rooms (e.g. <see cref="T:MegaCrit.Sts2.Core.Models.Events.DenseVegetation" />'s
	/// <see cref="T:MegaCrit.Sts2.Core.Models.Encounters.DenseVegetationEventEncounter" />), we need multiple of these per map point.
	/// </summary>
	// Token: 0x02000123 RID: 291
	[NullableContext(1)]
	[Nullable(0)]
	public class MapPointRoomHistoryEntry : IPacketSerializable
	{
		/// <summary>
		/// The type of room this is. Tells us what type of model ModelId refers to.
		/// </summary>
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x0005B45B File Offset: 0x0005965B
		// (set) Token: 0x06000E40 RID: 3648 RVA: 0x0005B463 File Offset: 0x00059663
		[JsonPropertyName("room_type")]
		public RoomType RoomType { get; set; }

		/// <summary>
		/// The model id of the encounter or event at each room in this map point. ie CEREMONIAL_BEAST
		/// </summary>
		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x0005B46C File Offset: 0x0005966C
		// (set) Token: 0x06000E42 RID: 3650 RVA: 0x0005B474 File Offset: 0x00059674
		[Nullable(2)]
		[JsonPropertyName("model_id")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotTypeDefault)]
		public ModelId ModelId
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		/// <summary>
		/// The model IDs of the monsters in the encounter, if this was an encounter.
		/// This contains the actual monsters that the encounter resolved to, if the encounter has random monsters.
		/// </summary>
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0005B47D File Offset: 0x0005967D
		// (set) Token: 0x06000E44 RID: 3652 RVA: 0x0005B485 File Offset: 0x00059685
		[JsonPropertyName("monster_ids")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> MonsterIds { get; set; } = new List<ModelId>();

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x0005B48E File Offset: 0x0005968E
		// (set) Token: 0x06000E46 RID: 3654 RVA: 0x0005B496 File Offset: 0x00059696
		[JsonPropertyName("turns_taken")]
		public int TurnsTaken { get; set; }

		// Token: 0x06000E47 RID: 3655 RVA: 0x0005B4A0 File Offset: 0x000596A0
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<RoomType>(this.RoomType);
			writer.WriteBool(this.ModelId != null);
			if (this.ModelId != null)
			{
				writer.WriteFullModelId(this.ModelId);
			}
			writer.WriteModelEntriesInList(this.MonsterIds);
			writer.WriteInt(this.TurnsTaken, 32);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0005B4FF File Offset: 0x000596FF
		public void Deserialize(PacketReader reader)
		{
			this.RoomType = reader.ReadEnum<RoomType>();
			if (reader.ReadBool())
			{
				this.ModelId = reader.ReadFullModelId();
			}
			this.MonsterIds = reader.ReadModelIdListAssumingType<MonsterModel>();
			this.TurnsTaken = reader.ReadInt(32);
		}
	}
}
