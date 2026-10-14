using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets
{
	// Token: 0x02000BD5 RID: 3029
	[NullableContext(1)]
	[Nullable(0)]
	public class ThaFontPathSet : FontPathSet
	{
		// Token: 0x06007DDE RID: 32222 RVA: 0x002AD0B8 File Offset: 0x002AB2B8
		public override string GetPath(FontType type)
		{
			string text;
			switch (type)
			{
			case FontType.Regular:
				text = "res://themes/fonts/tha/cs_chat_thai_ui_shared.tres";
				break;
			case FontType.Bold:
				text = "res://themes/fonts/tha/cs_chat_thai_ui_shared.tres";
				break;
			case FontType.Italic:
				text = "res://themes/fonts/tha/cs_chat_thai_ui_shared.tres";
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			return text;
		}

		// Token: 0x04002A17 RID: 10775
		private const string _regular = "res://themes/fonts/tha/cs_chat_thai_ui_shared.tres";
	}
}
