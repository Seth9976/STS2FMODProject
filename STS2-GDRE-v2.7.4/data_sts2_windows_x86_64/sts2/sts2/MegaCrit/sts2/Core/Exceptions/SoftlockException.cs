using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Exceptions
{
	// Token: 0x02000C4B RID: 3147
	public class SoftlockException : Exception
	{
		// Token: 0x0600819A RID: 33178 RVA: 0x002BF299 File Offset: 0x002BD499
		[NullableContext(1)]
		public SoftlockException(string message)
			: base(message)
		{
		}
	}
}
