using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A01 RID: 2561
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HiddenDaggers : CardModel
	{
		// Token: 0x06007303 RID: 29443 RVA: 0x0028E8A7 File Offset: 0x0028CAA7
		public HiddenDaggers()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E8E RID: 7822
		// (get) Token: 0x06007304 RID: 29444 RVA: 0x0028E8B4 File Offset: 0x0028CAB4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(2),
					new DynamicVar("Shivs", 2m)
				});
			}
		}

		// Token: 0x17001E8F RID: 7823
		// (get) Token: 0x06007305 RID: 29445 RVA: 0x0028E8DD File Offset: 0x0028CADD
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(base.IsUpgraded));
			}
		}

		// Token: 0x06007306 RID: 29446 RVA: 0x0028E8F0 File Offset: 0x0028CAF0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHandForDiscard(choiceContext, base.Owner, cardSelectorPrefs, null, this);
			IEnumerable<CardModel> enumerable2 = enumerable;
			await CardCmd.Discard(choiceContext, enumerable2);
			IEnumerable<CardModel> enumerable3 = await Shiv.CreateInHand(base.Owner, base.DynamicVars["Shivs"].IntValue, base.CombatState, null);
			if (base.IsUpgraded)
			{
				foreach (CardModel cardModel in enumerable3)
				{
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				}
			}
		}

		// Token: 0x04002880 RID: 10368
		private const string _shivKey = "Shivs";
	}
}
