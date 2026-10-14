using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using Steamworks;

namespace MegaCrit.Sts2.Core.Platform.Steam
{
	// Token: 0x02000161 RID: 353
	[NullableContext(1)]
	[Nullable(0)]
	public class SteamLeaderboardStrategy : ILeaderboardStrategy
	{
		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x00062B9F File Offset: 0x00060D9F
		public PlatformType Platform
		{
			get
			{
				return PlatformType.Steam;
			}
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00062BA4 File Offset: 0x00060DA4
		public async Task<ILeaderboardHandle> GetOrCreateLeaderboard(string name, CancellationToken cancelToken)
		{
			SteamAPICall_t steamAPICall_t = SteamUserStats.FindOrCreateLeaderboard(name, 2, 1);
			ILeaderboardHandle leaderboardHandle;
			using (SteamCallResult<LeaderboardFindResult_t> callResult = new SteamCallResult<LeaderboardFindResult_t>(steamAPICall_t, cancelToken))
			{
				LeaderboardFindResult_t leaderboardFindResult_t = await callResult.Task;
				LeaderboardFindResult_t leaderboardFindResult_t2 = leaderboardFindResult_t;
				if (cancelToken.IsCancellationRequested)
				{
					throw new TaskCanceledException();
				}
				if (leaderboardFindResult_t2.m_bLeaderboardFound == 0)
				{
					throw new InvalidOperationException("Steam FindOrCreateLeaderboard returned 0 from leaderboard found!");
				}
				leaderboardHandle = new SteamLeaderboardHandle
				{
					leaderboard = leaderboardFindResult_t2.m_hSteamLeaderboard
				};
			}
			return leaderboardHandle;
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00062BF0 File Offset: 0x00060DF0
		[return: Nullable(new byte[] { 1, 2 })]
		public async Task<ILeaderboardHandle> GetLeaderboard(string name, CancellationToken cancelToken)
		{
			SteamAPICall_t steamAPICall_t = SteamUserStats.FindLeaderboard(name);
			ILeaderboardHandle leaderboardHandle;
			using (SteamCallResult<LeaderboardFindResult_t> callResult = new SteamCallResult<LeaderboardFindResult_t>(steamAPICall_t, cancelToken))
			{
				LeaderboardFindResult_t leaderboardFindResult_t = await callResult.Task;
				LeaderboardFindResult_t leaderboardFindResult_t2 = leaderboardFindResult_t;
				if (cancelToken.IsCancellationRequested || leaderboardFindResult_t2.m_bLeaderboardFound == 0)
				{
					leaderboardHandle = null;
				}
				else
				{
					leaderboardHandle = new SteamLeaderboardHandle
					{
						leaderboard = leaderboardFindResult_t2.m_hSteamLeaderboard
					};
				}
			}
			return leaderboardHandle;
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x00062C3C File Offset: 0x00060E3C
		public async Task UploadLocalScore(ILeaderboardHandle handleInterface, int score, IReadOnlyList<ulong> userIds)
		{
			int num;
			int[] array = this.PackScoreDetails(userIds, out num);
			SteamLeaderboardHandle steamLeaderboardHandle = (SteamLeaderboardHandle)handleInterface;
			SteamAPICall_t steamAPICall_t = SteamUserStats.UploadLeaderboardScore(steamLeaderboardHandle.leaderboard, 2, score, array, num);
			using (SteamCallResult<LeaderboardScoreUploaded_t> callResult = new SteamCallResult<LeaderboardScoreUploaded_t>(steamAPICall_t, SteamInitializer.DisconnectToken))
			{
				LeaderboardScoreUploaded_t leaderboardScoreUploaded_t = await callResult.Task;
				LeaderboardScoreUploaded_t leaderboardScoreUploaded_t2 = leaderboardScoreUploaded_t;
				if (leaderboardScoreUploaded_t2.m_bSuccess == 0)
				{
					throw new IOException("Steam score upload failed!");
				}
			}
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00062C98 File Offset: 0x00060E98
		public async Task<List<LeaderboardEntry>> QueryLeaderboard(ILeaderboardHandle handleInterface, LeaderboardQueryType type, int startIndex, int count, CancellationToken cancelToken = default(CancellationToken))
		{
			SteamLeaderboardHandle steamLeaderboardHandle = (SteamLeaderboardHandle)handleInterface;
			int num = startIndex;
			if (type == LeaderboardQueryType.Global)
			{
				num++;
			}
			SteamAPICall_t steamAPICall_t = SteamUserStats.DownloadLeaderboardEntries(steamLeaderboardHandle.leaderboard, this.LeaderboardDataRequestTypeFrom(type), num, num + count - 1);
			List<LeaderboardEntry> list2;
			using (SteamCallResult<LeaderboardScoresDownloaded_t> callResult = new SteamCallResult<LeaderboardScoresDownloaded_t>(steamAPICall_t, cancelToken))
			{
				LeaderboardScoresDownloaded_t leaderboardScoresDownloaded_t = await callResult.Task;
				LeaderboardScoresDownloaded_t leaderboardScoresDownloaded_t2 = leaderboardScoresDownloaded_t;
				List<LeaderboardEntry> list = new List<LeaderboardEntry>();
				if (cancelToken.IsCancellationRequested)
				{
					list2 = new List<LeaderboardEntry>();
				}
				else
				{
					int num2 = 0;
					int num3 = leaderboardScoresDownloaded_t2.m_cEntryCount;
					if (type == LeaderboardQueryType.FriendsOnly)
					{
						num2 = startIndex;
						num3 = Math.Min(startIndex + count, leaderboardScoresDownloaded_t2.m_cEntryCount);
					}
					if (num2 >= num3)
					{
						list2 = new List<LeaderboardEntry>();
					}
					else
					{
						for (int i = num2; i < num3; i++)
						{
							LeaderboardEntry_t leaderboardEntry_t;
							if (!SteamUserStats.GetDownloadedLeaderboardEntry(leaderboardScoresDownloaded_t2.m_hSteamLeaderboardEntries, i, ref leaderboardEntry_t, this._cachedUserDetails, this._cachedUserDetails.Length))
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(84, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Failed to download leaderboard entry at index ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(i);
								defaultInterpolatedStringHandler.AppendLiteral(". Start result index: ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
								defaultInterpolatedStringHandler.AppendLiteral(". result count: ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(num3);
								throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							list.Add(this.LeaderboardEntryFromSteamType(leaderboardEntry_t));
						}
						list2 = list;
					}
				}
			}
			return list2;
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00062D08 File Offset: 0x00060F08
		public async Task<List<LeaderboardEntry>> QueryLeaderboardForUsers(ILeaderboardHandle handleInterface, IReadOnlyList<ulong> userIds, CancellationToken cancelToken = default(CancellationToken))
		{
			SteamLeaderboardHandle steamLeaderboardHandle = (SteamLeaderboardHandle)handleInterface;
			for (int i = 0; i < userIds.Count; i++)
			{
				this._cachedUsers[i] = new CSteamID(userIds[i]);
			}
			SteamAPICall_t steamAPICall_t = SteamUserStats.DownloadLeaderboardEntriesForUsers(steamLeaderboardHandle.leaderboard, this._cachedUsers, userIds.Count);
			List<LeaderboardEntry> list2;
			using (SteamCallResult<LeaderboardScoresDownloaded_t> callResult = new SteamCallResult<LeaderboardScoresDownloaded_t>(steamAPICall_t, cancelToken))
			{
				LeaderboardScoresDownloaded_t leaderboardScoresDownloaded_t = await callResult.Task;
				LeaderboardScoresDownloaded_t leaderboardScoresDownloaded_t2 = leaderboardScoresDownloaded_t;
				List<LeaderboardEntry> list = new List<LeaderboardEntry>();
				if (cancelToken.IsCancellationRequested)
				{
					list2 = new List<LeaderboardEntry>();
				}
				else
				{
					for (int j = 0; j < leaderboardScoresDownloaded_t2.m_cEntryCount; j++)
					{
						LeaderboardEntry_t leaderboardEntry_t;
						if (!SteamUserStats.GetDownloadedLeaderboardEntry(leaderboardScoresDownloaded_t2.m_hSteamLeaderboardEntries, j, ref leaderboardEntry_t, this._cachedUserDetails, this._cachedUserDetails.Length))
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Failed to download leaderboard entry at index ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(j);
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						list.Add(this.LeaderboardEntryFromSteamType(leaderboardEntry_t));
					}
					list2 = list;
				}
			}
			return list2;
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00062D64 File Offset: 0x00060F64
		public int GetLeaderboardEntryCount(ILeaderboardHandle handleInterface)
		{
			SteamLeaderboardHandle steamLeaderboardHandle = (SteamLeaderboardHandle)handleInterface;
			return SteamUserStats.GetLeaderboardEntryCount(steamLeaderboardHandle.leaderboard);
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00062D84 File Offset: 0x00060F84
		private LeaderboardEntry LeaderboardEntryFromSteamType(LeaderboardEntry_t entry)
		{
			return new LeaderboardEntry
			{
				id = entry.m_steamIDUser.m_SteamID,
				name = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser),
				rank = entry.m_nGlobalRank - 1,
				score = entry.m_nScore,
				userIds = this.UnpackScoreDetails(this._cachedUserDetails, entry.m_cDetails)
			};
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00062DEC File Offset: 0x00060FEC
		private ELeaderboardDataRequest LeaderboardDataRequestTypeFrom(LeaderboardQueryType type)
		{
			ELeaderboardDataRequest eleaderboardDataRequest;
			switch (type)
			{
			case LeaderboardQueryType.None:
				throw new InvalidOperationException("LeaderboardQueryType.None should never be passed to an API!");
			case LeaderboardQueryType.Global:
				eleaderboardDataRequest = 0;
				break;
			case LeaderboardQueryType.AroundUser:
				eleaderboardDataRequest = 1;
				break;
			case LeaderboardQueryType.FriendsOnly:
				eleaderboardDataRequest = 2;
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(type);
				break;
			}
			return eleaderboardDataRequest;
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00062E34 File Offset: 0x00061034
		private int[] PackScoreDetails(IReadOnlyList<ulong> userIds, out int scoreDetailsCount)
		{
			this._writer.Reset();
			foreach (ulong num in userIds)
			{
				this._writer.WriteULong(num, 64);
			}
			scoreDetailsCount = (int)Math.Ceiling((double)((float)this._writer.BitPosition / 32f));
			if (scoreDetailsCount > 64)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to write ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(userIds.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" user IDs into ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(scoreDetailsCount);
				defaultInterpolatedStringHandler.AppendLiteral(" integers, exceeding the max limit (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(64);
				defaultInterpolatedStringHandler.AppendLiteral(")!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			for (int i = 0; i < scoreDetailsCount; i++)
			{
				this._cachedUserDetails[i] = BitConverter.ToInt32(this._writer.Buffer, i * 4);
			}
			return this._cachedUserDetails;
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00062F44 File Offset: 0x00061144
		private List<ulong> UnpackScoreDetails(int[] scoreDetails, int scoreDetailsCount)
		{
			this._writer.Reset();
			for (int i = 0; i < scoreDetailsCount; i++)
			{
				this._writer.WriteInt(scoreDetails[i], 32);
			}
			PacketReader packetReader = new PacketReader();
			packetReader.Reset(this._writer.Buffer);
			int num = (int)Math.Floor((double)((float)this._writer.BitPosition / 64f));
			List<ulong> list = new List<ulong>();
			for (int j = 0; j < num; j++)
			{
				list.Add(packetReader.ReadULong(64));
			}
			return list;
		}

		// Token: 0x040005F7 RID: 1527
		private readonly PacketWriter _writer = new PacketWriter();

		// Token: 0x040005F8 RID: 1528
		private readonly int[] _cachedUserDetails = new int[64];

		// Token: 0x040005F9 RID: 1529
		private readonly CSteamID[] _cachedUsers = new CSteamID[10];
	}
}
