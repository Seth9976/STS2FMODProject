using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x0200079D RID: 1949
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BowlbugEgg : MonsterModel
	{
		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x06005F8A RID: 24458 RVA: 0x0025B3C4 File Offset: 0x002595C4
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 23, 21);
			}
		}

		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x06005F8B RID: 24459 RVA: 0x0025B3D0 File Offset: 0x002595D0
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 24, 22);
			}
		}

		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x06005F8C RID: 24460 RVA: 0x0025B3DC File Offset: 0x002595DC
		private int BiteDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x06005F8D RID: 24461 RVA: 0x0025B3E7 File Offset: 0x002595E7
		private int ProtectBlock
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x06005F8E RID: 24462 RVA: 0x0025B3F2 File Offset: 0x002595F2
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/workbug_egg/workbug_egg_die";
			}
		}

		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x06005F8F RID: 24463 RVA: 0x0025B3F9 File Offset: 0x002595F9
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/workbug_egg/workbug_egg_attack";
			}
		}

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x06005F90 RID: 24464 RVA: 0x0025B400 File Offset: 0x00259600
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Insect;
			}
		}

		// Token: 0x06005F91 RID: 24465 RVA: 0x0025B404 File Offset: 0x00259604
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			skeleton.SetSkin(skeleton.GetData().FindSkin("cocoon"));
			skeleton.SetSlotsToSetupPose();
			Node node = spine.BoundObject as Node;
			Node node2 = ((node != null) ? node.GetNodeOrNull("CocoonSlotNode/Cocoon") : null);
			if (node2 != null)
			{
				MegaSprite megaSprite = new MegaSprite(node2);
				MegaSkeleton skeleton2 = megaSprite.GetSkeleton();
				if (skeleton2 != null)
				{
					skeleton2.SetSkin(skeleton2.GetData().FindSkin("egg1"));
					megaSprite.GetAnimationState().SetAnimation("egg_idle_loop", true, 0);
					skeleton2.SetSlotsToSetupPose();
				}
			}
		}

		// Token: 0x06005F92 RID: 24466 RVA: 0x0025B498 File Offset: 0x00259698
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
		}

		// Token: 0x06005F93 RID: 24467 RVA: 0x0025B4DC File Offset: 0x002596DC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BiteDamage),
				new DefendIntent()
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005F94 RID: 24468 RVA: 0x0025B538 File Offset: 0x00259738
		private async Task BiteMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BiteDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, this.ProtectBlock, ValueProp.Move, null, false);
		}

		// Token: 0x06005F95 RID: 24469 RVA: 0x0025B57C File Offset: 0x0025977C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("spit", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			return creatureAnimator;
		}
	}
}
