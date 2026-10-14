using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;

namespace MegaCrit.Sts2.Core.Localization.Formatters
{
	// Token: 0x02000BCA RID: 3018
	[NullableContext(1)]
	[Nullable(0)]
	public class PercentLessFormatter : IFormatter
	{
		// Token: 0x170022BA RID: 8890
		// (get) Token: 0x06007DB8 RID: 32184 RVA: 0x002AC9F9 File Offset: 0x002AABF9
		// (set) Token: 0x06007DB9 RID: 32185 RVA: 0x002ACA00 File Offset: 0x002AAC00
		public string Name
		{
			get
			{
				return "percentLess";
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170022BB RID: 8891
		// (get) Token: 0x06007DBA RID: 32186 RVA: 0x002ACA07 File Offset: 0x002AAC07
		// (set) Token: 0x06007DBB RID: 32187 RVA: 0x002ACA0F File Offset: 0x002AAC0F
		public bool CanAutoDetect { get; set; }

		// Token: 0x06007DBC RID: 32188 RVA: 0x002ACA18 File Offset: 0x002AAC18
		public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			object currentValue = formattingInfo.CurrentValue;
			DynamicVar dynamicVar = currentValue as DynamicVar;
			decimal num;
			if (dynamicVar != null)
			{
				num = dynamicVar.BaseValue;
			}
			else
			{
				try
				{
					num = Convert.ToDecimal(formattingInfo.CurrentValue);
				}
				catch (FormatException)
				{
					return false;
				}
				catch (InvalidCastException)
				{
					return false;
				}
			}
			formattingInfo.Write(Convert.ToInt32((1m - num) * 100m).ToString(CultureInfo.InvariantCulture));
			return true;
		}
	}
}
