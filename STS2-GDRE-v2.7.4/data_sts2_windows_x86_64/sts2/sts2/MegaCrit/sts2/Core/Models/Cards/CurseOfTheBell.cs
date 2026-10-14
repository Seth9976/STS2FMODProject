using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200096F RID: 2415
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CurseOfTheBell : CardModel
	{
		// Token: 0x06006FE9 RID: 28649 RVA: 0x00288614 File Offset: 0x00286814
		public CurseOfTheBell()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001D30 RID: 7472
		// (get) Token: 0x06006FEA RID: 28650 RVA: 0x00288622 File Offset: 0x00286822
		public override bool CanBeGeneratedByModifiers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D31 RID: 7473
		// (get) Token: 0x06006FEB RID: 28651 RVA: 0x00288625 File Offset: 0x00286825
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001D32 RID: 7474
		// (get) Token: 0x06006FEC RID: 28652 RVA: 0x00288628 File Offset: 0x00286828
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
