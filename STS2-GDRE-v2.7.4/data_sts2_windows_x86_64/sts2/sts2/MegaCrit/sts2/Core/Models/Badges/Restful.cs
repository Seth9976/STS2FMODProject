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
	/// The player rested at every rest site and won.
	/// </summary>
	// Token: 0x02000B6F RID: 2927
	public class Restful : Badge
	{
		/// <summary>
		/// The player rested at every rest site and won.
		/// </summary>
		// Token: 0x06007AEF RID: 31471 RVA: 0x0029FE2B File Offset: 0x0029E02B
		[NullableContext(1)]
		public Restful(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "RESTFUL", true, false)
		{
		}

		// Token: 0x170021EB RID: 8683
		// (get) Token: 0x06007AF0 RID: 31472 RVA: 0x0029FE3D File Offset: 0x0029E03D
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007AF1 RID: 31473 RVA: 0x0029FE40 File Offset: 0x0029E040
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
								if (playerMapPointHistoryEntry.PlayerId == this._localPlayer.NetId && !playerMapPointHistoryEntry.RestSiteChoices.Contains("HEAL"))
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
