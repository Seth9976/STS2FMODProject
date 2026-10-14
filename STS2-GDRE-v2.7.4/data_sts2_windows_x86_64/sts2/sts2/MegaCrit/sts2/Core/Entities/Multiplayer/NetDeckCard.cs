using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	// Token: 0x02000C88 RID: 3208
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetDeckCard : IPacketSerializable
	{
		// Token: 0x170023E6 RID: 9190
		// (get) Token: 0x0600836E RID: 33646 RVA: 0x002C4CE2 File Offset: 0x002C2EE2
		// (set) Token: 0x0600836F RID: 33647 RVA: 0x002C4CEA File Offset: 0x002C2EEA
		public uint DeckIndex { readonly get; private set; }

		// Token: 0x06008370 RID: 33648 RVA: 0x002C4CF4 File Offset: 0x002C2EF4
		public static NetDeckCard FromModel(CardModel card)
		{
			if (!card.IsMutable)
			{
				throw new InvalidOperationException("Immutable instances of CardModel should not be serialized through NetDeckCard! Send the ModelId instead.");
			}
			if (card.Pile == null)
			{
				throw new InvalidOperationException("Cannot use NetDeckCard to serialize a card without a pile!");
			}
			if (card.Pile.Type != PileType.Deck)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot use ");
				defaultInterpolatedStringHandler.AppendFormatted("NetDeckCard");
				defaultInterpolatedStringHandler.AppendLiteral(" to serialize card ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral(" that is in a non-deck pile!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new NetDeckCard
			{
				DeckIndex = (uint)card.Pile.Cards.IndexOf(card)
			};
		}

		// Token: 0x06008371 RID: 33649 RVA: 0x002C4DA5 File Offset: 0x002C2FA5
		public readonly CardModel ToCardModel(Player player)
		{
			return player.Deck.Cards[(int)this.DeckIndex];
		}

		// Token: 0x06008372 RID: 33650 RVA: 0x002C4DBD File Offset: 0x002C2FBD
		public void Serialize(PacketWriter writer)
		{
			writer.WriteUInt(this.DeckIndex, 16);
		}

		// Token: 0x06008373 RID: 33651 RVA: 0x002C4DCD File Offset: 0x002C2FCD
		public void Deserialize(PacketReader reader)
		{
			this.DeckIndex = reader.ReadUInt(16);
		}

		// Token: 0x06008374 RID: 33652 RVA: 0x002C4DE0 File Offset: 0x002C2FE0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted("NetDeckCard");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.DeckIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
