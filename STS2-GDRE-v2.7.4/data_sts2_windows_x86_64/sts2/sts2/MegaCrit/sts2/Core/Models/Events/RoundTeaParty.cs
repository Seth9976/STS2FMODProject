using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
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
	// Token: 0x0200084D RID: 2125
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RoundTeaParty : EventModel
	{
		// Token: 0x060068A8 RID: 26792 RVA: 0x00278AE3 File Offset: 0x00276CE3
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Creature.CurrentHp >= 12);
		}

		// Token: 0x060068A9 RID: 26793 RVA: 0x00278B10 File Offset: 0x00276D10
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.EnjoyTea), "ROUND_TEA_PARTY.pages.INITIAL.options.ENJOY_TEA", HoverTipFactory.FromRelic<RoyalPoison>()),
				new EventOption(this, new Func<Task>(this.PickFight), "ROUND_TEA_PARTY.pages.INITIAL.options.PICK_FIGHT", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars.Damage.BaseValue)
			});
		}

		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x060068AA RID: 26794 RVA: 0x00278B7B File Offset: 0x00276D7B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(11m, ValueProp.Unblockable | ValueProp.Unpowered),
					new StringVar("Relic", ModelDb.Relic<RoyalPoison>().Title.GetFormattedText())
				});
			}
		}

		// Token: 0x060068AB RID: 26795 RVA: 0x00278BB4 File Offset: 0x00276DB4
		private async Task EnjoyTea()
		{
			Creature targetCreature = base.Owner.Creature;
			await RelicCmd.Obtain<RoyalPoison>(base.Owner);
			await CreatureCmd.Heal(targetCreature, targetCreature.MaxHp - targetCreature.CurrentHp, true);
			base.SetEventFinished(base.L10NLookup("ROUND_TEA_PARTY.pages.ENJOY_TEA.description"));
		}

		// Token: 0x060068AC RID: 26796 RVA: 0x00278BF7 File Offset: 0x00276DF7
		private Task PickFight()
		{
			this.SetEventState(base.L10NLookup("ROUND_TEA_PARTY.pages.PICK_FIGHT.description"), new <>z__ReadOnlySingleElementList<EventOption>(new EventOption(this, new Func<Task>(this.ContinueFight), "ROUND_TEA_PARTY.pages.PICK_FIGHT.options.CONTINUE_FIGHT", Array.Empty<IHoverTip>()).ThatWontSaveToChoiceHistory()));
			return Task.CompletedTask;
		}

		// Token: 0x060068AD RID: 26797 RVA: 0x00278C38 File Offset: 0x00276E38
		private async Task ContinueFight()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Damage, null, null, null);
			await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable(), base.Owner, -1);
			base.SetEventFinished(base.L10NLookup("ROUND_TEA_PARTY.pages.CONTINUE_FIGHT.description"));
		}
	}
}
