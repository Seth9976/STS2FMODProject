using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200056E RID: 1390
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Kaleidoscope : RelicModel
	{
		// Token: 0x060051C1 RID: 20929 RVA: 0x00241858 File Offset: 0x0023FA58
		public override bool IsAllowedAtNeow(Player player)
		{
			return base.IsAllowedAtNeow(player) && player.UnlockState.CharacterCardPools.Count<CardPoolModel>() == ModelDb.AllCharacters.Count<CharacterModel>();
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x060051C2 RID: 20930 RVA: 0x00241881 File Offset: 0x0023FA81
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x060051C3 RID: 20931 RVA: 0x00241884 File Offset: 0x0023FA84
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x060051C4 RID: 20932 RVA: 0x00241887 File Offset: 0x0023FA87
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x00241894 File Offset: 0x0023FA94
		public override async Task AfterObtained()
		{
			List<Reward> list = new List<Reward>();
			CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithDefaultOdds(Array.Empty<CardPoolModel>(), null);
			for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
			{
				List<CardModel> list2 = new List<CardModel>();
				IEnumerable<CardPoolModel> enumerable = base.Owner.UnlockState.CharacterCardPools.Where<CardPoolModel>((CardPoolModel p) => p != base.Owner.Character.CardPool).ToList<CardPoolModel>().StableShuffle<CardPoolModel>(base.Owner.RunState.Rng.Niche)
					.Take<CardPoolModel>(3);
				foreach (CardPoolModel cardPoolModel in enumerable)
				{
					CardCreationOptions cardCreationOptions2 = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(cardPoolModel), CardCreationSource.Other, CardRarityOddsType.RegularEncounter, null).WithFlags(CardCreationFlags.NoCardPoolModifications);
					list2.Add(CardFactory.CreateForReward(base.Owner, 1, cardCreationOptions2).First<CardCreationResult>().Card);
				}
				list.Add(new CardReward(list2, CardCreationSource.Other, base.Owner, cardCreationOptions, null));
			}
			await RewardsCmd.OfferCustom(base.Owner, list);
		}
	}
}
