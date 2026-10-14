using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x02000739 RID: 1849
	public sealed class MockModifyStarCostPower : PowerModel
	{
		// Token: 0x17001492 RID: 5266
		// (get) Token: 0x06005CE6 RID: 23782 RVA: 0x00257261 File Offset: 0x00255461
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001493 RID: 5267
		// (get) Token: 0x06005CE7 RID: 23783 RVA: 0x00257264 File Offset: 0x00255464
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001494 RID: 5268
		// (get) Token: 0x06005CE8 RID: 23784 RVA: 0x00257267 File Offset: 0x00255467
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x06005CE9 RID: 23785 RVA: 0x0025726A File Offset: 0x0025546A
		public override bool AllowNegative
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x0025726D File Offset: 0x0025546D
		[NullableContext(1)]
		public override bool TryModifyStarCost(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost + base.Amount;
			return true;
		}
	}
}
