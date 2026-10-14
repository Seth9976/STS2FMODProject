using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200085C RID: 2140
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheFutureOfPotions : EventModel
	{
		// Token: 0x170019E9 RID: 6633
		// (get) Token: 0x06006944 RID: 26948 RVA: 0x0027BA4C File Offset: 0x00279C4C
		private LocString ChoiceTitle
		{
			get
			{
				return new LocString("events", "THE_FUTURE_OF_POTIONS.pages.INITIAL.options.POTION.title");
			}
		}

		// Token: 0x170019EA RID: 6634
		// (get) Token: 0x06006945 RID: 26949 RVA: 0x0027BA5D File Offset: 0x00279C5D
		private LocString ChoiceDescription
		{
			get
			{
				return new LocString("events", "THE_FUTURE_OF_POTIONS.pages.INITIAL.options.POTION.description");
			}
		}

		// Token: 0x06006946 RID: 26950 RVA: 0x0027BA6E File Offset: 0x00279C6E
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Potions.Count<PotionModel>() >= 2);
		}

		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x06006947 RID: 26951 RVA: 0x0027BA9C File Offset: 0x00279C9C
		private unsafe Dictionary<PotionModel, CardType> PotionToCardType
		{
			get
			{
				base.AssertMutable();
				if (this._cardTypes == null)
				{
					this._cardTypes = new Dictionary<PotionModel, CardType>();
					foreach (PotionModel potionModel in base.Owner.Potions)
					{
						int num = 3;
						List<CardType> list = new List<CardType>(num);
						CollectionsMarshal.SetCount<CardType>(list, num);
						Span<CardType> span = CollectionsMarshal.AsSpan<CardType>(list);
						int num2 = 0;
						*span[num2] = CardType.Attack;
						num2++;
						*span[num2] = CardType.Skill;
						num2++;
						*span[num2] = CardType.Power;
						List<CardType> list2 = list;
						if (potionModel.Rarity == PotionRarity.Common || potionModel.Rarity == PotionRarity.Token)
						{
							list2.Remove(CardType.Power);
						}
						this._cardTypes.Add(potionModel, base.Rng.NextItem<CardType>(list2));
					}
				}
				return this._cardTypes;
			}
		}

		// Token: 0x06006948 RID: 26952 RVA: 0x0027BB8C File Offset: 0x00279D8C
		protected override Task BeforeEventStarted(bool isPreFinished)
		{
			base.Owner.CanUseOrRemovePotions = false;
			return Task.CompletedTask;
		}

		// Token: 0x06006949 RID: 26953 RVA: 0x0027BB9F File Offset: 0x00279D9F
		protected override void OnEventFinished()
		{
			base.Owner.CanUseOrRemovePotions = true;
		}

		// Token: 0x0600694A RID: 26954 RVA: 0x0027BBB0 File Offset: 0x00279DB0
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			List<EventOption> list = new List<EventOption>();
			List<PotionModel> list2 = base.Owner.Potions.ToList<PotionModel>();
			int num = Mathf.Min(3, list2.Count);
			for (int i = 0; i < num; i++)
			{
				TheFutureOfPotions.<>c__DisplayClass14_0 CS$<>8__locals1 = new TheFutureOfPotions.<>c__DisplayClass14_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.potion = list2[i];
				LocString choiceTitle = this.ChoiceTitle;
				choiceTitle.Add("Rarity", CS$<>8__locals1.potion.Rarity.ToLocString().GetFormattedText());
				LocString choiceDescription = this.ChoiceDescription;
				choiceDescription.Add("Potion", CS$<>8__locals1.potion.Title.GetFormattedText());
				choiceDescription.Add("Rarity", this.GetCardRarity(CS$<>8__locals1.potion).ToLocString().GetFormattedText());
				choiceDescription.Add("Type", this.PotionToCardType[CS$<>8__locals1.potion].ToLocString().GetFormattedText());
				list.Add(new EventOption(this, delegate
				{
					TheFutureOfPotions.<>c__DisplayClass14_0.<<GenerateInitialOptions>b__0>d <<GenerateInitialOptions>b__0>d;
					<<GenerateInitialOptions>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<GenerateInitialOptions>b__0>d.<>4__this = CS$<>8__locals1;
					<<GenerateInitialOptions>b__0>d.<>1__state = -1;
					<<GenerateInitialOptions>b__0>d.<>t__builder.Start<TheFutureOfPotions.<>c__DisplayClass14_0.<<GenerateInitialOptions>b__0>d>(ref <<GenerateInitialOptions>b__0>d);
					return <<GenerateInitialOptions>b__0>d.<>t__builder.Task;
				}, choiceTitle, choiceDescription, "THE_FUTURE_OF_POTIONS.pages.INITIAL.options.POTION", CS$<>8__locals1.potion.HoverTips).ThatHasDynamicTitle());
			}
			return list;
		}

		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x0600694B RID: 26955 RVA: 0x0027BCE0 File Offset: 0x00279EE0
		public override IEnumerable<LocString> GameInfoOptions
		{
			get
			{
				List<LocString> list = base.GameInfoOptions.ToList<LocString>();
				if (list.Count != 2)
				{
					throw new InvalidOperationException("TheFutureOfPotions must've changed loc format, please update its\nGameInfoOptions method.");
				}
				LocString locString = list.First<LocString>((LocString o) => o.LocEntryKey.EndsWith(".title"));
				locString.Add("Rarity", "[rarity]");
				LocString locString2 = list.First<LocString>((LocString o) => o.LocEntryKey.EndsWith(".description"));
				locString2.Add("Potion", "[potion]");
				locString2.Add("Rarity", "[same-rarity]");
				locString2.Add("Type", "[card of random type]");
				return list;
			}
		}

		// Token: 0x0600694C RID: 26956 RVA: 0x0027BD9C File Offset: 0x00279F9C
		private async Task Trade(PotionModel potion)
		{
			TheFutureOfPotions.<>c__DisplayClass17_0 CS$<>8__locals1 = new TheFutureOfPotions.<>c__DisplayClass17_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.potion = potion;
			CS$<>8__locals1.targetRarity = this.GetCardRarity(CS$<>8__locals1.potion);
			await PotionCmd.Discard(CS$<>8__locals1.potion);
			CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithUniformOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(base.Owner.Character.CardPool), (CardModel c) => c.Rarity == CS$<>8__locals1.targetRarity && c.Type == CS$<>8__locals1.<>4__this.PotionToCardType[CS$<>8__locals1.potion]).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
			CS$<>8__locals1.reward = new CardReward(cardCreationOptions, 3, base.Owner, null);
			CS$<>8__locals1.reward.AfterGenerated += CS$<>8__locals1.<Trade>g__UpgradeCardsInReward|1;
			await RewardsCmd.OfferCustom(base.Owner, new List<Reward>(1) { CS$<>8__locals1.reward });
			await this.Done();
		}

		// Token: 0x0600694D RID: 26957 RVA: 0x0027BDE7 File Offset: 0x00279FE7
		private Task Done()
		{
			base.SetEventFinished(base.L10NLookup("THE_FUTURE_OF_POTIONS.pages.DONE.description"));
			return Task.CompletedTask;
		}

		// Token: 0x0600694E RID: 26958 RVA: 0x0027BE00 File Offset: 0x0027A000
		private CardRarity GetCardRarity(PotionModel potion)
		{
			CardRarity cardRarity;
			switch (potion.Rarity)
			{
			case PotionRarity.Common:
			case PotionRarity.Token:
				cardRarity = CardRarity.Common;
				break;
			case PotionRarity.Uncommon:
				cardRarity = CardRarity.Uncommon;
				break;
			case PotionRarity.Rare:
			case PotionRarity.Event:
				cardRarity = CardRarity.Rare;
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Potion ");
				defaultInterpolatedStringHandler.AppendFormatted(potion.Id.Entry);
				defaultInterpolatedStringHandler.AppendLiteral(" has invalid rarity ");
				defaultInterpolatedStringHandler.AppendFormatted<PotionRarity>(potion.Rarity);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return cardRarity;
		}

		// Token: 0x040027D9 RID: 10201
		private const string _choiceKey = "THE_FUTURE_OF_POTIONS.pages.INITIAL.options.POTION";

		// Token: 0x040027DA RID: 10202
		private const string _potionKey = "Potion";

		// Token: 0x040027DB RID: 10203
		private const string _rarityKey = "Rarity";

		// Token: 0x040027DC RID: 10204
		private const string _typeKey = "Type";

		// Token: 0x040027DD RID: 10205
		[Nullable(new byte[] { 2, 1 })]
		private Dictionary<PotionModel, CardType> _cardTypes;
	}
}
