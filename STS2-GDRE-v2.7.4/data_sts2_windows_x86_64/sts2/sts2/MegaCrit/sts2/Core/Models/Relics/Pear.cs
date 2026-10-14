using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005B1 RID: 1457
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Pear : RelicModel
	{
		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x06005398 RID: 21400 RVA: 0x00245103 File Offset: 0x00243303
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x06005399 RID: 21401 RVA: 0x00245106 File Offset: 0x00243306
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x0600539A RID: 21402 RVA: 0x00245109 File Offset: 0x00243309
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(10m));
			}
		}

		// Token: 0x0600539B RID: 21403 RVA: 0x0024511C File Offset: 0x0024331C
		public override async Task AfterObtained()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
		}
	}
}
