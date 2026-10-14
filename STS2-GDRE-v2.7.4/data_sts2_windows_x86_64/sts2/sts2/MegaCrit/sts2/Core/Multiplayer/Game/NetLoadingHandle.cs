using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// A utility class for marking the start and end of a loading sequence. Basically just equivalent of calling
	/// NetService.SetGameLoading with true, and then false at the end of the scope.
	/// Construct one of these with a using statement any time we enter a long loading sequence to let the networking layer
	/// know that the game's framerate might dip. If you don't do this, we might accidentally display a "waiting for
	/// connection from host" overlay because messages aren't being processed.
	/// </summary>
	// Token: 0x020004A1 RID: 1185
	[NullableContext(1)]
	[Nullable(0)]
	public class NetLoadingHandle : IDisposable
	{
		// Token: 0x06004782 RID: 18306 RVA: 0x0021C6C0 File Offset: 0x0021A8C0
		public NetLoadingHandle(INetGameService netService)
		{
			this._netService = netService;
			int num;
			if (!NetLoadingHandle._loadCounts.TryGetValue(this._netService, out num) || num == 0)
			{
				this._netService.SetGameLoading(true);
			}
			NetLoadingHandle._loadCounts[this._netService] = num + 1;
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0021C710 File Offset: 0x0021A910
		public void Dispose()
		{
			if (NetLoadingHandle._loadCounts[this._netService] == 1)
			{
				this._netService.SetGameLoading(false);
			}
			Dictionary<INetGameService, int> loadCounts = NetLoadingHandle._loadCounts;
			INetGameService netService = this._netService;
			int num = loadCounts[netService];
			loadCounts[netService] = num - 1;
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0021C759 File Offset: 0x0021A959
		public static void Release(INetGameService netService)
		{
			NetLoadingHandle._loadCounts.Remove(netService);
		}

		// Token: 0x04001C0B RID: 7179
		private static readonly Dictionary<INetGameService, int> _loadCounts = new Dictionary<INetGameService, int>();

		// Token: 0x04001C0C RID: 7180
		private readonly INetGameService _netService;
	}
}
