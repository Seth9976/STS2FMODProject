using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006C9 RID: 1737
	[NullableContext(1)]
	[Nullable(0)]
	public class PiercingWailPower : TemporaryStrengthPower
	{
		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x06005A23 RID: 23075 RVA: 0x00251B39 File Offset: 0x0024FD39
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<PiercingWail>();
			}
		}

		// Token: 0x17001334 RID: 4916
		// (get) Token: 0x06005A24 RID: 23076 RVA: 0x00251B40 File Offset: 0x0024FD40
		protected override bool IsPositive
		{
			get
			{
				return false;
			}
		}
	}
}
