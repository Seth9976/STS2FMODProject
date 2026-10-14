using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization
{
	// Token: 0x02000BB9 RID: 3001
	[NullableContext(1)]
	[Nullable(0)]
	public class LocException : Exception
	{
		// Token: 0x06007CF7 RID: 31991 RVA: 0x002A9102 File Offset: 0x002A7302
		public LocException(string message)
			: base(message)
		{
		}

		// Token: 0x06007CF8 RID: 31992 RVA: 0x002A910B File Offset: 0x002A730B
		public LocException(string message, Exception e)
			: base(message, e)
		{
		}
	}
}
