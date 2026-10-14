using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using SmartFormat.Core.Extensions;

namespace MegaCrit.Sts2.Core.Localization.Formatters
{
	// Token: 0x02000BC5 RID: 3013
	[NullableContext(1)]
	[Nullable(0)]
	public class AbsoluteValueFormatter : IFormatter
	{
		// Token: 0x170022AE RID: 8878
		// (get) Token: 0x06007D98 RID: 32152 RVA: 0x002AC52A File Offset: 0x002AA72A
		// (set) Token: 0x06007D99 RID: 32153 RVA: 0x002AC531 File Offset: 0x002AA731
		public string Name
		{
			get
			{
				return "abs";
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170022AF RID: 8879
		// (get) Token: 0x06007D9A RID: 32154 RVA: 0x002AC538 File Offset: 0x002AA738
		// (set) Token: 0x06007D9B RID: 32155 RVA: 0x002AC540 File Offset: 0x002AA740
		public bool CanAutoDetect { get; set; }

		// Token: 0x06007D9C RID: 32156 RVA: 0x002AC54C File Offset: 0x002AA74C
		public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			object currentValue = formattingInfo.CurrentValue;
			string text;
			if (currentValue is decimal)
			{
				decimal num = (decimal)currentValue;
				text = Math.Abs(num).ToString(AbsoluteValueFormatter.Culture);
			}
			else if (currentValue is double)
			{
				double num2 = (double)currentValue;
				text = Math.Abs(num2).ToString(AbsoluteValueFormatter.Culture);
			}
			else if (currentValue is float)
			{
				float num3 = (float)currentValue;
				text = Math.Abs(num3).ToString(AbsoluteValueFormatter.Culture);
			}
			else if (currentValue is int)
			{
				int num4 = (int)currentValue;
				text = Math.Abs(num4).ToString();
			}
			else if (currentValue is long)
			{
				long num5 = (long)currentValue;
				text = Math.Abs(num5).ToString();
			}
			else if (currentValue is short)
			{
				short num6 = (short)currentValue;
				text = Math.Abs(num6).ToString();
			}
			else
			{
				text = null;
			}
			string text2 = text;
			if (text2 == null)
			{
				return false;
			}
			formattingInfo.Write(text2);
			return true;
		}

		// Token: 0x170022B0 RID: 8880
		// (get) Token: 0x06007D9D RID: 32157 RVA: 0x002AC670 File Offset: 0x002AA870
		private static CultureInfo Culture
		{
			get
			{
				return LocManager.Instance.CultureInfo;
			}
		}
	}
}
