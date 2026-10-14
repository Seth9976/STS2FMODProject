using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200055B RID: 1371
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GoldenPearl : RelicModel
	{
		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x0600514A RID: 20810 RVA: 0x00240B71 File Offset: 0x0023ED71
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x0600514B RID: 20811 RVA: 0x00240B74 File Offset: 0x0023ED74
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x0600514C RID: 20812 RVA: 0x00240B77 File Offset: 0x0023ED77
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(150));
			}
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x00240B88 File Offset: 0x0023ED88
		public override async Task AfterObtained()
		{
			await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
		}
	}
}
