using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Runs.Metrics
{
	// Token: 0x02000117 RID: 279
	[NullableContext(1)]
	[Nullable(0)]
	public struct EncounterMetric
	{
		// Token: 0x06000D47 RID: 3399 RVA: 0x00052738 File Offset: 0x00050938
		public EncounterMetric(string id, int damage, int turns)
		{
			this.id = id;
			this.damage = Math.Clamp(damage, 0, 100);
			this.turns = turns;
		}

		// Token: 0x0400048F RID: 1167
		public readonly string id;

		// Token: 0x04000490 RID: 1168
		public readonly int damage;

		// Token: 0x04000491 RID: 1169
		public readonly int turns;
	}
}
