using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Rewards
{
	// Token: 0x0200014D RID: 333
	public static class RewardTypeExtensions
	{
		// Token: 0x06001019 RID: 4121 RVA: 0x000613B0 File Offset: 0x0005F5B0
		[NullableContext(1)]
		public static IEnumerable<string> GetAssetPaths(this RewardType rewardType)
		{
			switch (rewardType)
			{
			case RewardType.Card:
				return CardReward.AssetPaths;
			case RewardType.Gold:
				return GoldReward.AssetPaths;
			case RewardType.RemoveCard:
				return CardRemovalReward.AssetPaths;
			case RewardType.SpecialCard:
				return SpecialCardReward.AssetPaths;
			}
			return Array.Empty<string>();
		}
	}
}
