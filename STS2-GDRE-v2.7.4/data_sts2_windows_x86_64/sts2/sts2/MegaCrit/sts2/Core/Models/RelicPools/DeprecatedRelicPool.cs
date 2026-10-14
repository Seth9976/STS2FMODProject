using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.RelicPools
{
	// Token: 0x0200061E RID: 1566
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeprecatedRelicPool : RelicPoolModel
	{
		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x06005669 RID: 22121 RVA: 0x0024A83E File Offset: 0x00248A3E
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x0024A845 File Offset: 0x00248A45
		protected override IEnumerable<RelicModel> GenerateAllRelics()
		{
			return new <>z__ReadOnlySingleElementList<RelicModel>(ModelDb.Relic<DeprecatedRelic>());
		}
	}
}
