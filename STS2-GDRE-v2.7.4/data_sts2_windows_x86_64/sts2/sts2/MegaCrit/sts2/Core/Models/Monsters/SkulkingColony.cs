using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
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
	// Token: 0x020007E8 RID: 2024
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SkulkingColony : MonsterModel
	{
		// Token: 0x17001822 RID: 6178
		// (get) Token: 0x0600643F RID: 25663 RVA: 0x002694C3 File Offset: 0x002676C3
		protected override bool HasPhobiaSpineSkin
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001823 RID: 6179
		// (get) Token: 0x06006440 RID: 25664 RVA: 0x002694C6 File Offset: 0x002676C6
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_hurt";
			}
		}

		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x06006441 RID: 25665 RVA: 0x002694CD File Offset: 0x002676CD
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 80, 75);
			}
		}

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x06006442 RID: 25666 RVA: 0x002694D9 File Offset: 0x002676D9
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x06006443 RID: 25667 RVA: 0x002694E1 File Offset: 0x002676E1
		private int InertiaDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 9);
			}
		}

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x06006444 RID: 25668 RVA: 0x002694EE File Offset: 0x002676EE
		private int ZoomDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
			}
		}

		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x06006445 RID: 25669 RVA: 0x002694FB File Offset: 0x002676FB
		private int PiercingStabsDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x06006446 RID: 25670 RVA: 0x00269506 File Offset: 0x00267706
		private int PiercingStabsRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x06006447 RID: 25671 RVA: 0x00269509 File Offset: 0x00267709
		private int InertiaStrengthGain
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 2);
			}
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x06006448 RID: 25672 RVA: 0x00269514 File Offset: 0x00267714
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Stone;
			}
		}

		// Token: 0x06006449 RID: 25673 RVA: 0x00269518 File Offset: 0x00267718
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<HardenedShellPower>(new ThrowingPlayerChoiceContext(), base.Creature, 20m, base.Creature, null, false);
		}

		// Token: 0x0600644A RID: 25674 RVA: 0x0026955C File Offset: 0x0026775C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("ZOOM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ZoomMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ZoomDamage)
			});
			MoveState moveState2 = new MoveState("ZOOM_MOVE_2", new Func<IReadOnlyList<Creature>, Task>(this.ZoomMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ZoomDamage)
			});
			MoveState moveState3 = new MoveState("INERTIA_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.InertiaMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.InertiaDamage),
				new BuffIntent()
			});
			MoveState moveState4 = new MoveState("PIERCING_STABS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PiercingStabsMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.PiercingStabsDamage, this.PiercingStabsRepeat)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600644B RID: 25675 RVA: 0x0026966C File Offset: 0x0026786C
		private async Task InertiaMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.InertiaDamage).FromMonster(this).WithAttackerAnim("AttackBuff", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_thrust", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.InertiaStrengthGain, base.Creature, null, false);
		}

		// Token: 0x0600644C RID: 25676 RVA: 0x002696B0 File Offset: 0x002678B0
		private async Task PiercingStabsMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.PiercingStabsDamage).WithHitCount(this.PiercingStabsRepeat).OnlyPlayAnimOnce()
				.FromMonster(this)
				.WithAttackerAnim("AttackDouble", 0.45f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_spin", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600644D RID: 25677 RVA: 0x002696F4 File Offset: 0x002678F4
		private async Task ZoomMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ZoomDamage).FromMonster(this).WithAttackerAnim("AttackHeavy", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_kick", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600644E RID: 25678 RVA: 0x00269738 File Offset: 0x00267938
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack_buff", false);
			AnimState animState4 = new AnimState("attack_heavy", false);
			AnimState animState5 = new AnimState("attack_double", false);
			AnimState animState6 = new AnimState("hurt", false);
			AnimState animState7 = new AnimState("die", false);
			animState2.NextState = animState;
			animState6.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("AttackBuff", animState3, null);
			creatureAnimator.AddAnyState("AttackHeavy", animState4, null);
			creatureAnimator.AddAnyState("AttackDouble", animState5, null);
			creatureAnimator.AddAnyState("Dead", animState7, null);
			creatureAnimator.AddAnyState("Hit", animState6, null);
			return creatureAnimator;
		}

		// Token: 0x0400267F RID: 9855
		private const string _attackDoubleTrigger = "AttackDouble";

		// Token: 0x04002680 RID: 9856
		private const string _attackBuffTrigger = "AttackBuff";

		// Token: 0x04002681 RID: 9857
		private const string _attackHeavyTrigger = "AttackHeavy";

		// Token: 0x04002682 RID: 9858
		private const string _kickSfx = "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_kick";

		// Token: 0x04002683 RID: 9859
		private const string _spinSfx = "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_spin";

		// Token: 0x04002684 RID: 9860
		private const string _slapSfx = "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_slap";

		// Token: 0x04002685 RID: 9861
		private const string _thrustSfx = "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_thrust";
	}
}
