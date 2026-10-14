using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008D2 RID: 2258
	public sealed class Corrupted : EnchantmentModel
	{
		// Token: 0x17001B70 RID: 7024
		// (get) Token: 0x06006C4C RID: 27724 RVA: 0x00281B10 File Offset: 0x0027FD10
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006C4D RID: 27725 RVA: 0x00281B13 File Offset: 0x0027FD13
		public override bool CanEnchantCardType(CardType cardType)
		{
			return cardType == CardType.Attack;
		}

		// Token: 0x06006C4E RID: 27726 RVA: 0x00281B19 File Offset: 0x0027FD19
		public override decimal EnchantDamageMultiplicative(decimal originalDamage, ValueProp props)
		{
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			return 1.5m;
		}

		// Token: 0x06006C4F RID: 27727 RVA: 0x00281B34 File Offset: 0x0027FD34
		[NullableContext(1)]
		public override async Task OnPlay(PlayerChoiceContext choiceContext, [Nullable(2)] CardPlay cardPlay)
		{
			await CreatureCmd.Damage(choiceContext, base.Card.Owner.Creature, 2m, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, base.Card, cardPlay);
		}

		// Token: 0x04002832 RID: 10290
		private const decimal _damageAmount = 2m;
	}
}
