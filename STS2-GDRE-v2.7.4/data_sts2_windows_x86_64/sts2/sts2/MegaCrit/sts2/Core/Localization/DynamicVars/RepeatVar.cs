using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BEE RID: 3054
	[NullableContext(1)]
	[Nullable(0)]
	public class RepeatVar : DynamicVar
	{
		// Token: 0x06007E75 RID: 32373 RVA: 0x002AE2CE File Offset: 0x002AC4CE
		public RepeatVar(int times)
			: base("Repeat", times)
		{
		}

		// Token: 0x06007E76 RID: 32374 RVA: 0x002AE2E1 File Offset: 0x002AC4E1
		public RepeatVar(string name, int times)
			: base(name, times)
		{
		}

		// Token: 0x04002A3F RID: 10815
		public const string defaultName = "Repeat";
	}
}
