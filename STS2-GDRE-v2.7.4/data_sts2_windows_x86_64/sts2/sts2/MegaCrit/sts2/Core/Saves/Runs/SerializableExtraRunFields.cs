using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000C7 RID: 199
	public class SerializableExtraRunFields : IPacketSerializable
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x00044451 File Offset: 0x00042651
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x00044459 File Offset: 0x00042659
		[JsonPropertyName("started_with_neow")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool StartedWithNeow { get; set; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x00044462 File Offset: 0x00042662
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x0004446A File Offset: 0x0004266A
		[JsonPropertyName("test_subject_kills")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int TestSubjectKills { get; set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x00044473 File Offset: 0x00042673
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x0004447B File Offset: 0x0004267B
		[JsonPropertyName("freed_repy")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool FreedRepy { get; set; }

		// Token: 0x06000981 RID: 2433 RVA: 0x00044484 File Offset: 0x00042684
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteBool(this.StartedWithNeow);
			writer.WriteInt(this.TestSubjectKills, 32);
			writer.WriteBool(this.FreedRepy);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x000444AC File Offset: 0x000426AC
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.StartedWithNeow = reader.ReadBool();
			this.TestSubjectKills = reader.ReadInt(32);
			this.FreedRepy = reader.ReadBool();
		}
	}
}
