using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;

namespace MegaCrit.Sts2.Core.Localization.Formatters
{
	// Token: 0x02000BCB RID: 3019
	[NullableContext(1)]
	[Nullable(0)]
	public class PercentMoreFormatter : IFormatter
	{
		// Token: 0x170022BC RID: 8892
		// (get) Token: 0x06007DBE RID: 32190 RVA: 0x002ACAB4 File Offset: 0x002AACB4
		// (set) Token: 0x06007DBF RID: 32191 RVA: 0x002ACABB File Offset: 0x002AACBB
		public string Name
		{
			get
			{
				return "percentMore";
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170022BD RID: 8893
		// (get) Token: 0x06007DC0 RID: 32192 RVA: 0x002ACAC2 File Offset: 0x002AACC2
		// (set) Token: 0x06007DC1 RID: 32193 RVA: 0x002ACACA File Offset: 0x002AACCA
		public bool CanAutoDetect { get; set; }

		// Token: 0x06007DC2 RID: 32194 RVA: 0x002ACAD4 File Offset: 0x002AACD4
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
			formattingInfo.Write(Convert.ToInt32((num - 1m) * 100m).ToString(CultureInfo.InvariantCulture));
			return true;
		}
	}
}
