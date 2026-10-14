using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Entities.Potions
{
	// Token: 0x02000C7A RID: 3194
	public static class PotionRarityExtensions
	{
		// Token: 0x060082A5 RID: 33445 RVA: 0x002C2098 File Offset: 0x002C0298
		[NullableContext(1)]
		public static LocString ToLocString(this PotionRarity potionRarity)
		{
			LocString locString;
			switch (potionRarity)
			{
			case PotionRarity.None:
				throw new ArgumentOutOfRangeException("potionRarity", potionRarity, null);
			case PotionRarity.Common:
				locString = new LocString("gameplay_ui", "POTION_RARITY.COMMON");
				break;
			case PotionRarity.Uncommon:
				locString = new LocString("gameplay_ui", "POTION_RARITY.UNCOMMON");
				break;
			case PotionRarity.Rare:
				locString = new LocString("gameplay_ui", "POTION_RARITY.RARE");
				break;
			case PotionRarity.Event:
				locString = new LocString("gameplay_ui", "POTION_RARITY.EVENT");
				break;
			case PotionRarity.Token:
				locString = new LocString("gameplay_ui", "POTION_RARITY.TOKEN");
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(potionRarity);
				break;
			}
			return locString;
		}
	}
}
