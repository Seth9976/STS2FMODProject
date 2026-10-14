using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000D3 RID: 211
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableRunRngSet : IPacketSerializable
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0004587D File Offset: 0x00043A7D
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x00045885 File Offset: 0x00043A85
		[Nullable(2)]
		[JsonPropertyName("seed")]
		public string Seed
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0004588E File Offset: 0x00043A8E
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x00045896 File Offset: 0x00043A96
		[JsonPropertyName("rngs")]
		public Dictionary<RunRngType, SerializableRng> Rngs { get; set; } = new Dictionary<RunRngType, SerializableRng>();

		// Token: 0x06000A33 RID: 2611 RVA: 0x000458A0 File Offset: 0x00043AA0
		public void Serialize(PacketWriter writer)
		{
			writer.WriteString(this.Seed);
			writer.WriteInt(this.Rngs.Count, 8);
			foreach (RunRngType runRngType in Enum.GetValues<RunRngType>())
			{
				SerializableRng serializableRng;
				if (this.Rngs.TryGetValue(runRngType, out serializableRng))
				{
					writer.WriteEnum<RunRngType>(runRngType);
					writer.Write<SerializableRng>(serializableRng);
				}
			}
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00045904 File Offset: 0x00043B04
		public void Deserialize(PacketReader reader)
		{
			this.Seed = reader.ReadString();
			int num = reader.ReadInt(8);
			for (int i = 0; i < num; i++)
			{
				RunRngType runRngType = reader.ReadEnum<RunRngType>();
				SerializableRng serializableRng = reader.Read<SerializableRng>();
				this.Rngs[runRngType] = serializableRng;
			}
		}
	}
}
