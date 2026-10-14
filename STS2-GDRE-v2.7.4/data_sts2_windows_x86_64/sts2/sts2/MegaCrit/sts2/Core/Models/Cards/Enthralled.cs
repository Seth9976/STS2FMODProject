using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009A9 RID: 2473
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Enthralled : CardModel
	{
		// Token: 0x06007122 RID: 28962 RVA: 0x0028AA9C File Offset: 0x00288C9C
		public Enthralled()
			: base(2, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001DC1 RID: 7617
		// (get) Token: 0x06007123 RID: 28963 RVA: 0x0028AAAA File Offset: 0x00288CAA
		public override bool CanBeGeneratedByModifiers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001DC2 RID: 7618
		// (get) Token: 0x06007124 RID: 28964 RVA: 0x0028AAAD File Offset: 0x00288CAD
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001DC3 RID: 7619
		// (get) Token: 0x06007125 RID: 28965 RVA: 0x0028AAB0 File Offset: 0x00288CB0
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001DC4 RID: 7620
		// (get) Token: 0x06007126 RID: 28966 RVA: 0x0028AAB3 File Offset: 0x00288CB3
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Eternal);
			}
		}

		// Token: 0x06007127 RID: 28967 RVA: 0x0028AABB File Offset: 0x00288CBB
		public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
		{
			if (card.Owner != base.Owner)
			{
				return true;
			}
			CardPile pile = base.Pile;
			return pile == null || pile.Type != PileType.Hand || card is Enthralled || autoPlayType != AutoPlayType.None;
		}
	}
}
