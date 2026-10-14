using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000535 RID: 1333
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DreamCatcher : RelicModel
	{
		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x0600504D RID: 20557 RVA: 0x0023EDE3 File Offset: 0x0023CFE3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x0023EDE6 File Offset: 0x0023CFE6
		public override bool TryModifyRestSiteHealRewards(Player player, List<Reward> rewards, bool isMimicked)
		{
			if (player != base.Owner)
			{
				return false;
			}
			rewards.Add(new CardReward(CardCreationOptions.ForRoom(player, RoomType.Monster), 3, base.Owner, null));
			base.Flash();
			return true;
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x0023EE14 File Offset: 0x0023D014
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
