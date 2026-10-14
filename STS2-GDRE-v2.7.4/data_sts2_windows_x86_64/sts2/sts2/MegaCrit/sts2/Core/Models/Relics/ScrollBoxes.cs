using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005DA RID: 1498
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ScrollBoxes : RelicModel
	{
		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x060054A1 RID: 21665 RVA: 0x00246E93 File Offset: 0x00245093
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x00246E96 File Offset: 0x00245096
		public override bool IsAllowedAtNeow(Player player)
		{
			return ScrollBoxes.CanGenerateBundles(player) && base.IsAllowedAtNeow(player);
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x00246EAC File Offset: 0x002450AC
		public override async Task AfterObtained()
		{
			List<IReadOnlyList<CardModel>> list = ScrollBoxes.GenerateRandomBundles(base.Owner);
			List<IReadOnlyList<CardModel>> list2 = new List<IReadOnlyList<CardModel>>();
			foreach (IReadOnlyList<CardModel> readOnlyList in list)
			{
				list2.Add(readOnlyList.Select<CardModel, CardModel>((CardModel c) => base.Owner.RunState.CreateCard(c, base.Owner)).ToList<CardModel>());
			}
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromChooseABundleScreen(base.Owner, list2);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			}
			IEnumerator<CardModel> enumerator2 = null;
		}

		/// <summary>
		/// Checks if random bundles can be generated for the given player.
		/// Requires at least 4 unlocked commons and 2 unlocked uncommons.
		/// </summary>
		// Token: 0x060054A4 RID: 21668 RVA: 0x00246EF0 File Offset: 0x002450F0
		public static bool CanGenerateBundles(Player player)
		{
			IEnumerable<CardModel> unlockedCards = ScrollBoxes.GetCardPool(player.Character).GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint);
			int num = unlockedCards.Count<CardModel>((CardModel c) => c.Rarity == CardRarity.Common);
			int num2 = unlockedCards.Count<CardModel>((CardModel c) => c.Rarity == CardRarity.Uncommon);
			return num >= 4 && num2 >= 2;
		}

		/// <summary>
		/// Generates 2 random bundles for the player. Each bundle contains 2 commons and 1 uncommon.
		/// All 6 cards across both bundles are unique.
		/// For Defect, each bundle has a 1% chance to be 3x Claw instead.
		/// </summary>
		// Token: 0x060054A5 RID: 21669 RVA: 0x00246F78 File Offset: 0x00245178
		public static List<IReadOnlyList<CardModel>> GenerateRandomBundles(Player player)
		{
			Rng rewards = player.PlayerRng.Rewards;
			bool flag = player.Character is Defect;
			CardPoolModel cardPool = ScrollBoxes.GetCardPool(player.Character);
			CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithUniformOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(cardPool), (CardModel c) => c.Rarity == CardRarity.Common).WithFlags(CardCreationFlags.NoRarityModification);
			cardCreationOptions = Hook.ModifyCardRewardCreationOptions(player.RunState, player, cardCreationOptions);
			CardCreationOptions cardCreationOptions2 = CardCreationOptions.ForNonCombatWithUniformOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(cardPool), (CardModel c) => c.Rarity == CardRarity.Uncommon).WithFlags(CardCreationFlags.NoRarityModification);
			cardCreationOptions2 = Hook.ModifyCardRewardCreationOptions(player.RunState, player, cardCreationOptions2);
			List<CardModel> list = cardCreationOptions.GetPossibleCards(player).ToList<CardModel>();
			List<CardModel> list2 = cardCreationOptions2.GetPossibleCards(player).ToList<CardModel>();
			List<IReadOnlyList<CardModel>> list3 = new List<IReadOnlyList<CardModel>>();
			HashSet<ModelId> usedCardIds = new HashSet<ModelId>();
			Func<CardModel, bool> <>9__2;
			Func<CardModel, bool> <>9__3;
			for (int i = 0; i < 2; i++)
			{
				if (flag && rewards.NextInt(100) < 1)
				{
					CardModel cardModel = ModelDb.Card<Claw>();
					list3.Add(new <>z__ReadOnlyArray<CardModel>(new CardModel[] { cardModel, cardModel, cardModel }));
				}
				else
				{
					List<CardModel> list4 = new List<CardModel>();
					IEnumerable<CardModel> enumerable = list;
					Func<CardModel, bool> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = (CardModel c) => !usedCardIds.Contains(c.Id));
					}
					List<CardModel> list5 = enumerable.Where<CardModel>(func).ToList<CardModel>();
					for (int j = 0; j < 2; j++)
					{
						CardModel cardModel2 = rewards.NextItem<CardModel>(list5);
						list4.Add(cardModel2);
						usedCardIds.Add(cardModel2.Id);
						list5.Remove(cardModel2);
					}
					IEnumerable<CardModel> enumerable2 = list2;
					Func<CardModel, bool> func2;
					if ((func2 = <>9__3) == null)
					{
						func2 = (<>9__3 = (CardModel c) => !usedCardIds.Contains(c.Id));
					}
					List<CardModel> list6 = enumerable2.Where<CardModel>(func2).ToList<CardModel>();
					CardModel cardModel3 = rewards.NextItem<CardModel>(list6);
					list4.Add(cardModel3);
					usedCardIds.Add(cardModel3.Id);
					list3.Add(list4);
				}
			}
			return list3;
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x0024718D File Offset: 0x0024538D
		private static CardPoolModel GetCardPool(CharacterModel character)
		{
			if (TestMode.IsOn && character is Deprived)
			{
				return ModelDb.Character<Ironclad>().CardPool;
			}
			return character.CardPool;
		}

		// Token: 0x040024B5 RID: 9397
		private const int _clawBundleChancePercent = 1;
	}
}
