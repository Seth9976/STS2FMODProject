using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006A9 RID: 1705
	public sealed class MachineLearningPower : PowerModel
	{
		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x0600596C RID: 22892 RVA: 0x00250833 File Offset: 0x0024EA33
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x0600596D RID: 22893 RVA: 0x00250836 File Offset: 0x0024EA36
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x00250839 File Offset: 0x0024EA39
		[NullableContext(1)]
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner.Player)
			{
				return count;
			}
			return count + base.Amount;
		}
	}
}
