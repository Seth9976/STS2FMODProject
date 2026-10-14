using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.AutoSlay
{
	/// <summary>
	/// Exception thrown when AutoSlay times out waiting for a condition or progress.
	/// Filtered from Sentry since these are expected during automated testing.
	/// </summary>
	// Token: 0x02000D6C RID: 3436
	public class AutoSlayTimeoutException : TimeoutException
	{
		// Token: 0x06008AF0 RID: 35568 RVA: 0x002E60CF File Offset: 0x002E42CF
		[NullableContext(1)]
		public AutoSlayTimeoutException(string message)
			: base(message)
		{
		}
	}
}
