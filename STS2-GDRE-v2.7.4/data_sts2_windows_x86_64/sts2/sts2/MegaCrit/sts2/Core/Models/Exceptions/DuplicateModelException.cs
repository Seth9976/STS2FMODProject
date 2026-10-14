using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Models.Exceptions
{
	// Token: 0x02000825 RID: 2085
	public class DuplicateModelException : Exception
	{
		// Token: 0x0600675C RID: 26460 RVA: 0x00272628 File Offset: 0x00270828
		[NullableContext(1)]
		public DuplicateModelException(string message)
			: base(message)
		{
		}
	}
}
