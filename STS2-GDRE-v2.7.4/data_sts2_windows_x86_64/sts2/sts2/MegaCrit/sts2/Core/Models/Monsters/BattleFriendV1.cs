using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000799 RID: 1945
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BattleFriendV1 : MonsterModel
	{
		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x06005F6E RID: 24430 RVA: 0x0025AF3F File Offset: 0x0025913F
		public override int MinInitialHp
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x06005F6F RID: 24431 RVA: 0x0025AF43 File Offset: 0x00259143
		public override int MaxInitialHp
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x06005F70 RID: 24432 RVA: 0x0025AF48 File Offset: 0x00259148
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkeletonDataResource data = skeleton.GetData();
			skeleton.SetSkin(data.FindSkin("v1"));
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06005F71 RID: 24433 RVA: 0x0025AF74 File Offset: 0x00259174
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			MoveState moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, Array.Empty<AbstractIntent>());
			moveState.FollowUpState = moveState;
			return new MonsterMoveStateMachine(new <>z__ReadOnlySingleElementList<MonsterState>(moveState), moveState);
		}

		// Token: 0x06005F72 RID: 24434 RVA: 0x0025AFC4 File Offset: 0x002591C4
		public override async Task AfterAddedToRoom()
		{
			await PowerCmd.Apply<BattlewornDummyTimeLimitPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, null, null, false);
		}

		// Token: 0x06005F73 RID: 24435 RVA: 0x0025B008 File Offset: 0x00259208
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("hurt", false);
			AnimState animState3 = new AnimState("die", false);
			AnimState animState4 = new AnimState("die_loop", true);
			animState2.NextState = animState;
			animState3.NextState = animState4;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Dead", animState3, null);
			creatureAnimator.AddAnyState("Hit", animState2, null);
			return creatureAnimator;
		}
	}
}
