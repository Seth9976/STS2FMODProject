using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000536 RID: 1334
	public class Driftwood : RelicModel
	{
		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06005051 RID: 20561 RVA: 0x0023EE98 File Offset: 0x0023D098
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x0023EE9C File Offset: 0x0023D09C
		[NullableContext(1)]
		public override bool TryModifyRewardsLate(Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			if (player != base.Owner)
			{
				return false;
			}
			foreach (CardReward cardReward in rewards.OfType<CardReward>())
			{
				cardReward.CanReroll = true;
			}
			return true;
		}
	}
}
