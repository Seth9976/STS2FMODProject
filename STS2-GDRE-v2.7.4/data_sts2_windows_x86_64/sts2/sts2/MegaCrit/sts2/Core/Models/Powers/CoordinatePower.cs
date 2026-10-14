using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200064C RID: 1612
	[NullableContext(1)]
	[Nullable(0)]
	public class CoordinatePower : TemporaryStrengthPower
	{
		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x06005762 RID: 22370 RVA: 0x0024CC7B File Offset: 0x0024AE7B
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<Coordinate>();
			}
		}
	}
}
