using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CAD RID: 3245
	[NullableContext(1)]
	[Nullable(0)]
	public static class CardKeywordOrder
	{
		/// <summary>
		/// All the keywords that should be added before the card's description.
		/// Order matters here. If a card has multiple keywords, they will appear in this order.
		/// </summary>
		// Token: 0x04002D89 RID: 11657
		public static readonly CardKeyword[] beforeDescription = new CardKeyword[]
		{
			CardKeyword.Ethereal,
			CardKeyword.Sly,
			CardKeyword.Retain,
			CardKeyword.Innate,
			CardKeyword.Unplayable
		};

		/// <summary>
		/// All the keywords that should be added after the card's description.
		/// Order matters here. If a card has multiple keywords, they will appear in this order.
		/// </summary>
		// Token: 0x04002D8A RID: 11658
		public static readonly CardKeyword[] afterDescription = new CardKeyword[]
		{
			CardKeyword.Exhaust,
			CardKeyword.Eternal
		};
	}
}
