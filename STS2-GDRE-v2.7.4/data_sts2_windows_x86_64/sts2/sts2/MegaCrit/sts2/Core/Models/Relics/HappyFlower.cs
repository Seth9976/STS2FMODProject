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
	// Token: 0x02000560 RID: 1376
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HappyFlower : RelicModel
	{
		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06005162 RID: 20834 RVA: 0x00240DCB File Offset: 0x0023EFCB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06005163 RID: 20835 RVA: 0x00240DCE File Offset: 0x0023EFCE
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06005164 RID: 20836 RVA: 0x00240DD1 File Offset: 0x0023EFD1
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

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06005165 RID: 20837 RVA: 0x00240DF7 File Offset: 0x0023EFF7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new DynamicVar("Turns", 3m)
				});
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06005166 RID: 20838 RVA: 0x00240E20 File Offset: 0x0023F020
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06005167 RID: 20839 RVA: 0x00240E2D File Offset: 0x0023F02D
		// (set) Token: 0x06005168 RID: 20840 RVA: 0x00240E35 File Offset: 0x0023F035
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

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06005169 RID: 20841 RVA: 0x00240E4A File Offset: 0x0023F04A
		// (set) Token: 0x0600516A RID: 20842 RVA: 0x00240E52 File Offset: 0x0023F052
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

		// Token: 0x0600516B RID: 20843 RVA: 0x00240E68 File Offset: 0x0023F068
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

		// Token: 0x0600516C RID: 20844 RVA: 0x00240EB4 File Offset: 0x0023F0B4
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x00240EF7 File Offset: 0x0023F0F7
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x04002458 RID: 9304
		public const int turnsThreshold = 3;

		// Token: 0x04002459 RID: 9305
		private const string _turnsKey = "Turns";

		// Token: 0x0400245A RID: 9306
		private bool _isActivating;

		// Token: 0x0400245B RID: 9307
		private int _turnsSeen;
	}
}
