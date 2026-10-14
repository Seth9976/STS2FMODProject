using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.GameActions
{
	/// <summary>
	/// This action is enqueued when the player presses the "End Turn" button in the UI. It begins the process of ending a
	/// player's turn. Notes:
	/// * This action only marks the player as ready to end turn, and encapsulates only that action. It does not block
	///   the queue while turns are ending.
	/// * SwitchSidesAction is enqueued after this one, and acts as a synchronization point for players who are ready to
	///   enter phase 2 of turn ending.
	/// </summary>
	// Token: 0x02000C2C RID: 3116
	[NullableContext(1)]
	[Nullable(0)]
	public class UndoEndPlayerTurnAction : GameAction
	{
		// Token: 0x17002331 RID: 9009
		// (get) Token: 0x060080AB RID: 32939 RVA: 0x002B9CEB File Offset: 0x002B7EEB
		public override ulong OwnerId
		{
			get
			{
				return this._player.NetId;
			}
		}

		// Token: 0x17002332 RID: 9010
		// (get) Token: 0x060080AC RID: 32940 RVA: 0x002B9CF8 File Offset: 0x002B7EF8
		public override GameActionType ActionType
		{
			get
			{
				return GameActionType.CombatPlayPhaseOnly;
			}
		}

		// Token: 0x060080AD RID: 32941 RVA: 0x002B9CFB File Offset: 0x002B7EFB
		public UndoEndPlayerTurnAction(Player player, int turnNumber)
		{
			this._player = player;
			this._turnNumber = turnNumber;
		}

		// Token: 0x060080AE RID: 32942 RVA: 0x002B9D14 File Offset: 0x002B7F14
		protected override Task ExecuteAction()
		{
			int turnNumber = this._player.PlayerCombatState.TurnNumber;
			if (turnNumber == this._turnNumber)
			{
				CombatManager.Instance.UndoReadyToEndTurn(this._player);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(87, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Ignoring undo end turn action. Current turn number: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(turnNumber);
				defaultInterpolatedStringHandler.AppendLiteral(" action turn number: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._turnNumber);
				defaultInterpolatedStringHandler.AppendLiteral(" CombatState: ");
				defaultInterpolatedStringHandler.AppendFormatted<ActionSynchronizerCombatState>(RunManager.Instance.ActionQueueSynchronizer.CombatState);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			return Task.CompletedTask;
		}

		// Token: 0x060080AF RID: 32943 RVA: 0x002B9DB8 File Offset: 0x002B7FB8
		public override INetAction ToNetAction()
		{
			return new NetUndoEndPlayerTurnAction
			{
				turnNumber = this._turnNumber
			};
		}

		// Token: 0x060080B0 RID: 32944 RVA: 0x002B9DE0 File Offset: 0x002B7FE0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 3);
			defaultInterpolatedStringHandler.AppendFormatted("UndoEndPlayerTurnAction");
			defaultInterpolatedStringHandler.AppendLiteral(" for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" turn ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._turnNumber);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002B0E RID: 11022
		private readonly Player _player;

		// Token: 0x04002B0F RID: 11023
		private readonly int _turnNumber;
	}
}
