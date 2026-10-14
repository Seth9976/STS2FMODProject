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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007E0 RID: 2016
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PunchConstruct : MonsterModel
	{
		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x060063B4 RID: 25524 RVA: 0x00267B3B File Offset: 0x00265D3B
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 60, 55);
			}
		}

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x060063B5 RID: 25525 RVA: 0x00267B47 File Offset: 0x00265D47
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x060063B6 RID: 25526 RVA: 0x00267B4F File Offset: 0x00265D4F
		private int StrongPunchDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
			}
		}

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x060063B7 RID: 25527 RVA: 0x00267B5C File Offset: 0x00265D5C
		private int FastPunchDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
			}
		}

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x060063B8 RID: 25528 RVA: 0x00267B67 File Offset: 0x00265D67
		private int FastPunchRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x060063B9 RID: 25529 RVA: 0x00267B6A File Offset: 0x00265D6A
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x060063BA RID: 25530 RVA: 0x00267B6D File Offset: 0x00265D6D
		// (set) Token: 0x060063BB RID: 25531 RVA: 0x00267B75 File Offset: 0x00265D75
		public bool StartsWithFastPunch
		{
			get
			{
				return this._startsWithFastPunch;
			}
			set
			{
				base.AssertMutable();
				this._startsWithFastPunch = value;
			}
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x060063BC RID: 25532 RVA: 0x00267B84 File Offset: 0x00265D84
		// (set) Token: 0x060063BD RID: 25533 RVA: 0x00267B8C File Offset: 0x00265D8C
		public int StartingHpReduction
		{
			get
			{
				return this._startingHpReduction;
			}
			set
			{
				base.AssertMutable();
				this._startingHpReduction = value;
			}
		}

		// Token: 0x060063BE RID: 25534 RVA: 0x00267B9C File Offset: 0x00265D9C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			if (this.StartingHpReduction > 0)
			{
				base.Creature.SetCurrentHpInternal(Math.Max(1, base.Creature.CurrentHp - this.StartingHpReduction));
			}
		}

		// Token: 0x060063BF RID: 25535 RVA: 0x00267BE0 File Offset: 0x00265DE0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("READY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReadyMove), new AbstractIntent[]
			{
				new DefendIntent()
			});
			MoveState moveState2 = new MoveState("STRONG_PUNCH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StrongPunchMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.StrongPunchDamage)
			});
			MoveState moveState3 = new MoveState("FAST_PUNCH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FastPunchMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.FastPunchDamage, this.FastPunchRepeat),
				new DebuffIntent(false)
			});
			moveState.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState3);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, this.StartsWithFastPunch ? moveState3 : moveState);
		}

		// Token: 0x060063C0 RID: 25536 RVA: 0x00267CBC File Offset: 0x00265EBC
		private async Task ReadyMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.8f);
			await CreatureCmd.GainBlock(base.Creature, 10m, ValueProp.Move, null, false);
		}

		// Token: 0x060063C1 RID: 25537 RVA: 0x00267D00 File Offset: 0x00265F00
		private async Task StrongPunchMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.StrongPunchDamage).FromMonster(this).WithAttackerAnim("Attack", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_attack_single", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x060063C2 RID: 25538 RVA: 0x00267D44 File Offset: 0x00265F44
		private async Task FastPunchMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.FastPunchDamage).WithHitCount(this.FastPunchRepeat).FromMonster(this)
				.WithAttackerAnim("DoubleAttack", 0.2f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_attack_double", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x060063C3 RID: 25539 RVA: 0x00267D90 File Offset: 0x00265F90
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack_double", false);
			AnimState animState3 = new AnimState("block", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState2.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState3, null);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("DoubleAttack", animState2, null);
			return creatureAnimator;
		}

		// Token: 0x0400265A RID: 9818
		private const string _attackDoubleTrigger = "DoubleAttack";

		// Token: 0x0400265B RID: 9819
		private bool _startsWithFastPunch;

		// Token: 0x0400265C RID: 9820
		private int _startingHpReduction;

		// Token: 0x0400265D RID: 9821
		private const string _attackSingleSfx = "event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_attack_single";

		// Token: 0x0400265E RID: 9822
		private const string _attackDoubleSfx = "event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_attack_double";

		// Token: 0x0400265F RID: 9823
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_buff";
	}
}
