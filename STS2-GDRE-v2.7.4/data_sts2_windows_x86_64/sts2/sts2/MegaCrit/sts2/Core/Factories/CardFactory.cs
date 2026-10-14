using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Factories
{
	// Token: 0x02000C43 RID: 3139
	[NullableContext(1)]
	[Nullable(0)]
	public static class CardFactory
	{
		// Token: 0x1700235B RID: 9051
		// (get) Token: 0x06008173 RID: 33139 RVA: 0x002BE470 File Offset: 0x002BC670
		private static decimal UpgradedCardOddScaling
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.125m, 0.25m);
			}
		}

		// Token: 0x06008174 RID: 33140 RVA: 0x002BE490 File Offset: 0x002BC690
		private static IEnumerable<CardModel> FilterForPlayerCount(IRunState runState, IEnumerable<CardModel> options)
		{
			if (runState.Players.Count > 1)
			{
				return options.Where<CardModel>((CardModel c) => c.MultiplayerConstraint != CardMultiplayerConstraint.SingleplayerOnly);
			}
			return options.Where<CardModel>((CardModel c) => c.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly);
		}

		/// <summary>
		/// Creates a card to display to the player at the merchant. Takes in a card type, and rarity is rolled.
		/// </summary>
		/// <param name="player">The player for which we're creating cards to display.</param>
		/// <param name="options">The cards to pull from.</param>
		/// <param name="type">The card type to generate.</param>
		// Token: 0x06008175 RID: 33141 RVA: 0x002BE4F8 File Offset: 0x002BC6F8
		public static CardCreationResult CreateForMerchant(Player player, IEnumerable<CardModel> options, CardType type)
		{
			if (player.Character is Deprived)
			{
				throw new InvalidOperationException("Merchant inventory can't be generated for the test character. Update your test to use Ironclad.");
			}
			options = Hook.ModifyMerchantCardPool(player.RunState, player, options);
			options = options.Where<CardModel>((CardModel c) => c.Rarity != CardRarity.Basic);
			options = CardFactory.FilterForPlayerCount(player.RunState, options);
			CardModel[] optionsArr = options.ToArray<CardModel>();
			CardRarity cardRarity = Hook.ModifyMerchantCardRarity(player.RunState, player, player.PlayerOdds.CardRarity.RollWithoutChangingFutureOdds(CardRarityOddsType.Shop));
			CardRarity rarity = CardFactory.GetNextAllowedRarity(cardRarity, (CardRarity r) => optionsArr.Any<CardModel>((CardModel c) => c.Rarity == r && c.Type == type));
			if (rarity == CardRarity.None)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Can't generate valid rarity for merchant card type ");
				defaultInterpolatedStringHandler.AppendFormatted<CardType>(type);
				defaultInterpolatedStringHandler.AppendLiteral(" with card options: ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<ModelId>(",", optionsArr.Select<CardModel, ModelId>((CardModel c) => c.Id)));
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<CardModel> list = optionsArr.Where<CardModel>((CardModel c) => c.Rarity == rarity && c.Type == type).ToList<CardModel>();
			CardModel cardModel = player.RunState.CreateCard(player.PlayerRng.Shops.NextItem<CardModel>(list), player);
			CardFactory.RollForUpgrade(player, cardModel, -999999999m);
			return new CardCreationResult(cardModel);
		}

		/// <summary>
		/// Creates a set of cards to display to the player at the merchant.
		/// The rarity of cards are affected by the source.
		/// </summary>
		/// <param name="player">The player for which we're creating cards to display.</param>
		/// <param name="options">The cards to pull from.</param>
		/// <param name="rarity">The card rarity to generate. This rarity may be modified by hooks.</param>
		// Token: 0x06008176 RID: 33142 RVA: 0x002BE684 File Offset: 0x002BC884
		public static CardCreationResult CreateForMerchant(Player player, IEnumerable<CardModel> options, CardRarity rarity)
		{
			options = Hook.ModifyMerchantCardPool(player.RunState, player, options);
			options = options.Where<CardModel>((CardModel c) => c.Rarity != CardRarity.Basic);
			options = CardFactory.FilterForPlayerCount(player.RunState, options);
			CardModel[] array = options.ToArray<CardModel>();
			CardRarity modifiedRarity = Hook.ModifyMerchantCardRarity(player.RunState, player, rarity);
			IEnumerable<CardModel> enumerable = array.Where<CardModel>((CardModel c) => c.Rarity == modifiedRarity);
			CardModel cardModel = player.RunState.CreateCard(player.PlayerRng.Shops.NextItem<CardModel>(enumerable), player);
			CardFactory.RollForUpgrade(player, cardModel, -999999999m);
			return new CardCreationResult(cardModel);
		}

		/// <summary>
		/// Creates a set of cards for the player to choose 1 from.
		/// The rarity of cards are affected by the source.
		/// </summary>
		/// <param name="player">The player for which we're creating rewards.</param>
		/// <param name="cardCount">How many choices to offer (usually 3).</param>
		/// <param name="options">The options to use when creating the cards.</param>
		// Token: 0x06008177 RID: 33143 RVA: 0x002BE740 File Offset: 0x002BC940
		public static IEnumerable<CardCreationResult> CreateForReward(Player player, int cardCount, CardCreationOptions options)
		{
			List<CardModel> list = new List<CardModel>();
			List<CardCreationResult> list2 = new List<CardCreationResult>();
			for (int i = 0; i < cardCount; i++)
			{
				CardModel cardModel = CardFactory.CreateForReward(player, list, options);
				list.Add(cardModel.CanonicalInstance);
				list2.Add(new CardCreationResult(cardModel));
				if (!options.Flags.HasFlag(CardCreationFlags.NoUpgradeRoll))
				{
					Rng rng = options.RngOverride ?? player.PlayerRng.Rewards;
					CardFactory.RollForUpgrade(player, cardModel, 0m, rng);
				}
			}
			List<AbstractModel> list3;
			if (!options.Flags.HasFlag(CardCreationFlags.NoModifyHooks) && Hook.TryModifyCardRewardOptions(player.RunState, player, list2, options, out list3))
			{
				TaskHelper.RunSafely(Hook.AfterModifyingCardRewardOptions(player.RunState, list3));
			}
			return list2;
		}

		/// <summary>
		/// Get a set of distinct cards for use in combat generation effects like Attack Potion or Discovery.
		/// </summary>
		/// <param name="player">The owner of the newly created cards.</param>
		/// <param name="cards">Cards to choose from.</param>
		/// <param name="count">Number of cards to get.</param>
		/// <param name="rng">RNG to use.</param>
		/// <returns>Mutable cards owned by the player and created within its combat state, for combat generation effects.</returns>
		// Token: 0x06008178 RID: 33144 RVA: 0x002BE804 File Offset: 0x002BCA04
		public static IEnumerable<CardModel> GetDistinctForCombat(Player player, IEnumerable<CardModel> cards, int count, Rng rng)
		{
			List<CardModel> list = TestRngInjector.ConsumeCombatCardGenerationOverride();
			if (list != null)
			{
				return list;
			}
			cards = CardFactory.FilterForPlayerCount(player.RunState, cards);
			return from c in CardFactory.FilterForCombat(cards).TakeRandom<CardModel>(count, rng)
				select player.Creature.CombatState.CreateCard(c, player);
		}

		/// <summary>
		/// Get a set of cards for use in combat generation effects like Calamity.
		/// Can include multiple of the same card, unlike <see cref="M:MegaCrit.Sts2.Core.Factories.CardFactory.GetDistinctForCombat(MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.IEnumerable{MegaCrit.Sts2.Core.Models.CardModel},System.Int32,MegaCrit.Sts2.Core.Random.Rng)" />.
		/// </summary>
		/// <param name="player">The owner of the newly created cards.</param>
		/// <param name="cards">Cards to choose from.</param>
		/// <param name="count">Number of cards to get.</param>
		/// <param name="rng">RNG to use.</param>
		/// <returns>Cards for combat generation effects.</returns>
		// Token: 0x06008179 RID: 33145 RVA: 0x002BE85C File Offset: 0x002BCA5C
		public static IEnumerable<CardModel> GetForCombat(Player player, IEnumerable<CardModel> cards, int count, Rng rng)
		{
			List<CardModel> list = CardFactory.FilterForCombat(cards).ToList<CardModel>();
			list = CardFactory.FilterForPlayerCount(player.RunState, list).ToList<CardModel>();
			List<CardModel> list2 = new List<CardModel>();
			for (int i = 0; i < count; i++)
			{
				CardModel cardModel = rng.NextItem<CardModel>(list);
				CardModel cardModel2 = player.Creature.CombatState.CreateCard(cardModel, player);
				list2.Add(cardModel2);
			}
			return list2;
		}

		/// <summary>
		/// Filter out cards that should not be included in combat card generation effects like Attack Potion or Discovery.
		/// </summary>
		/// <param name="cards">Cards to filter.</param>
		/// <returns>Cards for combat generation effects.</returns>
		// Token: 0x0600817A RID: 33146 RVA: 0x002BE8BE File Offset: 0x002BCABE
		public static IEnumerable<CardModel> FilterForCombat(IEnumerable<CardModel> cards)
		{
			return cards.Where<CardModel>((CardModel c) => c.CanBeGeneratedInCombat && c.Rarity != CardRarity.Basic && c.Rarity != CardRarity.Ancient && c.Rarity != CardRarity.Event).Distinct<CardModel>();
		}

		/// <summary>
		/// Get the default set of cards that the specified card should be able to be transformed into.
		/// </summary>
		/// <param name="original">Card that will be transformed.</param>
		/// <param name="isInCombat">Whether the transformation is happening in combat.</param>
		/// <returns>The set of cards that the card can be transformed into.</returns>
		// Token: 0x0600817B RID: 33147 RVA: 0x002BE8EC File Offset: 0x002BCAEC
		public static IEnumerable<CardModel> GetDefaultTransformationOptions(CardModel original, bool isInCombat)
		{
			CardPoolModel cardPoolModel = ((original.Type != CardType.Quest && original.Rarity != CardRarity.Event && original.Rarity != CardRarity.Ancient && original.Rarity != CardRarity.Token) ? original.Pool : ModelDb.CardPool<ColorlessCardPool>());
			IEnumerable<CardModel> unlockedCards = cardPoolModel.GetUnlockedCards(original.Owner.UnlockState, original.RunState.CardMultiplayerConstraint);
			return CardFactory.GetFilteredTransformationOptions(original, unlockedCards, isInCombat);
		}

		// Token: 0x0600817C RID: 33148 RVA: 0x002BE95C File Offset: 0x002BCB5C
		public static CardModel CreateRandomCardForTransform(CardModel original, bool isInCombat, Rng rng)
		{
			IEnumerable<CardModel> defaultTransformationOptions = CardFactory.GetDefaultTransformationOptions(original, isInCombat);
			return original.CardScope.CreateCard(rng.NextItem<CardModel>(defaultTransformationOptions), original.Owner);
		}

		// Token: 0x0600817D RID: 33149 RVA: 0x002BE98C File Offset: 0x002BCB8C
		public static CardModel CreateRandomCardForTransform(CardModel original, IEnumerable<CardModel> options, bool isInCombat, Rng rng)
		{
			CardModel[] filteredTransformationOptions = CardFactory.GetFilteredTransformationOptions(original, options, isInCombat);
			return original.CardScope.CreateCard(rng.NextItem<CardModel>(filteredTransformationOptions), original.Owner);
		}

		// Token: 0x0600817E RID: 33150 RVA: 0x002BE9BC File Offset: 0x002BCBBC
		private static CardModel[] GetFilteredTransformationOptions(CardModel original, IEnumerable<CardModel> originalOptions, bool isInCombat)
		{
			IEnumerable<CardModel> enumerable = originalOptions;
			CardRarity rarity = original.Rarity;
			if (rarity - CardRarity.Status > 1)
			{
				enumerable = enumerable.Where<CardModel>(delegate(CardModel c)
				{
					CardRarity rarity2 = c.Rarity;
					return rarity2 - CardRarity.Common <= 2;
				});
			}
			if (isInCombat)
			{
				enumerable = enumerable.Where<CardModel>((CardModel c) => c.CanBeGeneratedInCombat);
			}
			enumerable = enumerable.Where<CardModel>((CardModel c) => c.Id != original.Id).ToList<CardModel>();
			CardModel[] array = CardFactory.FilterForPlayerCount(original.Owner.RunState, enumerable).ToArray<CardModel>();
			if (array.Length == 0)
			{
				throw new InvalidOperationException("All transformation options provided are invalid! Original options: " + string.Join<CardModel>(",", originalOptions));
			}
			return array;
		}

		// Token: 0x0600817F RID: 33151 RVA: 0x002BEA9C File Offset: 0x002BCC9C
		private static CardModel CreateForReward(Player player, IEnumerable<CardModel> blacklist, CardCreationOptions options)
		{
			options = Hook.ModifyCardRewardCreationOptions(player.RunState, player, options);
			IEnumerable<CardModel> enumerable = options.GetPossibleCards(player).Except<CardModel>(blacklist).ToList<CardModel>();
			enumerable = CardFactory.FilterForPlayerCount(player.RunState, enumerable).ToArray<CardModel>();
			CardRarity? selectedRarity = null;
			IEnumerable<CardModel> enumerable2;
			if (options.RarityOdds == CardRarityOddsType.Uniform)
			{
				enumerable2 = enumerable.Where<CardModel>((CardModel c) => c.Rarity != CardRarity.Basic && c.Rarity != CardRarity.Ancient);
			}
			else
			{
				HashSet<CardRarity> hashSet = enumerable.Select<CardModel, CardRarity>((CardModel c) => c.Rarity).ToHashSet<CardRarity>();
				selectedRarity = new CardRarity?(CardFactory.RollForRarity(player, options.RarityOdds, options.Source, hashSet, options.Flags.HasFlag(CardCreationFlags.ForceRarityOddsChange)));
				CardRarity? selectedRarity3 = selectedRarity;
				CardRarity cardRarity = CardRarity.None;
				if ((selectedRarity3.GetValueOrDefault() == cardRarity) & (selectedRarity3 != null))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(109, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to create a card for a reward, but we couldn't generate a valid rarity! Odds: ");
					defaultInterpolatedStringHandler.AppendFormatted<CardRarityOddsType>(options.RarityOdds);
					defaultInterpolatedStringHandler.AppendLiteral(" Card pool: ");
					defaultInterpolatedStringHandler.AppendFormatted(string.Join<CardModel>(",", enumerable));
					defaultInterpolatedStringHandler.AppendLiteral(", blacklist: ");
					defaultInterpolatedStringHandler.AppendFormatted(string.Join<CardModel>(",", blacklist));
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				enumerable2 = enumerable.Where<CardModel>(delegate(CardModel card)
				{
					CardRarity rarity = card.Rarity;
					CardRarity? selectedRarity2 = selectedRarity;
					return (rarity == selectedRarity2.GetValueOrDefault()) & (selectedRarity2 != null);
				});
			}
			Rng rng = options.RngOverride ?? player.PlayerRng.Rewards;
			CardModel cardModel = rng.NextItem<CardModel>(enumerable2);
			if (cardModel == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(127, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to create a card for a reward, but we couldn't generate a valid card! Selected rarity: ");
				defaultInterpolatedStringHandler.AppendFormatted<CardRarity?>(selectedRarity);
				defaultInterpolatedStringHandler.AppendLiteral(", card pool: ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<CardModel>(",", enumerable));
				defaultInterpolatedStringHandler.AppendLiteral(", blacklist: ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<CardModel>(",", blacklist));
				defaultInterpolatedStringHandler.AppendLiteral(", odds: ");
				defaultInterpolatedStringHandler.AppendFormatted<CardRarityOddsType>(options.RarityOdds);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return player.RunState.CreateCard(cardModel, player);
		}

		// Token: 0x06008180 RID: 33152 RVA: 0x002BECE0 File Offset: 0x002BCEE0
		private static CardRarity RollForRarity(Player player, CardRarityOddsType rollMethod, CardCreationSource source, HashSet<CardRarity> allowedRarities, bool forceRarityOddsChange)
		{
			bool flag = forceRarityOddsChange;
			if (!flag)
			{
				bool flag2 = source == CardCreationSource.Encounter;
				bool flag3 = flag2;
				if (flag3)
				{
					bool flag4 = rollMethod - CardRarityOddsType.RegularEncounter <= 2;
					flag3 = flag4;
				}
				flag = flag3;
			}
			CardRarity cardRarity;
			if (flag)
			{
				cardRarity = player.PlayerOdds.CardRarity.Roll(rollMethod);
			}
			else
			{
				cardRarity = player.PlayerOdds.CardRarity.RollWithBaseOdds(rollMethod);
			}
			return CardFactory.GetNextAllowedRarity(cardRarity, new Func<CardRarity, bool>(allowedRarities.Contains));
		}

		// Token: 0x06008181 RID: 33153 RVA: 0x002BED54 File Offset: 0x002BCF54
		private unsafe static CardRarity GetNextAllowedRarity(CardRarity rarity, Func<CardRarity, bool> isAllowed)
		{
			int num = 1;
			List<CardRarity> list = new List<CardRarity>(num);
			CollectionsMarshal.SetCount<CardRarity>(list, num);
			Span<CardRarity> span = CollectionsMarshal.AsSpan<CardRarity>(list);
			int num2 = 0;
			*span[num2] = rarity;
			List<CardRarity> list2 = list;
			while (!isAllowed(rarity) && rarity != CardRarity.None)
			{
				rarity = rarity.GetNextHighestRarityWithWrapping();
				if (list2.Contains(rarity))
				{
					return CardRarity.None;
				}
			}
			return rarity;
		}

		// Token: 0x06008182 RID: 33154 RVA: 0x002BEDA6 File Offset: 0x002BCFA6
		private static void RollForUpgrade(Player player, CardModel card, decimal baseChance)
		{
			CardFactory.RollForUpgrade(player, card, baseChance, player.PlayerRng.Rewards);
		}

		// Token: 0x06008183 RID: 33155 RVA: 0x002BEDBC File Offset: 0x002BCFBC
		private static void RollForUpgrade(Player player, CardModel card, decimal baseChance, Rng rng)
		{
			decimal num = (decimal)rng.NextFloat(1f);
			if (!card.IsUpgradable)
			{
				return;
			}
			decimal num2 = baseChance;
			if (card.Rarity != CardRarity.Rare)
			{
				int currentActIndex = player.RunState.CurrentActIndex;
				num2 += currentActIndex * CardFactory.UpgradedCardOddScaling;
			}
			num2 = Hook.ModifyCardRewardUpgradeOdds(player.RunState, player, card, num2);
			if (num <= num2)
			{
				CardCmd.Upgrade(card, CardPreviewStyle.HorizontalLayout);
			}
		}
	}
}
