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
	// Token: 0x0200079B RID: 1947
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BattleFriendV3 : MonsterModel
	{
		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x06005F7C RID: 24444 RVA: 0x0025B1E8 File Offset: 0x002593E8
		public override int MinInitialHp
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x06005F7D RID: 24445 RVA: 0x0025B1EF File Offset: 0x002593EF
		public override int MaxInitialHp
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x06005F7E RID: 24446 RVA: 0x0025B1F8 File Offset: 0x002593F8
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkeletonDataResource data = skeleton.GetData();
			skeleton.SetSkin(data.FindSkin("v3"));
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06005F7F RID: 24447 RVA: 0x0025B224 File Offset: 0x00259424
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			MoveState moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, Array.Empty<AbstractIntent>());
			moveState.FollowUpState = moveState;
			return new MonsterMoveStateMachine(new <>z__ReadOnlySingleElementList<MonsterState>(moveState), moveState);
		}

		// Token: 0x06005F80 RID: 24448 RVA: 0x0025B274 File Offset: 0x00259474
		public override async Task AfterAddedToRoom()
		{
			await PowerCmd.Apply<BattlewornDummyTimeLimitPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, null, null, false);
		}

		// Token: 0x06005F81 RID: 24449 RVA: 0x0025B2B8 File Offset: 0x002594B8
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
