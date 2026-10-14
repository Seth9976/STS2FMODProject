using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000692 RID: 1682
	public sealed class HelloWorldPower : PowerModel
	{
		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x060058DC RID: 22748 RVA: 0x0024F5FB File Offset: 0x0024D7FB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x060058DD RID: 22749 RVA: 0x0024F5FE File Offset: 0x0024D7FE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x0024F604 File Offset: 0x0024D804
		[NullableContext(1)]
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner.Player)
			{
				if (base.AmountOnTurnStart >= 1)
				{
					base.Flash();
					IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(base.Owner.Player, from c in base.Owner.Player.Character.CardPool.GetUnlockedCards(base.Owner.Player.UnlockState, base.Owner.Player.RunState.CardMultiplayerConstraint)
						where c.Rarity == CardRarity.Common
						select c, base.AmountOnTurnStart, base.Owner.Player.RunState.Rng.CombatCardGeneration);
					await CardPileCmd.AddGeneratedCardsToCombat(distinctForCombat, PileType.Hand, base.Owner.Player, CardPilePosition.Bottom);
				}
			}
		}
	}
}
