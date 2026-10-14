using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000839 RID: 2105
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EndlessConveyor : EventModel
	{
		// Token: 0x060067DC RID: 26588 RVA: 0x00274887 File Offset: 0x00272A87
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Gold >= 120);
		}

		// Token: 0x17001978 RID: 6520
		// (get) Token: 0x060067DD RID: 26589 RVA: 0x002748B3 File Offset: 0x00272AB3
		// (set) Token: 0x060067DE RID: 26590 RVA: 0x002748BB File Offset: 0x00272ABB
		private int NumOfGrabs
		{
			get
			{
				return this._numOfGrabs;
			}
			set
			{
				base.AssertMutable();
				this._numOfGrabs = value;
			}
		}

		// Token: 0x17001979 RID: 6521
		// (get) Token: 0x060067DF RID: 26591 RVA: 0x002748CA File Offset: 0x00272ACA
		// (set) Token: 0x060067E0 RID: 26592 RVA: 0x002748D2 File Offset: 0x00272AD2
		private EndlessConveyor.Dish CurrentDish
		{
			get
			{
				return this._currentDish;
			}
			set
			{
				base.AssertMutable();
				this._currentDish = value;
			}
		}

		// Token: 0x1700197A RID: 6522
		// (get) Token: 0x060067E1 RID: 26593 RVA: 0x002748E4 File Offset: 0x00272AE4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new GoldVar(40),
					new GoldVar("GoldenFyshGold", 75),
					new HealVar("ClamRollHeal", 10m),
					new MaxHpVar("CaviarMaxHp", 4m),
					new StringVar("CurrentDishTitle", ""),
					new StringVar("LastDishTitle", "")
				});
			}
		}

		// Token: 0x060067E2 RID: 26594 RVA: 0x00274960 File Offset: 0x00272B60
		public override void CalculateVars()
		{
			this.RollDish();
			((StringVar)base.DynamicVars["CurrentDishTitle"]).StringValue = this.CurrentDish.title.GetFormattedText();
		}

		// Token: 0x060067E3 RID: 26595 RVA: 0x00274992 File Offset: 0x00272B92
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				this.GenerateGrabSomethingOffTheBeltOption(),
				new EventOption(this, new Func<Task>(this.ObserveChef), "ENDLESS_CONVEYOR.pages.INITIAL.options.OBSERVE_CHEF", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x060067E4 RID: 26596 RVA: 0x002749C8 File Offset: 0x00272BC8
		private async Task GrabSomethingOffTheBelt()
		{
			if (this._currentDish.id != "GOLDEN_FYSH")
			{
				await PlayerCmd.LoseGold(base.DynamicVars.Gold.IntValue, base.Owner, GoldLossType.Spent);
			}
			await this.CurrentDish.action();
			this.RollDish();
			((StringVar)base.DynamicVars["LastDishTitle"]).StringValue = ((StringVar)base.DynamicVars["CurrentDishTitle"]).StringValue;
			((StringVar)base.DynamicVars["CurrentDishTitle"]).StringValue = this.CurrentDish.title.GetFormattedText();
			this.SetEventState(base.L10NLookup("ENDLESS_CONVEYOR.pages.GRAB_SOMETHING_OFF_THE_BELT.description"), new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				this.GenerateGrabSomethingOffTheBeltOption(),
				new EventOption(this, new Func<Task>(this.Leave), "ENDLESS_CONVEYOR.pages.GRAB_SOMETHING_OFF_THE_BELT.options.LEAVE", Array.Empty<IHoverTip>())
			}));
		}

		// Token: 0x060067E5 RID: 26597 RVA: 0x00274A0C File Offset: 0x00272C0C
		private EventOption GenerateGrabSomethingOffTheBeltOption()
		{
			if (base.Owner.Gold >= base.DynamicVars.Gold.IntValue)
			{
				return new EventOption(this, new Func<Task>(this.GrabSomethingOffTheBelt), this._currentDish.optionKey, this._currentDish.hoverTips);
			}
			return new EventOption(this, null, "ENDLESS_CONVEYOR.pages.ALL.options.LOCKED", Array.Empty<IHoverTip>());
		}

		// Token: 0x060067E6 RID: 26598 RVA: 0x00274A70 File Offset: 0x00272C70
		private async Task ClamRoll()
		{
			await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars["ClamRollHeal"].IntValue, true);
		}

		// Token: 0x060067E7 RID: 26599 RVA: 0x00274AB4 File Offset: 0x00272CB4
		private async Task Caviar()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars["CaviarMaxHp"].IntValue);
		}

		// Token: 0x060067E8 RID: 26600 RVA: 0x00274AF8 File Offset: 0x00272CF8
		private async Task SuspiciousCondiment()
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
		}

		// Token: 0x060067E9 RID: 26601 RVA: 0x00274B3C File Offset: 0x00272D3C
		private async Task JellyLiver()
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForTransformation(base.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1), null);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				await CardCmd.TransformToRandom(cardModel, base.Rng, CardPreviewStyle.EventLayout);
			}
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x00274B80 File Offset: 0x00272D80
		private async Task SeapunkSalad()
		{
			CardModel cardModel = base.Owner.RunState.CreateCard<FeedingFrenzy>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.EventLayout);
		}

		// Token: 0x060067EB RID: 26603 RVA: 0x00274BC4 File Offset: 0x00272DC4
		private async Task FriedEel()
		{
			CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithDefaultOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(ModelDb.CardPool<ColorlessCardPool>()), null);
			CardModel card = CardFactory.CreateForReward(base.Owner, 1, cardCreationOptions).First<CardCreationResult>().Card;
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.EventLayout);
		}

		// Token: 0x060067EC RID: 26604 RVA: 0x00274C08 File Offset: 0x00272E08
		private async Task GoldenFysh()
		{
			await PlayerCmd.GainGold(base.DynamicVars["GoldenFyshGold"].BaseValue, base.Owner, false);
		}

		// Token: 0x060067ED RID: 26605 RVA: 0x00274C4C File Offset: 0x00272E4C
		private Task SpicySnappy()
		{
			List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.IsUpgradable).ToList<CardModel>();
			if (list.Count != 0)
			{
				CardModel cardModel = base.Rng.NextItem<CardModel>(list);
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			return Task.CompletedTask;
		}

		/// <summary>
		/// Generates a list of possible dishes and uses a weighted roll to determine a dish.
		/// Lots of exceptions and info here.
		/// </summary>
		// Token: 0x060067EE RID: 26606 RVA: 0x00274CB8 File Offset: 0x00272EB8
		private unsafe void RollDish()
		{
			int num = this.NumOfGrabs;
			this.NumOfGrabs = num + 1;
			if (this.NumOfGrabs % 5 == 0)
			{
				this._lastDishId = "SEAPUNK_SALAD";
				this._currentDish = new EndlessConveyor.Dish("SEAPUNK_SALAD", new Func<Task>(this.SeapunkSalad), HoverTipFactory.FromCardWithCardHoverTips<FeedingFrenzy>(false), 0f);
				return;
			}
			num = 4;
			List<EndlessConveyor.Dish> list = new List<EndlessConveyor.Dish>(num);
			CollectionsMarshal.SetCount<EndlessConveyor.Dish>(list, num);
			Span<EndlessConveyor.Dish> span = CollectionsMarshal.AsSpan<EndlessConveyor.Dish>(list);
			int num2 = 0;
			*span[num2] = new EndlessConveyor.Dish("CAVIAR", new Func<Task>(this.Caviar), Array.Empty<IHoverTip>(), 6f);
			num2++;
			*span[num2] = new EndlessConveyor.Dish("SPICY_SNAPPY", new Func<Task>(this.SpicySnappy), Array.Empty<IHoverTip>(), 3f);
			num2++;
			*span[num2] = new EndlessConveyor.Dish("JELLY_LIVER", new Func<Task>(this.JellyLiver), new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform, Array.Empty<DynamicVar>())), 3f);
			num2++;
			*span[num2] = new EndlessConveyor.Dish("FRIED_EEL", new Func<Task>(this.FriedEel), Array.Empty<IHoverTip>(), 3f);
			List<EndlessConveyor.Dish> list2 = list;
			if (base.Owner.HasOpenPotionSlots)
			{
				list2.Add(new EndlessConveyor.Dish("SUSPICIOUS_CONDIMENT", new Func<Task>(this.SuspiciousCondiment), Array.Empty<IHoverTip>(), 3f));
			}
			if (base.Owner.Creature.CurrentHp != base.Owner.Creature.MaxHp)
			{
				list2.Add(new EndlessConveyor.Dish("CLAM_ROLL", new Func<Task>(this.ClamRoll), Array.Empty<IHoverTip>(), 6f));
			}
			if (this.NumOfGrabs > 1)
			{
				list2.Add(new EndlessConveyor.Dish("GOLDEN_FYSH", new Func<Task>(this.GoldenFysh), Array.Empty<IHoverTip>(), 1f));
			}
			list2.RemoveAll((EndlessConveyor.Dish d) => d.id == this._lastDishId);
			float num3 = 0f;
			foreach (EndlessConveyor.Dish dish in list2)
			{
				num3 += dish.weight;
			}
			float num4 = base.Rng.NextFloat(1f) * num3;
			float num5 = 0f;
			foreach (EndlessConveyor.Dish dish2 in list2)
			{
				num5 += dish2.weight;
				if (num4 < num5)
				{
					this._lastDishId = dish2.id;
					this._currentDish = dish2;
					break;
				}
			}
		}

		/// <summary>
		/// Player chose not to partake in the feast. Sad.
		/// </summary>
		// Token: 0x060067EF RID: 26607 RVA: 0x00274F88 File Offset: 0x00273188
		private Task ObserveChef()
		{
			IEnumerable<CardModel> enumerable = base.Owner.Deck.Cards.Where<CardModel>((CardModel c) => c.IsUpgradable);
			IEnumerable<CardModel> enumerable2 = (enumerable as CardModel[]) ?? enumerable.ToArray<CardModel>();
			if (enumerable2.Any<CardModel>())
			{
				CardCmd.Upgrade(base.Rng.NextItem<CardModel>(enumerable2), CardPreviewStyle.HorizontalLayout);
			}
			base.SetEventFinished(base.L10NLookup("ENDLESS_CONVEYOR.pages.OBSERVE_CHEF.description"));
			return Task.CompletedTask;
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x0027500B File Offset: 0x0027320B
		private Task Leave()
		{
			base.SetEventFinished(base.L10NLookup("ENDLESS_CONVEYOR.pages.LEAVE.description"));
			return Task.CompletedTask;
		}

		// Token: 0x04002777 RID: 10103
		private const string _currentDishTitleKey = "CurrentDishTitle";

		// Token: 0x04002778 RID: 10104
		private const string _lastDishTitleKey = "LastDishTitle";

		// Token: 0x04002779 RID: 10105
		private const string _goldenFyshGoldKey = "GoldenFyshGold";

		// Token: 0x0400277A RID: 10106
		private const string _clamRollHealKey = "ClamRollHeal";

		// Token: 0x0400277B RID: 10107
		private const string _caviarMaxHpKey = "CaviarMaxHp";

		// Token: 0x0400277C RID: 10108
		private string _lastDishId = "";

		// Token: 0x0400277D RID: 10109
		private int _numOfGrabs;

		// Token: 0x0400277E RID: 10110
		private EndlessConveyor.Dish _currentDish;

		// Token: 0x02001F0B RID: 7947
		[Nullable(0)]
		private struct Dish : IEquatable<EndlessConveyor.Dish>
		{
			// Token: 0x0600B6E0 RID: 46816 RVA: 0x003C4E70 File Offset: 0x003C3070
			public Dish(string id, Func<Task> action, IEnumerable<IHoverTip> hoverTips, float weight)
			{
				this.id = id;
				this.title = new LocString("events", "ENDLESS_CONVEYOR.DISHES." + id + ".title");
				this.optionKey = "ENDLESS_CONVEYOR.pages.ALL.options." + id;
				this.action = action;
				this.hoverTips = hoverTips;
				this.weight = weight;
			}

			// Token: 0x0600B6E1 RID: 46817 RVA: 0x003C4ECC File Offset: 0x003C30CC
			[NullableContext(0)]
			[CompilerGenerated]
			public override readonly string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Dish");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600B6E2 RID: 46818 RVA: 0x003C4F18 File Offset: 0x003C3118
			[NullableContext(0)]
			[CompilerGenerated]
			private readonly bool PrintMembers(StringBuilder builder)
			{
				builder.Append("id = ");
				builder.Append(this.id);
				builder.Append(", title = ");
				builder.Append(this.title);
				builder.Append(", optionKey = ");
				builder.Append(this.optionKey);
				builder.Append(", hoverTips = ");
				builder.Append(this.hoverTips);
				builder.Append(", weight = ");
				builder.Append(this.weight.ToString());
				builder.Append(", action = ");
				builder.Append(this.action);
				return true;
			}

			// Token: 0x0600B6E3 RID: 46819 RVA: 0x003C4FCA File Offset: 0x003C31CA
			[CompilerGenerated]
			public static bool operator !=(EndlessConveyor.Dish left, EndlessConveyor.Dish right)
			{
				return !(left == right);
			}

			// Token: 0x0600B6E4 RID: 46820 RVA: 0x003C4FD6 File Offset: 0x003C31D6
			[CompilerGenerated]
			public static bool operator ==(EndlessConveyor.Dish left, EndlessConveyor.Dish right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600B6E5 RID: 46821 RVA: 0x003C4FE0 File Offset: 0x003C31E0
			[CompilerGenerated]
			public override readonly int GetHashCode()
			{
				return ((((EqualityComparer<string>.Default.GetHashCode(this.id) * -1521134295 + EqualityComparer<LocString>.Default.GetHashCode(this.title)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.optionKey)) * -1521134295 + EqualityComparer<IEnumerable<IHoverTip>>.Default.GetHashCode(this.hoverTips)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(this.weight)) * -1521134295 + EqualityComparer<Func<Task>>.Default.GetHashCode(this.action);
			}

			// Token: 0x0600B6E6 RID: 46822 RVA: 0x003C5070 File Offset: 0x003C3270
			[NullableContext(0)]
			[CompilerGenerated]
			public override readonly bool Equals(object obj)
			{
				return obj is EndlessConveyor.Dish && this.Equals((EndlessConveyor.Dish)obj);
			}

			// Token: 0x0600B6E7 RID: 46823 RVA: 0x003C5088 File Offset: 0x003C3288
			[CompilerGenerated]
			public readonly bool Equals(EndlessConveyor.Dish other)
			{
				return EqualityComparer<string>.Default.Equals(this.id, other.id) && EqualityComparer<LocString>.Default.Equals(this.title, other.title) && EqualityComparer<string>.Default.Equals(this.optionKey, other.optionKey) && EqualityComparer<IEnumerable<IHoverTip>>.Default.Equals(this.hoverTips, other.hoverTips) && EqualityComparer<float>.Default.Equals(this.weight, other.weight) && EqualityComparer<Func<Task>>.Default.Equals(this.action, other.action);
			}

			// Token: 0x04007EB8 RID: 32440
			public readonly string id;

			// Token: 0x04007EB9 RID: 32441
			public readonly LocString title;

			// Token: 0x04007EBA RID: 32442
			public readonly string optionKey;

			// Token: 0x04007EBB RID: 32443
			public readonly IEnumerable<IHoverTip> hoverTips;

			// Token: 0x04007EBC RID: 32444
			public readonly float weight;

			// Token: 0x04007EBD RID: 32445
			public readonly Func<Task> action;
		}
	}
}
