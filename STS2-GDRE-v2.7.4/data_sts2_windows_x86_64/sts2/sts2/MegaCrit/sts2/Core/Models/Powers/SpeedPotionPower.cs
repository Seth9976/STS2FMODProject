using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Potions;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006FC RID: 1788
	[NullableContext(1)]
	[Nullable(0)]
	public class SpeedPotionPower : TemporaryDexterityPower
	{
		// Token: 0x170013CC RID: 5068
		// (get) Token: 0x06005B66 RID: 23398 RVA: 0x002542B7 File Offset: 0x002524B7
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Potion<SpeedPotion>();
			}
		}
	}
}
