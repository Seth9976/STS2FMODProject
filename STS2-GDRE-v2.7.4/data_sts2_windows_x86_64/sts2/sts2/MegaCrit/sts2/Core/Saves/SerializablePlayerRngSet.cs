using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x020000B0 RID: 176
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializablePlayerRngSet : IPacketSerializable
	{
		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00040409 File Offset: 0x0003E609
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x00040411 File Offset: 0x0003E611
		[JsonPropertyName("seed")]
		public ulong Seed { get; set; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0004041A File Offset: 0x0003E61A
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x00040422 File Offset: 0x0003E622
		[JsonPropertyName("rngs")]
		public Dictionary<PlayerRngType, SerializableRng> Rngs { get; set; } = new Dictionary<PlayerRngType, SerializableRng>();

		// Token: 0x0600081D RID: 2077 RVA: 0x0004042C File Offset: 0x0003E62C
		public void Serialize(PacketWriter writer)
		{
			writer.WriteULong(this.Seed, 64);
			writer.WriteInt(this.Rngs.Count, 8);
			foreach (PlayerRngType playerRngType in Enum.GetValues<PlayerRngType>())
			{
				SerializableRng serializableRng;
				if (this.Rngs.TryGetValue(playerRngType, out serializableRng))
				{
					writer.WriteEnum<PlayerRngType>(playerRngType);
					writer.Write<SerializableRng>(serializableRng);
				}
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00040490 File Offset: 0x0003E690
		public void Deserialize(PacketReader reader)
		{
			this.Seed = reader.ReadULong(64);
			int num = reader.ReadInt(8);
			for (int i = 0; i < num; i++)
			{
				PlayerRngType playerRngType = reader.ReadEnum<PlayerRngType>();
				SerializableRng serializableRng = reader.Read<SerializableRng>();
				this.Rngs[playerRngType] = serializableRng;
			}
		}
	}
}
