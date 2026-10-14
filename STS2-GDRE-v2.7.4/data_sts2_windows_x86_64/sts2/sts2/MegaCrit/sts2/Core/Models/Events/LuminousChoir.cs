using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000841 RID: 2113
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LuminousChoir : EventModel
	{
		// Token: 0x06006827 RID: 26663 RVA: 0x00275CD0 File Offset: 0x00273ED0
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Gold >= this.DynamicVars.Gold.BaseValue && p.RelicGrabBag.HasAvailableRelics(runState));
		}

		// Token: 0x17001987 RID: 6535
		// (get) Token: 0x06006828 RID: 26664 RVA: 0x00275D0D File Offset: 0x00273F0D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(149));
			}
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x00275D1E File Offset: 0x00273F1E
		public override void CalculateVars()
		{
			base.DynamicVars.Gold.BaseValue -= base.Rng.NextInt(0, 50);
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x00275D50 File Offset: 0x00273F50
		protected unsafe override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			int num = 1;
			List<EventOption> list = new List<EventOption>(num);
			CollectionsMarshal.SetCount<EventOption>(list, num);
			Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(list);
			int num2 = 0;
			*span[num2] = new EventOption(this, new Func<Task>(this.ReachIntoTheFlesh), "LUMINOUS_CHOIR.pages.INITIAL.options.REACH_INTO_THE_FLESH", HoverTipFactory.FromCardWithCardHoverTips<SporeMind>(false));
			List<EventOption> list2 = list;
			if (base.Owner.Gold >= base.DynamicVars.Gold.IntValue)
			{
				list2.Add(new EventOption(this, new Func<Task>(this.OfferTribute), "LUMINOUS_CHOIR.pages.INITIAL.options.OFFER_TRIBUTE", Array.Empty<IHoverTip>()));
			}
			else
			{
				list2.Add(new EventOption(this, null, "LUMINOUS_CHOIR.pages.INITIAL.options.OFFER_TRIBUTE_LOCKED", Array.Empty<IHoverTip>()));
			}
			return list2;
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x00275DF8 File Offset: 0x00273FF8
		private async Task ReachIntoTheFlesh()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 2);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, cardSelectorPrefs, null);
			List<CardModel> list = enumerable.ToList<CardModel>();
			await CardPileCmd.RemoveFromDeck(list, true);
			await CardPileCmd.AddCurseToDeck<SporeMind>(base.Owner);
			base.SetEventFinished(base.L10NLookup("LUMINOUS_CHOIR.pages.REACH_INTO_THE_FLESH.description"));
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x00275E3C File Offset: 0x0027403C
		private async Task OfferTribute()
		{
			await PlayerCmd.LoseGold(base.DynamicVars.Gold.IntValue, base.Owner, GoldLossType.Spent);
			RelicModel relicModel = RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable();
			await RelicCmd.Obtain(relicModel, base.Owner, -1);
			base.SetEventFinished(base.L10NLookup("LUMINOUS_CHOIR.pages.OFFER_TRIBUTE.description"));
		}
	}
}
