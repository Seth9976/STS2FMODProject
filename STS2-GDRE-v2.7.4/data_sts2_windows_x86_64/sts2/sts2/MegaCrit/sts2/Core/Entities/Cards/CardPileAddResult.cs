using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CB1 RID: 3249
	public struct CardPileAddResult
	{
		// Token: 0x04002D99 RID: 11673
		public bool success;

		// Token: 0x04002D9A RID: 11674
		[Nullable(1)]
		public CardModel cardAdded;

		// Token: 0x04002D9B RID: 11675
		[Nullable(2)]
		public CardPile oldPile;

		// Token: 0x04002D9C RID: 11676
		[Nullable(new byte[] { 2, 1 })]
		public List<AbstractModel> modifyingModels;
	}
}
