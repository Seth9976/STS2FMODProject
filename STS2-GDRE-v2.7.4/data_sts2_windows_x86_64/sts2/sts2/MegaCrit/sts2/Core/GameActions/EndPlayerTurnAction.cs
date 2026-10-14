using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
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
	// Token: 0x02000C1F RID: 3103
	[NullableContext(1)]
	[Nullable(0)]
	public class EndPlayerTurnAction : GameAction
	{
		// Token: 0x17002312 RID: 8978
		// (get) Token: 0x06008021 RID: 32801 RVA: 0x002B7DBC File Offset: 0x002B5FBC
		public override ulong OwnerId
		{
			get
			{
				return this._player.NetId;
			}
		}

		// Token: 0x17002313 RID: 8979
		// (get) Token: 0x06008022 RID: 32802 RVA: 0x002B7DC9 File Offset: 0x002B5FC9
		public override GameActionType ActionType
		{
			get
			{
				return GameActionType.CombatPlayPhaseOnly;
			}
		}

		// Token: 0x06008023 RID: 32803 RVA: 0x002B7DCC File Offset: 0x002B5FCC
		public EndPlayerTurnAction(Player player, int turnNumber)
		{
			this._player = player;
			this._turnNumber = turnNumber;
		}

		// Token: 0x06008024 RID: 32804 RVA: 0x002B7DE4 File Offset: 0x002B5FE4
		protected override Task ExecuteAction()
		{
			int turnNumber = this._player.PlayerCombatState.TurnNumber;
			if (turnNumber == this._turnNumber)
			{
				PlayerCmd.EndTurn(this._player, true, null);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(82, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Ignoring end turn action. Current turn number: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(turnNumber);
				defaultInterpolatedStringHandler.AppendLiteral(" action turn number: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._turnNumber);
				defaultInterpolatedStringHandler.AppendLiteral(" CombatState: ");
				defaultInterpolatedStringHandler.AppendFormatted<ActionSynchronizerCombatState>(RunManager.Instance.ActionQueueSynchronizer.CombatState);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06008025 RID: 32805 RVA: 0x002B7E88 File Offset: 0x002B6088
		public override INetAction ToNetAction()
		{
			return new NetEndPlayerTurnAction
			{
				turnNumber = this._turnNumber
			};
		}

		// Token: 0x06008026 RID: 32806 RVA: 0x002B7EB0 File Offset: 0x002B60B0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 3);
			defaultInterpolatedStringHandler.AppendFormatted("EndPlayerTurnAction");
			defaultInterpolatedStringHandler.AppendLiteral(" for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" turn ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._turnNumber);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002ADD RID: 10973
		private readonly Player _player;

		// Token: 0x04002ADE RID: 10974
		private readonly int _turnNumber;
	}
}
