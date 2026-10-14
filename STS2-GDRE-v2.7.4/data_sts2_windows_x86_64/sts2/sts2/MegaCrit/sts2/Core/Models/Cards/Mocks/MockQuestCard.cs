using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards.Mocks
{
	// Token: 0x02000B48 RID: 2888
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockQuestCard : MockCardModel
	{
		// Token: 0x17002177 RID: 8567
		// (get) Token: 0x06007A15 RID: 31253 RVA: 0x0029CDC3 File Offset: 0x0029AFC3
		protected override int CanonicalEnergyCost
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17002178 RID: 8568
		// (get) Token: 0x06007A16 RID: 31254 RVA: 0x0029CDC6 File Offset: 0x0029AFC6
		public override CardType Type
		{
			get
			{
				return CardType.Quest;
			}
		}

		// Token: 0x17002179 RID: 8569
		// (get) Token: 0x06007A17 RID: 31255 RVA: 0x0029CDC9 File Offset: 0x0029AFC9
		public override CardRarity Rarity
		{
			get
			{
				return CardRarity.Quest;
			}
		}

		// Token: 0x1700217A RID: 8570
		// (get) Token: 0x06007A18 RID: 31256 RVA: 0x0029CDCD File Offset: 0x0029AFCD
		public override TargetType TargetType
		{
			get
			{
				return TargetType.None;
			}
		}

		// Token: 0x1700217B RID: 8571
		// (get) Token: 0x06007A19 RID: 31257 RVA: 0x0029CDD0 File Offset: 0x0029AFD0
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700217C RID: 8572
		// (get) Token: 0x06007A1A RID: 31258 RVA: 0x0029CDD3 File Offset: 0x0029AFD3
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x06007A1B RID: 31259 RVA: 0x0029CDDB File Offset: 0x0029AFDB
		public override MockCardModel MockBlock(int block)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007A1C RID: 31260 RVA: 0x0029CDE2 File Offset: 0x0029AFE2
		protected override int GetBaseBlock()
		{
			return 0;
		}
	}
}
