using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AE0 RID: 2784
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Splash : CardModel
	{
		// Token: 0x060077C6 RID: 30662 RVA: 0x0029858F File Offset: 0x0029678F
		public Splash()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x060077C7 RID: 30663 RVA: 0x0029859C File Offset: 0x0029679C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			CardModel cardModel3;
			if (this._mockGeneratedCard == null)
			{
				List<CardPoolModel> list = base.Owner.UnlockState.CharacterCardPools.ToList<CardPoolModel>();
				if (list.Count > 1)
				{
					list.Remove(base.Owner.Character.CardPool);
				}
				IEnumerable<CardModel> enumerable = from c in list.SelectMany<CardPoolModel, CardModel>((CardPoolModel c) => c.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint))
					where c.Type == CardType.Attack
					select c;
				List<CardModel> list2 = CardFactory.GetDistinctForCombat(base.Owner, enumerable, 3, base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
				if (base.IsUpgraded)
				{
					foreach (CardModel cardModel in list2)
					{
						CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
					}
				}
				CardModel cardModel2 = await CardSelectCmd.FromChooseACardScreen(choiceContext, list2, base.Owner, true);
				cardModel3 = cardModel2;
			}
			else
			{
				cardModel3 = this._mockGeneratedCard;
				if (base.IsUpgraded)
				{
					CardCmd.Upgrade(cardModel3, CardPreviewStyle.HorizontalLayout);
				}
			}
			if (cardModel3 != null)
			{
				cardModel3.SetToFreeThisTurn();
				await CardPileCmd.AddGeneratedCardToCombat(cardModel3, PileType.Hand, base.Owner, CardPilePosition.Bottom);
			}
		}

		/// <summary>
		/// ONLY USE THIS IN TESTS!
		/// </summary>
		// Token: 0x060077C8 RID: 30664 RVA: 0x002985E7 File Offset: 0x002967E7
		public void MockGeneratedCard(CardModel card)
		{
			base.AssertMutable();
			this._mockGeneratedCard = card;
		}

		// Token: 0x040028C5 RID: 10437
		[Nullable(2)]
		private CardModel _mockGeneratedCard;
	}
}
