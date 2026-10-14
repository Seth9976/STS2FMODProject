using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets
{
	// Token: 0x02000BD2 RID: 3026
	[NullableContext(1)]
	[Nullable(0)]
	public class JpnFontPathSet : FontPathSet
	{
		// Token: 0x06007DD8 RID: 32216 RVA: 0x002ACFBC File Offset: 0x002AB1BC
		public override string GetPath(FontType type)
		{
			string text;
			switch (type)
			{
			case FontType.Regular:
				text = "res://themes/fonts/jpn/noto_sans_cjkjp_regular_shared.tres";
				break;
			case FontType.Bold:
				text = "res://themes/fonts/jpn/noto_sans_cjkjp_bold_shared.tres";
				break;
			case FontType.Italic:
				text = "res://themes/fonts/jpn/noto_sans_cjkjp_medium_shared.tres";
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			return text;
		}

		// Token: 0x04002A10 RID: 10768
		private const string _regular = "res://themes/fonts/jpn/noto_sans_cjkjp_regular_shared.tres";

		// Token: 0x04002A11 RID: 10769
		private const string _bold = "res://themes/fonts/jpn/noto_sans_cjkjp_bold_shared.tres";

		// Token: 0x04002A12 RID: 10770
		private const string _italic = "res://themes/fonts/jpn/noto_sans_cjkjp_medium_shared.tres";
	}
}
