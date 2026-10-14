using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.DevConsole
{
	// Token: 0x02000CDC RID: 3292
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	public class FixedSizedQueue<[Nullable(2)] T> : List<T>
	{
		// Token: 0x06008560 RID: 34144 RVA: 0x002CCE48 File Offset: 0x002CB048
		public FixedSizedQueue(int limit)
		{
			this._limit = limit;
		}

		// Token: 0x06008561 RID: 34145 RVA: 0x002CCE57 File Offset: 0x002CB057
		public void Enqueue(T obj)
		{
			if (base.Count >= this._limit)
			{
				base.RemoveAt(base.Count - 1);
			}
			base.Insert(0, obj);
		}

		// Token: 0x04002E5C RID: 11868
		private readonly int _limit;
	}
}
