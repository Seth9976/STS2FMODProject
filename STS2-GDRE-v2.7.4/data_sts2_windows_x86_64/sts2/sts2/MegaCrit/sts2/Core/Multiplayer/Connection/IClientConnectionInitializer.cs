using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;

namespace MegaCrit.Sts2.Core.Multiplayer.Connection
{
	// Token: 0x020004BB RID: 1211
	[NullableContext(1)]
	public interface IClientConnectionInitializer
	{
		// Token: 0x060048CA RID: 18634
		Task<NetErrorInfo?> Connect(INetClientGameService netService, CancellationToken cancelToken = default(CancellationToken));
	}
}
