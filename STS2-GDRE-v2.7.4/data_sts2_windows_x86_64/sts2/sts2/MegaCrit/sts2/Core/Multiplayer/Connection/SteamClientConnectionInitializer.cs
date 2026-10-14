using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Multiplayer.Connection
{
	// Token: 0x020004BC RID: 1212
	[NullableContext(1)]
	[Nullable(0)]
	public class SteamClientConnectionInitializer : IClientConnectionInitializer
	{
		// Token: 0x060048CB RID: 18635 RVA: 0x00225057 File Offset: 0x00223257
		public static SteamClientConnectionInitializer FromPlayer(ulong playerSteamId)
		{
			return new SteamClientConnectionInitializer
			{
				_playerSteamId = new ulong?(playerSteamId)
			};
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x0022506A File Offset: 0x0022326A
		public static SteamClientConnectionInitializer FromLobby(ulong lobbySteamId)
		{
			return new SteamClientConnectionInitializer
			{
				_lobbySteamId = new ulong?(lobbySteamId)
			};
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x00225080 File Offset: 0x00223280
		public async Task<NetErrorInfo?> Connect(INetClientGameService gameService, CancellationToken cancelToken = default(CancellationToken))
		{
			if (gameService.IsConnected)
			{
				throw new InvalidOperationException("NetClientGameService must not be connected when passed to SteamClientConnectionInitializer!");
			}
			SteamClient steamClient = new SteamClient(gameService);
			gameService.Initialize(steamClient, PlatformType.Steam);
			NetErrorInfo? netErrorInfo2;
			if (this._playerSteamId != null)
			{
				NetErrorInfo? netErrorInfo = await steamClient.ConnectToLobbyOwnedByFriend(this._playerSteamId.Value, cancelToken);
				netErrorInfo2 = netErrorInfo;
			}
			else
			{
				if (this._lobbySteamId == null)
				{
					throw new InvalidOperationException("Neither player nor lobby is set!");
				}
				netErrorInfo2 = await steamClient.ConnectToLobby(this._lobbySteamId.Value, cancelToken);
			}
			return netErrorInfo2;
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x002250D4 File Offset: 0x002232D4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 3);
			defaultInterpolatedStringHandler.AppendFormatted("SteamClientConnectionInitializer");
			defaultInterpolatedStringHandler.AppendLiteral(" player: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong?>(this._playerSteamId);
			defaultInterpolatedStringHandler.AppendLiteral(" lobby: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong?>(this._lobbySteamId);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04001C9A RID: 7322
		private ulong? _playerSteamId;

		// Token: 0x04001C9B RID: 7323
		private ulong? _lobbySteamId;
	}
}
