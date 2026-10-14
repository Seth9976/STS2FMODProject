using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005CA RID: 1482
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RedSkull : RelicModel
	{
		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x0600544C RID: 21580 RVA: 0x002464C8 File Offset: 0x002446C8
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x0600544D RID: 21581 RVA: 0x002464CB File Offset: 0x002446CB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("HpThreshold", 50m),
					new PowerVar<StrengthPower>(3m)
				});
			}
		}

		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x0600544E RID: 21582 RVA: 0x002464FC File Offset: 0x002446FC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x0600544F RID: 21583 RVA: 0x0024651C File Offset: 0x0024471C
		// (set) Token: 0x06005450 RID: 21584 RVA: 0x00246524 File Offset: 0x00244724
		private bool StrengthApplied
		{
			get
			{
				return this._strengthApplied;
			}
			set
			{
				base.AssertMutable();
				this._strengthApplied = value;
			}
		}

		// Token: 0x06005451 RID: 21585 RVA: 0x00246534 File Offset: 0x00244734
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is CombatRoom)
			{
				await this.ModifyStrengthIfNecessary();
			}
		}

		// Token: 0x06005452 RID: 21586 RVA: 0x0024657F File Offset: 0x0024477F
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this.StrengthApplied = false;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x06005453 RID: 21587 RVA: 0x00246594 File Offset: 0x00244794
		public override async Task AfterCurrentHpChanged(Creature creature, decimal _)
		{
			if (CombatManager.Instance.IsInProgress)
			{
				await this.ModifyStrengthIfNecessary();
			}
		}

		// Token: 0x06005454 RID: 21588 RVA: 0x002465D8 File Offset: 0x002447D8
		private async Task ModifyStrengthIfNecessary()
		{
			Creature creature = base.Owner.Creature;
			bool flag = creature.CurrentHp > creature.MaxHp * (base.DynamicVars["HpThreshold"].BaseValue / 100m);
			base.Status = (flag ? RelicStatus.Normal : RelicStatus.Active);
			decimal baseValue = base.DynamicVars.Strength.BaseValue;
			if (flag && this.StrengthApplied)
			{
				base.Flash();
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), creature, -baseValue, creature, null, false);
				this.StrengthApplied = false;
			}
			else if (!flag && !this.StrengthApplied)
			{
				base.Flash();
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), creature, baseValue, creature, null, false);
				this.StrengthApplied = true;
			}
		}

		// Token: 0x040024B1 RID: 9393
		private const string _hpThresholdKey = "HpThreshold";

		// Token: 0x040024B2 RID: 9394
		private bool _strengthApplied;
	}
}
