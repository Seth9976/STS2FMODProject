using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005C3 RID: 1475
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PrismaticGem : RelicModel
	{
		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x06005417 RID: 21527 RVA: 0x00245F2B File Offset: 0x0024412B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x06005418 RID: 21528 RVA: 0x00245F2E File Offset: 0x0024412E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x06005419 RID: 21529 RVA: 0x00245F3B File Offset: 0x0024413B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x00245F48 File Offset: 0x00244148
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.IntValue;
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x00245F70 File Offset: 0x00244170
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
			if (options.CardPools.All<CardPoolModel>((CardPoolModel p) => p.IsColorless))
			{
				return options;
			}
			return options.WithCardPools(player.UnlockState.CharacterCardPools.Union<CardPoolModel>(options.CardPools));
		}
	}
}
