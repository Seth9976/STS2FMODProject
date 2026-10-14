using System;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C07 RID: 3079
	[NullableContext(1)]
	[Nullable(0)]
	public static class GlyphConverter
	{
		// Token: 0x06007FA5 RID: 32677 RVA: 0x002B52DA File Offset: 0x002B34DA
		private static TextServer GetTextServer()
		{
			return TextServerManager.Singleton.GetPrimaryInterface();
		}

		// Token: 0x06007FA6 RID: 32678 RVA: 0x002B52E6 File Offset: 0x002B34E6
		public static uint CharToGlyphIdx(Rid font, char c)
		{
			return (uint)GlyphConverter.GetTextServer().FontGetGlyphIndex(font, 1L, (long)((ulong)c), 0L);
		}

		// Token: 0x06007FA7 RID: 32679 RVA: 0x002B52FA File Offset: 0x002B34FA
		public static char GlyphIdxToChar(CharFXTransform charFx)
		{
			return (char)GlyphConverter.GetTextServer().FontGetCharFromGlyphIndex(charFx.Font, 1L, (long)((ulong)charFx.GlyphIndex));
		}
	}
}
