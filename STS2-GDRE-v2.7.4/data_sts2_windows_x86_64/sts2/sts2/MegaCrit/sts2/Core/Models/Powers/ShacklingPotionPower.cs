using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Potions;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006E8 RID: 1768
	[NullableContext(1)]
	[Nullable(0)]
	public class ShacklingPotionPower : TemporaryStrengthPower
	{
		// Token: 0x1700138F RID: 5007
		// (get) Token: 0x06005AE7 RID: 23271 RVA: 0x00253492 File Offset: 0x00251692
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Potion<ShacklingPotion>();
			}
		}

		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x06005AE8 RID: 23272 RVA: 0x00253499 File Offset: 0x00251699
		protected override bool IsPositive
		{
			get
			{
				return false;
			}
		}
	}
}
