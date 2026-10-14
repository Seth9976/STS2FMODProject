using System;

namespace MegaCrit.Sts2.Core.TestSupport
{
	// Token: 0x02000085 RID: 133
	public class TestModeOffException : Exception
	{
		// Token: 0x06000469 RID: 1129 RVA: 0x00015C58 File Offset: 0x00013E58
		public TestModeOffException()
			: base("Only call this in test mode.")
		{
		}
	}
}
