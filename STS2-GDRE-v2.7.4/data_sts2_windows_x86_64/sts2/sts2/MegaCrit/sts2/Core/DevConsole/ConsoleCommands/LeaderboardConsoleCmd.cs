using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CF7 RID: 3319
	[NullableContext(1)]
	[Nullable(0)]
	public class LeaderboardConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024E5 RID: 9445
		// (get) Token: 0x0600862C RID: 34348 RVA: 0x002D0AE2 File Offset: 0x002CECE2
		public override string CmdName
		{
			get
			{
				return "leaderboard";
			}
		}

		// Token: 0x170024E6 RID: 9446
		// (get) Token: 0x0600862D RID: 34349 RVA: 0x002D0AE9 File Offset: 0x002CECE9
		public override string Args
		{
			get
			{
				return "[option:string] [name:string] <score:int> [count:int]";
			}
		}

		// Token: 0x170024E7 RID: 9447
		// (get) Token: 0x0600862E RID: 34350 RVA: 0x002D0AF0 File Offset: 0x002CECF0
		public override string Description
		{
			get
			{
				return "Adds scores to the leaderboard. Option can be upload|random. If random, <count> random scores will be uploaded to leaderboard <name>. If upload, one score will be uploaded to leaderboard <name> with <score>.";
			}
		}

		// Token: 0x170024E8 RID: 9448
		// (get) Token: 0x0600862F RID: 34351 RVA: 0x002D0AF7 File Offset: 0x002CECF7
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008630 RID: 34352 RVA: 0x002D0AFC File Offset: 0x002CECFC
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length == 0)
			{
				return new CmdResult(false, "Option must be specified (random or upload).");
			}
			string text = args[0];
			string text2 = DailyRunUtility.GetLeaderboardName(DateTimeOffset.Now, 1);
			if (args.Length > 1 && args[1] != "-")
			{
				text2 = args[1];
			}
			if (text == "random")
			{
				int num = 100;
				if (args.Length >= 3 && !int.TryParse(args[2], out num))
				{
					return new CmdResult(false, "Count must be a valid integer.");
				}
				TaskHelper.RunSafely(this.UploadRandomScores(text2, num));
				bool flag = true;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Adding ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" entries to leaderboard ");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else
			{
				if (!(text == "upload"))
				{
					return new CmdResult(false, "Invalid option " + text + ", must be upload or random");
				}
				if (args.Length < 3)
				{
					return new CmdResult(false, "Score must be specified when upload is the option.\nUse - as the leaderboard name for the default.");
				}
				int num2;
				if (!int.TryParse(args[2], out num2))
				{
					return new CmdResult(false, "Score must be a valid integer.");
				}
				TaskHelper.RunSafely(this.UploadScore(text2, num2));
				bool flag2 = true;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Uploading score ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
				defaultInterpolatedStringHandler.AppendLiteral(" to leaderboard ");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				return new CmdResult(flag2, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06008631 RID: 34353 RVA: 0x002D0C5C File Offset: 0x002CEE5C
		private async Task UploadRandomScores(string leaderboardName, int count)
		{
			ILeaderboardHandle leaderboardHandle = await LeaderboardManager.GetOrCreateLeaderboard(leaderboardName, default(CancellationToken));
			ILeaderboardHandle leaderboardHandle2 = leaderboardHandle;
			Rng rng = new Rng(Rng.Chaotic.NextUnsignedLong());
			for (int i = 0; i < count; i++)
			{
				LeaderboardManager.DebugAddEntry(leaderboardHandle2, new LeaderboardEntry
				{
					id = (ulong)rng.NextUnsignedInt(uint.MaxValue),
					name = rng.NextUnsignedInt(uint.MaxValue).ToString(),
					score = rng.NextInt(int.MaxValue)
				});
			}
		}

		// Token: 0x06008632 RID: 34354 RVA: 0x002D0CA8 File Offset: 0x002CEEA8
		private async Task UploadScore(string leaderboardName, int score)
		{
			ILeaderboardHandle leaderboardHandle = await LeaderboardManager.GetOrCreateLeaderboard(leaderboardName, default(CancellationToken));
			ILeaderboardHandle leaderboardHandle2 = leaderboardHandle;
			await LeaderboardManager.UploadLocalScore(leaderboardHandle2, score, Array.Empty<ulong>());
		}

		// Token: 0x06008633 RID: 34355 RVA: 0x002D0CF4 File Offset: 0x002CEEF4
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				string partial = ((args.Length != 0) ? args[0] : "");
				List<string> list;
				if (string.IsNullOrWhiteSpace(partial))
				{
					list = LeaderboardConsoleCmd._validOptions;
				}
				else
				{
					list = LeaderboardConsoleCmd._validOptions.Where<string>((string option) => option.Contains(partial, StringComparison.OrdinalIgnoreCase)).ToList<string>();
				}
				return new CompletionResult
				{
					Candidates = list,
					Type = CompletionType.Argument,
					ArgumentContext = this.CmdName
				};
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}

		// Token: 0x06008635 RID: 34357 RVA: 0x002D0D94 File Offset: 0x002CEF94
		// Note: this type is marked as 'beforefieldinit'.
		unsafe static LeaderboardConsoleCmd()
		{
			int num = 2;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = "upload";
			num2++;
			*span[num2] = "random";
			LeaderboardConsoleCmd._validOptions = list;
		}

		// Token: 0x04002E6B RID: 11883
		private static readonly List<string> _validOptions;
	}
}
