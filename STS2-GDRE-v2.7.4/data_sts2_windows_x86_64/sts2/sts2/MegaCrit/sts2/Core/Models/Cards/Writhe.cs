using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B41 RID: 2881
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Writhe : CardModel
	{
		// Token: 0x060079CD RID: 31181 RVA: 0x0029C7A3 File Offset: 0x0029A9A3
		public Writhe()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x1700215B RID: 8539
		// (get) Token: 0x060079CE RID: 31182 RVA: 0x0029C7B1 File Offset: 0x0029A9B1
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700215C RID: 8540
		// (get) Token: 0x060079CF RID: 31183 RVA: 0x0029C7B4 File Offset: 0x0029A9B4
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Innate,
					CardKeyword.Unplayable
				});
			}
		}
	}
}
