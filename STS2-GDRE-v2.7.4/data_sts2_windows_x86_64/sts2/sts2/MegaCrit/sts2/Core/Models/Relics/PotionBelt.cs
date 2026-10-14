using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005BD RID: 1469
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PotionBelt : RelicModel
	{
		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x060053FC RID: 21500 RVA: 0x00245CDF File Offset: 0x00243EDF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x060053FD RID: 21501 RVA: 0x00245CE2 File Offset: 0x00243EE2
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x060053FE RID: 21502 RVA: 0x00245CE5 File Offset: 0x00243EE5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("PotionSlots", 2m));
			}
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x00245CFC File Offset: 0x00243EFC
		public override async Task AfterObtained()
		{
			await PlayerCmd.GainMaxPotionCount(base.DynamicVars["PotionSlots"].IntValue, base.Owner);
		}

		// Token: 0x040024A8 RID: 9384
		private const string _potionSlotsKey = "PotionSlots";
	}
}
