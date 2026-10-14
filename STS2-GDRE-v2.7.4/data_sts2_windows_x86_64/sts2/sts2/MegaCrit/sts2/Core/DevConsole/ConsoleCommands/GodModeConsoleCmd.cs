using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	/// <summary>
	/// Console commands which gives the player godmode.
	/// </summary>
	// Token: 0x02000CF2 RID: 3314
	[NullableContext(1)]
	[Nullable(0)]
	public class GodModeConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024D1 RID: 9425
		// (get) Token: 0x06008609 RID: 34313 RVA: 0x002D03B0 File Offset: 0x002CE5B0
		public override string CmdName
		{
			get
			{
				return "godmode";
			}
		}

		// Token: 0x170024D2 RID: 9426
		// (get) Token: 0x0600860A RID: 34314 RVA: 0x002D03B7 File Offset: 0x002CE5B7
		public override string Args
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170024D3 RID: 9427
		// (get) Token: 0x0600860B RID: 34315 RVA: 0x002D03BE File Offset: 0x002CE5BE
		public override string Description
		{
			get
			{
				return "Become invincible!";
			}
		}

		// Token: 0x170024D4 RID: 9428
		// (get) Token: 0x0600860C RID: 34316 RVA: 0x002D03C5 File Offset: 0x002CE5C5
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600860D RID: 34317 RVA: 0x002D03C8 File Offset: 0x002CE5C8
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (issuingPlayer == null || !RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run does not appear to be in progress");
			}
			Task task;
			if (this._godModeActive)
			{
				this._godModeActive = false;
				CombatManager.Instance.CombatSetUp -= this.OnCombatSetUp;
				this._godModePlayer = null;
				task = GodModeConsoleCmd.DisableGodMode(issuingPlayer);
				return new CmdResult(task, true, "Godmode deactivated!");
			}
			this._godModeActive = true;
			this._godModePlayer = issuingPlayer;
			CombatManager.Instance.CombatSetUp += this.OnCombatSetUp;
			task = GodModeConsoleCmd.EnableGodMode(issuingPlayer);
			return new CmdResult(task, true, "Godmode activated!");
		}

		// Token: 0x0600860E RID: 34318 RVA: 0x002D0468 File Offset: 0x002CE668
		private void OnCombatSetUp(CombatState combatState)
		{
			if (this._godModePlayer == null || !RunManager.Instance.IsInProgress)
			{
				return;
			}
			Player me = LocalContext.GetMe(combatState.RunState);
			if (me != this._godModePlayer)
			{
				return;
			}
			TaskHelper.RunSafely(GodModeConsoleCmd.EnableGodMode(this._godModePlayer));
		}

		// Token: 0x0600860F RID: 34319 RVA: 0x002D04B4 File Offset: 0x002CE6B4
		private static async Task DisableGodMode(Player player)
		{
			Creature playerCreature = player.Creature;
			await PowerCmd.Remove<StrengthPower>(playerCreature);
			await PowerCmd.Remove<BufferPower>(playerCreature);
			await PowerCmd.Remove<RegenPower>(playerCreature);
		}

		// Token: 0x06008610 RID: 34320 RVA: 0x002D04F8 File Offset: 0x002CE6F8
		private static async Task EnableGodMode(Player player)
		{
			Creature playerCreature = player.Creature;
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), playerCreature, 999999999m, playerCreature, null, false);
			await PowerCmd.Apply<BufferPower>(new ThrowingPlayerChoiceContext(), playerCreature, 999999999m, playerCreature, null, false);
			await PowerCmd.Apply<RegenPower>(new ThrowingPlayerChoiceContext(), playerCreature, 999999999m, playerCreature, null, false);
		}

		// Token: 0x04002E69 RID: 11881
		private bool _godModeActive;

		// Token: 0x04002E6A RID: 11882
		[Nullable(2)]
		private Player _godModePlayer;
	}
}
