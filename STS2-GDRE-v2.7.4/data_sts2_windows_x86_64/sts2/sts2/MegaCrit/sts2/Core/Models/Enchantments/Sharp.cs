using System;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008DD RID: 2269
	public sealed class Sharp : EnchantmentModel
	{
		// Token: 0x06006C83 RID: 27779 RVA: 0x00281F87 File Offset: 0x00280187
		public override bool CanEnchantCardType(CardType cardType)
		{
			return cardType == CardType.Attack;
		}

		// Token: 0x17001B82 RID: 7042
		// (get) Token: 0x06006C84 RID: 27780 RVA: 0x00281F8D File Offset: 0x0028018D
		public override bool ShowAmount
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006C85 RID: 27781 RVA: 0x00281F90 File Offset: 0x00280190
		public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
		{
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			return base.Amount;
		}
	}
}
