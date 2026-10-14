using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x02000738 RID: 1848
	public sealed class MockModifyEnergyCostPower : PowerModel
	{
		// Token: 0x1700148E RID: 5262
		// (get) Token: 0x06005CE0 RID: 23776 RVA: 0x00257233 File Offset: 0x00255433
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700148F RID: 5263
		// (get) Token: 0x06005CE1 RID: 23777 RVA: 0x00257236 File Offset: 0x00255436
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001490 RID: 5264
		// (get) Token: 0x06005CE2 RID: 23778 RVA: 0x00257239 File Offset: 0x00255439
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001491 RID: 5265
		// (get) Token: 0x06005CE3 RID: 23779 RVA: 0x0025723C File Offset: 0x0025543C
		public override bool AllowNegative
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005CE4 RID: 23780 RVA: 0x0025723F File Offset: 0x0025543F
		[NullableContext(1)]
		public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost + base.Amount;
			return true;
		}
	}
}
