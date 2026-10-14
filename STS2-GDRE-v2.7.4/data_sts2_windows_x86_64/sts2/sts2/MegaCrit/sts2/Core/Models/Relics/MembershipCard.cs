using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000588 RID: 1416
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MembershipCard : RelicModel
	{
		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06005277 RID: 21111 RVA: 0x00242E38 File Offset: 0x00241038
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Discount", 50m));
			}
		}

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06005278 RID: 21112 RVA: 0x00242E50 File Offset: 0x00241050
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x00242E54 File Offset: 0x00241054
		public override decimal ModifyMerchantPrice(Player player, MerchantEntry entry, decimal originalPrice)
		{
			if (player != base.Owner)
			{
				return originalPrice;
			}
			if (!LocalContext.IsMe(base.Owner))
			{
				return originalPrice;
			}
			return originalPrice * (base.DynamicVars["Discount"].BaseValue / 100m);
		}

		// Token: 0x04002475 RID: 9333
		private const string _discountKey = "Discount";
	}
}
