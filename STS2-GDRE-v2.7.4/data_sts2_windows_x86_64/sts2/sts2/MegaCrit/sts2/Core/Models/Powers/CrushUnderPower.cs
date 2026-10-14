using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000655 RID: 1621
	[NullableContext(1)]
	[Nullable(0)]
	public class CrushUnderPower : TemporaryStrengthPower
	{
		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x06005795 RID: 22421 RVA: 0x0024D1D0 File Offset: 0x0024B3D0
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<CrushUnder>();
			}
		}

		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x06005796 RID: 22422 RVA: 0x0024D1D7 File Offset: 0x0024B3D7
		protected override bool IsPositive
		{
			get
			{
				return false;
			}
		}
	}
}
