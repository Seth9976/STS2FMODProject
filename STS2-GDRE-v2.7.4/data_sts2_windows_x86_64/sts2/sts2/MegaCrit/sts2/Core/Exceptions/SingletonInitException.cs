using System;

namespace MegaCrit.Sts2.Core.Exceptions
{
	// Token: 0x02000C4A RID: 3146
	public class SingletonInitException : Exception
	{
		// Token: 0x06008199 RID: 33177 RVA: 0x002BF28C File Offset: 0x002BD48C
		public SingletonInitException()
			: base("The singleton was used before initialization.")
		{
		}
	}
}
