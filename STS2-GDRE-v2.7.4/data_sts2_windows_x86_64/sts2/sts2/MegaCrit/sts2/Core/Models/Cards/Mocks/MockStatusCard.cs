using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards.Mocks
{
	// Token: 0x02000B4A RID: 2890
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockStatusCard : MockCardModel
	{
		// Token: 0x17002180 RID: 8576
		// (get) Token: 0x06007A30 RID: 31280 RVA: 0x0029D100 File Offset: 0x0029B300
		protected override int CanonicalEnergyCost
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17002181 RID: 8577
		// (get) Token: 0x06007A31 RID: 31281 RVA: 0x0029D103 File Offset: 0x0029B303
		public override CardType Type
		{
			get
			{
				return CardType.Status;
			}
		}

		// Token: 0x17002182 RID: 8578
		// (get) Token: 0x06007A32 RID: 31282 RVA: 0x0029D106 File Offset: 0x0029B306
		public override CardRarity Rarity
		{
			get
			{
				return CardRarity.Status;
			}
		}

		// Token: 0x17002183 RID: 8579
		// (get) Token: 0x06007A33 RID: 31283 RVA: 0x0029D109 File Offset: 0x0029B309
		public override TargetType TargetType
		{
			get
			{
				return TargetType.None;
			}
		}

		// Token: 0x17002184 RID: 8580
		// (get) Token: 0x06007A34 RID: 31284 RVA: 0x0029D10C File Offset: 0x0029B30C
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
			}
		}

		// Token: 0x06007A35 RID: 31285 RVA: 0x0029D114 File Offset: 0x0029B314
		public override MockCardModel MockBlock(int block)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007A36 RID: 31286 RVA: 0x0029D11B File Offset: 0x0029B31B
		protected override int GetBaseBlock()
		{
			return 0;
		}
	}
}
