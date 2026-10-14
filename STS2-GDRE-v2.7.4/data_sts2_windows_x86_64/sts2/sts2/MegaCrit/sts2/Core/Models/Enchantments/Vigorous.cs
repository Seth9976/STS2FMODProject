using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008E6 RID: 2278
	public sealed class Vigorous : EnchantmentModel
	{
		// Token: 0x06006CA7 RID: 27815 RVA: 0x00282337 File Offset: 0x00280537
		public override bool CanEnchantCardType(CardType cardType)
		{
			return cardType == CardType.Attack;
		}

		// Token: 0x17001B8D RID: 7053
		// (get) Token: 0x06006CA8 RID: 27816 RVA: 0x0028233D File Offset: 0x0028053D
		public override bool ShowAmount
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006CA9 RID: 27817 RVA: 0x00282340 File Offset: 0x00280540
		public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
		{
			if (base.Status != EnchantmentStatus.Normal)
			{
				return 0m;
			}
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			return base.Amount;
		}

		// Token: 0x06006CAA RID: 27818 RVA: 0x00282369 File Offset: 0x00280569
		[NullableContext(1)]
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card != base.Card)
			{
				return Task.CompletedTask;
			}
			base.Status = EnchantmentStatus.Disabled;
			return Task.CompletedTask;
		}
	}
}
