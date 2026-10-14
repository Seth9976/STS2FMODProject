using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000504 RID: 1284
	public sealed class BlackStar : RelicModel
	{
		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06004F3A RID: 20282 RVA: 0x0023CF17 File Offset: 0x0023B117
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x0023CF1A File Offset: 0x0023B11A
		[NullableContext(1)]
		public override bool TryModifyRewards(Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			if (player != base.Owner)
			{
				return false;
			}
			if (room == null || room.RoomType != RoomType.Elite)
			{
				return false;
			}
			rewards.Add(new RelicReward(player));
			return true;
		}
	}
}
