using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	/// <summary>
	/// Console commands which manipulate the player's current health.
	/// </summary>
	// Token: 0x02000CF4 RID: 3316
	[NullableContext(1)]
	[Nullable(0)]
	public class HealConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024D9 RID: 9433
		// (get) Token: 0x06008618 RID: 34328 RVA: 0x002D05FC File Offset: 0x002CE7FC
		public override string CmdName
		{
			get
			{
				return "heal";
			}
		}

		// Token: 0x170024DA RID: 9434
		// (get) Token: 0x06008619 RID: 34329 RVA: 0x002D0603 File Offset: 0x002CE803
		public override string Args
		{
			get
			{
				return "<amount:int> [index:int]";
			}
		}

		// Token: 0x170024DB RID: 9435
		// (get) Token: 0x0600861A RID: 34330 RVA: 0x002D060A File Offset: 0x002CE80A
		public override string Description
		{
			get
			{
				return "Heal the player some amount of HP.";
			}
		}

		// Token: 0x170024DC RID: 9436
		// (get) Token: 0x0600861B RID: 34331 RVA: 0x002D0611 File Offset: 0x002CE811
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600861C RID: 34332 RVA: 0x002D0614 File Offset: 0x002CE814
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length < 1)
			{
				return new CmdResult(false, "An amount is required");
			}
			int num;
			if (!int.TryParse(args[0], out num))
			{
				return new CmdResult(false, "First argument (the heal amount) must be an int.");
			}
			if (!RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run does not appear to be in progress");
			}
			if (num < 0)
			{
				return new CmdResult(false, "The heal amount cannot be negative.");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			Creature creature;
			if (args.Length > 1)
			{
				int num2;
				if (!int.TryParse(args[1], out num2))
				{
					return new CmdResult(false, "Arg 2 must be the target index (int), got '" + args[1] + "'.");
				}
				IReadOnlyList<Creature> allies = CombatManager.Instance.DebugOnlyGetState().Allies;
				if (num2 < 0 || num2 >= allies.Count)
				{
					bool flag = false;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid target index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
					defaultInterpolatedStringHandler.AppendLiteral(". Valid range: 0-");
					defaultInterpolatedStringHandler.AppendFormatted<int>(allies.Count - 1);
					return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				creature = allies[num2];
			}
			else
			{
				creature = issuingPlayer.Creature;
			}
			Task task = CreatureCmd.Heal(creature, num, true);
			Task task2 = task;
			bool flag2 = true;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Healed '");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("' HP to ");
			defaultInterpolatedStringHandler.AppendFormatted<Creature>(creature);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			return new CmdResult(task2, flag2, defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
