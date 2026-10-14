using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport
{
	// Token: 0x02000430 RID: 1072
	[NullableContext(1)]
	public interface INetHandler
	{
		// Token: 0x0600445E RID: 17502
		void OnPacketReceived(ulong senderId, byte[] packetBytes, NetTransferMode mode, int channel);
	}
}
