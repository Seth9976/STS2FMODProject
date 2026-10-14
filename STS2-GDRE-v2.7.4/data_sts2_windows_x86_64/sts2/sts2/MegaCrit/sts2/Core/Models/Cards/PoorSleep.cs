using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A7A RID: 2682
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PoorSleep : CardModel
	{
		// Token: 0x06007599 RID: 30105 RVA: 0x00293F17 File Offset: 0x00292117
		public PoorSleep()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001FA1 RID: 8097
		// (get) Token: 0x0600759A RID: 30106 RVA: 0x00293F25 File Offset: 0x00292125
		public override bool CanBeGeneratedByModifiers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001FA2 RID: 8098
		// (get) Token: 0x0600759B RID: 30107 RVA: 0x00293F28 File Offset: 0x00292128
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001FA3 RID: 8099
		// (get) Token: 0x0600759C RID: 30108 RVA: 0x00293F2B File Offset: 0x0029212B
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Unplayable,
					CardKeyword.Retain
				});
			}
		}
	}
}
