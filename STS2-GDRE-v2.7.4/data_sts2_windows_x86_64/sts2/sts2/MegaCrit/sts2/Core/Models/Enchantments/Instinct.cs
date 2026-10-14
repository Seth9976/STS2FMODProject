using System;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008D8 RID: 2264
	public sealed class Instinct : EnchantmentModel
	{
		// Token: 0x06006C6D RID: 27757 RVA: 0x00281E43 File Offset: 0x00280043
		public override bool CanEnchantCardType(CardType cardType)
		{
			return cardType == CardType.Attack;
		}

		// Token: 0x06006C6E RID: 27758 RVA: 0x00281E49 File Offset: 0x00280049
		public override decimal EnchantDamageMultiplicative(decimal originalDamage, ValueProp props)
		{
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			return 2m;
		}
	}
}
