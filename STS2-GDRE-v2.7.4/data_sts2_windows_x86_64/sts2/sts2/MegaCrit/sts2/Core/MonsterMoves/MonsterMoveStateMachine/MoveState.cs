using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine
{
	// Token: 0x020004C1 RID: 1217
	[NullableContext(1)]
	[Nullable(0)]
	public class MoveState : MonsterState
	{
		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x060048E9 RID: 18665 RVA: 0x00225417 File Offset: 0x00223617
		// (set) Token: 0x060048EA RID: 18666 RVA: 0x0022541F File Offset: 0x0022361F
		public IReadOnlyList<AbstractIntent> Intents { get; private set; }

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x060048EB RID: 18667 RVA: 0x00225428 File Offset: 0x00223628
		public string StateId { get; }

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x060048EC RID: 18668 RVA: 0x00225430 File Offset: 0x00223630
		// (set) Token: 0x060048ED RID: 18669 RVA: 0x00225438 File Offset: 0x00223638
		public bool MustPerformOnceBeforeTransitioning { get; set; }

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x060048EE RID: 18670 RVA: 0x00225441 File Offset: 0x00223641
		// (set) Token: 0x060048EF RID: 18671 RVA: 0x00225449 File Offset: 0x00223649
		[Nullable(2)]
		public string FollowUpStateId
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x060048F0 RID: 18672 RVA: 0x00225452 File Offset: 0x00223652
		// (set) Token: 0x060048F1 RID: 18673 RVA: 0x0022545A File Offset: 0x0022365A
		[Nullable(2)]
		public MonsterState FollowUpState
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x060048F2 RID: 18674 RVA: 0x00225463 File Offset: 0x00223663
		public override bool CanTransitionAway
		{
			get
			{
				return !this.MustPerformOnceBeforeTransitioning || this._performedAtLeastOnce;
			}
		}

		// Token: 0x060048F3 RID: 18675 RVA: 0x00225475 File Offset: 0x00223675
		public MoveState()
		{
			string text = "UNSET_MOVE";
			Func<IReadOnlyList<Creature>, Task> func;
			if ((func = MoveState.<>O.<0>__UnsetMove) == null)
			{
				func = (MoveState.<>O.<0>__UnsetMove = new Func<IReadOnlyList<Creature>, Task>(MoveState.UnsetMove));
			}
			this..ctor(text, func, Array.Empty<AbstractIntent>());
		}

		// Token: 0x060048F4 RID: 18676 RVA: 0x002254A2 File Offset: 0x002236A2
		public MoveState(string stateId, Func<IReadOnlyList<Creature>, Task> onPerform, params AbstractIntent[] intents)
		{
			this._onPerform = onPerform;
			this.Intents = intents;
			this.StateId = stateId;
		}

		// Token: 0x060048F5 RID: 18677 RVA: 0x002254C0 File Offset: 0x002236C0
		public async Task PerformMove(IEnumerable<Creature> targets)
		{
			this._performedAtLeastOnce = true;
			Creature[] array = (targets as Creature[]) ?? targets.ToArray<Creature>();
			await this._onPerform(array);
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x0022550B File Offset: 0x0022370B
		public override void OnExitState()
		{
			this._performedAtLeastOnce = false;
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x00225514 File Offset: 0x00223714
		public override string GetNextState(Creature owner, Rng rng)
		{
			MonsterState followUpState = this.FollowUpState;
			string text = ((followUpState != null) ? followUpState.Id : null) ?? this.FollowUpStateId;
			if (text == null)
			{
				throw new InvalidOperationException("No valid followup state.");
			}
			return text;
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x00225541 File Offset: 0x00223741
		public override void RegisterStates(Dictionary<string, MonsterState> monsterStates)
		{
			monsterStates.Add(this.Id, this);
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x060048F9 RID: 18681 RVA: 0x00225550 File Offset: 0x00223750
		public override bool IsMove
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x060048FA RID: 18682 RVA: 0x00225553 File Offset: 0x00223753
		public override string Id
		{
			get
			{
				return this.StateId;
			}
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x0022555B File Offset: 0x0022375B
		private static Task UnsetMove(IEnumerable<Creature> c)
		{
			throw new InvalidOperationException("No move has been set for the monster");
		}

		// Token: 0x04001CA8 RID: 7336
		private bool _performedAtLeastOnce;

		// Token: 0x04001CA9 RID: 7337
		private readonly Func<IReadOnlyList<Creature>, Task> _onPerform;

		// Token: 0x020019F4 RID: 6644
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04006663 RID: 26211
			[Nullable(new byte[] { 0, 1, 1, 1 })]
			public static Func<IReadOnlyList<Creature>, Task> <0>__UnsetMove;
		}
	}
}
