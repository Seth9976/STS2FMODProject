using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine
{
	// Token: 0x020004BE RID: 1214
	[NullableContext(1)]
	[Nullable(0)]
	public class ConditionalBranchState : MonsterState
	{
		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x060048D0 RID: 18640 RVA: 0x00225138 File Offset: 0x00223338
		public string BranchId { get; }

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x060048D1 RID: 18641 RVA: 0x00225140 File Offset: 0x00223340
		public override string Id
		{
			get
			{
				return this.BranchId;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x060048D2 RID: 18642 RVA: 0x00225148 File Offset: 0x00223348
		private List<ConditionalBranchState.ConditionalBranch> States { get; } = new List<ConditionalBranchState.ConditionalBranch>();

		// Token: 0x060048D3 RID: 18643 RVA: 0x00225150 File Offset: 0x00223350
		public ConditionalBranchState(string stateId)
		{
			this.BranchId = stateId;
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x0022516A File Offset: 0x0022336A
		public void AddState(MonsterState move, Func<bool> condition)
		{
			this.States.Add(new ConditionalBranchState.ConditionalBranch(move, condition));
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x00225180 File Offset: 0x00223380
		public override string GetNextState(Creature _, Rng __)
		{
			foreach (ConditionalBranchState.ConditionalBranch conditionalBranch in this.States)
			{
				if (conditionalBranch.Evaluate() > 0f)
				{
					return conditionalBranch.id;
				}
			}
			throw new InvalidOperationException("No valid next state found.");
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x002251F0 File Offset: 0x002233F0
		public override void RegisterStates(Dictionary<string, MonsterState> monsterStates)
		{
			monsterStates.Add(this.Id, this);
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x060048D7 RID: 18647 RVA: 0x002251FF File Offset: 0x002233FF
		public override bool ShouldAppearInLogs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x020019F3 RID: 6643
		[Nullable(0)]
		private readonly struct ConditionalBranch
		{
			// Token: 0x0600ABE5 RID: 44005 RVA: 0x003642FA File Offset: 0x003624FA
			public ConditionalBranch(MonsterState state, Func<bool> condition)
			{
				this.id = state.Id;
				this._conditionalLambda = condition;
			}

			// Token: 0x0600ABE6 RID: 44006 RVA: 0x0036430F File Offset: 0x0036250F
			public float Evaluate()
			{
				if (this._conditionalLambda != null)
				{
					return this._conditionalLambda() > false;
				}
				return 1f;
			}

			// Token: 0x04006661 RID: 26209
			public readonly string id;

			// Token: 0x04006662 RID: 26210
			private readonly Func<bool> _conditionalLambda;
		}
	}
}
