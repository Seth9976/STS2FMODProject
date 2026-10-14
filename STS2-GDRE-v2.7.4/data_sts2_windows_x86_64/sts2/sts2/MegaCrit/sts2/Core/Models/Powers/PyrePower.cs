using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006D1 RID: 1745
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PyrePower : PowerModel
	{
		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x06005A58 RID: 23128 RVA: 0x00252267 File Offset: 0x00250467
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x06005A59 RID: 23129 RVA: 0x0025226A File Offset: 0x0025046A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x06005A5A RID: 23130 RVA: 0x0025226D File Offset: 0x0025046D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005A5B RID: 23131 RVA: 0x0025227A File Offset: 0x0025047A
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
