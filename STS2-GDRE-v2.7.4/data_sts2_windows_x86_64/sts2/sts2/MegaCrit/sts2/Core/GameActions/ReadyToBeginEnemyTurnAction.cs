using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.GameActions
{
	/// <summary>
	/// This action indicates that the player is ready to proceed to phase two of the end of the turn, where player state
	/// cleanup and switching to the next side occurs. It comes after EndPlayerTurnAction.
	/// Note that this action is automatically enqueued without player intervention.
	/// </summary>
	// Token: 0x02000C2A RID: 3114
	[NullableContext(1)]
	[Nullable(0)]
	public class ReadyToBeginEnemyTurnAction : GameAction
	{
		// Token: 0x1700232F RID: 9007
		// (get) Token: 0x060080A0 RID: 32928 RVA: 0x002B9BF2 File Offset: 0x002B7DF2
		public override ulong OwnerId
		{
			get
			{
				return this._player.NetId;
			}
		}

		// Token: 0x17002330 RID: 9008
		// (get) Token: 0x060080A1 RID: 32929 RVA: 0x002B9BFF File Offset: 0x002B7DFF
		public override GameActionType ActionType
		{
			get
			{
				return GameActionType.Combat;
			}
		}

		// Token: 0x060080A2 RID: 32930 RVA: 0x002B9C02 File Offset: 0x002B7E02
		public ReadyToBeginEnemyTurnAction(Player player, [Nullable(new byte[] { 2, 1 })] Func<Task> actionDuringEnemyTurn = null)
		{
			this._player = player;
			this._actionDuringEnemyTurn = actionDuringEnemyTurn;
		}

		// Token: 0x060080A3 RID: 32931 RVA: 0x002B9C18 File Offset: 0x002B7E18
		protected override Task ExecuteAction()
		{
			CombatManager.Instance.SetReadyToBeginEnemyTurn(this._player, this._actionDuringEnemyTurn);
			return Task.CompletedTask;
		}

		// Token: 0x060080A4 RID: 32932 RVA: 0x002B9C38 File Offset: 0x002B7E38
		public override INetAction ToNetAction()
		{
			return default(NetReadyToBeginEnemyTurnAction);
		}

		// Token: 0x060080A5 RID: 32933 RVA: 0x002B9C54 File Offset: 0x002B7E54
		protected override void CancelAction()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cancel\n");
			defaultInterpolatedStringHandler.AppendFormatted<StackTrace>(new StackTrace());
			Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x060080A6 RID: 32934 RVA: 0x002B9C90 File Offset: 0x002B7E90
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted("ReadyToBeginEnemyTurnAction");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002B0C RID: 11020
		private readonly Player _player;

		// Token: 0x04002B0D RID: 11021
		[Nullable(new byte[] { 2, 1 })]
		private readonly Func<Task> _actionDuringEnemyTurn;
	}
}
