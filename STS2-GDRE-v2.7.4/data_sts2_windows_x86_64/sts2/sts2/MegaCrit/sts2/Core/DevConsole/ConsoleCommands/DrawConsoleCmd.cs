using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CEB RID: 3307
	[NullableContext(1)]
	[Nullable(0)]
	public class DrawConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024B3 RID: 9395
		// (get) Token: 0x060085CF RID: 34255 RVA: 0x002CEC89 File Offset: 0x002CCE89
		public override string CmdName
		{
			get
			{
				return "draw";
			}
		}

		// Token: 0x170024B4 RID: 9396
		// (get) Token: 0x060085D0 RID: 34256 RVA: 0x002CEC90 File Offset: 0x002CCE90
		public override string Args
		{
			get
			{
				return "<count:int>";
			}
		}

		// Token: 0x170024B5 RID: 9397
		// (get) Token: 0x060085D1 RID: 34257 RVA: 0x002CEC97 File Offset: 0x002CCE97
		public override string Description
		{
			get
			{
				return "Draw X many cards.";
			}
		}

		// Token: 0x170024B6 RID: 9398
		// (get) Token: 0x060085D2 RID: 34258 RVA: 0x002CEC9E File Offset: 0x002CCE9E
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060085D3 RID: 34259 RVA: 0x002CECA4 File Offset: 0x002CCEA4
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			int num;
			if (args.Length == 0)
			{
				num = 1;
			}
			else
			{
				int num2;
				if (!int.TryParse(args[0], out num2))
				{
					return new CmdResult(false, "First argument is not an int");
				}
				num = num2;
			}
			if (num <= 0)
			{
				return new CmdResult(false, "Draw nothing?");
			}
			if (!RunManager.Instance.IsInProgress || issuingPlayer == null)
			{
				return new CmdResult(false, "A run hasn't started");
			}
			Task task = this.DrawTask(issuingPlayer, num);
			Task task2 = task;
			bool flag = true;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Drawn '");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("' cards.");
			return new CmdResult(task2, flag, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060085D4 RID: 34260 RVA: 0x002CED44 File Offset: 0x002CCF44
		private async Task DrawTask(Player player, int count)
		{
			HookPlayerChoiceContext hookPlayerChoiceContext = new HookPlayerChoiceContext(player, LocalContext.NetId.Value, GameActionType.Combat);
			Task task = CardPileCmd.Draw(hookPlayerChoiceContext, count, player, false);
			await hookPlayerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(task);
		}
	}
}
