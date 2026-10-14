using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009CE RID: 2510
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Folly : CardModel
	{
		// Token: 0x060071F3 RID: 29171 RVA: 0x0028C651 File Offset: 0x0028A851
		public Folly()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001E1C RID: 7708
		// (get) Token: 0x060071F4 RID: 29172 RVA: 0x0028C65F File Offset: 0x0028A85F
		public override bool CanBeGeneratedByModifiers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001E1D RID: 7709
		// (get) Token: 0x060071F5 RID: 29173 RVA: 0x0028C662 File Offset: 0x0028A862
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001E1E RID: 7710
		// (get) Token: 0x060071F6 RID: 29174 RVA: 0x0028C665 File Offset: 0x0028A865
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Unplayable,
					CardKeyword.Eternal,
					CardKeyword.Innate,
					CardKeyword.Ethereal
				});
			}
		}
	}
}
