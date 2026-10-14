using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000514 RID: 1300
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BurningBlood : RelicModel
	{
		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06004FA5 RID: 20389 RVA: 0x0023DC73 File Offset: 0x0023BE73
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Starter;
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06004FA6 RID: 20390 RVA: 0x0023DC76 File Offset: 0x0023BE76
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(6m));
			}
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x0023DC88 File Offset: 0x0023BE88
		public override async Task AfterCombatVictory(CombatRoom _)
		{
			if (!base.Owner.Creature.IsDead)
			{
				base.Flash();
				await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue, true);
			}
		}
	}
}
