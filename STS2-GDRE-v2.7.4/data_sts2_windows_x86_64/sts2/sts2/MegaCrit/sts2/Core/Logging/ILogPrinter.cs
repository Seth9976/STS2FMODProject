using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Logging
{
	// Token: 0x02000BB1 RID: 2993
	[NullableContext(1)]
	public interface ILogPrinter
	{
		// Token: 0x06007CCF RID: 31951
		void Print(LogLevel logLevel, string text, int skipFrames);
	}
}
