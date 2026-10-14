using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BEB RID: 3051
	[NullableContext(1)]
	[Nullable(0)]
	public class MaxHpVar : DynamicVar
	{
		// Token: 0x06007E6B RID: 32363 RVA: 0x002AE16C File Offset: 0x002AC36C
		public MaxHpVar(decimal maxHp)
			: base("MaxHp", maxHp)
		{
		}

		// Token: 0x06007E6C RID: 32364 RVA: 0x002AE17A File Offset: 0x002AC37A
		public MaxHpVar(string name, decimal maxHp)
			: base(name, maxHp)
		{
		}

		// Token: 0x04002A3C RID: 10812
		public const string defaultName = "MaxHp";
	}
}
