using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000D1 RID: 209
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableRoomSet : IPacketSerializable
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0004551F File Offset: 0x0004371F
		// (set) Token: 0x06000A0E RID: 2574 RVA: 0x00045527 File Offset: 0x00043727
		[JsonPropertyName("event_ids")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> EventIds { get; set; }

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x00045530 File Offset: 0x00043730
		// (set) Token: 0x06000A10 RID: 2576 RVA: 0x00045538 File Offset: 0x00043738
		[JsonPropertyName("events_visited")]
		public int EventsVisited { get; set; }

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x00045541 File Offset: 0x00043741
		// (set) Token: 0x06000A12 RID: 2578 RVA: 0x00045549 File Offset: 0x00043749
		[JsonPropertyName("normal_encounter_ids")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> NormalEncounterIds { get; set; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x00045552 File Offset: 0x00043752
		// (set) Token: 0x06000A14 RID: 2580 RVA: 0x0004555A File Offset: 0x0004375A
		[JsonPropertyName("normal_encounters_visited")]
		public int NormalEncountersVisited { get; set; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x00045563 File Offset: 0x00043763
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0004556B File Offset: 0x0004376B
		[JsonPropertyName("elite_encounter_ids")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> EliteEncounterIds { get; set; }

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00045574 File Offset: 0x00043774
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x0004557C File Offset: 0x0004377C
		[JsonPropertyName("elite_encounters_visited")]
		public int EliteEncountersVisited { get; set; }

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00045585 File Offset: 0x00043785
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x0004558D File Offset: 0x0004378D
		[JsonPropertyName("boss_encounters_visited")]
		public int BossEncountersVisited { get; set; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00045596 File Offset: 0x00043796
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x0004559E File Offset: 0x0004379E
		[Nullable(2)]
		[JsonPropertyName("boss_id")]
		public ModelId BossId
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x000455A7 File Offset: 0x000437A7
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x000455AF File Offset: 0x000437AF
		[Nullable(2)]
		[JsonPropertyName("second_boss_id")]
		public ModelId SecondBossId
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x000455B8 File Offset: 0x000437B8
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x000455C0 File Offset: 0x000437C0
		[Nullable(2)]
		[JsonPropertyName("ancient_id")]
		public ModelId AncientId
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x000455CC File Offset: 0x000437CC
		public void Serialize(PacketWriter writer)
		{
			writer.WriteModelEntriesInList(this.EventIds);
			writer.WriteInt(this.EventsVisited, 32);
			writer.WriteModelEntriesInList(this.NormalEncounterIds);
			writer.WriteInt(this.NormalEncountersVisited, 32);
			writer.WriteModelEntriesInList(this.EliteEncounterIds);
			writer.WriteInt(this.EliteEncountersVisited, 32);
			writer.WriteInt(this.BossEncountersVisited, 32);
			writer.WriteBool(this.BossId != null);
			if (this.BossId != null)
			{
				writer.WriteModelEntry(this.BossId);
			}
			writer.WriteBool(this.SecondBossId != null);
			if (this.SecondBossId != null)
			{
				writer.WriteModelEntry(this.SecondBossId);
			}
			writer.WriteBool(this.AncientId != null);
			if (this.AncientId != null)
			{
				writer.WriteModelEntry(this.AncientId);
			}
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x000456BC File Offset: 0x000438BC
		public void Deserialize(PacketReader reader)
		{
			this.EventIds = reader.ReadModelIdListAssumingType<EventModel>();
			this.EventsVisited = reader.ReadInt(32);
			this.NormalEncounterIds = reader.ReadModelIdListAssumingType<EncounterModel>();
			this.NormalEncountersVisited = reader.ReadInt(32);
			this.EliteEncounterIds = reader.ReadModelIdListAssumingType<EncounterModel>();
			this.EliteEncountersVisited = reader.ReadInt(32);
			this.BossEncountersVisited = reader.ReadInt(32);
			if (reader.ReadBool())
			{
				this.BossId = reader.ReadModelIdAssumingType<EncounterModel>();
			}
			if (reader.ReadBool())
			{
				this.SecondBossId = reader.ReadModelIdAssumingType<EncounterModel>();
			}
			if (reader.ReadBool())
			{
				this.AncientId = reader.ReadModelIdAssumingType<AncientEventModel>();
			}
		}
	}
}
