using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Provides additional client-related methods on top of the default game service.
	/// </summary>
	// Token: 0x02000497 RID: 1175
	public interface INetClientGameService : INetGameService, INetClientHandler, INetHandler
	{
		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06004743 RID: 18243
		[Nullable(2)]
		NetClient NetClient
		{
			[NullableContext(2)]
			get;
		}
	}
}
