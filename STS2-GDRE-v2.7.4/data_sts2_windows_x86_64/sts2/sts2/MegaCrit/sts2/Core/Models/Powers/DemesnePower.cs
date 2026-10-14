using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200065D RID: 1629
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DemesnePower : PowerModel
	{
		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x060057C3 RID: 22467 RVA: 0x0024D727 File Offset: 0x0024B927
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x060057C4 RID: 22468 RVA: 0x0024D72A File Offset: 0x0024B92A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x060057C5 RID: 22469 RVA: 0x0024D72D File Offset: 0x0024B92D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x0024D73A File Offset: 0x0024B93A
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner.Player)
			{
				return count;
			}
			return count + base.Amount;
		}

		// Token: 0x060057C7 RID: 22471 RVA: 0x0024D75D File Offset: 0x0024B95D
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner.Player)
			{
				return amount;
			}
			return amount + base.Amount;
		}
	}
}
