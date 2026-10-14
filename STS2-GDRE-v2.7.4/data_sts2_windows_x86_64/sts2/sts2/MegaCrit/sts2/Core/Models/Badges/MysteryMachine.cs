using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Traveled to 15 or more ? rooms.
	/// </summary>
	// Token: 0x02000B6D RID: 2925
	public class MysteryMachine : Badge
	{
		/// <summary>
		/// Traveled to 15 or more ? rooms.
		/// </summary>
		// Token: 0x06007AE9 RID: 31465 RVA: 0x0029FB93 File Offset: 0x0029DD93
		[NullableContext(1)]
		public MysteryMachine(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "MYSTERY_MACHINE", false, false)
		{
		}

		// Token: 0x170021E9 RID: 8681
		// (get) Token: 0x06007AEA RID: 31466 RVA: 0x0029FBA5 File Offset: 0x0029DDA5
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007AEB RID: 31467 RVA: 0x0029FBA8 File Offset: 0x0029DDA8
		public override bool IsObtained()
		{
			int num = 0;
			foreach (List<MapPointHistoryEntry> list in this._run.MapPointHistory)
			{
				foreach (MapPointHistoryEntry mapPointHistoryEntry in list)
				{
					if (mapPointHistoryEntry.MapPointType == MapPointType.Unknown)
					{
						num++;
					}
				}
			}
			return num >= 15;
		}

		// Token: 0x04002903 RID: 10499
		private const int _eventCount = 15;
	}
}
