using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005AE RID: 1454
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaperKrane : RelicModel
	{
		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x0600538C RID: 21388 RVA: 0x00244FC7 File Offset: 0x002431C7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x0600538D RID: 21389 RVA: 0x00244FCC File Offset: 0x002431CC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x0600538E RID: 21390 RVA: 0x00244FEC File Offset: 0x002431EC
		[NullableContext(2)]
		public decimal ModifyWeakMultiplier([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (target != base.Owner.Creature)
			{
				return amount;
			}
			if (!props.IsPoweredAttack())
			{
				return amount;
			}
			return amount - 0.15m;
		}
	}
}
