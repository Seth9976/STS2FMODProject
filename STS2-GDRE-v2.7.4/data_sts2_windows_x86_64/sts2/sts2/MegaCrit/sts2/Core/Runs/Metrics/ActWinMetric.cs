using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Runs.Metrics
{
	// Token: 0x02000114 RID: 276
	[NullableContext(1)]
	[Nullable(0)]
	public struct ActWinMetric
	{
		// Token: 0x06000D44 RID: 3396 RVA: 0x00052593 File Offset: 0x00050793
		public ActWinMetric(string actId, bool win)
		{
			this.act = actId;
			this.win = win;
		}

		// Token: 0x04000489 RID: 1161
		public readonly string act;

		// Token: 0x0400048A RID: 1162
		public readonly bool win;
	}
}
