using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;

namespace MegaCrit.Sts2.Core.Localization.Formatters
{
	// Token: 0x02000BC8 RID: 3016
	[NullableContext(1)]
	[Nullable(0)]
	public class HighlightDifferencesInverseFormatter : IFormatter
	{
		// Token: 0x170022B5 RID: 8885
		// (get) Token: 0x06007DAB RID: 32171 RVA: 0x002AC8C8 File Offset: 0x002AAAC8
		// (set) Token: 0x06007DAC RID: 32172 RVA: 0x002AC8CF File Offset: 0x002AAACF
		public string Name
		{
			get
			{
				return "inverseDiff";
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170022B6 RID: 8886
		// (get) Token: 0x06007DAD RID: 32173 RVA: 0x002AC8D6 File Offset: 0x002AAAD6
		// (set) Token: 0x06007DAE RID: 32174 RVA: 0x002AC8DE File Offset: 0x002AAADE
		public bool CanAutoDetect { get; set; }

		// Token: 0x06007DAF RID: 32175 RVA: 0x002AC8E8 File Offset: 0x002AAAE8
		public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			DynamicVar dynamicVar = formattingInfo.CurrentValue as DynamicVar;
			if (dynamicVar == null)
			{
				return false;
			}
			formattingInfo.Write(dynamicVar.ToHighlightedString(true));
			return true;
		}
	}
}
