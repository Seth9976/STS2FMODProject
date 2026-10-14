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
	// Token: 0x02000617 RID: 1559
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WhiteStar : RelicModel
	{
		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x06005639 RID: 22073 RVA: 0x0024A216 File Offset: 0x00248416
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x0024A219 File Offset: 0x00248419
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x0024A221 File Offset: 0x00248421
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
			rewards.Add(new CardReward(CardCreationOptions.ForRoom(base.Owner, RoomType.Boss), 3, player, null));
			return true;
		}
	}
}
