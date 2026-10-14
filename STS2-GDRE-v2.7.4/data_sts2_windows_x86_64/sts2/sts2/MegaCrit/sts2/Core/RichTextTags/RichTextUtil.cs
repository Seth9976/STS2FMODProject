using System;
using Godot;

namespace MegaCrit.Sts2.Core.RichTextTags
{
	// Token: 0x02000143 RID: 323
	public static class RichTextUtil
	{
		// Token: 0x06000F82 RID: 3970 RVA: 0x0005F7E8 File Offset: 0x0005D9E8
		// Note: this type is marked as 'beforefieldinit'.
		static RichTextUtil()
		{
			string text = "color";
			RichTextUtil.colorKey = Variant.From<string>(ref text);
			text = "visible";
			RichTextUtil.visibleKey = Variant.From<string>(ref text);
		}

		// Token: 0x04000582 RID: 1410
		public static readonly Variant colorKey;

		// Token: 0x04000583 RID: 1411
		public static readonly Variant visibleKey;
	}
}
