using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Platform.Steam
{
	// Token: 0x0200015D RID: 349
	public static class SteamDisconnectionReasonExtensions
	{
		// Token: 0x06001096 RID: 4246 RVA: 0x000624CB File Offset: 0x000606CB
		public static SteamDisconnectionReason ToSteam(this NetError reason)
		{
			return SteamDisconnectionReason.AppGeneric + (int)reason;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x000624D4 File Offset: 0x000606D4
		public static NetError ToApp(this SteamDisconnectionReason steamReason)
		{
			if (steamReason < SteamDisconnectionReason.AppGeneric || steamReason > (SteamDisconnectionReason)1999)
			{
				if (steamReason >= SteamDisconnectionReason.LocalMin)
				{
					if (steamReason > SteamDisconnectionReason.LocalMax)
					{
						if (steamReason != SteamDisconnectionReason.RemoteTimeout)
						{
							if (steamReason - SteamDisconnectionReason.BadCrypt <= 1)
							{
								return NetError.SecureConnectionFailed;
							}
							switch (steamReason)
							{
							case SteamDisconnectionReason.MiscTimeout:
								break;
							case SteamDisconnectionReason.RelayConnectivity:
							case SteamDisconnectionReason.SteamConnectivity:
								return NetError.NoInternet;
							case SteamDisconnectionReason.NoRelaySessions:
							case (SteamDisconnectionReason)5007:
							case (SteamDisconnectionReason)5009:
								return NetError.UnknownNetworkError;
							case SteamDisconnectionReason.RendezvousTimeout:
							case SteamDisconnectionReason.RelayReceivedUnexpectedNoConnectionPacket:
								return NetError.TryAgainLater;
							default:
								return NetError.UnknownNetworkError;
							}
						}
						return NetError.Timeout;
					}
					return NetError.NoInternet;
				}
				if (steamReason == SteamDisconnectionReason.None)
				{
					return NetError.None;
				}
				return NetError.UnknownNetworkError;
			}
			NetError netError = (NetError)(steamReason - 1000);
			if (!Enum.IsDefined<NetError>(netError))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received unknown application error from Steam: ");
				defaultInterpolatedStringHandler.AppendFormatted<SteamDisconnectionReason>(steamReason);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return NetError.UnknownNetworkError;
			}
			return netError;
		}
	}
}
