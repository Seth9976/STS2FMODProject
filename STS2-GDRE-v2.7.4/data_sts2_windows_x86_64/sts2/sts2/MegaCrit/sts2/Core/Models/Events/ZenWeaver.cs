using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200086B RID: 2155
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ZenWeaver : EventModel
	{
		// Token: 0x060069D4 RID: 27092 RVA: 0x0027E6B7 File Offset: 0x0027C8B7
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Gold >= base.DynamicVars["EmotionalAwarenessCost"].BaseValue);
		}

		// Token: 0x060069D5 RID: 27093 RVA: 0x0027E6D0 File Offset: 0x0027C8D0
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			int gold = base.Owner.Gold;
			EventOption eventOption = new EventOption(this, new Func<Task>(this.BreathingTechniques), "ZEN_WEAVER.pages.INITIAL.options.BREATHING_TECHNIQUES", HoverTipFactory.FromCardWithCardHoverTips<Enlightenment>(false));
			EventOption eventOption2;
			if (gold >= base.DynamicVars["EmotionalAwarenessCost"].IntValue)
			{
				eventOption2 = new EventOption(this, new Func<Task>(this.EmotionalAwareness), "ZEN_WEAVER.pages.INITIAL.options.EMOTIONAL_AWARENESS", Array.Empty<IHoverTip>());
			}
			else
			{
				eventOption2 = this.CreateLockedOption();
			}
			EventOption eventOption3;
			if (gold >= base.DynamicVars["ArachnidAcupunctureCost"].IntValue)
			{
				eventOption3 = new EventOption(this, new Func<Task>(this.ArachnidAcupuncture), "ZEN_WEAVER.pages.INITIAL.options.ARACHNID_ACUPUNCTURE", Array.Empty<IHoverTip>());
			}
			else
			{
				eventOption3 = this.CreateLockedOption();
			}
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[] { eventOption, eventOption2, eventOption3 });
		}

		// Token: 0x17001A0A RID: 6666
		// (get) Token: 0x060069D6 RID: 27094 RVA: 0x0027E79C File Offset: 0x0027C99C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("BreathingTechniquesCost", 50m),
					new DynamicVar("EmotionalAwarenessCost", 125m),
					new DynamicVar("ArachnidAcupunctureCost", 250m)
				});
			}
		}

		// Token: 0x060069D7 RID: 27095 RVA: 0x0027E7F4 File Offset: 0x0027C9F4
		private async Task BreathingTechniques()
		{
			await PlayerCmd.LoseGold(base.DynamicVars["BreathingTechniquesCost"].IntValue, base.Owner, GoldLossType.Spent);
			CardModel[] array = new CardModel[2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = base.Owner.RunState.CreateCard<Enlightenment>(base.Owner);
			}
			IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.Add(array, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardCmd.PreviewCardPileAdd(readOnlyList, 1.2f, CardPreviewStyle.HorizontalLayout);
			base.SetEventFinished(base.L10NLookup("ZEN_WEAVER.pages.BREATHING_TECHNIQUES.description"));
		}

		// Token: 0x060069D8 RID: 27096 RVA: 0x0027E838 File Offset: 0x0027CA38
		private async Task EmotionalAwareness()
		{
			await this.RemoveCardsAndProceed(base.DynamicVars["EmotionalAwarenessCost"].IntValue, 1);
			base.SetEventFinished(base.L10NLookup("ZEN_WEAVER.pages.EMOTIONAL_AWARENESS.description"));
		}

		// Token: 0x060069D9 RID: 27097 RVA: 0x0027E87C File Offset: 0x0027CA7C
		private async Task ArachnidAcupuncture()
		{
			await this.RemoveCardsAndProceed(base.DynamicVars["ArachnidAcupunctureCost"].IntValue, 2);
			base.SetEventFinished(base.L10NLookup("ZEN_WEAVER.pages.ARACHNID_ACUPUNCTURE.description"));
		}

		// Token: 0x060069DA RID: 27098 RVA: 0x0027E8C0 File Offset: 0x0027CAC0
		private async Task RemoveCardsAndProceed(int cost, int count)
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, count), null);
			IEnumerable<CardModel> enumerable2 = enumerable;
			await CardPileCmd.RemoveFromDeck(enumerable2.ToList<CardModel>(), true);
			await PlayerCmd.LoseGold(cost, base.Owner, GoldLossType.Spent);
		}

		// Token: 0x060069DB RID: 27099 RVA: 0x0027E913 File Offset: 0x0027CB13
		private EventOption CreateLockedOption()
		{
			return new EventOption(this, null, "ZEN_WEAVER.pages.INITIAL.options.LOCKED", Array.Empty<IHoverTip>());
		}

		// Token: 0x040027FA RID: 10234
		private const string _breathingTechniquesCostKey = "BreathingTechniquesCost";

		// Token: 0x040027FB RID: 10235
		private const string _emotionalAwarenessCostKey = "EmotionalAwarenessCost";

		// Token: 0x040027FC RID: 10236
		private const string _arachnidAcupunctureCostKey = "ArachnidAcupunctureCost";
	}
}
