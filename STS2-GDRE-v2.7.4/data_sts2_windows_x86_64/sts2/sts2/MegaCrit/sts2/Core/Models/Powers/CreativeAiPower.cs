using System;
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
	// Token: 0x02000652 RID: 1618
	public sealed class CreativeAiPower : PowerModel
	{
		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06005785 RID: 22405 RVA: 0x0024D04F File Offset: 0x0024B24F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06005786 RID: 22406 RVA: 0x0024D052 File Offset: 0x0024B252
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x0024D058 File Offset: 0x0024B258
		[NullableContext(1)]
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner.Player)
			{
				for (int i = 0; i < base.Amount; i++)
				{
					CardModel cardModel = CardFactory.GetDistinctForCombat(player, from c in player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
						where c.Type == CardType.Power
						select c, 1, player.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
					if (cardModel != null)
					{
						await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner.Player, CardPilePosition.Bottom);
					}
				}
			}
		}
	}
}
