using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000853 RID: 2131
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StoneOfAllTime : EventModel
	{
		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x060068DD RID: 26845 RVA: 0x00279793 File Offset: 0x00277993
		// (set) Token: 0x060068DE RID: 26846 RVA: 0x0027979B File Offset: 0x0027799B
		[Nullable(2)]
		private PotionModel DrinkAndLiftPotion
		{
			[NullableContext(2)]
			get
			{
				return this._drinkAndLiftPotion;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._drinkAndLiftPotion = value;
			}
		}

		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x060068DF RID: 26847 RVA: 0x002797AC File Offset: 0x002779AC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("DrinkRandomPotion", ""),
					new DynamicVar("DrinkMaxHpGain", 10m),
					new DynamicVar("PushHpLoss", 6m),
					new DynamicVar("PushVigorousAmount", 8m)
				});
			}
		}

		/// <summary>
		/// Only appears in Act 2 and requires all players to have at least 1 potion
		/// </summary>
		// Token: 0x060068E0 RID: 26848 RVA: 0x00279810 File Offset: 0x00277A10
		public override bool IsAllowed(IRunState runState)
		{
			if (runState.CurrentActIndex == 1)
			{
				return runState.Players.All<Player>((Player player) => player.Potions.Any<PotionModel>());
			}
			return false;
		}

		// Token: 0x060068E1 RID: 26849 RVA: 0x00279847 File Offset: 0x00277A47
		protected override Task BeforeEventStarted(bool isPreFinished)
		{
			base.Owner.CanUseOrRemovePotions = false;
			return Task.CompletedTask;
		}

		// Token: 0x060068E2 RID: 26850 RVA: 0x0027985A File Offset: 0x00277A5A
		protected override void OnEventFinished()
		{
			base.Owner.CanUseOrRemovePotions = true;
		}

		// Token: 0x060068E3 RID: 26851 RVA: 0x00279868 File Offset: 0x00277A68
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			this.DrinkAndLiftPotion = base.Rng.NextItem<PotionModel>(base.Owner.Potions);
			EventOption eventOption;
			if (this.DrinkAndLiftPotion != null)
			{
				StringVar stringVar = (StringVar)base.DynamicVars["DrinkRandomPotion"];
				stringVar.StringValue = this.DrinkAndLiftPotion.Title.GetFormattedText();
				eventOption = new EventOption(this, new Func<Task>(this.Lift), "STONE_OF_ALL_TIME.pages.INITIAL.options.LIFT", new IHoverTip[] { HoverTipFactory.FromPotion(this.DrinkAndLiftPotion) });
			}
			else
			{
				eventOption = new EventOption(this, null, "STONE_OF_ALL_TIME.pages.INITIAL.options.LIFT_LOCKED", Array.Empty<IHoverTip>());
			}
			EventOption eventOption2;
			if (CardPile.Get(PileType.Deck, base.Owner).Cards.Count<CardModel>((CardModel c) => ModelDb.Enchantment<Vigorous>().CanEnchant(c)) >= 1)
			{
				eventOption2 = new EventOption(this, new Func<Task>(this.Push), "STONE_OF_ALL_TIME.pages.INITIAL.options.PUSH", HoverTipFactory.FromEnchantment<Vigorous>(base.DynamicVars["PushVigorousAmount"].IntValue)).ThatDoesDamage(base.DynamicVars["PushHpLoss"].BaseValue);
			}
			else
			{
				eventOption2 = new EventOption(this, null, "STONE_OF_ALL_TIME.pages.INITIAL.options.PUSH_LOCKED", Array.Empty<IHoverTip>());
			}
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[] { eventOption, eventOption2 });
		}

		// Token: 0x060068E4 RID: 26852 RVA: 0x002799B0 File Offset: 0x00277BB0
		private async Task Lift()
		{
			await PotionCmd.Discard(this.DrinkAndLiftPotion);
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars["DrinkMaxHpGain"].BaseValue);
			base.Rng.NextInt(100);
			LocString locString = base.L10NLookup("STONE_OF_ALL_TIME.pages.LIFT.description");
			locString.Add(base.DynamicVars["DrinkRandomPotion"]);
			base.SetEventFinished(locString);
		}

		// Token: 0x060068E5 RID: 26853 RVA: 0x002799F4 File Offset: 0x00277BF4
		private async Task Push()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["PushHpLoss"].BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
			Vigorous vigorous = ModelDb.Enchantment<Vigorous>();
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, vigorous, base.DynamicVars["PushVigorousAmount"].IntValue, cardSelectorPrefs);
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.Enchant(vigorous.ToMutable(), cardModel, base.DynamicVars["PushVigorousAmount"].BaseValue);
				CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.HorizontalLayout);
			}
			base.Rng.NextInt(100);
			base.SetEventFinished(base.L10NLookup("STONE_OF_ALL_TIME.pages.PUSH.description"));
		}

		// Token: 0x040027B9 RID: 10169
		private const string _drinkRandomPotionKey = "DrinkRandomPotion";

		// Token: 0x040027BA RID: 10170
		private const string _drinkMaxHpGain = "DrinkMaxHpGain";

		// Token: 0x040027BB RID: 10171
		private const string _pushHpLoss = "PushHpLoss";

		// Token: 0x040027BC RID: 10172
		private const string _pushVigorousAmountKey = "PushVigorousAmount";

		// Token: 0x040027BD RID: 10173
		[Nullable(2)]
		private PotionModel _drinkAndLiftPotion;
	}
}
