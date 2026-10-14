using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200066B RID: 1643
	[NullableContext(1)]
	[Nullable(0)]
	public class EnfeeblingTouchPower : TemporaryStrengthPower
	{
		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06005815 RID: 22549 RVA: 0x0024E0EF File Offset: 0x0024C2EF
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<EnfeeblingTouch>();
			}
		}

		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06005816 RID: 22550 RVA: 0x0024E0F6 File Offset: 0x0024C2F6
		protected override bool IsPositive
		{
			get
			{
				return false;
			}
		}
	}
}
