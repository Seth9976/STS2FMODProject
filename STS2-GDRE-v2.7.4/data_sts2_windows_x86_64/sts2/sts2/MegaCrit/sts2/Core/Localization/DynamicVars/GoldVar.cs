using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BE6 RID: 3046
	[NullableContext(1)]
	[Nullable(0)]
	public class GoldVar : DynamicVar
	{
		// Token: 0x06007E62 RID: 32354 RVA: 0x002AE0EC File Offset: 0x002AC2EC
		public GoldVar(int gold)
			: base("Gold", gold)
		{
		}

		// Token: 0x06007E63 RID: 32355 RVA: 0x002AE0FF File Offset: 0x002AC2FF
		public GoldVar(string name, int gold)
			: base(name, gold)
		{
		}

		// Token: 0x04002A37 RID: 10807
		public const string defaultName = "Gold";
	}
}
