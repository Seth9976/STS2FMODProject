using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CAC RID: 3244
	[NullableContext(1)]
	[Nullable(0)]
	internal static class CardKeywordExtensions
	{
		// Token: 0x06008493 RID: 33939 RVA: 0x002CA302 File Offset: 0x002C8502
		public static string GetLocKeyPrefix(this CardKeyword keyword)
		{
			return StringHelper.Slugify(keyword.ToString());
		}

		// Token: 0x06008494 RID: 33940 RVA: 0x002CA316 File Offset: 0x002C8516
		public static LocString GetTitle(this CardKeyword keyword)
		{
			return new LocString("card_keywords", keyword.GetLocKeyPrefix() + ".title");
		}

		// Token: 0x06008495 RID: 33941 RVA: 0x002CA332 File Offset: 0x002C8532
		public static LocString GetDescription(this CardKeyword keyword)
		{
			return new LocString("card_keywords", keyword.GetLocKeyPrefix() + ".description");
		}

		// Token: 0x06008496 RID: 33942 RVA: 0x002CA34E File Offset: 0x002C854E
		public static string GetCardText(this CardKeyword keyword)
		{
			return "[gold]" + keyword.GetTitle().GetFormattedText() + "[/gold]" + CardKeywordExtensions._period.GetRawText();
		}

		// Token: 0x04002D88 RID: 11656
		private static readonly LocString _period = new LocString("card_keywords", "PERIOD");
	}
}
