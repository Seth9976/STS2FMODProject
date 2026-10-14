using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000654 RID: 1620
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CrueltyPower : PowerModel
	{
		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x06005790 RID: 22416 RVA: 0x0024D16D File Offset: 0x0024B36D
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x06005791 RID: 22417 RVA: 0x0024D170 File Offset: 0x0024B370
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x06005792 RID: 22418 RVA: 0x0024D174 File Offset: 0x0024B374
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x0024D194 File Offset: 0x0024B394
		[NullableContext(2)]
		public decimal ModifyVulnerableMultiplier([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (target == base.Owner)
			{
				return amount;
			}
			if (!props.IsPoweredAttack())
			{
				return amount;
			}
			return amount + base.Amount / 100m;
		}
	}
}
