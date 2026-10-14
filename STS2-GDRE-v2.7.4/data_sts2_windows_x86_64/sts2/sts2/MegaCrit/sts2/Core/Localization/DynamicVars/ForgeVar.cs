using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BE5 RID: 3045
	[NullableContext(1)]
	[Nullable(0)]
	public class ForgeVar : DynamicVar
	{
		// Token: 0x06007E60 RID: 32352 RVA: 0x002AE0CF File Offset: 0x002AC2CF
		public ForgeVar(int forge)
			: this("Forge", forge)
		{
		}

		// Token: 0x06007E61 RID: 32353 RVA: 0x002AE0DD File Offset: 0x002AC2DD
		public ForgeVar(string name, int forge)
			: base(name, forge)
		{
		}

		// Token: 0x04002A36 RID: 10806
		public const string defaultName = "Forge";
	}
}
