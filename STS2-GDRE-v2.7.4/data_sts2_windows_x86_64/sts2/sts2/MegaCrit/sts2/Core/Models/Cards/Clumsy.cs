using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000955 RID: 2389
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Clumsy : CardModel
	{
		// Token: 0x06006F63 RID: 28515 RVA: 0x002874B7 File Offset: 0x002856B7
		public Clumsy()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001CF6 RID: 7414
		// (get) Token: 0x06006F64 RID: 28516 RVA: 0x002874C5 File Offset: 0x002856C5
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Unplayable,
					CardKeyword.Ethereal
				});
			}
		}

		// Token: 0x17001CF7 RID: 7415
		// (get) Token: 0x06006F65 RID: 28517 RVA: 0x002874DA File Offset: 0x002856DA
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}
	}
}
