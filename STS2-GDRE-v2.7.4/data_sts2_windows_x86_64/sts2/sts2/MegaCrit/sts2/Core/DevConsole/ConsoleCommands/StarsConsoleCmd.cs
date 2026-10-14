using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000D00 RID: 3328
	[NullableContext(1)]
	[Nullable(0)]
	public class StarsConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x1700250B RID: 9483
		// (get) Token: 0x06008676 RID: 34422 RVA: 0x002D1EFB File Offset: 0x002D00FB
		public override string CmdName
		{
			get
			{
				return "stars";
			}
		}

		// Token: 0x1700250C RID: 9484
		// (get) Token: 0x06008677 RID: 34423 RVA: 0x002D1F02 File Offset: 0x002D0102
		public override string Args
		{
			get
			{
				return "<amount:int>";
			}
		}

		// Token: 0x1700250D RID: 9485
		// (get) Token: 0x06008678 RID: 34424 RVA: 0x002D1F09 File Offset: 0x002D0109
		public override string Description
		{
			get
			{
				return "Adds stars to player";
			}
		}

		// Token: 0x1700250E RID: 9486
		// (get) Token: 0x06008679 RID: 34425 RVA: 0x002D1F10 File Offset: 0x002D0110
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600867A RID: 34426 RVA: 0x002D1F14 File Offset: 0x002D0114
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length == 0)
			{
				return new CmdResult(false, "The first argument must be an int.");
			}
			int num;
			if (!int.TryParse(args[0], out num))
			{
				return new CmdResult(false, "The first argument must be an int.");
			}
			if (issuingPlayer == null)
			{
				return new CmdResult(false, "This command only works during a run.");
			}
			if (issuingPlayer.PlayerCombatState == null)
			{
				return new CmdResult(false, "This command only works in combat.");
			}
			if (num < 0)
			{
				return new CmdResult(false, "The stars amount cannot be negative.");
			}
			Task task = PlayerCmd.GainStars(num, issuingPlayer);
			Task task2 = task;
			bool flag = true;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Added '");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("' stars.");
			return new CmdResult(task2, flag, defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
