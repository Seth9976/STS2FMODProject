using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000540 RID: 1344
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeHappyFlower : RelicModel
	{
		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x0600508D RID: 20621 RVA: 0x0023F523 File Offset: 0x0023D723
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x0600508E RID: 20622 RVA: 0x0023F526 File Offset: 0x0023D726
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x0600508F RID: 20623 RVA: 0x0023F529 File Offset: 0x0023D729
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.TurnsSeen;
				}
				return base.DynamicVars["Turns"].IntValue;
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06005090 RID: 20624 RVA: 0x0023F54F File Offset: 0x0023D74F
		public override int MerchantCost
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06005091 RID: 20625 RVA: 0x0023F553 File Offset: 0x0023D753
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new DynamicVar("Turns", 5m)
				});
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06005092 RID: 20626 RVA: 0x0023F57C File Offset: 0x0023D77C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06005093 RID: 20627 RVA: 0x0023F589 File Offset: 0x0023D789
		// (set) Token: 0x06005094 RID: 20628 RVA: 0x0023F591 File Offset: 0x0023D791
		private bool IsActivating
		{
			get
			{
				return this._isActivating;
			}
			set
			{
				base.AssertMutable();
				this._isActivating = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06005095 RID: 20629 RVA: 0x0023F5A6 File Offset: 0x0023D7A6
		// (set) Token: 0x06005096 RID: 20630 RVA: 0x0023F5AE File Offset: 0x0023D7AE
		[SavedProperty]
		public int TurnsSeen
		{
			get
			{
				return this._turnsSeen;
			}
			set
			{
				base.AssertMutable();
				this._turnsSeen = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x0023F5C4 File Offset: 0x0023D7C4
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				this.TurnsSeen = (this.TurnsSeen + 1) % base.DynamicVars["Turns"].IntValue;
				base.Status = ((this.TurnsSeen == base.DynamicVars["Turns"].IntValue - 1) ? RelicStatus.Active : RelicStatus.Normal);
				if (this.TurnsSeen == 0)
				{
					TaskHelper.RunSafely(this.DoActivateVisuals());
					await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
				}
			}
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x0023F610 File Offset: 0x0023D810
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x0023F653 File Offset: 0x0023D853
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x04002442 RID: 9282
		private const string _turnsKey = "Turns";

		// Token: 0x04002443 RID: 9283
		private bool _isActivating;

		// Token: 0x04002444 RID: 9284
		private int _turnsSeen;
	}
}
