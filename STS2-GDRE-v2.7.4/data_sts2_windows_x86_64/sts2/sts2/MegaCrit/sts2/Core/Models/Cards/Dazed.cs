using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000977 RID: 2423
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Dazed : CardModel
	{
		// Token: 0x0600700F RID: 28687 RVA: 0x00288B29 File Offset: 0x00286D29
		public Dazed()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x17001D3E RID: 7486
		// (get) Token: 0x06007010 RID: 28688 RVA: 0x00288B36 File Offset: 0x00286D36
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001D3F RID: 7487
		// (get) Token: 0x06007011 RID: 28689 RVA: 0x00288B39 File Offset: 0x00286D39
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Ethereal,
					CardKeyword.Unplayable
				});
			}
		}
	}
}
