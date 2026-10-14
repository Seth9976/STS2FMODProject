using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200083E RID: 2110
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InfestedAutomaton : EventModel
	{
		// Token: 0x06006813 RID: 26643 RVA: 0x00275760 File Offset: 0x00273960
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Study), "INFESTED_AUTOMATON.pages.INITIAL.options.STUDY", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.TouchCore), "INFESTED_AUTOMATON.pages.INITIAL.options.TOUCH_CORE", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x17001983 RID: 6531
		// (get) Token: 0x06006814 RID: 26644 RVA: 0x002757B6 File Offset: 0x002739B6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return Array.Empty<DynamicVar>();
			}
		}

		// Token: 0x06006815 RID: 26645 RVA: 0x002757C0 File Offset: 0x002739C0
		private async Task Study()
		{
			CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithDefaultOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(base.Owner.Character.CardPool), (CardModel c) => c.Type == CardType.Power);
			CardCreationResult cardCreationResult = CardFactory.CreateForReward(base.Owner, 1, cardCreationOptions).FirstOrDefault<CardCreationResult>();
			CardModel cardModel = ((cardCreationResult != null) ? cardCreationResult.Card : null);
			if (cardModel != null)
			{
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.EventLayout);
			}
			base.SetEventFinished(base.L10NLookup("INFESTED_AUTOMATON.pages.STUDY.description"));
		}

		// Token: 0x06006816 RID: 26646 RVA: 0x00275804 File Offset: 0x00273A04
		private async Task TouchCore()
		{
			CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithDefaultOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(base.Owner.Character.CardPool), delegate(CardModel c)
			{
				CardEnergyCost energyCost = c.EnergyCost;
				return energyCost != null && energyCost.Canonical == 0 && !energyCost.CostsX;
			}).WithFlags(CardCreationFlags.NoCardPoolModifications);
			CardCreationResult cardCreationResult = CardFactory.CreateForReward(base.Owner, 1, cardCreationOptions).FirstOrDefault<CardCreationResult>();
			CardModel cardModel = ((cardCreationResult != null) ? cardCreationResult.Card : null);
			if (cardModel != null)
			{
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.EventLayout);
			}
			base.SetEventFinished(base.L10NLookup("INFESTED_AUTOMATON.pages.TOUCH_CORE.description"));
		}
	}
}
