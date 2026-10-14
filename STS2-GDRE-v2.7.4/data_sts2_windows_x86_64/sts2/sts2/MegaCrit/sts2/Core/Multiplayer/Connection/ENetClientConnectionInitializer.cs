using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Transport.ENet;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Multiplayer.Connection
{
	// Token: 0x020004BA RID: 1210
	[NullableContext(1)]
	[Nullable(0)]
	public class ENetClientConnectionInitializer : IClientConnectionInitializer
	{
		// Token: 0x060048C8 RID: 18632 RVA: 0x00224FE6 File Offset: 0x002231E6
		public ENetClientConnectionInitializer(ulong netId, string ip, ushort port)
		{
			this._netId = netId;
			this._ip = ip;
			this._port = port;
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x00225004 File Offset: 0x00223204
		public async Task<NetErrorInfo?> Connect(INetClientGameService netService, CancellationToken cancelToken = default(CancellationToken))
		{
			if (netService.IsConnected)
			{
				throw new InvalidOperationException("NetClientGameService must not be connected when passed to ENetClientConnectionInitializer!");
			}
			ENetClient enetClient = new ENetClient(netService);
			netService.Initialize(enetClient, PlatformType.None);
			return await enetClient.ConnectToHost(this._netId, this._ip, this._port, cancelToken);
		}

		// Token: 0x04001C97 RID: 7319
		private readonly ulong _netId;

		// Token: 0x04001C98 RID: 7320
		private readonly string _ip;

		// Token: 0x04001C99 RID: 7321
		private readonly ushort _port;
	}
}
