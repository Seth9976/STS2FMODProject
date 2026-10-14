using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008DC RID: 2268
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RoyallyApproved : EnchantmentModel
	{
		// Token: 0x06006C7F RID: 27775 RVA: 0x00281F2C File Offset: 0x0028012C
		public override bool CanEnchantCardType(CardType cardType)
		{
			return cardType - CardType.Attack <= 1;
		}

		// Token: 0x17001B81 RID: 7041
		// (get) Token: 0x06006C80 RID: 27776 RVA: 0x00281F46 File Offset: 0x00280146
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromKeyword(CardKeyword.Innate),
					HoverTipFactory.FromKeyword(CardKeyword.Retain)
				});
			}
		}

		// Token: 0x06006C81 RID: 27777 RVA: 0x00281F65 File Offset: 0x00280165
		protected override void OnEnchant()
		{
			base.Card.AddKeyword(CardKeyword.Innate);
			base.Card.AddKeyword(CardKeyword.Retain);
		}
	}
}
