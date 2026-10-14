using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005E1 RID: 1505
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SignetRing : RelicModel
	{
		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x060054D5 RID: 21717 RVA: 0x002476D3 File Offset: 0x002458D3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x060054D6 RID: 21718 RVA: 0x002476D6 File Offset: 0x002458D6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(999));
			}
		}

		// Token: 0x060054D7 RID: 21719 RVA: 0x002476E8 File Offset: 0x002458E8
		public override async Task AfterObtained()
		{
			await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
		}
	}
}
