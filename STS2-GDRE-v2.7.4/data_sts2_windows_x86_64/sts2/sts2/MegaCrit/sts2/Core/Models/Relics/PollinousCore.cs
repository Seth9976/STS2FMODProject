using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005BB RID: 1467
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PollinousCore : RelicModel
	{
		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x060053E9 RID: 21481 RVA: 0x00245AB1 File Offset: 0x00243CB1
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x060053EA RID: 21482 RVA: 0x00245AB4 File Offset: 0x00243CB4
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x060053EB RID: 21483 RVA: 0x00245AB7 File Offset: 0x00243CB7
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

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x060053EC RID: 21484 RVA: 0x00245ADD File Offset: 0x00243CDD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(2),
					new DynamicVar("Turns", 4m)
				});
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x060053ED RID: 21485 RVA: 0x00245B06 File Offset: 0x00243D06
		// (set) Token: 0x060053EE RID: 21486 RVA: 0x00245B0E File Offset: 0x00243D0E
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
				this.UpdateDisplay();
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x060053EF RID: 21487 RVA: 0x00245B23 File Offset: 0x00243D23
		// (set) Token: 0x060053F0 RID: 21488 RVA: 0x00245B2B File Offset: 0x00243D2B
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
				this.UpdateDisplay();
			}
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x00245B40 File Offset: 0x00243D40
		private void UpdateDisplay()
		{
			if (this.IsActivating)
			{
				base.Status = RelicStatus.Normal;
			}
			else
			{
				int intValue = base.DynamicVars["Turns"].IntValue;
				base.Status = ((this.TurnsSeen == intValue - 1) ? RelicStatus.Active : RelicStatus.Normal);
			}
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x00245B90 File Offset: 0x00243D90
		public override Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player != base.Owner)
			{
				return Task.CompletedTask;
			}
			int turnsSeen = this.TurnsSeen;
			this.TurnsSeen = turnsSeen + 1;
			return Task.CompletedTask;
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x00245BC1 File Offset: 0x00243DC1
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x00245BD0 File Offset: 0x00243DD0
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner)
			{
				return count;
			}
			if (this.TurnsSeen < base.DynamicVars["Turns"].IntValue)
			{
				return count;
			}
			return count + base.DynamicVars.Cards.BaseValue;
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x00245C1D File Offset: 0x00243E1D
		public override Task AfterModifyingHandDraw()
		{
			this.TurnsSeen = 0;
			TaskHelper.RunSafely(this.DoActivateVisuals());
			return Task.CompletedTask;
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x00245C38 File Offset: 0x00243E38
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x040024A5 RID: 9381
		private const string _turnsKey = "Turns";

		// Token: 0x040024A6 RID: 9382
		private bool _isActivating;

		// Token: 0x040024A7 RID: 9383
		private int _turnsSeen;
	}
}
