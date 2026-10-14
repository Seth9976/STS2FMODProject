using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CE0 RID: 3296
	[NullableContext(1)]
	[Nullable(0)]
	public class ActConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x17002484 RID: 9348
		// (get) Token: 0x06008580 RID: 34176 RVA: 0x002CD510 File Offset: 0x002CB710
		public override string CmdName
		{
			get
			{
				return "act";
			}
		}

		// Token: 0x17002485 RID: 9349
		// (get) Token: 0x06008581 RID: 34177 RVA: 0x002CD517 File Offset: 0x002CB717
		public override string Args
		{
			get
			{
				return "<int|string: act>";
			}
		}

		// Token: 0x17002486 RID: 9350
		// (get) Token: 0x06008582 RID: 34178 RVA: 0x002CD51E File Offset: 0x002CB71E
		public override string Description
		{
			get
			{
				return "Jumps to an act. If integer, will jump to that act. Otherwise, replaces the current act with the act passed.";
			}
		}

		// Token: 0x17002487 RID: 9351
		// (get) Token: 0x06008583 RID: 34179 RVA: 0x002CD525 File Offset: 0x002CB725
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008584 RID: 34180 RVA: 0x002CD528 File Offset: 0x002CB728
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length != 1)
			{
				return new CmdResult(false, "There must be one argument.");
			}
			if (((issuingPlayer != null) ? issuingPlayer.RunState : null) == null)
			{
				return new CmdResult(false, "This command only works during a run.");
			}
			int num;
			if (int.TryParse(args[0], out num))
			{
				int count = issuingPlayer.RunState.Acts.Count;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (num > count || num < 1)
				{
					bool flag = false;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(84, 1);
					defaultInterpolatedStringHandler.AppendLiteral("The act you are trying to navigate to does not exist. Select act indexes between: 1-");
					defaultInterpolatedStringHandler.AppendFormatted<int>(count);
					return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int num2 = num - 1;
				Task task = ActConsoleCmd.NextAct(num2);
				Task task2 = task;
				bool flag2 = true;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Navigated to act '");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				return new CmdResult(task2, flag2, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else
			{
				string actName = args[0].ToUpperInvariant();
				ActModel actModel = ModelDb.Acts.FirstOrDefault<ActModel>((ActModel c) => c.Id.Entry == actName);
				if (actModel == null)
				{
					return new CmdResult(false, "Act named " + actName + " not found.");
				}
				RunState runState = (RunState)issuingPlayer.RunState;
				actModel = actModel.ToMutable();
				runState.SetActDebug(actModel);
				actModel.GenerateRooms(runState.Rng.UpFront, runState.UnlockState, runState.Players.Count > 1);
				Task task3 = RunManager.Instance.EnterAct(runState.CurrentActIndex, true);
				return new CmdResult(task3, true, "Replaced current act with act " + actName + ".");
			}
		}

		// Token: 0x06008585 RID: 34181 RVA: 0x002CD6C4 File Offset: 0x002CB8C4
		private static async Task NextAct(int actIndex)
		{
			NMapScreen.Instance.SetTravelEnabled(true);
			await RunManager.Instance.EnterAct(actIndex, true);
		}
	}
}
