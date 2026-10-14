using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000868 RID: 2152
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Wellspring : EventModel
	{
		// Token: 0x17001A07 RID: 6663
		// (get) Token: 0x060069C0 RID: 27072 RVA: 0x0027E1AB File Offset: 0x0027C3AB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("BatheCurses", 1m));
			}
		}

		// Token: 0x060069C1 RID: 27073 RVA: 0x0027E1C4 File Offset: 0x0027C3C4
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Bottle), "WELLSPRING.pages.INITIAL.options.BOTTLE", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Bathe), "WELLSPRING.pages.INITIAL.options.BATHE", HoverTipFactory.FromCardWithCardHoverTips<Guilty>(false))
			});
		}

		// Token: 0x060069C2 RID: 27074 RVA: 0x0027E21C File Offset: 0x0027C41C
		private async Task Bottle()
		{
			IEnumerable<PotionModel> enumerable = base.Owner.Character.PotionPool.GetUnlockedPotions(base.Owner.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(base.Owner.UnlockState));
			PotionModel potionModel = base.Owner.PlayerRng.Rewards.NextItem<PotionModel>(enumerable);
			if (potionModel != null)
			{
				await RewardsCmd.OfferCustom(base.Owner, new List<Reward>(1)
				{
					new PotionReward(potionModel.ToMutable(), base.Owner)
				});
			}
			base.SetEventFinished(base.L10NLookup("WELLSPRING.pages.BOTTLE.description"));
		}

		// Token: 0x060069C3 RID: 27075 RVA: 0x0027E260 File Offset: 0x0027C460
		private async Task Bathe()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, cardSelectorPrefs, null);
			List<CardModel> list = enumerable.ToList<CardModel>();
			await CardPileCmd.RemoveFromDeck(list, true);
			await this.AddGuilty(base.DynamicVars["BatheCurses"].IntValue);
			base.SetEventFinished(base.L10NLookup("WELLSPRING.pages.BATHE.description"));
		}

		// Token: 0x060069C4 RID: 27076 RVA: 0x0027E2A4 File Offset: 0x0027C4A4
		private async Task AddGuilty(int amount)
		{
			await CardPileCmd.AddCursesToDeck(Enumerable.Repeat<Guilty>(ModelDb.Card<Guilty>(), amount), base.Owner);
		}

		// Token: 0x040027F4 RID: 10228
		private const string _batheKey = "BatheCurses";
	}
}
