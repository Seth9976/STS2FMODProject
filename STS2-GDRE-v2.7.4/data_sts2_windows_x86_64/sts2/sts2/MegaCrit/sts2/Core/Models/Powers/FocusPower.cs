using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200067B RID: 1659
	public sealed class FocusPower : PowerModel
	{
		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06005860 RID: 22624 RVA: 0x0024E822 File Offset: 0x0024CA22
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x06005861 RID: 22625 RVA: 0x0024E825 File Offset: 0x0024CA25
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06005862 RID: 22626 RVA: 0x0024E828 File Offset: 0x0024CA28
		public override bool AllowNegative
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005863 RID: 22627 RVA: 0x0024E82B File Offset: 0x0024CA2B
		[NullableContext(1)]
		public override decimal ModifyOrbValue(OrbModel orb, decimal value)
		{
			if (base.Owner.Player != orb.Owner)
			{
				return value;
			}
			return Math.Max(value + base.Amount, 0m);
		}
	}
}
