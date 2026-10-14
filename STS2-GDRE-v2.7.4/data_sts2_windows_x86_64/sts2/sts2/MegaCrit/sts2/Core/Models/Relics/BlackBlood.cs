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
	// Token: 0x02000503 RID: 1283
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BlackBlood : RelicModel
	{
		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06004F36 RID: 20278 RVA: 0x0023CEB3 File Offset: 0x0023B0B3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Starter;
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06004F37 RID: 20279 RVA: 0x0023CEB6 File Offset: 0x0023B0B6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(12m));
			}
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x0023CECC File Offset: 0x0023B0CC
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
