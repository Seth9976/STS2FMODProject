using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Potions;

namespace MegaCrit.Sts2.Core.Models.PotionPools
{
	// Token: 0x02000786 RID: 1926
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EventPotionPool : PotionPoolModel
	{
		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x06005ED9 RID: 24281 RVA: 0x00259A06 File Offset: 0x00257C06
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x06005EDA RID: 24282 RVA: 0x00259A0D File Offset: 0x00257C0D
		protected override IEnumerable<PotionModel> GenerateAllPotions()
		{
			return new <>z__ReadOnlyArray<PotionModel>(new PotionModel[]
			{
				ModelDb.Potion<Ambergris>(),
				ModelDb.Potion<FoulPotion>(),
				ModelDb.Potion<GlowwaterPotion>()
			});
		}
	}
}
