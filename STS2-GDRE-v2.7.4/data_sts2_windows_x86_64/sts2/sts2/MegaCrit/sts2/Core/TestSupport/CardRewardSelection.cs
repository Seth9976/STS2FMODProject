using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.TestSupport
{
	// Token: 0x02000082 RID: 130
	[NullableContext(2)]
	[Nullable(0)]
	public struct CardRewardSelection
	{
		// Token: 0x0400015A RID: 346
		public CardModel card;

		// Token: 0x0400015B RID: 347
		public CardRewardAlternative alternative;
	}
}
