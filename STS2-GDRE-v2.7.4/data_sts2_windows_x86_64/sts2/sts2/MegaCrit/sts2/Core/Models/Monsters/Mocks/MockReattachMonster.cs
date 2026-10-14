using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters.Mocks
{
	// Token: 0x02000812 RID: 2066
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockReattachMonster : MonsterModel
	{
		// Token: 0x17001953 RID: 6483
		// (get) Token: 0x06006716 RID: 26390 RVA: 0x002719FE File Offset: 0x0026FBFE
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001954 RID: 6484
		// (get) Token: 0x06006717 RID: 26391 RVA: 0x00271A01 File Offset: 0x0026FC01
		public override LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");
			}
		}

		// Token: 0x17001955 RID: 6485
		// (get) Token: 0x06006718 RID: 26392 RVA: 0x00271A0D File Offset: 0x0026FC0D
		public override int MinInitialHp
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001956 RID: 6486
		// (get) Token: 0x06006719 RID: 26393 RVA: 0x00271A10 File Offset: 0x0026FC10
		public override int MaxInitialHp
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001957 RID: 6487
		// (get) Token: 0x0600671A RID: 26394 RVA: 0x00271A13 File Offset: 0x0026FC13
		// (set) Token: 0x0600671B RID: 26395 RVA: 0x00271A1B File Offset: 0x0026FC1B
		public MoveState DeadState
		{
			get
			{
				return this._deadState;
			}
			private set
			{
				base.AssertMutable();
				this._deadState = value;
			}
		}

		// Token: 0x0600671C RID: 26396 RVA: 0x00271A2C File Offset: 0x0026FC2C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<ReattachPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x0600671D RID: 26397 RVA: 0x00271A70 File Offset: 0x0026FC70
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("NOTHING", new Func<IReadOnlyList<Creature>, Task>(this.NothingMove), new AbstractIntent[]
			{
				new HiddenIntent()
			});
			moveState.FollowUpState = moveState;
			MoveState moveState2 = new MoveState("REATTACH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReattachMove), new AbstractIntent[]
			{
				new HealIntent()
			})
			{
				MustPerformOnceBeforeTransitioning = true,
				FollowUpState = moveState
			};
			this.DeadState = new MoveState("DEAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.NothingMove), Array.Empty<AbstractIntent>())
			{
				FollowUpState = moveState2
			};
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(this.DeadState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600671E RID: 26398 RVA: 0x00271B2B File Offset: 0x0026FD2B
		private Task NothingMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}

		// Token: 0x0600671F RID: 26399 RVA: 0x00271B34 File Offset: 0x0026FD34
		private async Task ReattachMove(IReadOnlyList<Creature> targets)
		{
			await base.Creature.GetPower<ReattachPower>().DoReattach();
		}

		// Token: 0x0400274E RID: 10062
		private MoveState _deadState;
	}
}
