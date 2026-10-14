using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x02000823 RID: 2083
	public class Vintage : ModifierModel
	{
		// Token: 0x06006759 RID: 26457 RVA: 0x0027257C File Offset: 0x0027077C
		[NullableContext(1)]
		public override bool TryModifyRewardsLate(Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			CombatRoom combatRoom = room as CombatRoom;
			if (combatRoom == null)
			{
				return false;
			}
			if (combatRoom.Encounter.RoomType != RoomType.Monster)
			{
				return false;
			}
			for (int i = 0; i < rewards.Count; i++)
			{
				if (rewards[i] is CardReward)
				{
					rewards.RemoveAt(i);
					RelicReward relicReward = new RelicReward(player);
					rewards.Insert(i, relicReward);
				}
			}
			return true;
		}
	}
}
