using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace MegaCrit.Sts2.Core.TextEffects
{
	// Token: 0x02000080 RID: 128
	[NullableContext(1)]
	[Nullable(0)]
	public static class StsTextUtilities
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x0001596C File Offset: 0x00013B6C
		public static string HighlightChangeText(string text, int baseComparison)
		{
			StringBuilder stringBuilder = new StringBuilder(text);
			if (baseComparison == 0)
			{
				return stringBuilder.ToString();
			}
			string text2 = ((baseComparison > 0) ? "green" : "red");
			stringBuilder.Insert(0, "[" + text2 + "]");
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(3, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("[/");
			appendInterpolatedStringHandler.AppendFormatted(text2);
			appendInterpolatedStringHandler.AppendLiteral("]");
			stringBuilder3.Append(ref appendInterpolatedStringHandler);
			return stringBuilder.ToString();
		}

		// Token: 0x04000156 RID: 342
		private const string _creamColorCode = "#FFF6E2";

		// Token: 0x04000157 RID: 343
		private const string _buffPopColorCode = "#77ff67";

		// Token: 0x04000158 RID: 344
		private const string _debuffPopColorCode = "#ff6563";

		// Token: 0x04000159 RID: 345
		private const string _normalPopColor = "yellow";
	}
}
