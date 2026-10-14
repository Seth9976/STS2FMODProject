using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using Steamworks;

namespace MegaCrit.Sts2.Core.Platform.Steam
{
	/// <summary>
	/// Manages Steam user stats and global stat aggregates.
	/// Used to track per-user architect damage and fetch the global aggregate across all players.
	/// </summary>
	// Token: 0x02000165 RID: 357
	[NullableContext(1)]
	[Nullable(0)]
	public static class SteamStatsManager
	{
		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x00063CD2 File Offset: 0x00061ED2
		public static bool IsGlobalStatsReady
		{
			get
			{
				return SteamStatsManager._globalStatsReady;
			}
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00063CDC File Offset: 0x00061EDC
		public static void Initialize()
		{
			if (!SteamInitializer.Initialized)
			{
				return;
			}
			SteamStatsManager._userStatsReady = false;
			SteamStatsManager._globalStatsReady = false;
			SteamStatsManager._globalArchitectDamage = 0L;
			Callback<UserStatsReceived_t>.DispatchDelegate dispatchDelegate;
			if ((dispatchDelegate = SteamStatsManager.<>O.<0>__OnUserStatsReceived) == null)
			{
				dispatchDelegate = (SteamStatsManager.<>O.<0>__OnUserStatsReceived = new Callback<UserStatsReceived_t>.DispatchDelegate(SteamStatsManager.OnUserStatsReceived));
			}
			SteamStatsManager._userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(dispatchDelegate);
			SteamUserStats.RequestCurrentStats();
			TaskHelper.RunSafely(SteamStatsManager.RefreshGlobalStats());
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00063D3C File Offset: 0x00061F3C
		public static async Task RefreshGlobalStats()
		{
			if (SteamInitializer.Initialized)
			{
				SteamAPICall_t steamAPICall_t = SteamUserStats.RequestGlobalStats(0);
				using (SteamCallResult<GlobalStatsReceived_t> callResult = new SteamCallResult<GlobalStatsReceived_t>(steamAPICall_t, SteamInitializer.DisconnectToken))
				{
					try
					{
						GlobalStatsReceived_t globalStatsReceived_t = await callResult.Task;
						SteamStatsManager.OnGlobalStatsReceived(globalStatsReceived_t);
					}
					catch
					{
						Log.Warn("SteamStatsManager: Failed to receive global stats", 2);
					}
				}
			}
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00063D78 File Offset: 0x00061F78
		public static void IncrementArchitectDamage(int score)
		{
			if (!SteamStatsManager._userStatsReady)
			{
				Log.Warn("SteamStatsManager: Cannot increment architect damage, user stats not ready", 2);
				return;
			}
			int num;
			bool stat = SteamUserStats.GetStat("architect_damage", ref num);
			bool flag = SteamUserStats.SetStat("architect_damage", num + score);
			bool flag2 = SteamUserStats.StoreStats();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(81, 6);
			defaultInterpolatedStringHandler.AppendLiteral("SteamStatsManager: IncrementArchitectDamage by ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(score);
			defaultInterpolatedStringHandler.AppendLiteral(" (was ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(", now ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num + score);
			defaultInterpolatedStringHandler.AppendLiteral(") [get=");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(stat);
			defaultInterpolatedStringHandler.AppendLiteral(", set=");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(flag);
			defaultInterpolatedStringHandler.AppendLiteral(", store=");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(flag2);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00063E56 File Offset: 0x00062056
		public static long GetGlobalArchitectDamage()
		{
			return SteamStatsManager._globalArchitectDamage;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00063E60 File Offset: 0x00062060
		private static void OnUserStatsReceived(UserStatsReceived_t result)
		{
			if (result.m_nGameID != 2868840UL)
			{
				return;
			}
			if (result.m_eResult == 1)
			{
				SteamStatsManager._userStatsReady = true;
				Log.Info("SteamStatsManager: User stats received", 2);
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 1);
			defaultInterpolatedStringHandler.AppendLiteral("SteamStatsManager: User stats request failed with result ");
			defaultInterpolatedStringHandler.AppendFormatted<EResult>(result.m_eResult);
			Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00063EC8 File Offset: 0x000620C8
		private static void OnGlobalStatsReceived(GlobalStatsReceived_t result)
		{
			if (result.m_nGameID != 2868840UL)
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (result.m_eResult == 1)
			{
				long num;
				bool globalStat = SteamUserStats.GetGlobalStat("architect_damage", ref num);
				SteamStatsManager._globalArchitectDamage = num;
				SteamStatsManager._globalStatsReady = true;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
				defaultInterpolatedStringHandler.AppendLiteral("SteamStatsManager: Global stats received (found=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(globalStat);
				defaultInterpolatedStringHandler.AppendLiteral("), architect damage = ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(SteamStatsManager._globalArchitectDamage);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
			defaultInterpolatedStringHandler.AppendLiteral("SteamStatsManager: Global stats request failed with result ");
			defaultInterpolatedStringHandler.AppendFormatted<EResult>(result.m_eResult);
			Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x04000601 RID: 1537
		private const string _architectDamageStat = "architect_damage";

		// Token: 0x04000602 RID: 1538
		[Nullable(2)]
		private static Callback<UserStatsReceived_t> _userStatsReceivedCallback;

		// Token: 0x04000603 RID: 1539
		private static bool _userStatsReady;

		// Token: 0x04000604 RID: 1540
		private static bool _globalStatsReady;

		// Token: 0x04000605 RID: 1541
		private static long _globalArchitectDamage;

		// Token: 0x02000F45 RID: 3909
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003CB9 RID: 15545
			[Nullable(0)]
			public static Callback<UserStatsReceived_t>.DispatchDelegate <0>__OnUserStatsReceived;
		}
	}
}
