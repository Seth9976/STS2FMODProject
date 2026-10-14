using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200053A RID: 1338
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmberTea : RelicModel
	{
		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06005068 RID: 20584 RVA: 0x0023F127 File Offset: 0x0023D327
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06005069 RID: 20585 RVA: 0x0023F12A File Offset: 0x0023D32A
		public override bool IsUsedUp
		{
			get
			{
				return this.CombatsLeft <= 0;
			}
		}

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x0600506A RID: 20586 RVA: 0x0023F138 File Offset: 0x0023D338
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x0600506B RID: 20587 RVA: 0x0023F13B File Offset: 0x0023D33B
		public override int DisplayAmount
		{
			get
			{
				return Math.Max(0, this.CombatsLeft);
			}
		}

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x0600506C RID: 20588 RVA: 0x0023F149 File Offset: 0x0023D349
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("Combats", this.CombatsLeft),
					new PowerVar<StrengthPower>(2m)
				});
			}
		}

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x0600506D RID: 20589 RVA: 0x0023F17C File Offset: 0x0023D37C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x0600506E RID: 20590 RVA: 0x0023F19C File Offset: 0x0023D39C
		// (set) Token: 0x0600506F RID: 20591 RVA: 0x0023F1A4 File Offset: 0x0023D3A4
		[SavedProperty]
		public int CombatsLeft
		{
			get
			{
				return this._combatsLeft;
			}
			set
			{
				base.AssertMutable();
				this._combatsLeft = value;
				base.DynamicVars["Combats"].BaseValue = this._combatsLeft;
				base.InvokeDisplayAmountChanged();
				if (this.IsUsedUp)
				{
					base.Status = RelicStatus.Disabled;
				}
			}
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x0023F1F4 File Offset: 0x0023D3F4
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (!this.IsUsedUp)
			{
				if (room is CombatRoom)
				{
					base.Flash();
					await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Strength.BaseValue, null, null, false);
					this.CombatsLeft--;
				}
			}
		}

		// Token: 0x04002440 RID: 9280
		private const string _combatsKey = "Combats";

		// Token: 0x04002441 RID: 9281
		private int _combatsLeft = 5;
	}
}
