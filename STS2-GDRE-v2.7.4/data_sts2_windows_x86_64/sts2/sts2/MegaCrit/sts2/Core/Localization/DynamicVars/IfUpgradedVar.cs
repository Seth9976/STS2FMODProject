using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BE9 RID: 3049
	[NullableContext(1)]
	[Nullable(0)]
	public class IfUpgradedVar : DynamicVar
	{
		// Token: 0x06007E68 RID: 32360 RVA: 0x002AE13E File Offset: 0x002AC33E
		public IfUpgradedVar(UpgradeDisplay upgradeDisplay)
			: base("IfUpgraded", (int)upgradeDisplay)
		{
			this.upgradeDisplay = upgradeDisplay;
		}

		// Token: 0x06007E69 RID: 32361 RVA: 0x002AE158 File Offset: 0x002AC358
		public IfUpgradedVar(string name, decimal amount)
			: base(name, amount)
		{
		}

		// Token: 0x04002A3A RID: 10810
		public const string defaultName = "IfUpgraded";

		// Token: 0x04002A3B RID: 10811
		public UpgradeDisplay upgradeDisplay;
	}
}
