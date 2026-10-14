using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004E2 RID: 1250
	[NullableContext(1)]
	public interface IPoolModel
	{
		/// <summary>
		/// The name of the energy color that models in this pool should use.
		/// </summary>
		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06004CD9 RID: 19673
		string EnergyColorName { get; }
	}
}
