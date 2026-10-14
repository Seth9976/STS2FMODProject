using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200065B RID: 1627
	[NullableContext(1)]
	[Nullable(0)]
	public class DarkShacklesPower : TemporaryStrengthPower
	{
		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x060057B9 RID: 22457 RVA: 0x0024D62F File Offset: 0x0024B82F
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<DarkShackles>();
			}
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x060057BA RID: 22458 RVA: 0x0024D636 File Offset: 0x0024B836
		protected override bool IsPositive
		{
			get
			{
				return false;
			}
		}
	}
}
