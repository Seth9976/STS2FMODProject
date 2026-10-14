using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200059A RID: 1434
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NutritiousOyster : RelicModel
	{
		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x060052E4 RID: 21220 RVA: 0x00243BB3 File Offset: 0x00241DB3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x060052E5 RID: 21221 RVA: 0x00243BB6 File Offset: 0x00241DB6
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x060052E6 RID: 21222 RVA: 0x00243BB9 File Offset: 0x00241DB9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(11m));
			}
		}

		// Token: 0x060052E7 RID: 21223 RVA: 0x00243BCC File Offset: 0x00241DCC
		public override async Task AfterObtained()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
		}
	}
}
