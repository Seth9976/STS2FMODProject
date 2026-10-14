using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CF6 RID: 3318
	[NullableContext(1)]
	[Nullable(0)]
	public class KillConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024E1 RID: 9441
		// (get) Token: 0x06008624 RID: 34340 RVA: 0x002D07F2 File Offset: 0x002CE9F2
		public override string CmdName
		{
			get
			{
				return "kill";
			}
		}

		// Token: 0x170024E2 RID: 9442
		// (get) Token: 0x06008625 RID: 34341 RVA: 0x002D07F9 File Offset: 0x002CE9F9
		public override string Args
		{
			get
			{
				return "<target-index:int>|'all'";
			}
		}

		// Token: 0x170024E3 RID: 9443
		// (get) Token: 0x06008626 RID: 34342 RVA: 0x002D0800 File Offset: 0x002CEA00
		public override string Description
		{
			get
			{
				return "Will kill one target if the index is given, or all if 'all', or the first if no arguments.";
			}
		}

		// Token: 0x170024E4 RID: 9444
		// (get) Token: 0x06008627 RID: 34343 RVA: 0x002D0807 File Offset: 0x002CEA07
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008628 RID: 34344 RVA: 0x002D080C File Offset: 0x002CEA0C
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "This doesn't appear to be a combat!");
			}
			List<Creature> list = new List<Creature>();
			IReadOnlyList<Creature> readOnlyList = CombatManager.Instance.DebugOnlyGetState().Enemies.ToList<Creature>();
			if (args.Length == 0)
			{
				list.Add(readOnlyList[0]);
			}
			else
			{
				if (args[0].Equals("all"))
				{
					using (IEnumerator<Creature> enumerator = readOnlyList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Creature creature = enumerator.Current;
							list.Add(creature);
						}
						goto IL_0114;
					}
				}
				int num;
				if (!int.TryParse(args[0], out num))
				{
					return new CmdResult(false, "Invalid argument '" + args[0] + "'. Use a target index or 'all'.");
				}
				if (num < 0 || num >= readOnlyList.Count)
				{
					bool flag = false;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid target index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(". Valid range: 0-");
					defaultInterpolatedStringHandler.AppendFormatted<int>(readOnlyList.Count - 1);
					return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				list.Add(readOnlyList[num]);
			}
			IL_0114:
			IEnumerable<string> enumerable = from c in list
				select c.Monster into m
				where m != null
				select m.Id.Entry.ToString();
			TaskHelper.RunSafely(this.DoKill(list));
			return new CmdResult(true, "Killed: [" + string.Join(",", enumerable) + "]");
		}

		// Token: 0x06008629 RID: 34345 RVA: 0x002D09D8 File Offset: 0x002CEBD8
		private async Task DoKill(List<Creature> toKill)
		{
			foreach (Creature creature in toKill)
			{
				await CreatureCmd.Kill(creature, false);
			}
			List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
			await CombatManager.Instance.CheckWinCondition();
		}

		// Token: 0x0600862A RID: 34346 RVA: 0x002D0A1C File Offset: 0x002CEC1C
		public unsafe override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				int num = 1;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount<string>(list, num);
				Span<string> span = CollectionsMarshal.AsSpan<string>(list);
				int num2 = 0;
				*span[num2] = "all";
				List<string> list2 = list;
				if (CombatManager.Instance.IsInProgress)
				{
					CombatState combatState = CombatManager.Instance.DebugOnlyGetState();
					IReadOnlyList<Creature> readOnlyList = ((combatState != null) ? combatState.Enemies : null);
					if (readOnlyList != null)
					{
						for (int i = 0; i < readOnlyList.Count; i++)
						{
							list2.Add(i.ToString());
						}
					}
				}
				return base.CompleteArgument(list2, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Argument, null);
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}
	}
}
