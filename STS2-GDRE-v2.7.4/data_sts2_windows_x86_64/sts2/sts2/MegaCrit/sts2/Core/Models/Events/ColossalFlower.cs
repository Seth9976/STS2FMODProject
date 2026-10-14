using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000830 RID: 2096
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ColossalFlower : EventModel
	{
		// Token: 0x1700196A RID: 6506
		// (get) Token: 0x06006796 RID: 26518 RVA: 0x002733A3 File Offset: 0x002715A3
		// (set) Token: 0x06006797 RID: 26519 RVA: 0x002733AB File Offset: 0x002715AB
		private int NumberOfDigs
		{
			get
			{
				return this._numberOfDigs;
			}
			set
			{
				base.AssertMutable();
				this._numberOfDigs = value;
			}
		}

		// Token: 0x06006798 RID: 26520 RVA: 0x002733BA File Offset: 0x002715BA
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Creature.CurrentHp >= 19);
		}

		// Token: 0x1700196B RID: 6507
		// (get) Token: 0x06006799 RID: 26521 RVA: 0x002733E8 File Offset: 0x002715E8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new GoldVar(ColossalFlower._prizeKeys[0], ColossalFlower._prizeCosts[0]),
					new GoldVar(ColossalFlower._prizeKeys[1], ColossalFlower._prizeCosts[1]),
					new GoldVar(ColossalFlower._prizeKeys[2], ColossalFlower._prizeCosts[2])
				});
			}
		}

		// Token: 0x0600679A RID: 26522 RVA: 0x00273444 File Offset: 0x00271644
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			EventOption[] array = new EventOption[2];
			int num = 0;
			Func<Task> func = new Func<Task>(this.ExtractCurrentPrize);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
			defaultInterpolatedStringHandler.AppendLiteral("COLOSSAL_FLOWER.pages.INITIAL.options.EXTRACT_CURRENT_PRIZE_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.NumberOfDigs + 1);
			array[num] = new EventOption(this, func, defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<IHoverTip>());
			int num2 = 1;
			Func<Task> func2 = new Func<Task>(this.ReachDeeper);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
			defaultInterpolatedStringHandler.AppendLiteral("COLOSSAL_FLOWER.pages.INITIAL.options.REACH_DEEPER_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.NumberOfDigs + 1);
			array[num2] = new EventOption(this, func2, defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<IHoverTip>()).ThatDoesDamage(ColossalFlower._prizeDamage[this.NumberOfDigs]);
			return new <>z__ReadOnlyArray<EventOption>(array);
		}

		// Token: 0x0600679B RID: 26523 RVA: 0x00273500 File Offset: 0x00271700
		private async Task ReachDeeper()
		{
			await this.DealReachDeeperDamage();
			this.NumberOfDigs++;
			if (this.NumberOfDigs < 2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("COLOSSAL_FLOWER.pages.REACH_DEEPER_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.NumberOfDigs);
				defaultInterpolatedStringHandler.AppendLiteral(".description");
				LocString locString = base.L10NLookup(defaultInterpolatedStringHandler.ToStringAndClear());
				EventOption[] array = new EventOption[2];
				int num = 0;
				Func<Task> func = new Func<Task>(this.ExtractCurrentPrize);
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
				defaultInterpolatedStringHandler.AppendLiteral("COLOSSAL_FLOWER.pages.REACH_DEEPER_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.NumberOfDigs);
				defaultInterpolatedStringHandler.AppendLiteral(".options.EXTRACT_CURRENT_PRIZE_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.NumberOfDigs + 1);
				array[num] = new EventOption(this, func, defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<IHoverTip>());
				int num2 = 1;
				Func<Task> func2 = new Func<Task>(this.ReachDeeper);
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 2);
				defaultInterpolatedStringHandler.AppendLiteral("COLOSSAL_FLOWER.pages.REACH_DEEPER_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.NumberOfDigs);
				defaultInterpolatedStringHandler.AppendLiteral(".options.REACH_DEEPER_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.NumberOfDigs + 1);
				array[num2] = new EventOption(this, func2, defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<IHoverTip>()).ThatDoesDamage(ColossalFlower._prizeDamage[this.NumberOfDigs]);
				this.SetEventState(locString, new <>z__ReadOnlyArray<EventOption>(array));
			}
			else
			{
				this.SetEventState(base.L10NLookup("COLOSSAL_FLOWER.pages.REACH_DEEPER_2.description"), new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					new EventOption(this, new Func<Task>(this.ExtractInstead), "COLOSSAL_FLOWER.pages.REACH_DEEPER_2.options.EXTRACT_INSTEAD", Array.Empty<IHoverTip>()),
					new EventOption(this, new Func<Task>(this.ObtainPollinousCore), "COLOSSAL_FLOWER.pages.REACH_DEEPER_2.options.POLLINOUS_CORE", HoverTipFactory.FromRelic<PollinousCore>()).ThatDoesDamage(ColossalFlower._prizeDamage[this.NumberOfDigs])
				}));
			}
		}

		// Token: 0x0600679C RID: 26524 RVA: 0x00273544 File Offset: 0x00271744
		private async Task ExtractCurrentPrize()
		{
			await PlayerCmd.GainGold(ColossalFlower._prizeCosts[this.NumberOfDigs], base.Owner, false);
			base.SetEventFinished(base.L10NLookup("COLOSSAL_FLOWER.pages.EXTRACT_CURRENT_PRIZE.description"));
		}

		/// <summary>
		/// The player opted for 135 Gold rather than Pollinous Core + 7 damage
		/// </summary>
		// Token: 0x0600679D RID: 26525 RVA: 0x00273588 File Offset: 0x00271788
		private async Task ExtractInstead()
		{
			await PlayerCmd.GainGold(ColossalFlower._prizeCosts[this.NumberOfDigs], base.Owner, false);
			base.SetEventFinished(base.L10NLookup("COLOSSAL_FLOWER.pages.EXTRACT_INSTEAD.description"));
		}

		/// <summary>
		/// The player takes damage and gets the relic.
		/// </summary>
		// Token: 0x0600679E RID: 26526 RVA: 0x002735CC File Offset: 0x002717CC
		private async Task ObtainPollinousCore()
		{
			await this.DealReachDeeperDamage();
			await RelicCmd.Obtain<PollinousCore>(base.Owner);
			base.SetEventFinished(base.L10NLookup("COLOSSAL_FLOWER.pages.POLLINOUS_CORE.description"));
		}

		// Token: 0x0600679F RID: 26527 RVA: 0x00273610 File Offset: 0x00271810
		private async Task DealReachDeeperDamage()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, ColossalFlower._prizeDamage[this.NumberOfDigs], ValueProp.Unblockable | ValueProp.Unpowered, null, null);
		}

		// Token: 0x0400275E RID: 10078
		private static readonly string[] _prizeKeys = new string[] { "Prize1", "Prize2", "Prize3" };

		// Token: 0x0400275F RID: 10079
		private static readonly int[] _prizeCosts = new int[] { 35, 75, 135 };

		// Token: 0x04002760 RID: 10080
		private static readonly int[] _prizeDamage = new int[] { 5, 6, 7 };

		// Token: 0x04002761 RID: 10081
		private int _numberOfDigs;
	}
}
