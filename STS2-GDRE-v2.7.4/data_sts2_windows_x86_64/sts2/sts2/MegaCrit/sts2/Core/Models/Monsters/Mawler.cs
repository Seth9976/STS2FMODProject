using System;
using System.Collections.Generic;
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
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007D1 RID: 2001
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Mawler : MonsterModel
	{
		// Token: 0x1700178E RID: 6030
		// (get) Token: 0x060062D6 RID: 25302 RVA: 0x00265264 File Offset: 0x00263464
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 76, 72);
			}
		}

		// Token: 0x1700178F RID: 6031
		// (get) Token: 0x060062D7 RID: 25303 RVA: 0x00265270 File Offset: 0x00263470
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001790 RID: 6032
		// (get) Token: 0x060062D8 RID: 25304 RVA: 0x00265278 File Offset: 0x00263478
		private int RipAndTearDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
			}
		}

		// Token: 0x17001791 RID: 6033
		// (get) Token: 0x060062D9 RID: 25305 RVA: 0x00265285 File Offset: 0x00263485
		private int ClawDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
			}
		}

		// Token: 0x060062DA RID: 25306 RVA: 0x00265290 File Offset: 0x00263490
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("RIP_AND_TEAR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RipAndTearMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.RipAndTearDamage)
			});
			MoveState moveState2 = new MoveState("ROAR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RoarMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			MoveState moveState3 = new MoveState("CLAW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ClawMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.ClawDamage, 2)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat, 1f);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.UseOnlyOnce, 1f);
			randomBranchState.AddBranch(moveState3, MoveRepeatType.CannotRepeat, 1f);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(randomBranchState);
			return new MonsterMoveStateMachine(list, moveState3);
		}

		// Token: 0x060062DB RID: 25307 RVA: 0x00265394 File Offset: 0x00263594
		private async Task RipAndTearMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.RipAndTearDamage).FromMonster(this).WithAttackerAnim("Attack", 0.35f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060062DC RID: 25308 RVA: 0x002653D8 File Offset: 0x002635D8
		private async Task RoarMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, 3m, base.Creature, null, false);
		}

		// Token: 0x060062DD RID: 25309 RVA: 0x00265424 File Offset: 0x00263624
		private async Task ClawMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ClawDamage).WithHitCount(2).FromMonster(this)
				.WithAttackerAnim("Attack", 0.35f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060062DE RID: 25310 RVA: 0x00265468 File Offset: 0x00263668
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("roar", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x04002627 RID: 9767
		private const int _clawRepeat = 2;
	}
}
