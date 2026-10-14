using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Potions.Mocks;

namespace MegaCrit.Sts2.Core.Models.PotionPools
{
	/// <summary>
	/// Used for test-only potions.
	/// </summary>
	// Token: 0x02000788 RID: 1928
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockPotionPool : PotionPoolModel
	{
		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x06005EE1 RID: 24289 RVA: 0x00259A7B File Offset: 0x00257C7B
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x06005EE2 RID: 24290 RVA: 0x00259A7E File Offset: 0x00257C7E
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x06005EE3 RID: 24291 RVA: 0x00259A85 File Offset: 0x00257C85
		protected override IEnumerable<PotionModel> GenerateAllPotions()
		{
			return new <>z__ReadOnlySingleElementList<PotionModel>(ModelDb.Potion<MockDiscardAndAddShivsPotion>());
		}
	}
}
