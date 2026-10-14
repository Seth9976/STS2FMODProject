using System;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Collections;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet
{
	// Token: 0x0200043D RID: 1085
	[NullableContext(1)]
	[Nullable(0)]
	public struct ENetServiceData
	{
		// Token: 0x04001AA8 RID: 6824
		public ENetConnection.EventType type;

		// Token: 0x04001AA9 RID: 6825
		public ENetPacketPeer peer;

		// Token: 0x04001AAA RID: 6826
		public int channel;

		// Token: 0x04001AAB RID: 6827
		public NetTransferMode mode;

		// Token: 0x04001AAC RID: 6828
		public byte[] packetData;

		// Token: 0x04001AAD RID: 6829
		public Error error;

		// Token: 0x04001AAE RID: 6830
		public Array originalData;
	}
}
