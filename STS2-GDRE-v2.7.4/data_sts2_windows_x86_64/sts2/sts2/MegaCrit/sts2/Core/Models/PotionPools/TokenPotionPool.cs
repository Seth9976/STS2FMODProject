using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Potions;

namespace MegaCrit.Sts2.Core.Models.PotionPools
{
	/// <summary>
	/// Used for generated potions that should not be automatically generated in rewards or shops.
	/// </summary>
	// Token: 0x0200078D RID: 1933
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TokenPotionPool : PotionPoolModel
	{
		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x06005EF8 RID: 24312 RVA: 0x00259DCB File Offset: 0x00257FCB
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x06005EF9 RID: 24313 RVA: 0x00259DD2 File Offset: 0x00257FD2
		protected override IEnumerable<PotionModel> GenerateAllPotions()
		{
			return new <>z__ReadOnlySingleElementList<PotionModel>(ModelDb.Potion<PotionShapedRock>());
		}
	}
}
