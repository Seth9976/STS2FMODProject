using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BEF RID: 3055
	[NullableContext(1)]
	[Nullable(0)]
	public class StarsVar : DynamicVar
	{
		// Token: 0x06007E77 RID: 32375 RVA: 0x002AE2F0 File Offset: 0x002AC4F0
		public StarsVar(int stars)
			: this("Stars", stars)
		{
		}

		// Token: 0x06007E78 RID: 32376 RVA: 0x002AE2FE File Offset: 0x002AC4FE
		public StarsVar(string name, int stars)
			: base(name, stars)
		{
		}

		// Token: 0x04002A40 RID: 10816
		public const string defaultName = "Stars";
	}
}
