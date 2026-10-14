using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200062C RID: 1580
	[NullableContext(1)]
	[Nullable(0)]
	public class AnticipatePower : TemporaryDexterityPower
	{
		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x060056AE RID: 22190 RVA: 0x0024B96F File Offset: 0x00249B6F
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<Anticipate>();
			}
		}
	}
}
