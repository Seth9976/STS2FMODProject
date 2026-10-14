using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000867 RID: 2151
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WelcomeToWongos : EventModel
	{
		// Token: 0x17001A05 RID: 6661
		// (get) Token: 0x060069B5 RID: 27061 RVA: 0x0027DD6B File Offset: 0x0027BF6B
		// (set) Token: 0x060069B6 RID: 27062 RVA: 0x0027DD73 File Offset: 0x0027BF73
		[Nullable(2)]
		private RelicModel FeaturedItem
		{
			[NullableContext(2)]
			get
			{
				return this._featuredItem;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._featuredItem = value;
			}
		}

		// Token: 0x060069B7 RID: 27063 RVA: 0x0027DD82 File Offset: 0x0027BF82
		public override bool IsAllowed(IRunState runState)
		{
			if (runState.CurrentActIndex == 1)
			{
				return runState.Players.All<Player>((Player p) => p.Gold >= 100);
			}
			return false;
		}

		// Token: 0x17001A06 RID: 6662
		// (get) Token: 0x060069B8 RID: 27064 RVA: 0x0027DDBC File Offset: 0x0027BFBC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("BargainBinCost", 100m),
					new DynamicVar("MysteryBoxCost", 300m),
					new DynamicVar("FeaturedItemCost", 200m),
					new DynamicVar("MysteryBoxRelicCount", 3m),
					new DynamicVar("MysteryBoxCombatCount", 5m),
					new DynamicVar("WongoPointAmount", 0m),
					new DynamicVar("RemainingWongoPointAmount", 0m),
					new DynamicVar("TotalWongoBadgeAmount", 0m),
					new StringVar("RandomRelic", "")
				});
			}
		}

		// Token: 0x060069B9 RID: 27065 RVA: 0x0027DE88 File Offset: 0x0027C088
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			Player owner = base.Owner;
			this.FeaturedItem = RelicFactory.PullNextRelicFromFront(owner, RelicRarity.Rare, (RelicModel r) => r.IsAllowedInShops);
			((StringVar)base.DynamicVars["RandomRelic"]).StringValue = this.FeaturedItem.Title.GetFormattedText();
			List<EventOption> list = new List<EventOption>();
			if (owner.Gold >= base.DynamicVars["BargainBinCost"].BaseValue)
			{
				list.Add(new EventOption(this, new Func<Task>(this.BuyBargainBin), "WELCOME_TO_WONGOS.pages.INITIAL.options.BARGAIN_BIN", Array.Empty<IHoverTip>()));
			}
			else
			{
				list.Add(new EventOption(this, null, "WELCOME_TO_WONGOS.pages.INITIAL.options.BARGAIN_BIN_LOCKED", Array.Empty<IHoverTip>()));
			}
			if (owner.Gold >= base.DynamicVars["FeaturedItemCost"].BaseValue)
			{
				list.Add(new EventOption(this, new Func<Task>(this.BuyFeaturedItem), "WELCOME_TO_WONGOS.pages.INITIAL.options.FEATURED_ITEM", this.FeaturedItem.HoverTips));
			}
			else
			{
				list.Add(new EventOption(this, null, "WELCOME_TO_WONGOS.pages.INITIAL.options.FEATURED_ITEM_LOCKED", Array.Empty<IHoverTip>()));
			}
			if (owner.Gold >= base.DynamicVars["MysteryBoxCost"].BaseValue)
			{
				list.Add(new EventOption(this, new Func<Task>(this.BuyMysteryBox), "WELCOME_TO_WONGOS.pages.INITIAL.options.MYSTERY_BOX", Array.Empty<IHoverTip>()));
			}
			else
			{
				list.Add(new EventOption(this, null, "WELCOME_TO_WONGOS.pages.INITIAL.options.MYSTERY_BOX_LOCKED", Array.Empty<IHoverTip>()));
			}
			list.Add(new EventOption(this, new Func<Task>(this.Leave), "WELCOME_TO_WONGOS.pages.INITIAL.options.LEAVE", Array.Empty<IHoverTip>()));
			return list;
		}

		// Token: 0x060069BA RID: 27066 RVA: 0x0027E048 File Offset: 0x0027C248
		private async Task<LocString> CheckObtainWongoBadge(int pointsEarned)
		{
			int wongoPoints = SaveManager.Instance.Progress.WongoPoints;
			int num = wongoPoints % 2000;
			int num2 = num + pointsEarned;
			int num3 = wongoPoints + pointsEarned;
			base.DynamicVars["WongoPointAmount"].BaseValue = num2;
			base.DynamicVars["RemainingWongoPointAmount"].BaseValue = 2000 - num2;
			base.DynamicVars["TotalWongoBadgeAmount"].BaseValue = num3 / 2000;
			base.Owner.ExtraFields.WongoPoints = pointsEarned;
			LocString locString;
			if (num2 >= 2000)
			{
				await RelicCmd.Obtain<WongoCustomerAppreciationBadge>(base.Owner);
				locString = base.L10NLookup("WELCOME_TO_WONGOS.pages.AFTER_BUY_RECEIVE_BADGE.description");
			}
			else if (base.DynamicVars["TotalWongoBadgeAmount"].BaseValue > 0m)
			{
				locString = base.L10NLookup("WELCOME_TO_WONGOS.pages.AFTER_BUY_BADGE_COUNTER.description");
			}
			else
			{
				locString = base.L10NLookup("WELCOME_TO_WONGOS.pages.AFTER_BUY.description");
			}
			return locString;
		}

		// Token: 0x060069BB RID: 27067 RVA: 0x0027E094 File Offset: 0x0027C294
		private async Task BuyBargainBin()
		{
			await PlayerCmd.LoseGold(base.DynamicVars["BargainBinCost"].BaseValue, base.Owner, GoldLossType.Spent);
			RelicModel relicModel = RelicFactory.PullNextRelicFromFront(base.Owner, RelicRarity.Common, (RelicModel r) => r.IsAllowedInShops).ToMutable();
			await RelicCmd.Obtain(relicModel, base.Owner, -1);
			LocString locString = await this.CheckObtainWongoBadge(32);
			base.SetEventFinished(locString);
		}

		// Token: 0x060069BC RID: 27068 RVA: 0x0027E0D8 File Offset: 0x0027C2D8
		private async Task BuyMysteryBox()
		{
			await PlayerCmd.LoseGold(base.DynamicVars["MysteryBoxCost"].BaseValue, base.Owner, GoldLossType.Spent);
			await RelicCmd.Obtain<WongosMysteryTicket>(base.Owner);
			LocString locString = await this.CheckObtainWongoBadge(8);
			base.SetEventFinished(locString);
		}

		// Token: 0x060069BD RID: 27069 RVA: 0x0027E11C File Offset: 0x0027C31C
		private async Task BuyFeaturedItem()
		{
			await PlayerCmd.LoseGold(base.DynamicVars["FeaturedItemCost"].BaseValue, base.Owner, GoldLossType.Spent);
			await RelicCmd.Obtain(this.FeaturedItem.ToMutable(), base.Owner, -1);
			LocString locString = await this.CheckObtainWongoBadge(16);
			base.SetEventFinished(locString);
		}

		// Token: 0x060069BE RID: 27070 RVA: 0x0027E160 File Offset: 0x0027C360
		private async Task Leave()
		{
			Player owner = base.Owner;
			CardModel cardModel = base.Rng.NextItem<CardModel>(owner.Deck.Cards.Where<CardModel>((CardModel c) => c.IsUpgraded));
			if (cardModel != null)
			{
				CardCmd.Downgrade(cardModel);
				CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.HorizontalLayout);
				await Cmd.CustomScaledWait(0.5f, 1.2f, false, default(CancellationToken));
			}
			base.SetEventFinished(base.L10NLookup("WELCOME_TO_WONGOS.pages.LEAVE.description"));
		}

		// Token: 0x040027E9 RID: 10217
		private const int _wongoPointsForBadge = 2000;

		// Token: 0x040027EA RID: 10218
		private const string _bargainBinCostKey = "BargainBinCost";

		// Token: 0x040027EB RID: 10219
		private const string _featuredItemCostKey = "FeaturedItemCost";

		// Token: 0x040027EC RID: 10220
		private const string _mysteryBoxCostKey = "MysteryBoxCost";

		// Token: 0x040027ED RID: 10221
		private const string _mysteryBoxRelicCountKey = "MysteryBoxRelicCount";

		// Token: 0x040027EE RID: 10222
		private const string _mysteryBoxCombatCountKey = "MysteryBoxCombatCount";

		// Token: 0x040027EF RID: 10223
		private const string _wongoPointAmountKey = "WongoPointAmount";

		// Token: 0x040027F0 RID: 10224
		private const string _remainingWongoPointAmountKey = "RemainingWongoPointAmount";

		// Token: 0x040027F1 RID: 10225
		private const string _totalWongoBadgeAmountKey = "TotalWongoBadgeAmount";

		// Token: 0x040027F2 RID: 10226
		private const string _randomRelicKey = "RandomRelic";

		// Token: 0x040027F3 RID: 10227
		[Nullable(2)]
		private RelicModel _featuredItem;
	}
}
