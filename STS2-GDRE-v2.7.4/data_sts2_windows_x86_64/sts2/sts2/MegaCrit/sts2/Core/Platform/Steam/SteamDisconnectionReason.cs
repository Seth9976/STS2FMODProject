using System;

namespace MegaCrit.Sts2.Core.Platform.Steam
{
	// Token: 0x0200015C RID: 348
	public enum SteamDisconnectionReason
	{
		// Token: 0x040005D6 RID: 1494
		None,
		// Token: 0x040005D7 RID: 1495
		AppGeneric = 1000,
		// Token: 0x040005D8 RID: 1496
		AppInternalError = 1017,
		// Token: 0x040005D9 RID: 1497
		AppException = 2000,
		// Token: 0x040005DA RID: 1498
		LocalMin = 3000,
		// Token: 0x040005DB RID: 1499
		RunningInOfflineMode,
		// Token: 0x040005DC RID: 1500
		ManyRelayConnectivity,
		// Token: 0x040005DD RID: 1501
		HostedServerPrimaryRelay,
		// Token: 0x040005DE RID: 1502
		NetworkConfig,
		// Token: 0x040005DF RID: 1503
		LocalRights,
		// Token: 0x040005E0 RID: 1504
		LocalMax = 3999,
		// Token: 0x040005E1 RID: 1505
		RemoteTimeout = 4001,
		// Token: 0x040005E2 RID: 1506
		BadCrypt,
		// Token: 0x040005E3 RID: 1507
		BadCert,
		// Token: 0x040005E4 RID: 1508
		NotLoggedIn,
		// Token: 0x040005E5 RID: 1509
		NotRunningApp,
		// Token: 0x040005E6 RID: 1510
		BadProtocolVersion,
		// Token: 0x040005E7 RID: 1511
		MiscGeneric = 5001,
		// Token: 0x040005E8 RID: 1512
		InternalError,
		// Token: 0x040005E9 RID: 1513
		MiscTimeout,
		// Token: 0x040005EA RID: 1514
		RelayConnectivity,
		// Token: 0x040005EB RID: 1515
		SteamConnectivity,
		// Token: 0x040005EC RID: 1516
		NoRelaySessions,
		// Token: 0x040005ED RID: 1517
		RendezvousTimeout = 5008,
		// Token: 0x040005EE RID: 1518
		RelayReceivedUnexpectedNoConnectionPacket = 5010
	}
}
