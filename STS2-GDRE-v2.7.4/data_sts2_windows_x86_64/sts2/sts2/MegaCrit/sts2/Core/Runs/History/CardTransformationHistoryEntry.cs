using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs.History
{
	// Token: 0x02000120 RID: 288
	[NullableContext(1)]
	[Nullable(0)]
	public struct CardTransformationHistoryEntry : IPacketSerializable
	{
		// Token: 0x06000E23 RID: 3619 RVA: 0x0005AF25 File Offset: 0x00059125
		public CardTransformationHistoryEntry(CardModel originalCard, CardModel finalCard)
		{
			this.OriginalCard = originalCard.ToSerializable();
			this.FinalCard = finalCard.ToSerializable();
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x0005AF3F File Offset: 0x0005913F
		// (set) Token: 0x06000E25 RID: 3621 RVA: 0x0005AF47 File Offset: 0x00059147
		[JsonPropertyName("original_card")]
		public SerializableCard OriginalCard { readonly get; set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0005AF50 File Offset: 0x00059150
		// (set) Token: 0x06000E27 RID: 3623 RVA: 0x0005AF58 File Offset: 0x00059158
		[JsonPropertyName("final_card")]
		public SerializableCard FinalCard { readonly get; set; }

		// Token: 0x06000E28 RID: 3624 RVA: 0x0005AF61 File Offset: 0x00059161
		public void Serialize(PacketWriter writer)
		{
			writer.Write<SerializableCard>(this.OriginalCard);
			writer.Write<SerializableCard>(this.FinalCard);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0005AF7B File Offset: 0x0005917B
		public void Deserialize(PacketReader reader)
		{
			this.OriginalCard = reader.Read<SerializableCard>();
			this.FinalCard = reader.Read<SerializableCard>();
		}
	}
}
