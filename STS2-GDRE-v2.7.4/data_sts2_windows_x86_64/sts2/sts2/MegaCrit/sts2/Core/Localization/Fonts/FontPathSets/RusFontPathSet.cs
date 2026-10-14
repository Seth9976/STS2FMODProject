using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets
{
	// Token: 0x02000BD4 RID: 3028
	[NullableContext(1)]
	[Nullable(0)]
	public class RusFontPathSet : FontPathSet
	{
		// Token: 0x06007DDC RID: 32220 RVA: 0x002AD064 File Offset: 0x002AB264
		public override string GetPath(FontType type)
		{
			string text;
			switch (type)
			{
			case FontType.Regular:
				text = "res://themes/fonts/rus/fira_sans_extra_condensed_regular_shared.tres";
				break;
			case FontType.Bold:
				text = "res://themes/fonts/rus/fira_sans_extra_condensed_bold_shared.tres";
				break;
			case FontType.Italic:
				text = "res://themes/fonts/rus/fira_sans_extra_condensed_italic_shared.tres";
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			return text;
		}

		// Token: 0x04002A14 RID: 10772
		private const string _regular = "res://themes/fonts/rus/fira_sans_extra_condensed_regular_shared.tres";

		// Token: 0x04002A15 RID: 10773
		private const string _bold = "res://themes/fonts/rus/fira_sans_extra_condensed_bold_shared.tres";

		// Token: 0x04002A16 RID: 10774
		private const string _italic = "res://themes/fonts/rus/fira_sans_extra_condensed_italic_shared.tres";
	}
}
