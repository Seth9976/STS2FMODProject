using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005E5 RID: 1509
	public sealed class SmallCapsule : RelicModel
	{
		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x060054F7 RID: 21751 RVA: 0x00247B3F File Offset: 0x00245D3F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x060054F8 RID: 21752 RVA: 0x00247B44 File Offset: 0x00245D44
		[NullableContext(1)]
		public override async Task AfterObtained()
		{
			await RewardsCmd.OfferCustom(base.Owner, new List<Reward>(1)
			{
				new RelicReward(base.Owner)
			});
		}
	}
}
