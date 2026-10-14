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
	/// For each time you Mend in multiplayer, you get a higher tier badge!
	/// </summary>
	// Token: 0x02000B67 RID: 2919
	public class Healer : Badge
	{
		/// <summary>
		/// For each time you Mend in multiplayer, you get a higher tier badge!
		/// </summary>
		// Token: 0x06007AD7 RID: 31447 RVA: 0x0029F6EA File Offset: 0x0029D8EA
		[NullableContext(1)]
		public Healer(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "HEALER", false, true)
		{
		}

		// Token: 0x170021E3 RID: 8675
		// (get) Token: 0x06007AD8 RID: 31448 RVA: 0x0029F6FC File Offset: 0x0029D8FC
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
							if (mapPointRoomHistoryEntry.RoomType == RoomType.RestSite)
							{
								foreach (PlayerMapPointHistoryEntry playerMapPointHistoryEntry in mapPointHistoryEntry.PlayerStats)
								{
									if (playerMapPointHistoryEntry.PlayerId == this._localPlayer.NetId && playerMapPointHistoryEntry.RestSiteChoices.Contains("MEND"))
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

		// Token: 0x06007AD9 RID: 31449 RVA: 0x0029F868 File Offset: 0x0029DA68
		public override bool IsObtained()
		{
			return this.Rarity > BadgeRarity.None;
		}
	}
}
