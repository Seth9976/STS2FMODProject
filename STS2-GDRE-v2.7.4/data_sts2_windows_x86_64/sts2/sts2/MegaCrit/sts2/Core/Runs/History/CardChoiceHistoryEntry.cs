using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs.History
{
	// Token: 0x0200011E RID: 286
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public struct CardChoiceHistoryEntry
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x0005AE60 File Offset: 0x00059060
		// (set) Token: 0x06000E18 RID: 3608 RVA: 0x0005AE68 File Offset: 0x00059068
		[JsonPropertyName("card")]
		public SerializableCard Card { readonly get; set; }

		// Token: 0x06000E19 RID: 3609 RVA: 0x0005AE71 File Offset: 0x00059071
		public CardChoiceHistoryEntry(CardModel card, bool wasPicked)
		{
			this.Card = card.ToSerializable();
			this.wasPicked = wasPicked;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0005AE86 File Offset: 0x00059086
		public void Serialize<[Nullable(0)] T>(PacketWriter writer) where T : AbstractModel
		{
			writer.Write<SerializableCard>(this.Card);
			writer.WriteBool(this.wasPicked);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0005AEA0 File Offset: 0x000590A0
		public void Deserialize<[Nullable(0)] T>(PacketReader reader) where T : AbstractModel
		{
			this.Card = reader.Read<SerializableCard>();
			this.wasPicked = reader.ReadBool();
		}

		// Token: 0x04000523 RID: 1315
		[JsonPropertyName("was_picked")]
		public bool wasPicked;
	}
}
