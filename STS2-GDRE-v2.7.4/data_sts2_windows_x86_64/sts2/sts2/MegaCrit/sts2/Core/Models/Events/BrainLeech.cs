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
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200082C RID: 2092
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BrainLeech : EventModel
	{
		// Token: 0x0600677E RID: 26494 RVA: 0x00272DA7 File Offset: 0x00270FA7
		public override bool IsAllowed(IRunState runState)
		{
			return runState.CurrentActIndex < 2;
		}

		// Token: 0x0600677F RID: 26495 RVA: 0x00272DB4 File Offset: 0x00270FB4
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.ShareKnowledge), "BRAIN_LEECH.pages.INITIAL.options.SHARE_KNOWLEDGE", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Rip), "BRAIN_LEECH.pages.INITIAL.options.RIP", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars["RipHpLoss"].BaseValue)
			});
		}

		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x06006780 RID: 26496 RVA: 0x00272E24 File Offset: 0x00271024
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar("RipHpLoss", 5m, ValueProp.Unblockable | ValueProp.Unpowered),
					new IntVar("RewardCount", 1m),
					new IntVar("CardChoiceCount", 1m),
					new IntVar("FromCardChoiceCount", 5m)
				});
			}
		}

		// Token: 0x06006781 RID: 26497 RVA: 0x00272E88 File Offset: 0x00271088
		private async Task Rip()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, (DamageVar)base.DynamicVars["RipHpLoss"], null, null, null);
			for (int i = 0; i < base.DynamicVars["RewardCount"].IntValue; i++)
			{
				CardReward cardReward = new CardReward(CardCreationOptions.ForNonCombatWithDefaultOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(ModelDb.CardPool<ColorlessCardPool>()), null).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications), 3, base.Owner, null);
				await RewardsCmd.OfferCustom(base.Owner, new List<Reward>(1) { cardReward });
			}
			base.SetEventFinished(base.L10NLookup("BRAIN_LEECH.pages.RIP.description"));
		}

		// Token: 0x06006782 RID: 26498 RVA: 0x00272ECC File Offset: 0x002710CC
		private async Task ShareKnowledge()
		{
			Player owner = base.Owner;
			List<CardCreationResult> list = CardFactory.CreateForReward(owner, base.DynamicVars["FromCardChoiceCount"].IntValue, CardCreationOptions.ForNonCombatWithDefaultOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(owner.Character.CardPool), null)).ToList<CardCreationResult>();
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.L10NLookup("BRAIN_LEECH.pages.SHARE_KNOWLEDGE.selectionScreenPrompt"), 1)
			{
				Cancelable = false
			};
			await base.SelectCardsToAddToDeckFromGrid(list, cardSelectorPrefs);
			base.SetEventFinished(base.L10NLookup("BRAIN_LEECH.pages.SHARE_KNOWLEDGE.description"));
		}

		// Token: 0x04002759 RID: 10073
		private const string _ripHpLossKey = "RipHpLoss";

		// Token: 0x0400275A RID: 10074
		private const string _rewardCountKey = "RewardCount";

		// Token: 0x0400275B RID: 10075
		private const string _cardChoiceCountKey = "CardChoiceCount";

		// Token: 0x0400275C RID: 10076
		private const string _fromCardChoiceCountKey = "FromCardChoiceCount";
	}
}
