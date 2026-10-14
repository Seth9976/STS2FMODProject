using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using SmartFormat.Core.Extensions;
using SmartFormat.Core.Parsing;

namespace MegaCrit.Sts2.Core.Localization.Formatters
{
	/// <summary>
	/// Formats numeric types with locale-aware thousand separators (N0) by default.
	/// Named formatters (diff, energyIcons, etc.) take priority over this auto-detect formatter.
	/// </summary>
	// Token: 0x02000BC9 RID: 3017
	[NullableContext(1)]
	[Nullable(0)]
	public class LocaleNumberFormatter : IFormatter
	{
		// Token: 0x170022B7 RID: 8887
		// (get) Token: 0x06007DB1 RID: 32177 RVA: 0x002AC91C File Offset: 0x002AAB1C
		// (set) Token: 0x06007DB2 RID: 32178 RVA: 0x002AC923 File Offset: 0x002AAB23
		public string Name
		{
			get
			{
				return "n";
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170022B8 RID: 8888
		// (get) Token: 0x06007DB3 RID: 32179 RVA: 0x002AC92A File Offset: 0x002AAB2A
		// (set) Token: 0x06007DB4 RID: 32180 RVA: 0x002AC932 File Offset: 0x002AAB32
		public bool CanAutoDetect { get; set; } = true;

		// Token: 0x06007DB5 RID: 32181 RVA: 0x002AC93C File Offset: 0x002AAB3C
		public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			object currentValue = formattingInfo.CurrentValue;
			if (!(currentValue is int) && !(currentValue is uint) && !(currentValue is long) && !(currentValue is ulong) && !(currentValue is decimal) && !(currentValue is float) && !(currentValue is double))
			{
				return false;
			}
			Format format = formattingInfo.Format;
			string text = (string.IsNullOrEmpty((format != null) ? format.RawText : null) ? "N0" : formattingInfo.Format.RawText);
			string text2 = ((IFormattable)formattingInfo.CurrentValue).ToString(text, LocaleNumberFormatter.Culture);
			formattingInfo.Write(text2);
			return true;
		}

		// Token: 0x170022B9 RID: 8889
		// (get) Token: 0x06007DB6 RID: 32182 RVA: 0x002AC9DE File Offset: 0x002AABDE
		private static CultureInfo Culture
		{
			get
			{
				return LocManager.Instance.CultureInfo;
			}
		}
	}
}
