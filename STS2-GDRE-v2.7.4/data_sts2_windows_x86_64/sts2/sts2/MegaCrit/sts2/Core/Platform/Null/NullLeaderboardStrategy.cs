using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Platform.Null
{
	// Token: 0x0200016B RID: 363
	[NullableContext(1)]
	[Nullable(0)]
	public class NullLeaderboardStrategy : ILeaderboardStrategy
	{
		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x00063FE6 File Offset: 0x000621E6
		public PlatformType Platform
		{
			get
			{
				return PlatformType.None;
			}
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00063FEC File Offset: 0x000621EC
		public NullLeaderboardStrategy()
		{
			this._leaderboards = this.Read();
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00064030 File Offset: 0x00062230
		[return: Nullable(new byte[] { 1, 2 })]
		public Task<ILeaderboardHandle> GetLeaderboard(string name, CancellationToken cancelToken)
		{
			foreach (NullLeaderboard nullLeaderboard in this._leaderboards)
			{
				if (nullLeaderboard.name == name)
				{
					return Task.FromResult<ILeaderboardHandle>(new NullLeaderboardHandle
					{
						leaderboard = nullLeaderboard
					});
				}
			}
			return Task.FromResult<ILeaderboardHandle>(null);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x000640A8 File Offset: 0x000622A8
		public async Task<ILeaderboardHandle> GetOrCreateLeaderboard(string name, CancellationToken cancelToken)
		{
			await this.CheckRefreshLeaderboard(null, PlatformUtil.GetLocalPlayerId(PlatformType.None));
			ILeaderboardHandle leaderboardHandle = await this.GetLeaderboard(name, cancelToken);
			ILeaderboardHandle leaderboardHandle2;
			if (leaderboardHandle != null)
			{
				leaderboardHandle2 = leaderboardHandle;
			}
			else
			{
				NullLeaderboard nullLeaderboard = new NullLeaderboard
				{
					name = name,
					entries = new List<NullLeaderboardFileEntry>()
				};
				this._leaderboards.Add(nullLeaderboard);
				this.Write(this._leaderboards);
				leaderboardHandle2 = new NullLeaderboardHandle
				{
					leaderboard = nullLeaderboard
				};
			}
			return leaderboardHandle2;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000640FC File Offset: 0x000622FC
		public async Task UploadLocalScore(ILeaderboardHandle handleInterface, int score, IReadOnlyList<ulong> userIds)
		{
			NullLeaderboardHandle handle = (NullLeaderboardHandle)handleInterface;
			ulong id = PlatformUtil.GetLocalPlayerId(PlatformType.None);
			await this.CheckRefreshLeaderboard(handle, id);
			NullLeaderboardFileEntry nullLeaderboardFileEntry = new NullLeaderboardFileEntry
			{
				name = PlatformUtil.GetPlayerNameRaw(PlatformType.None, id),
				id = id,
				score = score,
				userIds = userIds.ToList<ulong>()
			};
			handle.leaderboard.entries.Add(nullLeaderboardFileEntry);
			this.Write(this._leaderboards);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00064158 File Offset: 0x00062358
		public async Task<List<LeaderboardEntry>> QueryLeaderboard(ILeaderboardHandle handleInterface, LeaderboardQueryType type, int startIndex, int count, CancellationToken cancelToken)
		{
			NullLeaderboardHandle handle = (NullLeaderboardHandle)handleInterface;
			ulong id = PlatformUtil.GetLocalPlayerId(PlatformType.None);
			await this.CheckRefreshLeaderboard(handle, id);
			List<LeaderboardEntry> list;
			switch (type)
			{
			case LeaderboardQueryType.Global:
			case LeaderboardQueryType.FriendsOnly:
				list = this.ToLeaderboardEntries(startIndex, handle.leaderboard.entries.Slice(startIndex, Math.Min(startIndex + count, handle.leaderboard.entries.Count) - startIndex));
				break;
			case LeaderboardQueryType.AroundUser:
			{
				int num = handle.leaderboard.entries.FindIndex((NullLeaderboardFileEntry e) => e.id == id);
				if (num < 0)
				{
					list = new List<LeaderboardEntry>();
				}
				else
				{
					int num2 = num + startIndex;
					int num3 = Math.Max(0, num2);
					int num4 = Math.Min(handle.leaderboard.entries.Count, num2 + count);
					if (num3 >= num4)
					{
						list = new List<LeaderboardEntry>();
					}
					else
					{
						list = this.ToLeaderboardEntries(num3, handle.leaderboard.entries.Slice(num3, num4 - num3));
					}
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			return list;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x000641BC File Offset: 0x000623BC
		public async Task<List<LeaderboardEntry>> QueryLeaderboardForUsers(ILeaderboardHandle handleInterface, IReadOnlyList<ulong> userIds, CancellationToken cancelToken)
		{
			NullLeaderboardHandle handle = (NullLeaderboardHandle)handleInterface;
			ulong localPlayerId = PlatformUtil.GetLocalPlayerId(PlatformType.None);
			await this.CheckRefreshLeaderboard(handle, localPlayerId);
			List<LeaderboardEntry> list = new List<LeaderboardEntry>();
			for (int i = 0; i < handle.leaderboard.entries.Count; i++)
			{
				NullLeaderboardFileEntry nullLeaderboardFileEntry = handle.leaderboard.entries[i];
				if (userIds.Contains(nullLeaderboardFileEntry.id))
				{
					list.Add(this.ToLeaderboardEntry(i, nullLeaderboardFileEntry));
				}
			}
			return list;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0006420F File Offset: 0x0006240F
		public int GetLeaderboardEntryCount(ILeaderboardHandle handleInterface)
		{
			return ((NullLeaderboardHandle)handleInterface).leaderboard.entries.Count;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00064228 File Offset: 0x00062428
		public void DebugAddEntry(ILeaderboardHandle handleInterface, LeaderboardEntry entry)
		{
			NullLeaderboardHandle nullLeaderboardHandle = (NullLeaderboardHandle)handleInterface;
			NullLeaderboardFileEntry nullLeaderboardFileEntry = new NullLeaderboardFileEntry
			{
				name = entry.name,
				id = entry.id,
				score = entry.score
			};
			nullLeaderboardHandle.leaderboard.entries.Add(nullLeaderboardFileEntry);
			nullLeaderboardHandle.leaderboard.entries.Sort((NullLeaderboardFileEntry x, NullLeaderboardFileEntry y) => y.score.CompareTo(x.score));
			this.Write(this._leaderboards);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000642B4 File Offset: 0x000624B4
		private List<LeaderboardEntry> ToLeaderboardEntries(int startIndex, List<NullLeaderboardFileEntry> nullEntries)
		{
			List<LeaderboardEntry> list = new List<LeaderboardEntry>();
			for (int i = 0; i < nullEntries.Count; i++)
			{
				list.Add(this.ToLeaderboardEntry(startIndex + i, nullEntries[i]));
			}
			return list;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000642EF File Offset: 0x000624EF
		private LeaderboardEntry ToLeaderboardEntry(int rank, NullLeaderboardFileEntry nullEntry)
		{
			return new LeaderboardEntry
			{
				name = nullEntry.name,
				score = nullEntry.score,
				rank = rank,
				id = 1UL,
				userIds = nullEntry.userIds
			};
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0006432C File Offset: 0x0006252C
		private List<NullLeaderboard> Read()
		{
			if (TestMode.IsOn)
			{
				return new List<NullLeaderboard>();
			}
			string text = this._fileIo.ReadFile("leaderboards.save");
			if (text == null)
			{
				return new List<NullLeaderboard>();
			}
			ReadSaveResult<NullLeaderboardFile> readSaveResult = JsonSerializationUtility.FromJson<NullLeaderboardFile>(text);
			if (readSaveResult.Status != ReadSaveStatus.Success)
			{
				return new List<NullLeaderboard>();
			}
			return readSaveResult.SaveData.leaderboards;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00064380 File Offset: 0x00062580
		private void Write(List<NullLeaderboard> leaderboards)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			string text = JsonSerializer.Serialize<NullLeaderboardFile>(new NullLeaderboardFile
			{
				leaderboards = leaderboards
			}, JsonSerializationUtility.GetTypeInfo<NullLeaderboardFile>());
			this._fileIo.WriteFile("leaderboards.save", text);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x000643C0 File Offset: 0x000625C0
		private async Task CheckRefreshLeaderboard([Nullable(2)] NullLeaderboardHandle handle, ulong id)
		{
			if (CommandLineHelper.HasArg("fastmp"))
			{
				await Task.Delay((int)(id * 0.5f));
				this._leaderboards = this.Read();
				if (handle != null)
				{
					handle.leaderboard = this._leaderboards.First<NullLeaderboard>((NullLeaderboard l) => l.name == handle.leaderboard.name);
				}
			}
		}

		// Token: 0x0400060F RID: 1551
		private readonly GodotFileIo _fileIo = new GodotFileIo(UserDataPathProvider.GetAccountScopedBasePath(null, null, null));

		// Token: 0x04000610 RID: 1552
		private List<NullLeaderboard> _leaderboards;
	}
}
