using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport
{
	// Token: 0x0200042F RID: 1071
	public interface INetClientHandler : INetHandler
	{
		// Token: 0x0600445B RID: 17499
		[NullableContext(1)]
		void Initialize(NetClient client, PlatformType platformType);

		// Token: 0x0600445C RID: 17500
		void OnConnectedToHost();

		// Token: 0x0600445D RID: 17501
		void OnDisconnectedFromHost(ulong hostNetId, NetErrorInfo info);
	}
}
