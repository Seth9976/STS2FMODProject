using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x0200081D RID: 2077
	[NullableContext(1)]
	[Nullable(0)]
	public class Midas : ModifierModel
	{
		// Token: 0x06006745 RID: 26437 RVA: 0x00272188 File Offset: 0x00270388
		public override bool TryModifyRewardsLate(Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			List<Reward> list = new List<Reward>();
			foreach (Reward reward in rewards)
			{
				GoldReward goldReward = reward as GoldReward;
				if (goldReward != null)
				{
					list.Add(new GoldReward(goldReward.Amount * 2, player, false));
				}
				else
				{
					list.Add(reward);
				}
			}
			rewards.Clear();
			rewards.AddRange(list);
			return true;
		}

		// Token: 0x06006746 RID: 26438 RVA: 0x0027220C File Offset: 0x0027040C
		public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
		{
			List<SmithRestSiteOption> list = options.OfType<SmithRestSiteOption>().ToList<SmithRestSiteOption>();
			foreach (SmithRestSiteOption smithRestSiteOption in list)
			{
				options.Remove(smithRestSiteOption);
			}
			return true;
		}
	}
}
