using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	/// <summary>
	/// Console commands which causes the player to win the combat
	/// </summary>
	// Token: 0x02000D05 RID: 3333
	[NullableContext(1)]
	[Nullable(0)]
	public class WinConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x1700251F RID: 9503
		// (get) Token: 0x060086A0 RID: 34464 RVA: 0x002D2C14 File Offset: 0x002D0E14
		public override string CmdName
		{
			get
			{
				return "win";
			}
		}

		// Token: 0x17002520 RID: 9504
		// (get) Token: 0x060086A1 RID: 34465 RVA: 0x002D2C1B File Offset: 0x002D0E1B
		public override string Args
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17002521 RID: 9505
		// (get) Token: 0x060086A2 RID: 34466 RVA: 0x002D2C22 File Offset: 0x002D0E22
		public override string Description
		{
			get
			{
				return "You win the combat";
			}
		}

		// Token: 0x17002522 RID: 9506
		// (get) Token: 0x060086A3 RID: 34467 RVA: 0x002D2C29 File Offset: 0x002D0E29
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060086A4 RID: 34468 RVA: 0x002D2C2C File Offset: 0x002D0E2C
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "This doesn't appear to be a combat!");
			}
			List<Creature> list = CombatManager.Instance.DebugOnlyGetState().Enemies.ToList<Creature>();
			Task task = this.KillEnemies(list);
			IEnumerable<string> enumerable = from c in list
				select c.Monster into m
				where m != null
				select m.Id.Entry.ToString();
			return new CmdResult(task, true, "Killed: [" + string.Join(",", enumerable) + "]");
		}

		// Token: 0x060086A5 RID: 34469 RVA: 0x002D2D00 File Offset: 0x002D0F00
		private async Task KillEnemies(List<Creature> creatures)
		{
			foreach (Creature creature in creatures)
			{
				creature.RemoveAllPowersInternalExcept(null);
				await CreatureCmd.Kill(creature, false);
			}
			List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
			await CombatManager.Instance.CheckWinCondition();
		}
	}
}
