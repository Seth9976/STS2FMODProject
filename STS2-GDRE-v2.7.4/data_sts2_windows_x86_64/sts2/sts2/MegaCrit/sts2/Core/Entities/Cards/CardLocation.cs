using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CAE RID: 3246
	public struct CardLocation : IEquatable<CardLocation>
	{
		// Token: 0x06008499 RID: 33945 RVA: 0x002CA3B5 File Offset: 0x002C85B5
		[NullableContext(1)]
		public CardLocation(Player player, PileType pileType, CardPilePosition position)
		{
			this.player = player;
			this.pileType = pileType;
			this.position = position;
		}

		// Token: 0x0600849A RID: 33946 RVA: 0x002CA3CC File Offset: 0x002C85CC
		[CompilerGenerated]
		public override readonly string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CardLocation");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600849B RID: 33947 RVA: 0x002CA418 File Offset: 0x002C8618
		[CompilerGenerated]
		private readonly bool PrintMembers(StringBuilder builder)
		{
			builder.Append("player = ");
			builder.Append(this.player);
			builder.Append(", pileType = ");
			PileType pileType = this.pileType;
			builder.Append(pileType.ToString());
			builder.Append(", position = ");
			CardPilePosition cardPilePosition = this.position;
			builder.Append(cardPilePosition.ToString());
			return true;
		}

		// Token: 0x0600849C RID: 33948 RVA: 0x002CA48D File Offset: 0x002C868D
		[CompilerGenerated]
		public static bool operator !=(CardLocation left, CardLocation right)
		{
			return !(left == right);
		}

		// Token: 0x0600849D RID: 33949 RVA: 0x002CA499 File Offset: 0x002C8699
		[CompilerGenerated]
		public static bool operator ==(CardLocation left, CardLocation right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600849E RID: 33950 RVA: 0x002CA4A3 File Offset: 0x002C86A3
		[CompilerGenerated]
		public override readonly int GetHashCode()
		{
			return (EqualityComparer<Player>.Default.GetHashCode(this.player) * -1521134295 + EqualityComparer<PileType>.Default.GetHashCode(this.pileType)) * -1521134295 + EqualityComparer<CardPilePosition>.Default.GetHashCode(this.position);
		}

		// Token: 0x0600849F RID: 33951 RVA: 0x002CA4E3 File Offset: 0x002C86E3
		[CompilerGenerated]
		public override readonly bool Equals(object obj)
		{
			return obj is CardLocation && this.Equals((CardLocation)obj);
		}

		// Token: 0x060084A0 RID: 33952 RVA: 0x002CA4FC File Offset: 0x002C86FC
		[CompilerGenerated]
		public readonly bool Equals(CardLocation other)
		{
			return EqualityComparer<Player>.Default.Equals(this.player, other.player) && EqualityComparer<PileType>.Default.Equals(this.pileType, other.pileType) && EqualityComparer<CardPilePosition>.Default.Equals(this.position, other.position);
		}

		// Token: 0x04002D8B RID: 11659
		[Nullable(1)]
		public Player player;

		// Token: 0x04002D8C RID: 11660
		public PileType pileType;

		// Token: 0x04002D8D RID: 11661
		public CardPilePosition position;
	}
}
