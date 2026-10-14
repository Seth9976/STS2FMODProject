using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000587 RID: 1415
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeatOnTheBone : RelicModel
	{
		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x06005270 RID: 21104 RVA: 0x00242D07 File Offset: 0x00240F07
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06005271 RID: 21105 RVA: 0x00242D0A File Offset: 0x00240F0A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("HpThreshold", 50m),
					new HealVar(12m)
				});
			}
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x00242D3A File Offset: 0x00240F3A
		public override Task BeforeCombatStart()
		{
			if (this.WillHealOnCombatFinished())
			{
				base.Status = RelicStatus.Active;
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x00242D50 File Offset: 0x00240F50
		public override Task AfterCurrentHpChanged(Creature creature, decimal delta)
		{
			if (creature != base.Owner.Creature)
			{
				return Task.CompletedTask;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return Task.CompletedTask;
			}
			base.Status = (this.WillHealOnCombatFinished() ? RelicStatus.Active : RelicStatus.Normal);
			return Task.CompletedTask;
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x00242D90 File Offset: 0x00240F90
		public override async Task AfterCombatVictoryEarly(CombatRoom _)
		{
			if (!base.Owner.Creature.IsDead)
			{
				Creature creature = base.Owner.Creature;
				if (this.WillHealOnCombatFinished())
				{
					base.Status = RelicStatus.Normal;
					await CreatureCmd.Heal(creature, base.DynamicVars.Heal.BaseValue, true);
				}
			}
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x00242DD4 File Offset: 0x00240FD4
		private bool WillHealOnCombatFinished()
		{
			Creature creature = base.Owner.Creature;
			int num = (int)(creature.MaxHp * (base.DynamicVars["HpThreshold"].BaseValue / 100m));
			return creature.CurrentHp <= num;
		}

		// Token: 0x04002474 RID: 9332
		private const string _hpThresholdKey = "HpThreshold";
	}
}
