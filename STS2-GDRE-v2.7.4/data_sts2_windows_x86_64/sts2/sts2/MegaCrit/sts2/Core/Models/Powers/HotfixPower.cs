using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000697 RID: 1687
	[NullableContext(1)]
	[Nullable(0)]
	public class HotfixPower : TemporaryFocusPower
	{
		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x060058FC RID: 22780 RVA: 0x0024FA5F File Offset: 0x0024DC5F
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<Hotfix>();
			}
		}
	}
}
