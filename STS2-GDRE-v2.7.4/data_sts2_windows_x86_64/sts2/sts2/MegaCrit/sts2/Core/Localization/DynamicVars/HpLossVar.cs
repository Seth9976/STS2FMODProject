using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BE8 RID: 3048
	[NullableContext(1)]
	[Nullable(0)]
	public class HpLossVar : DynamicVar
	{
		// Token: 0x06007E66 RID: 32358 RVA: 0x002AE126 File Offset: 0x002AC326
		public HpLossVar(decimal hpLoss)
			: base("HpLoss", hpLoss)
		{
		}

		// Token: 0x06007E67 RID: 32359 RVA: 0x002AE134 File Offset: 0x002AC334
		public HpLossVar(string name, decimal hpLoss)
			: base(name, hpLoss)
		{
		}

		// Token: 0x04002A39 RID: 10809
		public const string defaultName = "HpLoss";
	}
}
