using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005BF RID: 1471
	public sealed class PrayerWheel : RelicModel
	{
		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x06005405 RID: 21509 RVA: 0x00245DAB File Offset: 0x00243FAB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x00245DAE File Offset: 0x00243FAE
		[NullableContext(1)]
		public override bool TryModifyRewards(Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			if (player != base.Owner)
			{
				return false;
			}
			if (room == null || room.RoomType != RoomType.Monster)
			{
				return false;
			}
			rewards.Add(new CardReward(CardCreationOptions.ForRoom(player, RoomType.Monster), 3, player, null));
			return true;
		}
	}
}
