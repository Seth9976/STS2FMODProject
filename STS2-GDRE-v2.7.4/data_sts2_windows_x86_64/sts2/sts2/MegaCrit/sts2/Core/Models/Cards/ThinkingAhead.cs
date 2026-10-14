using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B13 RID: 2835
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ThinkingAhead : CardModel
	{
		// Token: 0x060078D6 RID: 30934 RVA: 0x0029AA80 File Offset: 0x00298C80
		public ThinkingAhead()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020F2 RID: 8434
		// (get) Token: 0x060078D7 RID: 30935 RVA: 0x0029AA8D File Offset: 0x00298C8D
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x170020F3 RID: 8435
		// (get) Token: 0x060078D8 RID: 30936 RVA: 0x0029AA95 File Offset: 0x00298C95
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x060078D9 RID: 30937 RVA: 0x0029AAA4 File Offset: 0x00298CA4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, null, this);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				await CardPileCmd.Add(cardModel, PileType.Draw, CardPilePosition.Top, null, false);
			}
		}

		// Token: 0x060078DA RID: 30938 RVA: 0x0029AAEF File Offset: 0x00298CEF
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
