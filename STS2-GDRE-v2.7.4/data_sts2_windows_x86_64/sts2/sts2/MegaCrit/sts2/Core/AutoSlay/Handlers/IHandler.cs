using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers
{
	/// <summary>
	/// Base interface for all AutoSlay handlers.
	/// </summary>
	// Token: 0x02000D72 RID: 3442
	[NullableContext(1)]
	public interface IHandler
	{
		/// <summary>Timeout for this handler.</summary>
		// Token: 0x1700262B RID: 9771
		// (get) Token: 0x06008B0A RID: 35594
		TimeSpan Timeout { get; }

		/// <summary>Executes the handler logic.</summary>
		// Token: 0x06008B0B RID: 35595
		Task HandleAsync(Rng random, CancellationToken ct);
	}
}
