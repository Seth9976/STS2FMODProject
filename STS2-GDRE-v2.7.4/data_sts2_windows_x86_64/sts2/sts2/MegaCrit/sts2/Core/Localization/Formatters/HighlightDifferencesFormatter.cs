using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;

namespace MegaCrit.Sts2.Core.Localization.Formatters
{
	// Token: 0x02000BC7 RID: 3015
	[NullableContext(1)]
	[Nullable(0)]
	public class HighlightDifferencesFormatter : IFormatter
	{
		// Token: 0x170022B3 RID: 8883
		// (get) Token: 0x06007DA5 RID: 32165 RVA: 0x002AC873 File Offset: 0x002AAA73
		// (set) Token: 0x06007DA6 RID: 32166 RVA: 0x002AC87A File Offset: 0x002AAA7A
		public string Name
		{
			get
			{
				return "diff";
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170022B4 RID: 8884
		// (get) Token: 0x06007DA7 RID: 32167 RVA: 0x002AC881 File Offset: 0x002AAA81
		// (set) Token: 0x06007DA8 RID: 32168 RVA: 0x002AC889 File Offset: 0x002AAA89
		public bool CanAutoDetect { get; set; }

		// Token: 0x06007DA9 RID: 32169 RVA: 0x002AC894 File Offset: 0x002AAA94
		public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			DynamicVar dynamicVar = formattingInfo.CurrentValue as DynamicVar;
			if (dynamicVar == null)
			{
				return false;
			}
			formattingInfo.Write(dynamicVar.ToHighlightedString(false));
			return true;
		}
	}
}
