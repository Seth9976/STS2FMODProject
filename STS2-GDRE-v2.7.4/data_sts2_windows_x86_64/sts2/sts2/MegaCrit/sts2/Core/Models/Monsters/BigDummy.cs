using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x0200079C RID: 1948
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BigDummy : MonsterModel
	{
		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x06005F83 RID: 24451 RVA: 0x0025B340 File Offset: 0x00259540
		public override LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");
			}
		}

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x06005F84 RID: 24452 RVA: 0x0025B34C File Offset: 0x0025954C
		protected override string VisualsPath
		{
			get
			{
				return SceneHelper.GetScenePath("creature_visuals/defect");
			}
		}

		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x06005F85 RID: 24453 RVA: 0x0025B358 File Offset: 0x00259558
		public override int MinInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x06005F86 RID: 24454 RVA: 0x0025B35F File Offset: 0x0025955F
		public override int MaxInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x06005F87 RID: 24455 RVA: 0x0025B368 File Offset: 0x00259568
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("NOTHING", new Func<IReadOnlyList<Creature>, Task>(this.NothingMove), new AbstractIntent[]
			{
				new HiddenIntent()
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005F88 RID: 24456 RVA: 0x0025B3B5 File Offset: 0x002595B5
		private Task NothingMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}
	}
}
