using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A8C RID: 2700
	public sealed class Quasar : CardModel
	{
		// Token: 0x060075F0 RID: 30192 RVA: 0x00294AC9 File Offset: 0x00292CC9
		public Quasar()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FC4 RID: 8132
		// (get) Token: 0x060075F1 RID: 30193 RVA: 0x00294AD6 File Offset: 0x00292CD6
		public override int CanonicalStarCost
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060075F2 RID: 30194 RVA: 0x00294ADC File Offset: 0x00292CDC
		[NullableContext(1)]
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			List<CardModel> list = CardFactory.GetDistinctForCombat(base.Owner, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint), 3, base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
			if (base.IsUpgraded)
			{
				CardCmd.Upgrade(list, CardPreviewStyle.HorizontalLayout);
			}
			CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, list, base.Owner, true);
			CardModel cardModel2 = cardModel;
			if (cardModel2 != null)
			{
				await CardPileCmd.AddGeneratedCardToCombat(cardModel2, PileType.Hand, base.Owner, CardPilePosition.Bottom);
			}
		}
	}
}
