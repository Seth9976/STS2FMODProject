using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000661 RID: 1633
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DexterityPower : PowerModel
	{
		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x060057D7 RID: 22487 RVA: 0x0024D8EF File Offset: 0x0024BAEF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x060057D8 RID: 22488 RVA: 0x0024D8F2 File Offset: 0x0024BAF2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x060057D9 RID: 22489 RVA: 0x0024D8F5 File Offset: 0x0024BAF5
		public override bool AllowNegative
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x060057DA RID: 22490 RVA: 0x0024D8F8 File Offset: 0x0024BAF8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060057DB RID: 22491 RVA: 0x0024D90C File Offset: 0x0024BB0C
		[NullableContext(2)]
		public override decimal ModifyBlockAdditive([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			if (cardSource != null)
			{
				if (cardSource.Owner.Creature != base.Owner)
				{
					return 0m;
				}
			}
			else if (base.Owner != target)
			{
				return 0m;
			}
			if (!props.IsPoweredCardOrMonsterMoveBlock())
			{
				return 0m;
			}
			return base.Amount;
		}
	}
}
