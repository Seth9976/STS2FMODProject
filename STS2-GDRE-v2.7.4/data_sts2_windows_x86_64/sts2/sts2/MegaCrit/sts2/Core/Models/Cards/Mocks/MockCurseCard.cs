using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards.Mocks
{
	// Token: 0x02000B46 RID: 2886
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockCurseCard : MockCardModel
	{
		// Token: 0x1700216E RID: 8558
		// (get) Token: 0x06007A04 RID: 31236 RVA: 0x0029CCA2 File Offset: 0x0029AEA2
		protected override int CanonicalEnergyCost
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700216F RID: 8559
		// (get) Token: 0x06007A05 RID: 31237 RVA: 0x0029CCA5 File Offset: 0x0029AEA5
		public override CardType Type
		{
			get
			{
				return CardType.Curse;
			}
		}

		// Token: 0x17002170 RID: 8560
		// (get) Token: 0x06007A06 RID: 31238 RVA: 0x0029CCA8 File Offset: 0x0029AEA8
		public override CardRarity Rarity
		{
			get
			{
				return CardRarity.Curse;
			}
		}

		// Token: 0x17002171 RID: 8561
		// (get) Token: 0x06007A07 RID: 31239 RVA: 0x0029CCAC File Offset: 0x0029AEAC
		public override TargetType TargetType
		{
			get
			{
				return TargetType.None;
			}
		}

		// Token: 0x17002172 RID: 8562
		// (get) Token: 0x06007A08 RID: 31240 RVA: 0x0029CCAF File Offset: 0x0029AEAF
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Unplayable,
					CardKeyword.Ethereal
				});
			}
		}

		// Token: 0x06007A09 RID: 31241 RVA: 0x0029CCC4 File Offset: 0x0029AEC4
		public override MockCardModel MockBlock(int block)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007A0A RID: 31242 RVA: 0x0029CCCB File Offset: 0x0029AECB
		protected override int GetBaseBlock()
		{
			return 0;
		}
	}
}
