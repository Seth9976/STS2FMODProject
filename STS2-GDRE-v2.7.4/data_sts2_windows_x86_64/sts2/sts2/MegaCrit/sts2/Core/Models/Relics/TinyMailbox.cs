using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005FC RID: 1532
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TinyMailbox : RelicModel
	{
		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x0600557E RID: 21886 RVA: 0x00248A43 File Offset: 0x00246C43
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x0600557F RID: 21887 RVA: 0x00248A46 File Offset: 0x00246C46
		public override bool TryModifyRestSiteHealRewards(Player player, List<Reward> rewards, bool isMimicked)
		{
			if (player != base.Owner)
			{
				return false;
			}
			rewards.Add(new PotionReward(player));
			rewards.Add(new PotionReward(player));
			base.Flash();
			return true;
		}

		// Token: 0x06005580 RID: 21888 RVA: 0x00248A74 File Offset: 0x00246C74
		public override IReadOnlyList<LocString> ModifyExtraRestSiteHealText(Player player, IReadOnlyList<LocString> currentExtraText)
		{
			if (!LocalContext.IsMe(base.Owner))
			{
				return currentExtraText;
			}
			int num = 0;
			LocString[] array = new LocString[1 + currentExtraText.Count];
			foreach (LocString locString in currentExtraText)
			{
				array[num] = locString;
				num++;
			}
			array[num] = base.AdditionalRestSiteHealText;
			return new <>z__ReadOnlyArray<LocString>(array);
		}
	}
}
