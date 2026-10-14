using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000C4 RID: 196
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class SerializableBadge : IPacketSerializable
	{
		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00043FDB File Offset: 0x000421DB
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x00043FE3 File Offset: 0x000421E3
		[RequiredMember]
		[JsonPropertyName("id")]
		public string Id { get; set; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x00043FEC File Offset: 0x000421EC
		// (set) Token: 0x0600095B RID: 2395 RVA: 0x00043FF4 File Offset: 0x000421F4
		[RequiredMember]
		[JsonPropertyName("rarity")]
		public BadgeRarity Rarity { get; set; }

		// Token: 0x0600095C RID: 2396 RVA: 0x00043FFD File Offset: 0x000421FD
		public void Serialize(PacketWriter writer)
		{
			writer.WriteString(this.Id);
			writer.WriteEnum<BadgeRarity>(this.Rarity);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00044017 File Offset: 0x00042217
		public void Deserialize(PacketReader reader)
		{
			this.Id = reader.ReadString();
			this.Rarity = reader.ReadEnum<BadgeRarity>();
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00044031 File Offset: 0x00042231
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public SerializableBadge()
		{
		}
	}
}
