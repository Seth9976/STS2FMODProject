using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BE3 RID: 3043
	[NullableContext(1)]
	[Nullable(0)]
	public class EnergyVar : DynamicVar
	{
		// Token: 0x170022EA RID: 8938
		// (get) Token: 0x06007E57 RID: 32343 RVA: 0x002AE029 File Offset: 0x002AC229
		// (set) Token: 0x06007E58 RID: 32344 RVA: 0x002AE031 File Offset: 0x002AC231
		public string ColorPrefix { get; set; } = string.Empty;

		// Token: 0x06007E59 RID: 32345 RVA: 0x002AE03A File Offset: 0x002AC23A
		public EnergyVar(int energy)
			: this("Energy", energy)
		{
		}

		// Token: 0x06007E5A RID: 32346 RVA: 0x002AE048 File Offset: 0x002AC248
		public EnergyVar(string name, int energy)
			: base(name, energy)
		{
		}

		// Token: 0x04002A32 RID: 10802
		public const string defaultName = "Energy";
	}
}
