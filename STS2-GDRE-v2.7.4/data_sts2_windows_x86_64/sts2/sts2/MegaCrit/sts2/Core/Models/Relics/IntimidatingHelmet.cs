using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000567 RID: 1383
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IntimidatingHelmet : RelicModel
	{
		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x0600518C RID: 20876 RVA: 0x00241256 File Offset: 0x0023F456
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x0600518D RID: 20877 RVA: 0x00241259 File Offset: 0x0023F459
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(4m, ValueProp.Unpowered),
					new EnergyVar(2)
				});
			}
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x0600518E RID: 20878 RVA: 0x0024127E File Offset: 0x0023F47E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x00241290 File Offset: 0x0023F490
		public override async Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (cardPlay.Resources.EnergyValue >= base.DynamicVars.Energy.IntValue)
				{
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
				}
			}
		}
	}
}
