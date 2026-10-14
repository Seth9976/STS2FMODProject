using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Potions;

namespace MegaCrit.Sts2.Core.Models.PotionPools
{
	// Token: 0x02000785 RID: 1925
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeprecatedPotionPool : PotionPoolModel
	{
		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x06005ED6 RID: 24278 RVA: 0x002599EB File Offset: 0x00257BEB
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x06005ED7 RID: 24279 RVA: 0x002599F2 File Offset: 0x00257BF2
		protected override IEnumerable<PotionModel> GenerateAllPotions()
		{
			return new <>z__ReadOnlySingleElementList<PotionModel>(ModelDb.Potion<DeprecatedPotion>());
		}
	}
}
