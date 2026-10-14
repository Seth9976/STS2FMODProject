using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.RelicPools
{
	// Token: 0x02000620 RID: 1568
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FallbackRelicPool : RelicPoolModel
	{
		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x0600566F RID: 22127 RVA: 0x0024ADA3 File Offset: 0x00248FA3
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x06005670 RID: 22128 RVA: 0x0024ADAA File Offset: 0x00248FAA
		protected override IEnumerable<RelicModel> GenerateAllRelics()
		{
			return new <>z__ReadOnlySingleElementList<RelicModel>(ModelDb.Relic<Circlet>());
		}
	}
}
