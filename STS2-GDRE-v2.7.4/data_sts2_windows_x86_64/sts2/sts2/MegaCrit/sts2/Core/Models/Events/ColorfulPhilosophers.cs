using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200082F RID: 2095
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ColorfulPhilosophers : EventModel
	{
		// Token: 0x17001968 RID: 6504
		// (get) Token: 0x06006790 RID: 26512 RVA: 0x002731CF File Offset: 0x002713CF
		private static IEnumerable<CardPoolModel> CardPoolColorOrder
		{
			get
			{
				return new <>z__ReadOnlyArray<CardPoolModel>(new CardPoolModel[]
				{
					ModelDb.CardPool<NecrobinderCardPool>(),
					ModelDb.CardPool<IroncladCardPool>(),
					ModelDb.CardPool<RegentCardPool>(),
					ModelDb.CardPool<SilentCardPool>(),
					ModelDb.CardPool<DefectCardPool>()
				});
			}
		}

		// Token: 0x17001969 RID: 6505
		// (get) Token: 0x06006791 RID: 26513 RVA: 0x00273204 File Offset: 0x00271404
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x06006792 RID: 26514 RVA: 0x00273211 File Offset: 0x00271411
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.UnlockState.CharacterCardPools.Count<CardPoolModel>() > 1);
		}

		// Token: 0x06006793 RID: 26515 RVA: 0x00273240 File Offset: 0x00271440
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			List<EventOption> list = new List<EventOption>();
			CharacterModel character = base.Owner.Character;
			List<CardPoolModel> list2 = base.Owner.UnlockState.CharacterCardPools.ToList<CardPoolModel>();
			using (IEnumerator<CardPoolModel> enumerator = ColorfulPhilosophers.CardPoolColorOrder.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CardPoolModel cardPool = enumerator.Current;
					if (character.CardPool != cardPool && list2.Contains(cardPool))
					{
						list.Add(new EventOption(this, () => this.OfferRewards(cardPool), "COLORFUL_PHILOSOPHERS.pages.INITIAL.options." + cardPool.EnergyColorName.ToUpperInvariant(), Array.Empty<IHoverTip>()));
					}
				}
			}
			int num = Mathf.Min(3, list.Count);
			while (list.Count > num)
			{
				list.RemoveAt(base.Rng.NextInt(list.Count));
			}
			return list;
		}

		// Token: 0x06006794 RID: 26516 RVA: 0x00273350 File Offset: 0x00271550
		private async Task OfferRewards(CardPoolModel pool)
		{
			CardCreationOptions cardCreationOptions = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(pool), CardCreationSource.Other, CardRarityOddsType.Uniform, (CardModel c) => c.Rarity == CardRarity.Common).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
			CardCreationOptions cardCreationOptions2 = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(pool), CardCreationSource.Other, CardRarityOddsType.Uniform, (CardModel c) => c.Rarity == CardRarity.Uncommon).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
			CardCreationOptions cardCreationOptions3 = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(pool), CardCreationSource.Other, CardRarityOddsType.Uniform, (CardModel c) => c.Rarity == CardRarity.Rare).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
			await RewardsCmd.OfferCustom(base.Owner, new List<Reward>(3)
			{
				new CardReward(cardCreationOptions, base.DynamicVars.Cards.IntValue, base.Owner, null),
				new CardReward(cardCreationOptions2, base.DynamicVars.Cards.IntValue, base.Owner, null),
				new CardReward(cardCreationOptions3, base.DynamicVars.Cards.IntValue, base.Owner, null)
			});
			base.SetEventFinished(base.L10NLookup("COLORFUL_PHILOSOPHERS.pages.DONE.description"));
		}
	}
}
