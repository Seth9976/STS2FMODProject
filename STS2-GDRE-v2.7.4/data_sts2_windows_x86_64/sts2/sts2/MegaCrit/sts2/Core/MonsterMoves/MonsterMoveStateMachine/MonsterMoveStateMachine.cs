using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine
{
	// Token: 0x020004BF RID: 1215
	[NullableContext(1)]
	[Nullable(0)]
	public class MonsterMoveStateMachine
	{
		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x060048D8 RID: 18648 RVA: 0x00225202 File Offset: 0x00223402
		public Dictionary<string, MonsterState> States { get; } = new Dictionary<string, MonsterState>();

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x060048D9 RID: 18649 RVA: 0x0022520A File Offset: 0x0022340A
		public List<MonsterState> StateLog { get; } = new List<MonsterState>();

		// Token: 0x060048DA RID: 18650 RVA: 0x00225214 File Offset: 0x00223414
		public MonsterMoveStateMachine(IEnumerable<MonsterState> states, MonsterState initialState)
		{
			foreach (MonsterState monsterState in states)
			{
				monsterState.RegisterStates(this.States);
			}
			this._initialState = initialState;
			this._currentState = this._initialState;
			if (this._currentState.ShouldAppearInLogs)
			{
				this.StateLog.Add(this._currentState);
			}
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x002252B0 File Offset: 0x002234B0
		public MoveState RollMove(IEnumerable<Creature> targets, Creature owner, Rng rng)
		{
			this.FindNextMoveState(targets, owner, rng, true);
			if (!this._currentState.IsMove)
			{
				throw new InvalidOperationException(this._currentState.Id + " is not a valid move state");
			}
			return (MoveState)this._currentState;
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x002252EF File Offset: 0x002234EF
		public void ForceCurrentState(MonsterState state)
		{
			this.SetCurrentState(state);
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x002252F8 File Offset: 0x002234F8
		public void OnMovePerformed(MoveState _)
		{
			this._performedFirstMove = true;
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x00225304 File Offset: 0x00223504
		private void FindNextMoveState(IEnumerable<Creature> targets, Creature owner, Rng rng, bool logMove)
		{
			if (this._currentState == null)
			{
				throw new InvalidOperationException("Cannot find next move state when current state is null.");
			}
			if (!this._currentState.CanTransitionAway)
			{
				return;
			}
			if (!this._performedFirstMove && this._currentState.IsMove)
			{
				return;
			}
			MonsterState monsterState = null;
			string nextState;
			for (;;)
			{
				nextState = this._currentState.GetNextState(owner, rng);
				if (!string.IsNullOrEmpty(nextState) && !this.States.ContainsKey(nextState))
				{
					break;
				}
				this.SetCurrentState(string.IsNullOrEmpty(nextState) ? this._initialState : this.States[nextState]);
				monsterState = ((monsterState == null && this._currentState.ShouldAppearInLogs) ? this._currentState : monsterState);
				if (this._currentState.IsMove)
				{
					goto Block_9;
				}
			}
			throw new InvalidOperationException("no valid state found: " + nextState);
			Block_9:
			if (logMove && monsterState != null)
			{
				this.StateLog.Add(monsterState);
			}
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x002253DB File Offset: 0x002235DB
		private void SetCurrentState(MonsterState state)
		{
			this._currentState.OnExitState();
			this._currentState = state;
			this._currentState.OnEnterState();
		}

		// Token: 0x04001CA3 RID: 7331
		private MonsterState _currentState;

		// Token: 0x04001CA4 RID: 7332
		private readonly MonsterState _initialState;

		// Token: 0x04001CA5 RID: 7333
		private bool _performedFirstMove;
	}
}
