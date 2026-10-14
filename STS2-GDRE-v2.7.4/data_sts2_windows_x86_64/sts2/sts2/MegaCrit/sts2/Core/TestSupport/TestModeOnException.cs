using System;

namespace MegaCrit.Sts2.Core.TestSupport
{
	// Token: 0x02000086 RID: 134
	public class TestModeOnException : Exception
	{
		// Token: 0x0600046A RID: 1130 RVA: 0x00015C65 File Offset: 0x00013E65
		public TestModeOnException()
			: base("Never call this in test mode.")
		{
		}
	}
}
