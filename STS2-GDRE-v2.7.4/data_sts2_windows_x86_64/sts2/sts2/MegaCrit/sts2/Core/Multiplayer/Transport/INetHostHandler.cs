using System;
using MegaCrit.Sts2.Core.Entities.Multiplayer;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport
{
	// Token: 0x02000431 RID: 1073
	public interface INetHostHandler : INetHandler
	{
		// Token: 0x0600445F RID: 17503
		void OnPeerConnected(ulong peerId);

		// Token: 0x06004460 RID: 17504
		void OnPeerDisconnected(ulong peerId, NetErrorInfo info);

		// Token: 0x06004461 RID: 17505
		void OnDisconnected(NetErrorInfo info);
	}
}
