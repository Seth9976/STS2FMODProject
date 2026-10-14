using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CE6 RID: 3302
	[NullableContext(1)]
	[Nullable(0)]
	public class BlockConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x1700249E RID: 9374
		// (get) Token: 0x060085AC RID: 34220 RVA: 0x002CE4AB File Offset: 0x002CC6AB
		public override string CmdName
		{
			get
			{
				return "block";
			}
		}

		// Token: 0x1700249F RID: 9375
		// (get) Token: 0x060085AD RID: 34221 RVA: 0x002CE4B2 File Offset: 0x002CC6B2
		public override string Args
		{
			get
			{
				return "<int:amount> <target-index:int>";
			}
		}

		// Token: 0x170024A0 RID: 9376
		// (get) Token: 0x060085AE RID: 34222 RVA: 0x002CE4B9 File Offset: 0x002CC6B9
		public override string Description
		{
			get
			{
				return "Gives block to player, or to target creature if index is given (0 is player).";
			}
		}

		// Token: 0x170024A1 RID: 9377
		// (get) Token: 0x060085AF RID: 34223 RVA: 0x002CE4C0 File Offset: 0x002CC6C0
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060085B0 RID: 34224 RVA: 0x002CE4C4 File Offset: 0x002CC6C4
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (issuingPlayer == null)
			{
				return new CmdResult(false, "This command only works during a run.");
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "This doesn't appear to be a combat!");
			}
			int num = args.Length;
			bool flag = num < 1 || num > 2;
			if (flag)
			{
				return new CmdResult(false, "There must be 1 or 2 args.");
			}
			int num2;
			if (!int.TryParse(args[0], out num2))
			{
				return new CmdResult(false, "Arg 1 must be the amount of block.");
			}
			if (num2 < 0)
			{
				return new CmdResult(false, "Removing block is not supported.");
			}
			Creature creature = issuingPlayer.Creature;
			Creature creature2;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (args.Length == 1)
			{
				creature2 = creature;
			}
			else
			{
				int num3;
				if (!int.TryParse(args[1], out num3))
				{
					return new CmdResult(false, "Arg 2 must be the target index if specified.");
				}
				IReadOnlyList<Creature> creatures = creature.CombatState.Creatures;
				if (num3 < 0 || num3 >= creatures.Count)
				{
					bool flag2 = false;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid target index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num3);
					defaultInterpolatedStringHandler.AppendLiteral(". Valid range: 0-");
					defaultInterpolatedStringHandler.AppendFormatted<int>(creatures.Count - 1);
					return new CmdResult(flag2, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				creature2 = creatures[num3];
			}
			Task task = BlockConsoleCmd.GainBlock(creature2, num2);
			Task task2 = task;
			bool flag3 = true;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Added '");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
			defaultInterpolatedStringHandler.AppendLiteral("' block to ");
			defaultInterpolatedStringHandler.AppendFormatted<Creature>(creature2);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			return new CmdResult(task2, flag3, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060085B1 RID: 34225 RVA: 0x002CE63C File Offset: 0x002CC83C
		private static async Task GainBlock(Creature target, int amount)
		{
			await CreatureCmd.GainBlock(target, new BlockVar(amount, ValueProp.Unpowered), null, false);
		}
	}
}
