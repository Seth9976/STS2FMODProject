using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000D0 RID: 208
	[NullableContext(2)]
	[Nullable(0)]
	public class SerializableRoom : IPacketSerializable
	{
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0004519D File Offset: 0x0004339D
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x000451A5 File Offset: 0x000433A5
		[JsonPropertyName("room_type")]
		public RoomType RoomType { get; set; }

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x000451AE File Offset: 0x000433AE
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x000451B6 File Offset: 0x000433B6
		[JsonPropertyName("encounter_id")]
		public ModelId EncounterId { get; set; }

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x000451BF File Offset: 0x000433BF
		// (set) Token: 0x060009FD RID: 2557 RVA: 0x000451C7 File Offset: 0x000433C7
		[JsonPropertyName("event_id")]
		public ModelId EventId { get; set; }

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x000451D0 File Offset: 0x000433D0
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x000451D8 File Offset: 0x000433D8
		[JsonPropertyName("is_pre_finished")]
		public bool IsPreFinished { get; set; }

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x000451E1 File Offset: 0x000433E1
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x000451E9 File Offset: 0x000433E9
		[JsonPropertyName("reward_proportion")]
		public float GoldProportion { get; set; }

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x000451F2 File Offset: 0x000433F2
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x000451FA File Offset: 0x000433FA
		[Nullable(1)]
		[JsonPropertyName("extra_rewards")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public Dictionary<ulong, List<SerializableReward>> ExtraRewards
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = new Dictionary<ulong, List<SerializableReward>>();

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x00045203 File Offset: 0x00043403
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x0004520B File Offset: 0x0004340B
		[JsonPropertyName("parent_event_id")]
		public ModelId ParentEventId { get; set; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x00045214 File Offset: 0x00043414
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0004521C File Offset: 0x0004341C
		[JsonPropertyName("should_resume_parent_event")]
		public bool ShouldResumeParentEvent { get; set; } = true;

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x00045225 File Offset: 0x00043425
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x0004522D File Offset: 0x0004342D
		[Nullable(1)]
		[JsonPropertyName("encounter_state")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public Dictionary<string, string> EncounterState
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = new Dictionary<string, string>();

		// Token: 0x06000A0A RID: 2570 RVA: 0x00045238 File Offset: 0x00043438
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt((int)this.RoomType, 32);
			writer.WriteBool(this.EncounterId != null);
			if (this.EncounterId != null)
			{
				writer.WriteModelEntry(this.EncounterId);
			}
			writer.WriteBool(this.EventId != null);
			if (this.EventId != null)
			{
				writer.WriteModelEntry(this.EventId);
			}
			writer.WriteFloat(this.GoldProportion, null);
			writer.WriteBool(this.IsPreFinished);
			writer.WriteInt(this.ExtraRewards.Count, 32);
			foreach (KeyValuePair<ulong, List<SerializableReward>> keyValuePair in this.ExtraRewards)
			{
				ulong num;
				List<SerializableReward> list;
				keyValuePair.Deconstruct(out num, out list);
				ulong num2 = num;
				List<SerializableReward> list2 = list;
				writer.WriteULong(num2, 64);
				writer.WriteList<SerializableReward>(list2, 32);
			}
			writer.WriteBool(this.ParentEventId != null);
			if (this.ParentEventId != null)
			{
				writer.WriteModelEntry(this.ParentEventId);
			}
			writer.WriteBool(this.ShouldResumeParentEvent);
			writer.WriteInt(this.EncounterState.Count, 32);
			foreach (KeyValuePair<string, string> keyValuePair2 in this.EncounterState)
			{
				string text;
				string text2;
				keyValuePair2.Deconstruct(out text, out text2);
				string text3 = text;
				string text4 = text2;
				writer.WriteString(text3);
				writer.WriteString(text4);
			}
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000453F0 File Offset: 0x000435F0
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.RoomType = (RoomType)reader.ReadInt(32);
			bool flag = reader.ReadBool();
			if (flag)
			{
				this.EncounterId = reader.ReadModelIdAssumingType<EncounterModel>();
			}
			bool flag2 = reader.ReadBool();
			if (flag2)
			{
				this.EventId = reader.ReadModelIdAssumingType<EventModel>();
			}
			this.GoldProportion = reader.ReadFloat(null);
			this.IsPreFinished = reader.ReadBool();
			int num = reader.ReadInt(32);
			for (int i = 0; i < num; i++)
			{
				ulong num2 = reader.ReadULong(64);
				this.ExtraRewards[num2] = reader.ReadList<SerializableReward>(32);
			}
			bool flag3 = reader.ReadBool();
			if (flag3)
			{
				this.ParentEventId = reader.ReadModelIdAssumingType<EventModel>();
			}
			this.ShouldResumeParentEvent = reader.ReadBool();
			int num3 = reader.ReadInt(32);
			this.EncounterState = new Dictionary<string, string>(num3);
			for (int j = 0; j < num3; j++)
			{
				string text = reader.ReadString();
				string text2 = reader.ReadString();
				this.EncounterState[text] = text2;
			}
		}
	}
}
