using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004E3 RID: 1251
	[NullableContext(1)]
	public interface ITemporaryPower
	{
		/// <summary>
		/// The canonical model that applies this power.
		/// </summary>
		/// <example><see cref="T:MegaCrit.Sts2.Core.Models.Potions.FlexPotion" /> for <see cref="T:MegaCrit.Sts2.Core.Models.Powers.FlexPotionPower" /></example>
		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06004CDA RID: 19674
		AbstractModel OriginModel { get; }

		/// <summary>
		/// The canonical power that this power internally applies.
		/// </summary>
		/// <example><see cref="T:MegaCrit.Sts2.Core.Models.Powers.StrengthPower" /> for <see cref="T:MegaCrit.Sts2.Core.Models.Powers.TemporaryStrengthPower" /></example>
		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06004CDB RID: 19675
		PowerModel InternallyAppliedPower { get; }
	}
}
