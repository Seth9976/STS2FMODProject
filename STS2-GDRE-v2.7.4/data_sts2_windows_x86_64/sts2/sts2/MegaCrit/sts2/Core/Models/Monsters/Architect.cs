using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000795 RID: 1941
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Architect : MonsterModel
	{
		// Token: 0x17001607 RID: 5639
		// (get) Token: 0x06005F45 RID: 24389 RVA: 0x0025A845 File Offset: 0x00258A45
		public override int MinInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x06005F46 RID: 24390 RVA: 0x0025A84C File Offset: 0x00258A4C
		public override int MaxInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x06005F47 RID: 24391 RVA: 0x0025A854 File Offset: 0x00258A54
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("NOTHING", (IReadOnlyList<Creature> _) => Task.CompletedTask, new AbstractIntent[]
			{
				new HiddenIntent()
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005F48 RID: 24392 RVA: 0x0025A8B4 File Offset: 0x00258AB4
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack", false);
			AnimState animState3 = new AnimState("hurt", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Attack", animState2, null);
			creatureAnimator.AddAnyState("Hit", animState3, null);
			return creatureAnimator;
		}
	}
}
