using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000682 RID: 1666
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FriendshipPower : PowerModel
	{
		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06005882 RID: 22658 RVA: 0x0024EC47 File Offset: 0x0024CE47
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06005883 RID: 22659 RVA: 0x0024EC4A File Offset: 0x0024CE4A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06005884 RID: 22660 RVA: 0x0024EC4D File Offset: 0x0024CE4D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x0024EC5A File Offset: 0x0024CE5A
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
