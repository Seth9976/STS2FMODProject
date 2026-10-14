using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A14 RID: 2580
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Injury : CardModel
	{
		// Token: 0x0600736F RID: 29551 RVA: 0x0028F57F File Offset: 0x0028D77F
		public Injury()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001EBF RID: 7871
		// (get) Token: 0x06007370 RID: 29552 RVA: 0x0028F58D File Offset: 0x0028D78D
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001EC0 RID: 7872
		// (get) Token: 0x06007371 RID: 29553 RVA: 0x0028F590 File Offset: 0x0028D790
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}
	}
}
