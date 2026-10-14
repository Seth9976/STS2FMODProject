using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Potions;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000678 RID: 1656
	[NullableContext(1)]
	[Nullable(0)]
	public class FlexPotionPower : TemporaryStrengthPower
	{
		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06005854 RID: 22612 RVA: 0x0024E737 File Offset: 0x0024C937
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Potion<FlexPotion>();
			}
		}
	}
}
