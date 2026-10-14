using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BEA RID: 3050
	public class IntVar : DynamicVar
	{
		// Token: 0x06007E6A RID: 32362 RVA: 0x002AE162 File Offset: 0x002AC362
		[NullableContext(1)]
		public IntVar(string name, decimal amount)
			: base(name, amount)
		{
		}
	}
}
