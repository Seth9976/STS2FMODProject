using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CE3 RID: 3299
	[NullableContext(1)]
	[Nullable(0)]
	public class ApplyPowerConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x17002490 RID: 9360
		// (get) Token: 0x06008595 RID: 34197 RVA: 0x002CDBA7 File Offset: 0x002CBDA7
		public override string CmdName
		{
			get
			{
				return "power";
			}
		}

		// Token: 0x17002491 RID: 9361
		// (get) Token: 0x06008596 RID: 34198 RVA: 0x002CDBAE File Offset: 0x002CBDAE
		public override string Args
		{
			get
			{
				return "<id:string> <amount:int> <target-index:int>";
			}
		}

		// Token: 0x17002492 RID: 9362
		// (get) Token: 0x06008597 RID: 34199 RVA: 0x002CDBB5 File Offset: 0x002CBDB5
		public override string Description
		{
			get
			{
				return "Grant power to given target at index.";
			}
		}

		// Token: 0x17002493 RID: 9363
		// (get) Token: 0x06008598 RID: 34200 RVA: 0x002CDBBC File Offset: 0x002CBDBC
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002494 RID: 9364
		// (get) Token: 0x06008599 RID: 34201 RVA: 0x002CDBC0 File Offset: 0x002CBDC0
		private static IEnumerable<PowerModel> AllPowers
		{
			get
			{
				if (ApplyPowerConsoleCmd._allPowers == null)
				{
					IEnumerable<Type> enumerable = ModelDb.AllAbstractModelSubtypes.Where<Type>((Type t) => t.IsSubclassOf(typeof(PowerModel)));
					Func<Type, PowerModel> func;
					if ((func = ApplyPowerConsoleCmd.<>O.<0>__DebugPower) == null)
					{
						func = (ApplyPowerConsoleCmd.<>O.<0>__DebugPower = new Func<Type, PowerModel>(ModelDb.DebugPower));
					}
					ApplyPowerConsoleCmd._allPowers = enumerable.Select<Type, PowerModel>(func).ToList<PowerModel>();
				}
				return ApplyPowerConsoleCmd._allPowers;
			}
		}

		// Token: 0x0600859A RID: 34202 RVA: 0x002CDC2C File Offset: 0x002CBE2C
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "This doesn't appear to be a combat!");
			}
			if (args.Length < 3)
			{
				return new CmdResult(false, "There must be 3 args.");
			}
			int num;
			if (!int.TryParse(args[1], out num))
			{
				return new CmdResult(false, "Arg 1 must be the amount of power to be applied.");
			}
			string powerId = args[0].ToUpperInvariant();
			PowerModel power = ApplyPowerConsoleCmd.AllPowers.FirstOrDefault<PowerModel>((PowerModel c) => c.Id.Entry == powerId);
			if (power == null)
			{
				return new CmdResult(false, "The power id " + powerId + " does not exist.");
			}
			int num2;
			if (!int.TryParse(args[2], out num2))
			{
				return new CmdResult(false, "Arg 2 must be the target index if specified.");
			}
			IReadOnlyList<Creature> creatures = CombatManager.Instance.DebugOnlyGetState().Creatures;
			if (num2 < 0 || num2 >= creatures.Count)
			{
				bool flag = false;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid target index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
				defaultInterpolatedStringHandler.AppendLiteral(". Valid range: 0-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(creatures.Count - 1);
				return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Creature creature = creatures[num2];
			PowerModel powerModel = creature.Powers.FirstOrDefault<PowerModel>((PowerModel p) => p.GetType() == power.GetType());
			PlayerChoiceContext playerChoiceContext = new BlockingPlayerChoiceContext();
			Task task;
			if (power.InstanceType != PowerInstanceType.None || powerModel == null)
			{
				task = PowerCmd.Apply(playerChoiceContext, power.ToMutable(0), creature, num, null, null, false);
			}
			else
			{
				task = PowerCmd.ModifyAmount(playerChoiceContext, powerModel, num, null, null, false);
			}
			Task task2 = task;
			bool flag2 = true;
			string text = "AppliedPower: [";
			string text2 = ",";
			string text3 = (creature.IsPlayer ? "PLAYER" : creature.Monster.Id.Entry);
			return new CmdResult(task2, flag2, text + string.Join(text2, new ReadOnlySpan<string>(ref text3)) + "]");
		}

		// Token: 0x0600859B RID: 34203 RVA: 0x002CDE08 File Offset: 0x002CC008
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = ApplyPowerConsoleCmd.AllPowers.Select<PowerModel, string>((PowerModel p) => p.Id.Entry).ToList<string>();
				return base.CompleteArgument(list, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Argument, null);
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}

		// Token: 0x04002E62 RID: 11874
		[Nullable(new byte[] { 2, 1 })]
		private static List<PowerModel> _allPowers;

		// Token: 0x020023E1 RID: 9185
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040097C3 RID: 38851
			[Nullable(0)]
			public static Func<Type, PowerModel> <0>__DebugPower;
		}
	}
}
