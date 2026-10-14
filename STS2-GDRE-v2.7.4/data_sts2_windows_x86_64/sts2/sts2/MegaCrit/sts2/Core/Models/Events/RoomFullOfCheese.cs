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
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200084C RID: 2124
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RoomFullOfCheese : EventModel
	{
		// Token: 0x060068A2 RID: 26786 RVA: 0x002789C7 File Offset: 0x00276BC7
		public override bool IsAllowed(IRunState runState)
		{
			return runState.CurrentActIndex < 2;
		}

		// Token: 0x170019BE RID: 6590
		// (get) Token: 0x060068A3 RID: 26787 RVA: 0x002789D2 File Offset: 0x00276BD2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(14m, ValueProp.Unblockable | ValueProp.Unpowered));
			}
		}

		// Token: 0x060068A4 RID: 26788 RVA: 0x002789E8 File Offset: 0x00276BE8
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Gorge), "ROOM_FULL_OF_CHEESE.pages.INITIAL.options.GORGE", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Search), "ROOM_FULL_OF_CHEESE.pages.INITIAL.options.SEARCH", HoverTipFactory.FromRelic<ChosenCheese>()).ThatDoesDamage(base.DynamicVars.Damage.BaseValue)
			});
		}

		// Token: 0x060068A5 RID: 26789 RVA: 0x00278A54 File Offset: 0x00276C54
		private async Task Gorge()
		{
			Player owner = base.Owner;
			CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithUniformOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(owner.Character.CardPool), (CardModel c) => c.Rarity == CardRarity.Common).WithFlags(CardCreationFlags.NoRarityModification);
			List<CardCreationResult> list = CardFactory.CreateForReward(owner, 8, cardCreationOptions).ToList<CardCreationResult>();
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.L10NLookup("ROOM_FULL_OF_CHEESE.pages.GORGE.selectionScreenPrompt"), 2);
			await base.SelectCardsToAddToDeckFromGrid(list, cardSelectorPrefs);
			base.SetEventFinished(base.L10NLookup("ROOM_FULL_OF_CHEESE.pages.GORGE.description"));
		}

		// Token: 0x060068A6 RID: 26790 RVA: 0x00278A98 File Offset: 0x00276C98
		private async Task Search()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Damage, null, null, null);
			await RelicCmd.Obtain<ChosenCheese>(base.Owner);
			base.SetEventFinished(base.L10NLookup("ROOM_FULL_OF_CHEESE.pages.SEARCH.description"));
		}
	}
}
