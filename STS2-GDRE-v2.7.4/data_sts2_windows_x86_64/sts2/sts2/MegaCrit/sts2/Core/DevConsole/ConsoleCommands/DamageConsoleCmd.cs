using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CE9 RID: 3305
	[NullableContext(1)]
	[Nullable(0)]
	public class DamageConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024AB RID: 9387
		// (get) Token: 0x060085C2 RID: 34242 RVA: 0x002CEA42 File Offset: 0x002CCC42
		public override string CmdName
		{
			get
			{
				return "damage";
			}
		}

		// Token: 0x170024AC RID: 9388
		// (get) Token: 0x060085C3 RID: 34243 RVA: 0x002CEA49 File Offset: 0x002CCC49
		public override string Args
		{
			get
			{
				return "<amount:int> <target-index:int>";
			}
		}

		// Token: 0x170024AD RID: 9389
		// (get) Token: 0x060085C4 RID: 34244 RVA: 0x002CEA50 File Offset: 0x002CCC50
		public override string Description
		{
			get
			{
				return "Damage all enemies, or target creature if index is given (0 is player).";
			}
		}

		// Token: 0x170024AE RID: 9390
		// (get) Token: 0x060085C5 RID: 34245 RVA: 0x002CEA57 File Offset: 0x002CCC57
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060085C6 RID: 34246 RVA: 0x002CEA5C File Offset: 0x002CCC5C
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
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
				return new CmdResult(false, "Arg 1 must be the amount of damage.");
			}
			if (num2 < 0)
			{
				return new CmdResult(false, "The damage amount cannot be negative.");
			}
			CombatState combatState = CombatManager.Instance.DebugOnlyGetState();
			IEnumerable<Creature> enumerable;
			if (args.Length < 2)
			{
				enumerable = combatState.Enemies;
			}
			else
			{
				int num3;
				if (!int.TryParse(args[1], out num3))
				{
					return new CmdResult(false, "Arg 2 must be the target index if specified.");
				}
				if (num3 < 0 || num3 >= combatState.Creatures.Count)
				{
					bool flag2 = false;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid target index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num3);
					defaultInterpolatedStringHandler.AppendLiteral(". Valid range: 0-");
					defaultInterpolatedStringHandler.AppendFormatted<int>(combatState.Creatures.Count - 1);
					return new CmdResult(flag2, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				enumerable = new <>z__ReadOnlySingleElementList<Creature>(combatState.Creatures[num3]);
			}
			IEnumerable<string> enumerable2 = enumerable.Select<Creature, string>(delegate(Creature c)
			{
				if (!c.IsPlayer)
				{
					return c.Monster.Id.Entry;
				}
				return "PLAYER";
			});
			Task task = this.DamageAndCheckWinCondition(enumerable, num2);
			return new CmdResult(task, true, "Damaged: [" + string.Join(",", enumerable2) + "]");
		}

		// Token: 0x060085C7 RID: 34247 RVA: 0x002CEBD4 File Offset: 0x002CCDD4
		private async Task DamageAndCheckWinCondition(IEnumerable<Creature> creatures, decimal amount)
		{
			await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), creatures.ToList<Creature>(), amount, ValueProp.Unpowered, null, null, null);
			await CombatManager.Instance.CheckWinCondition();
		}
	}
}
