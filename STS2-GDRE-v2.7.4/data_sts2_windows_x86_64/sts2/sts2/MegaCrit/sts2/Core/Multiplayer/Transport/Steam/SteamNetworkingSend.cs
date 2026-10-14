using System;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.Steam
{
	// Token: 0x0200043A RID: 1082
	[Flags]
	public enum SteamNetworkingSend
	{
		// Token: 0x04001A9D RID: 6813
		Unreliable = 0,
		// Token: 0x04001A9E RID: 6814
		Reliable = 8,
		// Token: 0x04001A9F RID: 6815
		NoNagle = 1,
		// Token: 0x04001AA0 RID: 6816
		NoDelay = 4
	}
}
