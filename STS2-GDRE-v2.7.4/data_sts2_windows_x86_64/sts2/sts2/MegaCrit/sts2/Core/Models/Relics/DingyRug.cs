using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200052E RID: 1326
	public sealed class DingyRug : RelicModel
	{
		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x0600502D RID: 20525 RVA: 0x0023EAF3 File Offset: 0x0023CCF3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x0023EAF8 File Offset: 0x0023CCF8
		[NullableContext(1)]
		public override CardCreationOptions ModifyCardRewardCreationOptions(Player player, CardCreationOptions options)
		{
			if (base.Owner != player)
			{
				return options;
			}
			if (options.Flags.HasFlag(CardCreationFlags.NoCardPoolModifications))
			{
				return options;
			}
			if (!options.Flags.HasFlag(CardCreationFlags.IsCardReward))
			{
				return options;
			}
			return options.WithCardPools(options.CardPools.Union<CardPoolModel>(new <>z__ReadOnlySingleElementList<CardPoolModel>(ModelDb.CardPool<ColorlessCardPool>())));
		}
	}
}
