using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007DD RID: 2013
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Parafright : MonsterModel
	{
		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x0600637A RID: 25466 RVA: 0x002670D9 File Offset: 0x002652D9
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/obscura/obscura_hologram_attack";
			}
		}

		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x0600637B RID: 25467 RVA: 0x002670E0 File Offset: 0x002652E0
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/obscura/obscura_hologram_die";
			}
		}

		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x0600637C RID: 25468 RVA: 0x002670E7 File Offset: 0x002652E7
		public override int MinInitialHp
		{
			get
			{
				return 21;
			}
		}

		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x0600637D RID: 25469 RVA: 0x002670EB File Offset: 0x002652EB
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x0600637E RID: 25470 RVA: 0x002670F3 File Offset: 0x002652F3
		private int SlamDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 16);
			}
		}

		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x0600637F RID: 25471 RVA: 0x00267100 File Offset: 0x00265300
		public override bool HasDeathSfx
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x06006380 RID: 25472 RVA: 0x00267103 File Offset: 0x00265303
		public override bool ShouldDisappearFromDoom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006381 RID: 25473 RVA: 0x00267108 File Offset: 0x00265308
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<IllusionPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x06006382 RID: 25474 RVA: 0x0026714C File Offset: 0x0026534C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SLAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SlamMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SlamDamage)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006383 RID: 25475 RVA: 0x002671A0 File Offset: 0x002653A0
		private async Task SlamMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SlamDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006384 RID: 25476 RVA: 0x002671E4 File Offset: 0x002653E4
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack", false);
			AnimState animState3 = new AnimState("die", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("hurt_stunned", false);
			AnimState animState6 = new AnimState("stunned_loop", true);
			AnimState animState7 = new AnimState("spawn", false);
			AnimState animState8 = new AnimState("wake_up", false);
			AnimState animState9 = new AnimState("stun", false);
			animState7.NextState = animState;
			animState2.NextState = animState;
			animState4.NextState = animState;
			animState8.NextState = animState;
			animState9.NextState = animState6;
			animState5.NextState = animState6;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState7, controller);
			creatureAnimator.AddAnyState("Attack", animState2, null);
			creatureAnimator.AddAnyState("Hit", animState4, () => !base.Creature.GetPower<IllusionPower>().IsReviving);
			creatureAnimator.AddAnyState("Hit", animState5, () => base.Creature.GetPower<IllusionPower>().IsReviving);
			creatureAnimator.AddAnyState("StunTrigger", animState9, null);
			creatureAnimator.AddAnyState("WakeUpTrigger", animState8, null);
			creatureAnimator.AddAnyState("Dead", animState3, () => !base.CombatState.GetTeammatesOf(base.Creature).Any<Creature>((Creature t) => t != null && t.IsPrimaryEnemy && t.IsAlive));
			return creatureAnimator;
		}

		// Token: 0x0400264F RID: 9807
		public const string healSfx = "event:/sfx/enemy/enemy_attacks/obscura/obscura_hologram_heal";
	}
}
