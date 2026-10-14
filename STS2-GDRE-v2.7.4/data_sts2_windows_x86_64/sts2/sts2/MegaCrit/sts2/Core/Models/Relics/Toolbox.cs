using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005FE RID: 1534
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Toolbox : RelicModel
	{
		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x06005587 RID: 21895 RVA: 0x00248B9B File Offset: 0x00246D9B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06005588 RID: 21896 RVA: 0x00248B9E File Offset: 0x00246D9E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x06005589 RID: 21897 RVA: 0x00248BAC File Offset: 0x00246DAC
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 1)
				{
					base.Flash();
					List<CardModel> list = CardFactory.GetDistinctForCombat(base.Owner, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint), base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
					CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, list, base.Owner, false);
					CardModel cardModel2 = cardModel;
					if (cardModel2 != null)
					{
						await CardPileCmd.AddGeneratedCardToCombat(cardModel2, PileType.Hand, player, CardPilePosition.Bottom);
					}
				}
			}
		}
	}
}
