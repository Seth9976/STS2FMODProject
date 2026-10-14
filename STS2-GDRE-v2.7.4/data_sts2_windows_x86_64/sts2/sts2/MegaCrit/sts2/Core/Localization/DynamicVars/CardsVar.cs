using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BDF RID: 3039
	[NullableContext(1)]
	[Nullable(0)]
	public class CardsVar : DynamicVar
	{
		// Token: 0x06007E03 RID: 32259 RVA: 0x002AD6C4 File Offset: 0x002AB8C4
		public CardsVar(int cards)
			: base("Cards", cards)
		{
		}

		// Token: 0x06007E04 RID: 32260 RVA: 0x002AD6D7 File Offset: 0x002AB8D7
		public CardsVar(string name, int cards)
			: base(name, cards)
		{
		}

		// Token: 0x04002A28 RID: 10792
		public const string defaultName = "Cards";
	}
}
