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
	// Token: 0x020008F1 RID: 2289
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Abundance : CardModel
	{
		// Token: 0x06006D69 RID: 28009 RVA: 0x002834D4 File Offset: 0x002816D4
		public Abundance()
			: base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x17001C27 RID: 7207
		// (get) Token: 0x06006D6A RID: 28010 RVA: 0x002834E1 File Offset: 0x002816E1
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C28 RID: 7208
		// (get) Token: 0x06006D6B RID: 28011 RVA: 0x002834E4 File Offset: 0x002816E4
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06006D6C RID: 28012 RVA: 0x002834EC File Offset: 0x002816EC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			List<CardModel> list = CardFactory.GetDistinctForCombat(base.Owner, from c in base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
				where c.Type == CardType.Power
				select c, 3, base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
			if (base.IsUpgraded)
			{
				foreach (CardModel cardModel in list)
				{
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				}
			}
			CardModel cardModel2 = await CardSelectCmd.FromChooseACardScreen(choiceContext, list, base.Owner, false);
			CardModel cardModel3 = cardModel2;
			if (cardModel3 != null)
			{
				cardModel3.SetToFreeThisTurn();
				await CardPileCmd.AddGeneratedCardToCombat(cardModel3, PileType.Hand, base.Owner, CardPilePosition.Bottom);
			}
		}
	}
}
