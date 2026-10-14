using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Exceptions
{
	// Token: 0x02000C49 RID: 3145
	public class SaveException : Exception
	{
		// Token: 0x06008198 RID: 33176 RVA: 0x002BF283 File Offset: 0x002BD483
		[NullableContext(1)]
		public SaveException(string message)
			: base(message)
		{
		}
	}
}
