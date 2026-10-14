using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets
{
	// Token: 0x02000BD3 RID: 3027
	[NullableContext(1)]
	[Nullable(0)]
	public class KorFontPathSet : FontPathSet
	{
		// Token: 0x06007DDA RID: 32218 RVA: 0x002AD010 File Offset: 0x002AB210
		public override string GetPath(FontType type)
		{
			string text;
			switch (type)
			{
			case FontType.Regular:
				text = "res://themes/fonts/kor/gyeonggi_cheonnyeon_batang_bold_shared.tres";
				break;
			case FontType.Bold:
				text = "res://themes/fonts/kor/gyeonggi_cheonnyeon_batang_bold_shared.tres";
				break;
			case FontType.Italic:
				text = "res://themes/fonts/kor/gyeonggi_cheonnyeon_batang_bold_shared.tres";
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			return text;
		}

		// Token: 0x04002A13 RID: 10771
		private const string _bold = "res://themes/fonts/kor/gyeonggi_cheonnyeon_batang_bold_shared.tres";
	}
}
