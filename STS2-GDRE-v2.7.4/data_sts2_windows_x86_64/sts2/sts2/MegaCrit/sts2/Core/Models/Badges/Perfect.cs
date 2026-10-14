using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Defeat a boss without taking damage.
	/// </summary>
	// Token: 0x02000B6E RID: 2926
	public class Perfect : Badge
	{
		/// <summary>
		/// Defeat a boss without taking damage.
		/// </summary>
		// Token: 0x06007AEC RID: 31468 RVA: 0x0029FC48 File Offset: 0x0029DE48
		[NullableContext(1)]
		public Perfect(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "PERFECT", false, false)
		{
		}

		// Token: 0x170021EA RID: 8682
		// (get) Token: 0x06007AED RID: 31469 RVA: 0x0029FC5C File Offset: 0x0029DE5C
		public override BadgeRarity Rarity
		{
			get
			{
				int num = 0;
				foreach (List<MapPointHistoryEntry> list in this._run.MapPointHistory)
				{
					foreach (MapPointHistoryEntry mapPointHistoryEntry in list)
					{
						foreach (MapPointRoomHistoryEntry mapPointRoomHistoryEntry in mapPointHistoryEntry.Rooms)
						{
							if (mapPointRoomHistoryEntry.RoomType == RoomType.Boss && (this._won || mapPointRoomHistoryEntry != this._run.MapPointHistory.Last<List<MapPointHistoryEntry>>().Last<MapPointHistoryEntry>().Rooms.Last<MapPointRoomHistoryEntry>()))
							{
								foreach (PlayerMapPointHistoryEntry playerMapPointHistoryEntry in mapPointHistoryEntry.PlayerStats)
								{
									if (playerMapPointHistoryEntry.PlayerId == this._localPlayer.NetId && playerMapPointHistoryEntry.DamageTaken <= 0)
									{
										num++;
									}
								}
							}
						}
					}
				}
				BadgeRarity badgeRarity;
				if (num < 3)
				{
					if (num != 1)
					{
						if (num != 2)
						{
							badgeRarity = BadgeRarity.None;
						}
						else
						{
							badgeRarity = BadgeRarity.Silver;
						}
					}
					else
					{
						badgeRarity = BadgeRarity.Bronze;
					}
				}
				else
				{
					badgeRarity = BadgeRarity.Gold;
				}
				return badgeRarity;
			}
		}

		// Token: 0x06007AEE RID: 31470 RVA: 0x0029FE20 File Offset: 0x0029E020
		public override bool IsObtained()
		{
			return this.Rarity > BadgeRarity.None;
		}
	}
}
