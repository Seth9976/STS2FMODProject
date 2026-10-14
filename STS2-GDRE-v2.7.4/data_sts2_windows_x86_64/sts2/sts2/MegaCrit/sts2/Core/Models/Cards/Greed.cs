using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009EA RID: 2538
	[NullableContext(1)]
	[Nullable(0)]
	public class Greed : CardModel
	{
		// Token: 0x06007291 RID: 29329 RVA: 0x0028D9D3 File Offset: 0x0028BBD3
		public Greed()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001E60 RID: 7776
		// (get) Token: 0x06007292 RID: 29330 RVA: 0x0028D9E1 File Offset: 0x0028BBE1
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001E61 RID: 7777
		// (get) Token: 0x06007293 RID: 29331 RVA: 0x0028D9E4 File Offset: 0x0028BBE4
		public override bool CanBeGeneratedByModifiers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001E62 RID: 7778
		// (get) Token: 0x06007294 RID: 29332 RVA: 0x0028D9E7 File Offset: 0x0028BBE7
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Eternal,
					CardKeyword.Unplayable
				});
			}
		}
	}
}
