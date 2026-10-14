using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs.History
{
	// Token: 0x0200011F RID: 287
	[NullableContext(1)]
	[Nullable(0)]
	public struct CardEnchantmentHistoryEntry : IPacketSerializable
	{
		// Token: 0x06000E1C RID: 3612 RVA: 0x0005AEBA File Offset: 0x000590BA
		public CardEnchantmentHistoryEntry(CardModel card, ModelId enchantment)
		{
			this.Card = card.ToSerializable();
			this.Enchantment = enchantment;
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x0005AECF File Offset: 0x000590CF
		// (set) Token: 0x06000E1E RID: 3614 RVA: 0x0005AED7 File Offset: 0x000590D7
		[JsonPropertyName("card")]
		public SerializableCard Card { readonly get; set; }

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x0005AEE0 File Offset: 0x000590E0
		// (set) Token: 0x06000E20 RID: 3616 RVA: 0x0005AEE8 File Offset: 0x000590E8
		[JsonPropertyName("enchantment")]
		public ModelId Enchantment { readonly get; set; }

		// Token: 0x06000E21 RID: 3617 RVA: 0x0005AEF1 File Offset: 0x000590F1
		public void Serialize(PacketWriter writer)
		{
			writer.Write<SerializableCard>(this.Card);
			writer.WriteModelEntry(this.Enchantment);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0005AF0B File Offset: 0x0005910B
		public void Deserialize(PacketReader reader)
		{
			this.Card = reader.Read<SerializableCard>();
			this.Enchantment = reader.ReadModelIdAssumingType<EnchantmentModel>();
		}
	}
}
