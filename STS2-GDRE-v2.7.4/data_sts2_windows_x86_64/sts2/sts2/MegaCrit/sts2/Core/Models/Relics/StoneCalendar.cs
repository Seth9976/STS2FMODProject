using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005EB RID: 1515
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StoneCalendar : RelicModel
	{
		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x0600551B RID: 21787 RVA: 0x00247F34 File Offset: 0x00246134
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x0600551C RID: 21788 RVA: 0x00247F37 File Offset: 0x00246137
		public override bool ShowCounter
		{
			get
			{
				return this.DisplayAmount > -1;
			}
		}

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x0600551D RID: 21789 RVA: 0x00247F44 File Offset: 0x00246144
		public override int DisplayAmount
		{
			get
			{
				if (!CombatManager.Instance.IsInProgress)
				{
					return -1;
				}
				if (base.IsCanonical)
				{
					return -1;
				}
				int intValue = base.DynamicVars["DamageTurn"].IntValue;
				if (this.IsActivating)
				{
					return intValue;
				}
				int turnNumber = base.Owner.PlayerCombatState.TurnNumber;
				if (turnNumber >= intValue)
				{
					return -1;
				}
				return turnNumber;
			}
		}

		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x0600551E RID: 21790 RVA: 0x00247FA1 File Offset: 0x002461A1
		// (set) Token: 0x0600551F RID: 21791 RVA: 0x00247FA9 File Offset: 0x002461A9
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

		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x06005520 RID: 21792 RVA: 0x00247FBE File Offset: 0x002461BE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(52m, ValueProp.Unpowered),
					new DynamicVar("DamageTurn", 7m)
				});
			}
		}

		// Token: 0x06005521 RID: 21793 RVA: 0x00247FF0 File Offset: 0x002461F0
		public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			if (base.Owner.PlayerCombatState.TurnNumber == base.DynamicVars["DamageTurn"].IntValue)
			{
				base.Status = RelicStatus.Active;
			}
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005522 RID: 21794 RVA: 0x00248050 File Offset: 0x00246250
		public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				int intValue = base.DynamicVars["DamageTurn"].IntValue;
				int turnNumber = base.Owner.PlayerCombatState.TurnNumber;
				base.Status = RelicStatus.Normal;
				if (turnNumber == intValue)
				{
					TaskHelper.RunSafely(this.DoActivateVisuals());
					await CreatureCmd.Damage(choiceContext, base.Owner.Creature.CombatState.HittableEnemies, base.DynamicVars.Damage, base.Owner.Creature);
					base.InvokeDisplayAmountChanged();
				}
			}
		}

		// Token: 0x06005523 RID: 21795 RVA: 0x002480A3 File Offset: 0x002462A3
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005524 RID: 21796 RVA: 0x002480B7 File Offset: 0x002462B7
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (!(room is CombatRoom))
			{
				return Task.CompletedTask;
			}
			base.Status = RelicStatus.Normal;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005525 RID: 21797 RVA: 0x002480DC File Offset: 0x002462DC
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x040024C1 RID: 9409
		private const string _damageTurnKey = "DamageTurn";

		// Token: 0x040024C2 RID: 9410
		private bool _isActivating;
	}
}
