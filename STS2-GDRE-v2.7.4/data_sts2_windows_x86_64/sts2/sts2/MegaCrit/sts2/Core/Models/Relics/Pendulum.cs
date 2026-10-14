using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
	// Token: 0x020005B2 RID: 1458
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Pendulum : RelicModel
	{
		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x0600539D RID: 21405 RVA: 0x00245167 File Offset: 0x00243367
		public override string FlashSfx
		{
			get
			{
				return "event:/sfx/ui/relic_activate_draw";
			}
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x0600539E RID: 21406 RVA: 0x0024516E File Offset: 0x0024336E
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x0600539F RID: 21407 RVA: 0x00245171 File Offset: 0x00243371
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x060053A0 RID: 21408 RVA: 0x00245174 File Offset: 0x00243374
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

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x060053A1 RID: 21409 RVA: 0x0024519A File Offset: 0x0024339A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(1),
					new DynamicVar("Turns", 3m)
				});
			}
		}

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x060053A2 RID: 21410 RVA: 0x002451C3 File Offset: 0x002433C3
		// (set) Token: 0x060053A3 RID: 21411 RVA: 0x002451CB File Offset: 0x002433CB
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

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x060053A4 RID: 21412 RVA: 0x002451E0 File Offset: 0x002433E0
		// (set) Token: 0x060053A5 RID: 21413 RVA: 0x002451E8 File Offset: 0x002433E8
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

		// Token: 0x060053A6 RID: 21414 RVA: 0x00245200 File Offset: 0x00243400
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				this.TurnsSeen = (this.TurnsSeen + 1) % base.DynamicVars["Turns"].IntValue;
				base.Status = ((this.TurnsSeen == base.DynamicVars["Turns"].IntValue - 1) ? RelicStatus.Active : RelicStatus.Normal);
				if (this.TurnsSeen == 0)
				{
					TaskHelper.RunSafely(this.DoActivateVisuals());
					await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
				}
			}
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x00245254 File Offset: 0x00243454
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x060053A8 RID: 21416 RVA: 0x00245297 File Offset: 0x00243497
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x04002496 RID: 9366
		private const string _turnsKey = "Turns";

		// Token: 0x04002497 RID: 9367
		private bool _isActivating;

		// Token: 0x04002498 RID: 9368
		private int _turnsSeen;
	}
}
