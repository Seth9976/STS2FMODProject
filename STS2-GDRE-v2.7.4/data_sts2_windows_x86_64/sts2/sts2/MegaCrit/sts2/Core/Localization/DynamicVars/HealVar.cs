using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BE7 RID: 3047
	[NullableContext(1)]
	[Nullable(0)]
	public class HealVar : DynamicVar
	{
		// Token: 0x06007E64 RID: 32356 RVA: 0x002AE10E File Offset: 0x002AC30E
		public HealVar(decimal healAmount)
			: base("Heal", healAmount)
		{
		}

		// Token: 0x06007E65 RID: 32357 RVA: 0x002AE11C File Offset: 0x002AC31C
		public HealVar(string name, decimal healAmount)
			: base(name, healAmount)
		{
		}

		// Token: 0x04002A38 RID: 10808
		public const string defaultName = "Heal";
	}
}
