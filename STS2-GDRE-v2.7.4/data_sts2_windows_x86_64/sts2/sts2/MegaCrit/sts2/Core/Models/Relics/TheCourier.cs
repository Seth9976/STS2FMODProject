using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005F9 RID: 1529
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheCourier : RelicModel
	{
		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x0600556C RID: 21868 RVA: 0x002488CF File Offset: 0x00246ACF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x0600556D RID: 21869 RVA: 0x002488D2 File Offset: 0x00246AD2
		public override bool IsAllowedInShops
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x0600556E RID: 21870 RVA: 0x002488D5 File Offset: 0x00246AD5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Discount", 20m));
			}
		}

		// Token: 0x0600556F RID: 21871 RVA: 0x002488ED File Offset: 0x00246AED
		public override decimal ModifyMerchantPrice(Player player, MerchantEntry entry, decimal originalPrice)
		{
			if (player != base.Owner)
			{
				return originalPrice;
			}
			return originalPrice * (1m - base.DynamicVars["Discount"].BaseValue / 100m);
		}

		// Token: 0x06005570 RID: 21872 RVA: 0x0024892B File Offset: 0x00246B2B
		public override bool ShouldRefillMerchantEntry(MerchantEntry entry, Player player)
		{
			return player == base.Owner;
		}

		// Token: 0x040024CE RID: 9422
		private const string _discountKey = "Discount";
	}
}
