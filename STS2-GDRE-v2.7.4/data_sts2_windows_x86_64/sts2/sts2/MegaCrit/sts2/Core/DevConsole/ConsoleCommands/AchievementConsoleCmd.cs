using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CDF RID: 3295
	[NullableContext(1)]
	[Nullable(0)]
	public class AchievementConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x17002480 RID: 9344
		// (get) Token: 0x06008579 RID: 34169 RVA: 0x002CD155 File Offset: 0x002CB355
		public override string CmdName
		{
			get
			{
				return "achievement";
			}
		}

		// Token: 0x17002481 RID: 9345
		// (get) Token: 0x0600857A RID: 34170 RVA: 0x002CD15C File Offset: 0x002CB35C
		public override string Args
		{
			get
			{
				return "<operation:string> [id:string]";
			}
		}

		// Token: 0x17002482 RID: 9346
		// (get) Token: 0x0600857B RID: 34171 RVA: 0x002CD163 File Offset: 0x002CB363
		public override string Description
		{
			get
			{
				return "Unlocks or revokes an achievement. If no achievement is provided, all achievements are unlocked or revoked.";
			}
		}

		// Token: 0x17002483 RID: 9347
		// (get) Token: 0x0600857C RID: 34172 RVA: 0x002CD16A File Offset: 0x002CB36A
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600857D RID: 34173 RVA: 0x002CD170 File Offset: 0x002CB370
		public unsafe override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			int num = 3;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = "unlock";
			num2++;
			*span[num2] = "revoke";
			num2++;
			*span[num2] = "check";
			List<string> list2 = list;
			if (args.Length < 1 || !list2.Contains(args[0]))
			{
				return new CmdResult(false, "First argument must be one of: " + string.Join(",", list2) + ".\n" + this.Args);
			}
			Achievement? achievement = null;
			if (args.Length >= 2)
			{
				string text = args[1].ToLowerInvariant();
				foreach (Achievement achievement2 in Enum.GetValues<Achievement>())
				{
					if (StringHelper.SnakeCase(achievement2.ToString()) == text)
					{
						achievement = new Achievement?(achievement2);
						break;
					}
				}
				if (achievement == null)
				{
					return new CmdResult(false, "Achievement '" + args[1] + "' unrecognized.\n" + this.Args);
				}
			}
			if (args[0] == "unlock")
			{
				if (achievement != null)
				{
					AchievementsUtil.Unlock(achievement.Value, issuingPlayer);
					bool flag = true;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unlocked ");
					defaultInterpolatedStringHandler.AppendFormatted<Achievement>(achievement.Value);
					return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				foreach (Achievement achievement3 in Enum.GetValues<Achievement>())
				{
					AchievementsUtil.Unlock(achievement3, issuingPlayer);
				}
				return new CmdResult(true, "Unlocked all achievements");
			}
			else if (args[0] == "revoke")
			{
				if (achievement != null)
				{
					AchievementsUtil.Revoke(achievement.Value);
					bool flag2 = true;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Revoked ");
					defaultInterpolatedStringHandler.AppendFormatted<Achievement>(achievement.Value);
					return new CmdResult(flag2, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				foreach (Achievement achievement4 in Enum.GetValues<Achievement>())
				{
					AchievementsUtil.Revoke(achievement4);
				}
				return new CmdResult(true, "Revoked all achievements");
			}
			else
			{
				if (!(args[0] == "check"))
				{
					throw new NotImplementedException();
				}
				if (achievement == null)
				{
					return new CmdResult(false, "Achievement name must be provided");
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (AchievementsUtil.IsUnlocked(achievement.Value))
				{
					bool flag3 = true;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
					defaultInterpolatedStringHandler.AppendFormatted<Achievement>(achievement.Value);
					defaultInterpolatedStringHandler.AppendLiteral(" is unlocked");
					return new CmdResult(flag3, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag4 = true;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Achievement>(achievement.Value);
				defaultInterpolatedStringHandler.AppendLiteral(" is not unlocked");
				return new CmdResult(flag4, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x0600857E RID: 34174 RVA: 0x002CD444 File Offset: 0x002CB644
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = new List<string> { "unlock", "revoke", "check" };
				return base.CompleteArgument(list, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Subcommand, null);
			}
			if (args.Length == 2)
			{
				List<string> list2 = (from a in Enum.GetValues<Achievement>()
					select StringHelper.SnakeCase(a.ToString())).ToList<string>();
				list2.Sort();
				return base.CompleteArgument(list2, new string[] { args[0] }, args[1], CompletionType.Argument, null);
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}
	}
}
