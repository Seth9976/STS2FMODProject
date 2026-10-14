using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008E0 RID: 2272
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoulsPower : EnchantmentModel
	{
		// Token: 0x06006C8F RID: 27791 RVA: 0x00282119 File Offset: 0x00280319
		public override bool CanEnchant(CardModel card)
		{
			return base.CanEnchant(card) && card.GetKeywordsWithSources(KeywordSources.Local).Contains(CardKeyword.Exhaust);
		}

		// Token: 0x17001B84 RID: 7044
		// (get) Token: 0x06006C90 RID: 27792 RVA: 0x00282138 File Offset: 0x00280338
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06006C91 RID: 27793 RVA: 0x00282145 File Offset: 0x00280345
		protected override void OnEnchant()
		{
			CardCmd.RemoveKeyword(base.Card, new CardKeyword[] { CardKeyword.Exhaust });
		}
	}
}
