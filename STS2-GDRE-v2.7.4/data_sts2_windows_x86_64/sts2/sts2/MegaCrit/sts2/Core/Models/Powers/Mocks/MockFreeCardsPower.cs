using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x02000735 RID: 1845
	public sealed class MockFreeCardsPower : PowerModel
	{
		// Token: 0x17001485 RID: 5253
		// (get) Token: 0x06005CCE RID: 23758 RVA: 0x00257143 File Offset: 0x00255343
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001486 RID: 5254
		// (get) Token: 0x06005CCF RID: 23759 RVA: 0x00257146 File Offset: 0x00255346
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001487 RID: 5255
		// (get) Token: 0x06005CD0 RID: 23760 RVA: 0x00257149 File Offset: 0x00255349
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005CD1 RID: 23761 RVA: 0x0025714C File Offset: 0x0025534C
		[NullableContext(1)]
		public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = 0m;
			return true;
		}
	}
}
