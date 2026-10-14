using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Runs
{
	/// <summary>
	/// Helper functions for calculating score, badges, and the score for daily leaderboards (Also called score, very confusing!)
	/// </summary>
	// Token: 0x02000113 RID: 275
	[NullableContext(1)]
	[Nullable(0)]
	public static class ScoreUtility
	{
		/// <summary>
		/// Calculates the score for a given <see cref="T:MegaCrit.Sts2.Core.Runs.IRunState" />. Only called in the Architect room.
		/// </summary>
		/// <param name="runState">The run that was just completed.</param>
		/// <param name="won">True if the run ended in a victory.</param>
		/// <returns>The final score for the run.</returns>
		// Token: 0x06000D38 RID: 3384 RVA: 0x000520F4 File Offset: 0x000502F4
		public static int CalculateScore(IRunState runState, bool won)
		{
			return ScoreUtility.CalculateScore(runState.MapPointHistory, runState.AscensionLevel, won, runState.Players.Count);
		}

		/// <summary>
		/// Calculates the score for a given <see cref="T:MegaCrit.Sts2.Core.Saves.SerializableRun" />.
		/// </summary>
		/// <param name="run">A serializable version of the run that was just completed.</param>
		/// <param name="won">True if the run ended in a victory.</param>
		/// <returns>The final score for the run.</returns>
		// Token: 0x06000D39 RID: 3385 RVA: 0x00052113 File Offset: 0x00050313
		public static int CalculateScore(SerializableRun run, bool won)
		{
			return ScoreUtility.CalculateScore(run.MapPointHistory, run.Ascension, won, run.Players.Count);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00052134 File Offset: 0x00050334
		private static int CalculateScore(IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> history, int ascension, bool won, int playerCount)
		{
			int num = 0;
			num += ScoreUtility.GetScoreForFloor(history);
			num += ScoreUtility.GetScoreForGoldGained(history, playerCount);
			num += ScoreUtility.GetScoreForElitesKilled(ScoreUtility.GetElitesKilledCount(history));
			num += ScoreUtility.GetScoreForBossesSlain(ScoreUtility.GetBossesSlainCount(history, won));
			return (int)((double)num * (1.0 + (double)ascension * 0.1));
		}

		/// <summary>
		/// Helper function to calculate score based on total floors climbed.
		/// Useful as we display the score on the score line as well.
		/// </summary>
		// Token: 0x06000D3B RID: 3387 RVA: 0x00052190 File Offset: 0x00050390
		public static int GetScoreForFloor(IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> history)
		{
			int num = 0;
			int count = history.Count;
			for (int i = 0; i < count; i++)
			{
				num += history[i].Count * 10 * (i + 1);
			}
			return num;
		}

		/// <summary>
		/// Counts the total number of elites the player killed during a run.
		/// </summary>
		// Token: 0x06000D3C RID: 3388 RVA: 0x000521CC File Offset: 0x000503CC
		public static int GetElitesKilledCount(IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> history)
		{
			List<MapPointRoomHistoryEntry> list = history.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((IReadOnlyList<MapPointHistoryEntry> actEntries) => actEntries).SelectMany<MapPointHistoryEntry, MapPointRoomHistoryEntry>((MapPointHistoryEntry e) => e.Rooms).ToList<MapPointRoomHistoryEntry>();
			int num = list.Count<MapPointRoomHistoryEntry>((MapPointRoomHistoryEntry r) => r.RoomType == RoomType.Elite);
			if (list.Count > 0 && list.Last<MapPointRoomHistoryEntry>().RoomType == RoomType.Elite)
			{
				num--;
			}
			return num;
		}

		/// <summary>
		/// Helper function to calculate score based on Elites killed.
		/// Useful as we display the score on the score line as well.
		/// </summary>
		// Token: 0x06000D3D RID: 3389 RVA: 0x0005226A File Offset: 0x0005046A
		public static int GetScoreForElitesKilled(int elitesKilled)
		{
			return elitesKilled * 50;
		}

		/// <summary>
		/// Counts the total number of bosses the player killed during a run.
		/// </summary>
		// Token: 0x06000D3E RID: 3390 RVA: 0x00052270 File Offset: 0x00050470
		public static int GetBossesSlainCount(IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> history, bool won)
		{
			int num = 0;
			foreach (IReadOnlyList<MapPointHistoryEntry> readOnlyList in history)
			{
				foreach (MapPointHistoryEntry mapPointHistoryEntry in readOnlyList)
				{
					foreach (MapPointRoomHistoryEntry mapPointRoomHistoryEntry in mapPointHistoryEntry.Rooms)
					{
						bool flag = !won && readOnlyList == history.Last<IReadOnlyList<MapPointHistoryEntry>>() && mapPointHistoryEntry == readOnlyList.Last<MapPointHistoryEntry>() && mapPointRoomHistoryEntry == mapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>();
						if (mapPointRoomHistoryEntry.RoomType == RoomType.Boss && !flag)
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		/// <summary>
		/// Helper function to calculate score based on total bosses slain.
		/// Useful as we display the score on the score line as well.
		/// </summary>
		// Token: 0x06000D3F RID: 3391 RVA: 0x00052368 File Offset: 0x00050568
		public static int GetScoreForBossesSlain(int bossCount)
		{
			return bossCount * 100;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00052370 File Offset: 0x00050570
		public static int GetScoreForGoldGained(IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> history, int playerCount)
		{
			return history.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((IReadOnlyList<MapPointHistoryEntry> actEntries) => actEntries).SelectMany<MapPointHistoryEntry, PlayerMapPointHistoryEntry>((MapPointHistoryEntry e) => e.PlayerStats).Sum<PlayerMapPointHistoryEntry>((PlayerMapPointHistoryEntry p) => p.GoldGained) / (100 * playerCount);
		}

		/// <summary>
		/// Returns a list of badges that this run has obtained.
		/// </summary>
		// Token: 0x06000D41 RID: 3393 RVA: 0x000523F0 File Offset: 0x000505F0
		public static List<Badge> GetBadges(SerializableRun run, ulong playerId, bool won)
		{
			List<Badge> list = new List<Badge>();
			foreach (Badge badge in BadgePool.CreateAll(run, playerId, won))
			{
				if ((!badge.RequiresWin || won) && (!badge.MultiplayerOnly || run.Players.Count != 1) && badge.IsObtained())
				{
					list.Add(badge);
				}
			}
			return list;
		}

		/// <summary>
		/// Encodes the current run into a "daily upload score".
		/// This is greatly differentiated from the score used to unlock epochs. Cursed naming.
		/// </summary>
		// Token: 0x06000D42 RID: 3394 RVA: 0x00052470 File Offset: 0x00050670
		public static int CalculateDailyScore(SerializableRun run, ulong localPlayerNetId, bool isVictory)
		{
			int num = (isVictory ? 2 : 1);
			int num2 = Math.Clamp(run.FloorReached, 0, 99);
			int num3 = Math.Clamp(ScoreUtility.GetBadges(run, localPlayerNetId, isVictory).Count, 0, 99);
			int num4 = (int)Math.Clamp(isVictory ? run.WinTime : run.RunTime, 0L, 9999L);
			return num * 100000000 + num2 * 1000000 + num3 * 10000 + (9999 - num4);
		}

		/// <summary>
		/// Decodes our fancy encoded int into a struct.
		/// </summary>
		// Token: 0x06000D43 RID: 3395 RVA: 0x000524EC File Offset: 0x000506EC
		public static DecodedDailyScore DecodeDailyScore(int encodedScore)
		{
			int num = encodedScore / 100000000;
			int num2 = encodedScore / 1000000 % 100;
			int num3 = encodedScore / 10000 % 100;
			int num4 = 9999 - encodedScore % 10000;
			bool flag = num - 1 <= 2;
			bool flag2 = flag && num2 >= 0 && num2 <= 99 && num3 >= 0 && num3 <= 99 && num4 >= 0 && num4 <= 9999;
			return new DecodedDailyScore
			{
				isValid = flag2,
				victory = num,
				floors = num2,
				badges = num3,
				runTime = num4
			};
		}

		/// <summary>
		/// A sentinel value for clients in a multiplayer run to submit when uploading a daily run score.
		/// For more information on why this is needed, see <see cref="M:MegaCrit.Sts2.Core.Daily.DailyRunUtility.ShouldUploadScore(MegaCrit.Sts2.Core.Leaderboard.ILeaderboardHandle,System.Collections.Generic.IReadOnlyList{System.UInt64},System.Threading.CancellationToken)" />.
		/// </summary>
		// Token: 0x04000488 RID: 1160
		public const int clientScore = -999999999;
	}
}
