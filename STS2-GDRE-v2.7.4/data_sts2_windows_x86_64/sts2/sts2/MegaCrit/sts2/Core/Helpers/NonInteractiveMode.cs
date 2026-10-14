using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C0E RID: 3086
	[NullableContext(1)]
	[Nullable(0)]
	public static class NonInteractiveMode
	{
		// Token: 0x17002306 RID: 8966
		// (get) Token: 0x06007FC9 RID: 32713 RVA: 0x002B646A File Offset: 0x002B466A
		// (set) Token: 0x06007FCA RID: 32714 RVA: 0x002B6471 File Offset: 0x002B4671
		public static Func<bool> AutoSlayerCheck { get; set; } = () => false;

		// Token: 0x17002307 RID: 8967
		// (get) Token: 0x06007FCB RID: 32715 RVA: 0x002B6479 File Offset: 0x002B4679
		public static bool IsActive
		{
			get
			{
				return TestMode.IsOn || NonInteractiveMode.AutoSlayerCheck();
			}
		}
	}
}
