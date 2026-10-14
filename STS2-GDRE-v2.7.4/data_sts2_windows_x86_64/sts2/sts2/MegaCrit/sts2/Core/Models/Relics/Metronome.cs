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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200058A RID: 1418
	[NullableContext(1)]
	[Nullable(0)]
	public class Metronome : RelicModel
	{
		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x0600527F RID: 21119 RVA: 0x00242F1B File Offset: 0x0024111B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06005280 RID: 21120 RVA: 0x00242F1E File Offset: 0x0024111E
		public override bool ShowCounter
		{
			get
			{
				return CombatManager.Instance.IsInProgress && (this.OrbsChanneled < base.DynamicVars["OrbCount"].IntValue || this.IsActivating);
			}
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x06005281 RID: 21121 RVA: 0x00242F53 File Offset: 0x00241153
		public override int DisplayAmount
		{
			get
			{
				return Math.Min(this.OrbsChanneled, base.DynamicVars["OrbCount"].IntValue);
			}
		}

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x06005282 RID: 21122 RVA: 0x00242F75 File Offset: 0x00241175
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(30m, ValueProp.Unpowered),
					new DynamicVar("OrbCount", 7m)
				});
			}
		}

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x06005283 RID: 21123 RVA: 0x00242FA5 File Offset: 0x002411A5
		// (set) Token: 0x06005284 RID: 21124 RVA: 0x00242FAD File Offset: 0x002411AD
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

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x06005285 RID: 21125 RVA: 0x00242FC2 File Offset: 0x002411C2
		// (set) Token: 0x06005286 RID: 21126 RVA: 0x00242FCA File Offset: 0x002411CA
		private int OrbsChanneled
		{
			get
			{
				return this._orbsChanneled;
			}
			set
			{
				base.AssertMutable();
				this._orbsChanneled = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x00242FDF File Offset: 0x002411DF
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (!(room is CombatRoom))
			{
				return Task.CompletedTask;
			}
			this.OrbsChanneled = 0;
			this.UpdateDisplay();
			return Task.CompletedTask;
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x00243004 File Offset: 0x00241204
		public override async Task AfterOrbChanneled(PlayerChoiceContext choiceContext, Player player, OrbModel orb)
		{
			if (player == base.Owner)
			{
				int orbsChanneled = this.OrbsChanneled;
				this.OrbsChanneled = orbsChanneled + 1;
				if (this.OrbsChanneled == base.DynamicVars["OrbCount"].IntValue)
				{
					TaskHelper.RunSafely(this.DoActivateVisuals());
					await CreatureCmd.Damage(choiceContext, base.Owner.Creature.CombatState.HittableEnemies, base.DynamicVars.Damage, base.Owner.Creature);
				}
			}
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x00243057 File Offset: 0x00241257
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			this.OrbsChanneled = 0;
			this.UpdateDisplay();
			return Task.CompletedTask;
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x00243074 File Offset: 0x00241274
		private void UpdateDisplay()
		{
			int intValue = base.DynamicVars["OrbCount"].IntValue;
			if (this.OrbsChanneled == intValue - 1 && !this.IsActivating)
			{
				base.Status = RelicStatus.Active;
			}
			else
			{
				base.Status = RelicStatus.Normal;
			}
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x002430C0 File Offset: 0x002412C0
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x04002476 RID: 9334
		private const string _orbCountKey = "OrbCount";

		// Token: 0x04002477 RID: 9335
		private bool _isActivating;

		// Token: 0x04002478 RID: 9336
		private int _orbsChanneled;
	}
}
