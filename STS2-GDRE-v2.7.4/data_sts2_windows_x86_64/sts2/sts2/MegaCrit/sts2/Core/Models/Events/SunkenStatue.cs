using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000854 RID: 2132
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SunkenStatue : EventModel
	{
		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x060068E7 RID: 26855 RVA: 0x00279A40 File Offset: 0x00277C40
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("Relic", ModelDb.Relic<SwordOfStone>().Title.GetFormattedText()),
					new GoldVar(111),
					new DynamicVar("HpLoss", 7m)
				});
			}
		}

		// Token: 0x060068E8 RID: 26856 RVA: 0x00279A91 File Offset: 0x00277C91
		public override void CalculateVars()
		{
			base.DynamicVars.Gold.BaseValue += base.Rng.NextInt(-10, 11);
		}

		// Token: 0x060068E9 RID: 26857 RVA: 0x00279AC4 File Offset: 0x00277CC4
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.GrabSword), "SUNKEN_STATUE.pages.INITIAL.options.GRAB_SWORD", HoverTipFactory.FromRelic<SwordOfStone>()),
				new EventOption(this, new Func<Task>(this.DiveIntoWater), "SUNKEN_STATUE.pages.INITIAL.options.DIVE_INTO_WATER", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars["HpLoss"].BaseValue)
			});
		}

		// Token: 0x060068EA RID: 26858 RVA: 0x00279B34 File Offset: 0x00277D34
		private async Task GrabSword()
		{
			await RelicCmd.Obtain<SwordOfStone>(base.Owner);
			base.SetEventFinished(base.L10NLookup("SUNKEN_STATUE.pages.GRAB_SWORD.description"));
		}

		// Token: 0x060068EB RID: 26859 RVA: 0x00279B78 File Offset: 0x00277D78
		private async Task DiveIntoWater()
		{
			await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["HpLoss"].BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null, null);
			base.SetEventFinished(base.L10NLookup("SUNKEN_STATUE.pages.DIVE_INTO_WATER.description"));
		}

		// Token: 0x040027BE RID: 10174
		private const string _relicKey = "Relic";

		// Token: 0x040027BF RID: 10175
		private const string _hpLossKey = "HpLoss";

		// Token: 0x040027C0 RID: 10176
		private const int _goldVariance = 10;
	}
}
