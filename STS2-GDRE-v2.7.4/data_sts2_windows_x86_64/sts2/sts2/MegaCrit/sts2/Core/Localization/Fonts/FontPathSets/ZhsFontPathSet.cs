using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets
{
	// Token: 0x02000BD6 RID: 3030
	[NullableContext(1)]
	[Nullable(0)]
	public class ZhsFontPathSet : FontPathSet
	{
		// Token: 0x06007DE0 RID: 32224 RVA: 0x002AD10C File Offset: 0x002AB30C
		public override string GetPath(FontType type)
		{
			string text;
			switch (type)
			{
			case FontType.Regular:
				text = "res://themes/fonts/zhs/noto_sans_mono_cjksc_regular_shared.tres";
				break;
			case FontType.Bold:
				text = "res://themes/fonts/zhs/source_han_serif_sc_bold_shared.tres";
				break;
			case FontType.Italic:
				text = "res://themes/fonts/zhs/source_han_serif_sc_medium_shared.tres";
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			return text;
		}

		// Token: 0x04002A18 RID: 10776
		private const string _regular = "res://themes/fonts/zhs/noto_sans_mono_cjksc_regular_shared.tres";

		// Token: 0x04002A19 RID: 10777
		private const string _bold = "res://themes/fonts/zhs/source_han_serif_sc_bold_shared.tres";

		// Token: 0x04002A1A RID: 10778
		private const string _italic = "res://themes/fonts/zhs/source_han_serif_sc_medium_shared.tres";
	}
}
