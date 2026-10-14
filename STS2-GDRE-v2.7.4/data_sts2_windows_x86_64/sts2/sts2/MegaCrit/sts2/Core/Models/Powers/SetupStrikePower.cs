using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006E7 RID: 1767
	[NullableContext(1)]
	[Nullable(0)]
	public class SetupStrikePower : TemporaryStrengthPower
	{
		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x06005AE5 RID: 23269 RVA: 0x00253483 File Offset: 0x00251683
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<SetupStrike>();
			}
		}
	}
}
