using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	/// <summary>
	/// Console commands which manipulate the player's current gold.
	/// </summary>
	// Token: 0x02000CF3 RID: 3315
	[NullableContext(1)]
	[Nullable(0)]
	public class GoldConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024D5 RID: 9429
		// (get) Token: 0x06008612 RID: 34322 RVA: 0x002D0543 File Offset: 0x002CE743
		public override string CmdName
		{
			get
			{
				return "gold";
			}
		}

		// Token: 0x170024D6 RID: 9430
		// (get) Token: 0x06008613 RID: 34323 RVA: 0x002D054A File Offset: 0x002CE74A
		public override string Args
		{
			get
			{
				return "<amount:int>";
			}
		}

		// Token: 0x170024D7 RID: 9431
		// (get) Token: 0x06008614 RID: 34324 RVA: 0x002D0551 File Offset: 0x002CE751
		public override string Description
		{
			get
			{
				return "Manipulate player gold. Cha-ching!";
			}
		}

		// Token: 0x170024D8 RID: 9432
		// (get) Token: 0x06008615 RID: 34325 RVA: 0x002D0558 File Offset: 0x002CE758
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008616 RID: 34326 RVA: 0x002D055C File Offset: 0x002CE75C
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length < 1)
			{
				return new CmdResult(false, "An amount is required");
			}
			int num;
			if (!int.TryParse(args[0], out num))
			{
				return new CmdResult(false, "First argument (the gold amount) must be an int.");
			}
			if (issuingPlayer == null || !RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run does not appear to be in progress");
			}
			Task task = PlayerCmd.GainGold(num, issuingPlayer, false);
			Task task2 = task;
			bool flag = true;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler.AppendLiteral("'");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("' gold added.");
			return new CmdResult(task2, flag, defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
