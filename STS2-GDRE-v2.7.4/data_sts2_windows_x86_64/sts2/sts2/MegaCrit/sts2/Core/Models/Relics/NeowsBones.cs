using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000593 RID: 1427
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NeowsBones : RelicModel
	{
		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x060052B8 RID: 21176 RVA: 0x00243687 File Offset: 0x00241887
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x060052B9 RID: 21177 RVA: 0x0024368A File Offset: 0x0024188A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("Relics", 2m),
					new DynamicVar("Curses", 1m)
				});
			}
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x002436BC File Offset: 0x002418BC
		private static IEnumerable<RelicModel> GetValidRelics(Player owner)
		{
			return (from o in ModelDb.Event<Neow>().AllPossibleOptions
				where o.Relic != null && o.Relic.IsAllowedAtNeow(owner) && !(o.Relic is NeowsBones)
				select o.Relic).OfType<RelicModel>();
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x0024371C File Offset: 0x0024191C
		public override async Task AfterObtained()
		{
			List<RelicModel> list = NeowsBones.GetValidRelics(base.Owner).ToList<RelicModel>();
			base.Owner.PlayerRng.Rewards.Shuffle<RelicModel>(list);
			List<Reward> list2 = (from r in list.Take<RelicModel>(base.DynamicVars["Relics"].IntValue)
				select new RelicReward(r, base.Owner)).ToList<Reward>();
			await new RewardsSet(base.Owner, null).WithCustomRewards(list2).WithSkippingDisallowed().Offer();
			List<CardModel> availableCurses = (from c in ModelDb.CardPool<CurseCardPool>().GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
				where c.CanBeGeneratedByModifiers
				orderby c.Id
				select c).ToList<CardModel>();
			List<CardPileAddResult> curseResults = new List<CardPileAddResult>();
			for (int i = 0; i < base.DynamicVars["Curses"].IntValue; i++)
			{
				CardModel cardModel = base.Owner.RunState.Rng.Niche.NextItem<CardModel>(availableCurses);
				availableCurses.Remove(cardModel);
				curseResults.Add(await CardPileCmd.Add(base.Owner.RunState.CreateCard(cardModel, base.Owner), PileType.Deck, CardPilePosition.Bottom, null, false));
			}
			CardCmd.PreviewCardPileAdd(curseResults, 2f, CardPreviewStyle.HorizontalLayout);
			await Cmd.Wait(0.75f, false);
		}

		// Token: 0x0400247D RID: 9341
		private const string _relicCountKey = "Relics";

		// Token: 0x0400247E RID: 9342
		private const string _cursesCountKey = "Curses";
	}
}
