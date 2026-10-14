using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000579 RID: 1401
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LeesWaffle : RelicModel
	{
		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x0600521B RID: 21019 RVA: 0x002424AF File Offset: 0x002406AF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x0600521C RID: 21020 RVA: 0x002424B2 File Offset: 0x002406B2
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x0600521D RID: 21021 RVA: 0x002424B5 File Offset: 0x002406B5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(7m));
			}
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x002424C8 File Offset: 0x002406C8
		public override async Task AfterObtained()
		{
			Creature creature = base.Owner.Creature;
			await CreatureCmd.GainMaxHp(creature, base.DynamicVars.MaxHp.BaseValue);
			await CreatureCmd.Heal(creature, creature.MaxHp - creature.CurrentHp, true);
		}
	}
}
