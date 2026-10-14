using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008E3 RID: 2275
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Steady : EnchantmentModel
	{
		// Token: 0x17001B88 RID: 7048
		// (get) Token: 0x06006C9B RID: 27803 RVA: 0x00282243 File Offset: 0x00280443
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
			}
		}

		// Token: 0x06006C9C RID: 27804 RVA: 0x00282250 File Offset: 0x00280450
		protected override void OnEnchant()
		{
			base.Card.AddKeyword(CardKeyword.Retain);
		}
	}
}
