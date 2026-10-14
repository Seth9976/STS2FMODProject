using System;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet
{
	// Token: 0x02000444 RID: 1092
	public enum ENetPacketType : byte
	{
		// Token: 0x04001ABF RID: 6847
		HandshakeRequest,
		// Token: 0x04001AC0 RID: 6848
		HandshakeResponse,
		// Token: 0x04001AC1 RID: 6849
		Disconnection,
		// Token: 0x04001AC2 RID: 6850
		ApplicationMessage
	}
}
