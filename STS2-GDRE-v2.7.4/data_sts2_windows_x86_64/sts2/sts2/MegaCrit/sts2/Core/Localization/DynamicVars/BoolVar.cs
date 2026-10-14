using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BD9 RID: 3033
	[NullableContext(1)]
	[Nullable(0)]
	public class BoolVar : DynamicVar
	{
		// Token: 0x170022C3 RID: 8899
		// (get) Token: 0x06007DE8 RID: 32232 RVA: 0x002AD25F File Offset: 0x002AB45F
		// (set) Token: 0x06007DE9 RID: 32233 RVA: 0x002AD26C File Offset: 0x002AB46C
		public bool BoolVal
		{
			get
			{
				return Convert.ToBoolean(base.BaseValue);
			}
			set
			{
				base.BaseValue = Convert.ToDecimal(value);
			}
		}

		// Token: 0x06007DEA RID: 32234 RVA: 0x002AD27A File Offset: 0x002AB47A
		public BoolVar(string name)
			: base(name, 0m)
		{
		}

		// Token: 0x06007DEB RID: 32235 RVA: 0x002AD288 File Offset: 0x002AB488
		public BoolVar(string name, bool value)
			: base(name, Convert.ToDecimal(value))
		{
		}
	}
}
