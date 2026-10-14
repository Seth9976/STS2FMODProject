using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CEE RID: 3310
	[NullableContext(1)]
	[Nullable(0)]
	public class EnergyConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024BF RID: 9407
		// (get) Token: 0x060085E3 RID: 34275 RVA: 0x002CF044 File Offset: 0x002CD244
		public override string CmdName
		{
			get
			{
				return "energy";
			}
		}

		// Token: 0x170024C0 RID: 9408
		// (get) Token: 0x060085E4 RID: 34276 RVA: 0x002CF04B File Offset: 0x002CD24B
		public override string Args
		{
			get
			{
				return "<amount:int>";
			}
		}

		// Token: 0x170024C1 RID: 9409
		// (get) Token: 0x060085E5 RID: 34277 RVA: 0x002CF052 File Offset: 0x002CD252
		public override string Description
		{
			get
			{
				return "Adds energy to player";
			}
		}

		// Token: 0x170024C2 RID: 9410
		// (get) Token: 0x060085E6 RID: 34278 RVA: 0x002CF059 File Offset: 0x002CD259
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060085E7 RID: 34279 RVA: 0x002CF05C File Offset: 0x002CD25C
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
				return new CmdResult(false, "The energy amount cannot be negative.");
			}
			Task task = PlayerCmd.GainEnergy(num, issuingPlayer);
			Task task2 = task;
			bool flag = true;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Added '");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("' energy.");
			return new CmdResult(task2, flag, defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
