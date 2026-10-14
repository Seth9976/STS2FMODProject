using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008DA RID: 2266
	public sealed class Nimble : EnchantmentModel
	{
		// Token: 0x17001B7F RID: 7039
		// (get) Token: 0x06006C78 RID: 27768 RVA: 0x00281EC7 File Offset: 0x002800C7
		public override bool ShowAmount
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006C79 RID: 27769 RVA: 0x00281ECA File Offset: 0x002800CA
		[NullableContext(1)]
		public override bool CanEnchant(CardModel card)
		{
			return base.CanEnchant(card) && card.GainsBlock;
		}

		// Token: 0x06006C7A RID: 27770 RVA: 0x00281EDD File Offset: 0x002800DD
		public override decimal EnchantBlockAdditive(decimal originalBlock)
		{
			return base.Amount;
		}
	}
}
