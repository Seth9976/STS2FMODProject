using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets
{
	// Token: 0x02000BD7 RID: 3031
	[NullableContext(1)]
	[Nullable(0)]
	public class ZhtFontPathSet : FontPathSet
	{
		// Token: 0x06007DE2 RID: 32226 RVA: 0x002AD160 File Offset: 0x002AB360
		public override string GetPath(FontType type)
		{
			string text;
			switch (type)
			{
			case FontType.Regular:
				text = "res://themes/fonts/zht/noto_sans_mono_cjktc_regular_shared.tres";
				break;
			case FontType.Bold:
				text = "res://themes/fonts/zht/source_han_serif_tc_bold_shared.tres";
				break;
			case FontType.Italic:
				text = "res://themes/fonts/zht/source_han_serif_tc_medium_shared.tres";
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			return text;
		}

		// Token: 0x04002A1B RID: 10779
		private const string _regular = "res://themes/fonts/zht/noto_sans_mono_cjktc_regular_shared.tres";

		// Token: 0x04002A1C RID: 10780
		private const string _bold = "res://themes/fonts/zht/source_han_serif_tc_bold_shared.tres";

		// Token: 0x04002A1D RID: 10781
		private const string _italic = "res://themes/fonts/zht/source_han_serif_tc_medium_shared.tres";
	}
}
