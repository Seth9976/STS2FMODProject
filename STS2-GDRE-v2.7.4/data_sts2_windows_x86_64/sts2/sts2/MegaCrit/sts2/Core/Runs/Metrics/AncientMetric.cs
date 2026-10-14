using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Runs.Metrics
{
	// Token: 0x02000115 RID: 277
	[NullableContext(1)]
	[Nullable(0)]
	public struct AncientMetric
	{
		/// <param name="entry">The map point history entry.</param>
		/// <param name="playerEntry">The record of the player actions taken by the local player in the map point.</param>
		/// <exception cref="T:System.InvalidOperationException"></exception>
		// Token: 0x06000D45 RID: 3397 RVA: 0x000525A4 File Offset: 0x000507A4
		public AncientMetric(MapPointHistoryEntry entry, PlayerMapPointHistoryEntry playerEntry)
		{
			AncientChoiceHistoryEntry ancientChoiceHistoryEntry = playerEntry.AncientChoices.FirstOrDefault<AncientChoiceHistoryEntry>((AncientChoiceHistoryEntry o) => o.WasChosen);
			if (ancientChoiceHistoryEntry == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find chosen ancient choice! ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerEntry.PlayerId);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.picked = ancientChoiceHistoryEntry.TextKey;
			this.skipped = (from o in playerEntry.AncientChoices
				where !o.WasChosen
				select o.TextKey).ToList<string>();
		}

		// Token: 0x0400048B RID: 1163
		public readonly string picked;

		// Token: 0x0400048C RID: 1164
		public readonly List<string> skipped;
	}
}
