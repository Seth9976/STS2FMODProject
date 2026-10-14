using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000670 RID: 1648
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FadePower : TemporaryDexterityPower
	{
		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x0600582A RID: 22570 RVA: 0x0024E2CF File Offset: 0x0024C4CF
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<Fade>();
			}
		}
	}
}
