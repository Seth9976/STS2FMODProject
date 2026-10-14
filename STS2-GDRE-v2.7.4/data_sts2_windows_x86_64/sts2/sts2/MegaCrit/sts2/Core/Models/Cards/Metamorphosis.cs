using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A3B RID: 2619
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Metamorphosis : CardModel
	{
		// Token: 0x06007448 RID: 29768 RVA: 0x002913BC File Offset: 0x0028F5BC
		public Metamorphosis()
			: base(2, CardType.Skill, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x17001F13 RID: 7955
		// (get) Token: 0x06007449 RID: 29769 RVA: 0x002913C9 File Offset: 0x0028F5C9
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001F14 RID: 7956
		// (get) Token: 0x0600744A RID: 29770 RVA: 0x002913D1 File Offset: 0x0028F5D1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x0600744B RID: 29771 RVA: 0x002913E0 File Offset: 0x0028F5E0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<CardModel> forCombat = CardFactory.GetForCombat(base.Owner, from c in base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
				where c.Type == CardType.Attack
				select c, base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration);
			foreach (CardModel cardModel in forCombat)
			{
				cardModel.SetToFreeThisCombat();
				CardPileAddResult cardPileAddResult = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Draw, base.Owner, CardPilePosition.Random);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.HorizontalLayout);
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x0600744C RID: 29772 RVA: 0x00291423 File Offset: 0x0028F623
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(2m);
		}
	}
}
