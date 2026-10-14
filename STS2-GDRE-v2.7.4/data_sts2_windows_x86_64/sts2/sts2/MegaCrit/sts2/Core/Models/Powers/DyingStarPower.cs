using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000668 RID: 1640
	[NullableContext(1)]
	[Nullable(0)]
	public class DyingStarPower : TemporaryStrengthPower
	{
		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06005807 RID: 22535 RVA: 0x0024DFDF File Offset: 0x0024C1DF
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<DyingStar>();
			}
		}

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06005808 RID: 22536 RVA: 0x0024DFE6 File Offset: 0x0024C1E6
		protected override bool IsPositive
		{
			get
			{
				return false;
			}
		}
	}
}
