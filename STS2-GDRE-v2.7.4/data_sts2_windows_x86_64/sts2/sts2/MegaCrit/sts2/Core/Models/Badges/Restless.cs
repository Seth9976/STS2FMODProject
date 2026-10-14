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
	/// The player won a run without ever resting.
	/// </summary>
	// Token: 0x02000B70 RID: 2928
	public class Restless : Badge
	{
		/// <summary>
		/// The player won a run without ever resting.
		/// </summary>
		// Token: 0x06007AF2 RID: 31474 RVA: 0x0029FF94 File Offset: 0x0029E194
		[NullableContext(1)]
		public Restless(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "RESTLESS", true, false)
		{
		}

		// Token: 0x170021EC RID: 8684
		// (get) Token: 0x06007AF3 RID: 31475 RVA: 0x0029FFA6 File Offset: 0x0029E1A6
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007AF4 RID: 31476 RVA: 0x0029FFAC File Offset: 0x0029E1AC
		public override bool IsObtained()
		{
			int num = 0;
			foreach (List<MapPointHistoryEntry> list in this._run.MapPointHistory)
			{
				foreach (MapPointHistoryEntry mapPointHistoryEntry in list)
				{
					foreach (MapPointRoomHistoryEntry mapPointRoomHistoryEntry in mapPointHistoryEntry.Rooms)
					{
						if (mapPointRoomHistoryEntry.RoomType == RoomType.RestSite)
						{
							num++;
							foreach (PlayerMapPointHistoryEntry playerMapPointHistoryEntry in mapPointHistoryEntry.PlayerStats)
							{
								if (playerMapPointHistoryEntry.PlayerId == this._localPlayer.NetId && playerMapPointHistoryEntry.RestSiteChoices.Contains("HEAL"))
								{
									return false;
								}
							}
						}
					}
				}
			}
			return num > 0;
		}
	}
}
