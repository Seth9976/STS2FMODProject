using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Nodes.Pooling
{
	// Token: 0x02000386 RID: 902
	[NullableContext(1)]
	public interface INodePool
	{
		// Token: 0x0600359D RID: 13725
		IPoolable Get();

		// Token: 0x0600359E RID: 13726
		void Free(IPoolable poolable);
	}
}
