using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200058C RID: 1420
	public sealed class MiniatureTent : RelicModel
	{
		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x06005291 RID: 21137 RVA: 0x0024319D File Offset: 0x0024139D
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x002431A0 File Offset: 0x002413A0
		[NullableContext(1)]
		public override bool ShouldDisableRemainingRestSiteOptions(Player player)
		{
			if (player != base.Owner)
			{
				return true;
			}
			base.Flash();
			return false;
		}
	}
}
