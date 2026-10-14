using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	// Token: 0x02000C87 RID: 3207
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetCombatCard : IPacketSerializable, IEquatable<NetCombatCard>
	{
		// Token: 0x170023E5 RID: 9189
		// (get) Token: 0x06008360 RID: 33632 RVA: 0x002C4B52 File Offset: 0x002C2D52
		// (set) Token: 0x06008361 RID: 33633 RVA: 0x002C4B5A File Offset: 0x002C2D5A
		public uint CombatCardIndex { readonly get; private set; }

		// Token: 0x06008362 RID: 33634 RVA: 0x002C4B64 File Offset: 0x002C2D64
		public static NetCombatCard ForTesting(uint index)
		{
			return new NetCombatCard
			{
				CombatCardIndex = index
			};
		}

		// Token: 0x06008363 RID: 33635 RVA: 0x002C4B84 File Offset: 0x002C2D84
		public static NetCombatCard FromModel(CardModel card)
		{
			if (!card.IsMutable)
			{
				throw new InvalidOperationException("Immutable instances of CardModel should not be serialized through NetCombatCard! Send the ModelId instead.");
			}
			CardPile pile = card.Pile;
			if (pile != null && !pile.IsCombatPile)
			{
				throw new InvalidOperationException("Cannot use NetCombatCard to serialize a card in a non-combat pile!");
			}
			return new NetCombatCard
			{
				CombatCardIndex = NetCombatCardDb.Instance.GetCardId(card)
			};
		}

		// Token: 0x06008364 RID: 33636 RVA: 0x002C4BDC File Offset: 0x002C2DDC
		public readonly CardModel ToCardModel()
		{
			return NetCombatCardDb.Instance.GetCard(this.CombatCardIndex);
		}

		// Token: 0x06008365 RID: 33637 RVA: 0x002C4BF0 File Offset: 0x002C2DF0
		[NullableContext(2)]
		public readonly CardModel ToCardModelOrNull()
		{
			CardModel cardModel;
			NetCombatCardDb.Instance.TryGetCard(this.CombatCardIndex, out cardModel);
			return cardModel;
		}

		// Token: 0x06008366 RID: 33638 RVA: 0x002C4C11 File Offset: 0x002C2E11
		public void Serialize(PacketWriter writer)
		{
			writer.WriteUInt(this.CombatCardIndex, 16);
		}

		// Token: 0x06008367 RID: 33639 RVA: 0x002C4C21 File Offset: 0x002C2E21
		public void Deserialize(PacketReader reader)
		{
			this.CombatCardIndex = reader.ReadUInt(16);
		}

		// Token: 0x06008368 RID: 33640 RVA: 0x002C4C31 File Offset: 0x002C2E31
		public bool Equals(NetCombatCard other)
		{
			return this.CombatCardIndex == other.CombatCardIndex;
		}

		// Token: 0x06008369 RID: 33641 RVA: 0x002C4C42 File Offset: 0x002C2E42
		public static bool operator ==(NetCombatCard c1, NetCombatCard c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x0600836A RID: 33642 RVA: 0x002C4C4C File Offset: 0x002C2E4C
		public static bool operator !=(NetCombatCard c1, NetCombatCard c2)
		{
			return !c1.Equals(c2);
		}

		// Token: 0x0600836B RID: 33643 RVA: 0x002C4C5C File Offset: 0x002C2E5C
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is NetCombatCard)
			{
				NetCombatCard netCombatCard = (NetCombatCard)obj;
				return this.Equals(netCombatCard);
			}
			return false;
		}

		// Token: 0x0600836C RID: 33644 RVA: 0x002C4C84 File Offset: 0x002C2E84
		public override int GetHashCode()
		{
			return this.CombatCardIndex.GetHashCode();
		}

		// Token: 0x0600836D RID: 33645 RVA: 0x002C4CA0 File Offset: 0x002C2EA0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted("NetCombatCard");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.CombatCardIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
