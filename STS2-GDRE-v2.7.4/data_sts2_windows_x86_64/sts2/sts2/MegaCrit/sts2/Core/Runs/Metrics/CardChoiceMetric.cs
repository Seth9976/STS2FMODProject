using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Runs.Metrics
{
	// Token: 0x02000116 RID: 278
	[NullableContext(1)]
	[Nullable(0)]
	public struct CardChoiceMetric
	{
		// Token: 0x06000D46 RID: 3398 RVA: 0x00052698 File Offset: 0x00050898
		public CardChoiceMetric(List<CardChoiceHistoryEntry> choices)
		{
			this.picked = new List<string>();
			this.skipped = new List<string>();
			foreach (CardChoiceHistoryEntry cardChoiceHistoryEntry in choices)
			{
				if (cardChoiceHistoryEntry.wasPicked)
				{
					this.picked.Add(cardChoiceHistoryEntry.Card.Id.Entry);
				}
				else
				{
					this.skipped.Add(cardChoiceHistoryEntry.Card.Id.Entry);
				}
			}
		}

		// Token: 0x0400048D RID: 1165
		public readonly List<string> picked;

		// Token: 0x0400048E RID: 1166
		public readonly List<string> skipped;
	}
}
