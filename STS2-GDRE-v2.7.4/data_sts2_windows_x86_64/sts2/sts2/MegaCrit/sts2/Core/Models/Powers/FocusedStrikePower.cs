using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200067A RID: 1658
	[NullableContext(1)]
	[Nullable(0)]
	public class FocusedStrikePower : TemporaryFocusPower
	{
		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x0600585E RID: 22622 RVA: 0x0024E813 File Offset: 0x0024CA13
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<FocusedStrike>();
			}
		}
	}
}
