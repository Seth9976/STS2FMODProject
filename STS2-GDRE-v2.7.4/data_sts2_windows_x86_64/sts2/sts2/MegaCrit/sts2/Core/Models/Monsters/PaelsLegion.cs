using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007DC RID: 2012
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsLegion : MonsterModel
	{
		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x06006373 RID: 25459 RVA: 0x00266F83 File Offset: 0x00265183
		public override int MinInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x06006374 RID: 25460 RVA: 0x00266F8A File Offset: 0x0026518A
		public override int MaxInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x06006375 RID: 25461 RVA: 0x00266F91 File Offset: 0x00265191
		public override bool IsHealthBarVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006376 RID: 25462 RVA: 0x00266F94 File Offset: 0x00265194
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			string text;
			if (base.IsMutable)
			{
				text = base.Creature.PetOwner.GetRelic<PaelsLegion>().Skin;
			}
			else
			{
				text = PaelsLegion.SkinOptions[0];
			}
			MegaSkeletonDataResource data = skeleton.GetData();
			skeleton.SetSkin(data.FindSkin(text));
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06006377 RID: 25463 RVA: 0x00266FE4 File Offset: 0x002651E4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			MoveState moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, Array.Empty<AbstractIntent>());
			moveState.FollowUpState = moveState;
			return new MonsterMoveStateMachine(new <>z__ReadOnlySingleElementList<MonsterState>(moveState), moveState);
		}

		// Token: 0x06006378 RID: 25464 RVA: 0x00267034 File Offset: 0x00265234
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("block", false);
			AnimState animState3 = new AnimState("block_loop", false);
			AnimState animState4 = new AnimState("sleep", false);
			AnimState animState5 = new AnimState("wake_up", false);
			animState5.NextState = animState;
			animState2.NextState = animState3;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("BlockTrigger", animState2, null);
			creatureAnimator.AddAnyState("SleepTrigger", animState4, null);
			creatureAnimator.AddAnyState("WakeUpTrigger", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x0400264C RID: 9804
		public const string blockTrigger = "BlockTrigger";

		// Token: 0x0400264D RID: 9805
		public const string sleepTrigger = "SleepTrigger";

		// Token: 0x0400264E RID: 9806
		public const string wakeUpTrigger = "WakeUpTrigger";
	}
}
