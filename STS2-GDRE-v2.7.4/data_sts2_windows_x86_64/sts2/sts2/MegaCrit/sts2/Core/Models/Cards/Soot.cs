using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AD5 RID: 2773
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Soot : CardModel
	{
		// Token: 0x0600777D RID: 30589 RVA: 0x00297BFB File Offset: 0x00295DFB
		public Soot()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x1700206F RID: 8303
		// (get) Token: 0x0600777E RID: 30590 RVA: 0x00297C08 File Offset: 0x00295E08
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002070 RID: 8304
		// (get) Token: 0x0600777F RID: 30591 RVA: 0x00297C0B File Offset: 0x00295E0B
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002071 RID: 8305
		// (get) Token: 0x06007780 RID: 30592 RVA: 0x00297C0E File Offset: 0x00295E0E
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}
	}
}
