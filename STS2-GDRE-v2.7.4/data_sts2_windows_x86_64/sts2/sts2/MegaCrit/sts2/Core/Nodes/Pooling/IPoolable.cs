using System;

namespace MegaCrit.Sts2.Core.Nodes.Pooling
{
	// Token: 0x02000384 RID: 900
	public interface IPoolable
	{
		/// <summary>
		/// Called the first time an object is instantiated for use in a pool.
		/// </summary>
		// Token: 0x06003593 RID: 13715
		void OnInstantiated();

		/// <summary>
		/// Called when a pooled object is retrieved from a pool.
		/// </summary>
		// Token: 0x06003594 RID: 13716
		void OnReturnedFromPool();

		/// <summary>
		/// Called when a pooled object is freed and put back into a pool for reuse later.
		/// </summary>
		// Token: 0x06003595 RID: 13717
		void OnFreedToPool();
	}
}
