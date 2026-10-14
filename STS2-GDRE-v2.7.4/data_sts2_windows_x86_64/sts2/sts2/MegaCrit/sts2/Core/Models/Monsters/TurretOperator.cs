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
	// Token: 0x02000804 RID: 2052
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TurretOperator : MonsterModel
	{
		// Token: 0x170018FA RID: 6394
		// (get) Token: 0x06006648 RID: 26184 RVA: 0x0026F6D0 File Offset: 0x0026D8D0
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/turret_operator/turret_operator_hurt";
			}
		}

		// Token: 0x170018FB RID: 6395
		// (get) Token: 0x06006649 RID: 26185 RVA: 0x0026F6D7 File Offset: 0x0026D8D7
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 51, 41);
			}
		}

		// Token: 0x170018FC RID: 6396
		// (get) Token: 0x0600664A RID: 26186 RVA: 0x0026F6E3 File Offset: 0x0026D8E3
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170018FD RID: 6397
		// (get) Token: 0x0600664B RID: 26187 RVA: 0x0026F6EB File Offset: 0x0026D8EB
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Fur;
			}
		}

		// Token: 0x170018FE RID: 6398
		// (get) Token: 0x0600664C RID: 26188 RVA: 0x0026F6EE File Offset: 0x0026D8EE
		private int FireDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x0600664D RID: 26189 RVA: 0x0026F6FC File Offset: 0x0026D8FC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("UNLOAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.UnloadMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.FireDamage, 5)
			});
			MoveState moveState2 = new MoveState("UNLOAD_MOVE_2", new Func<IReadOnlyList<Creature>, Task>(this.UnloadMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.FireDamage, 5)
			});
			MoveState moveState3 = new MoveState("RELOAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReloadMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600664E RID: 26190 RVA: 0x0026F7C0 File Offset: 0x0026D9C0
		private async Task ReloadMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/turret_operator/turret_operator_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Crank", 0.4f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x0600664F RID: 26191 RVA: 0x0026F804 File Offset: 0x0026DA04
		private async Task UnloadMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.FireDamage).WithHitCount(5).FromMonster(this)
				.WithAttackerAnim("Attack", 0.4f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006650 RID: 26192 RVA: 0x0026F848 File Offset: 0x0026DA48
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("crank", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Crank", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x04002709 RID: 9993
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/turret_operator/turret_operator_buff";

		// Token: 0x0400270A RID: 9994
		private const int _fireRepeat = 5;

		// Token: 0x0400270B RID: 9995
		private const string _crankTrigger = "Crank";
	}
}
