using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008D6 RID: 2262
	public sealed class Imbued : EnchantmentModel
	{
		// Token: 0x06006C61 RID: 27745 RVA: 0x00281D0D File Offset: 0x0027FF0D
		public override bool CanEnchantCardType(CardType cardType)
		{
			return cardType == CardType.Skill;
		}

		// Token: 0x17001B76 RID: 7030
		// (get) Token: 0x06006C62 RID: 27746 RVA: 0x00281D13 File Offset: 0x0027FF13
		public override bool ShouldStartAtBottomOfDrawPile
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B77 RID: 7031
		// (get) Token: 0x06006C63 RID: 27747 RVA: 0x00281D16 File Offset: 0x0027FF16
		public override bool ShowAmount
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006C64 RID: 27748 RVA: 0x00281D1C File Offset: 0x0027FF1C
		[NullableContext(1)]
		public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Card.Owner)
			{
				if (player.PlayerCombatState.TurnNumber <= 1)
				{
					await CardCmd.AutoPlay(choiceContext, base.Card, null, AutoPlayType.Default, false, false);
				}
			}
		}
	}
}
