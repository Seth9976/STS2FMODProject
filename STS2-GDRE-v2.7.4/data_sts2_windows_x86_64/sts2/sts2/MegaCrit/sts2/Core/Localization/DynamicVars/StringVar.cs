using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BF0 RID: 3056
	[NullableContext(1)]
	[Nullable(0)]
	public class StringVar : DynamicVar
	{
		// Token: 0x170022ED RID: 8941
		// (get) Token: 0x06007E79 RID: 32377 RVA: 0x002AE30D File Offset: 0x002AC50D
		// (set) Token: 0x06007E7A RID: 32378 RVA: 0x002AE315 File Offset: 0x002AC515
		public string StringValue { get; set; }

		// Token: 0x06007E7B RID: 32379 RVA: 0x002AE31E File Offset: 0x002AC51E
		public StringVar(string name, string baseValue = "")
			: base(name, 0m)
		{
			this.StringValue = baseValue;
		}

		// Token: 0x06007E7C RID: 32380 RVA: 0x002AE333 File Offset: 0x002AC533
		public override string ToString()
		{
			return this.StringValue;
		}
	}
}
