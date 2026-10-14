using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine
{
	// Token: 0x020004C0 RID: 1216
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class MonsterState
	{
		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x060048E0 RID: 18656
		public abstract string Id { get; }

		// Token: 0x060048E1 RID: 18657
		public abstract string GetNextState(Creature owner, Rng rng);

		// Token: 0x060048E2 RID: 18658
		public abstract void RegisterStates(Dictionary<string, MonsterState> monsterStates);

		// Token: 0x060048E3 RID: 18659 RVA: 0x002253FA File Offset: 0x002235FA
		public virtual void OnEnterState()
		{
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x002253FC File Offset: 0x002235FC
		public virtual void OnExitState()
		{
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x060048E5 RID: 18661 RVA: 0x002253FE File Offset: 0x002235FE
		public virtual bool ShouldAppearInLogs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x060048E6 RID: 18662 RVA: 0x00225401 File Offset: 0x00223601
		public virtual bool CanTransitionAway
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x060048E7 RID: 18663 RVA: 0x00225404 File Offset: 0x00223604
		public virtual bool IsMove
		{
			get
			{
				return this is MoveState;
			}
		}
	}
}
