using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200072D RID: 1837
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WasteAwayPower : PowerModel
	{
		// Token: 0x17001469 RID: 5225
		// (get) Token: 0x06005CA1 RID: 23713 RVA: 0x00256D9F File Offset: 0x00254F9F
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x1700146A RID: 5226
		// (get) Token: 0x06005CA2 RID: 23714 RVA: 0x00256DA2 File Offset: 0x00254FA2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700146B RID: 5227
		// (get) Token: 0x06005CA3 RID: 23715 RVA: 0x00256DA5 File Offset: 0x00254FA5
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005CA4 RID: 23716 RVA: 0x00256DB2 File Offset: 0x00254FB2
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner.Player)
			{
				return amount;
			}
			return amount - base.Amount;
		}
	}
}
