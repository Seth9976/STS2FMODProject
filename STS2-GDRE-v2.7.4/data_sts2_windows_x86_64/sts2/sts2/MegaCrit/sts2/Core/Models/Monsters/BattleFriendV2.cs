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
	// Token: 0x0200079A RID: 1946
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BattleFriendV2 : MonsterModel
	{
		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x06005F75 RID: 24437 RVA: 0x0025B090 File Offset: 0x00259290
		public override int MinInitialHp
		{
			get
			{
				return 150;
			}
		}

		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x06005F76 RID: 24438 RVA: 0x0025B097 File Offset: 0x00259297
		public override int MaxInitialHp
		{
			get
			{
				return 150;
			}
		}

		// Token: 0x06005F77 RID: 24439 RVA: 0x0025B0A0 File Offset: 0x002592A0
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkeletonDataResource data = skeleton.GetData();
			skeleton.SetSkin(data.FindSkin("v2"));
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06005F78 RID: 24440 RVA: 0x0025B0CC File Offset: 0x002592CC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			MoveState moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, Array.Empty<AbstractIntent>());
			moveState.FollowUpState = moveState;
			return new MonsterMoveStateMachine(new <>z__ReadOnlySingleElementList<MonsterState>(moveState), moveState);
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x0025B11C File Offset: 0x0025931C
		public override async Task AfterAddedToRoom()
		{
			await PowerCmd.Apply<BattlewornDummyTimeLimitPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, null, null, false);
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x0025B160 File Offset: 0x00259360
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
