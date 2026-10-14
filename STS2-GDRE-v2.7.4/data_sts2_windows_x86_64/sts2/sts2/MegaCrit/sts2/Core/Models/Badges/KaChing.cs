using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Spent more than 1,000 Gold at the Merchant (aggregate).
	/// </summary>
	// Token: 0x02000B6B RID: 2923
	public class KaChing : Badge
	{
		/// <summary>
		/// Spent more than 1,000 Gold at the Merchant (aggregate).
		/// </summary>
		// Token: 0x06007AE3 RID: 31459 RVA: 0x0029F9D3 File Offset: 0x0029DBD3
		[NullableContext(1)]
		public KaChing(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "KACHING", false, false)
		{
		}

		// Token: 0x170021E7 RID: 8679
		// (get) Token: 0x06007AE4 RID: 31460 RVA: 0x0029F9E5 File Offset: 0x0029DBE5
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007AE5 RID: 31461 RVA: 0x0029F9E8 File Offset: 0x0029DBE8
		public override bool IsObtained()
		{
			int num = 0;
			foreach (List<MapPointHistoryEntry> list in this._run.MapPointHistory)
			{
				foreach (MapPointHistoryEntry mapPointHistoryEntry in list)
				{
					foreach (MapPointRoomHistoryEntry mapPointRoomHistoryEntry in mapPointHistoryEntry.Rooms)
					{
						if (mapPointRoomHistoryEntry.RoomType == RoomType.Shop)
						{
							foreach (PlayerMapPointHistoryEntry playerMapPointHistoryEntry in mapPointHistoryEntry.PlayerStats)
							{
								if (playerMapPointHistoryEntry.PlayerId == this._localPlayer.NetId)
								{
									num += playerMapPointHistoryEntry.GoldSpent;
								}
							}
						}
					}
				}
			}
			return num >= 1000;
		}

		// Token: 0x04002902 RID: 10498
		private const int _goldRequirement = 1000;
	}
}
