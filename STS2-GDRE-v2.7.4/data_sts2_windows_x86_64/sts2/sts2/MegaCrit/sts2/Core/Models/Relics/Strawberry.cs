using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005EF RID: 1519
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Strawberry : RelicModel
	{
		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x06005534 RID: 21812 RVA: 0x002482C7 File Offset: 0x002464C7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x06005535 RID: 21813 RVA: 0x002482CA File Offset: 0x002464CA
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x06005536 RID: 21814 RVA: 0x002482CD File Offset: 0x002464CD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(7m));
			}
		}

		// Token: 0x06005537 RID: 21815 RVA: 0x002482E0 File Offset: 0x002464E0
		public override async Task AfterObtained()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
		}
	}
}
