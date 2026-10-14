using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AE3 RID: 2787
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SporeMind : CardModel
	{
		// Token: 0x060077DD RID: 30685 RVA: 0x00298903 File Offset: 0x00296B03
		public SporeMind()
			: base(1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17002092 RID: 8338
		// (get) Token: 0x060077DE RID: 30686 RVA: 0x00298911 File Offset: 0x00296B11
		public override bool CanBeGeneratedByModifiers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002093 RID: 8339
		// (get) Token: 0x060077DF RID: 30687 RVA: 0x00298914 File Offset: 0x00296B14
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002094 RID: 8340
		// (get) Token: 0x060077E0 RID: 30688 RVA: 0x00298917 File Offset: 0x00296B17
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}
	}
}
