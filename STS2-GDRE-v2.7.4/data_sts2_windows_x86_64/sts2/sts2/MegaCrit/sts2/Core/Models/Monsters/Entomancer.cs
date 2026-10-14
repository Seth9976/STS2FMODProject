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
	// Token: 0x020007B3 RID: 1971
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Entomancer : MonsterModel
	{
		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x060060E5 RID: 24805 RVA: 0x0025EF49 File Offset: 0x0025D149
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 155, 145);
			}
		}

		// Token: 0x170016B9 RID: 5817
		// (get) Token: 0x060060E6 RID: 24806 RVA: 0x0025EF5B File Offset: 0x0025D15B
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170016BA RID: 5818
		// (get) Token: 0x060060E7 RID: 24807 RVA: 0x0025EF63 File Offset: 0x0025D163
		private int SpearMoveDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 20, 18);
			}
		}

		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x060060E8 RID: 24808 RVA: 0x0025EF70 File Offset: 0x0025D170
		private int BeesRepeat
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x060060E9 RID: 24809 RVA: 0x0025EF7B File Offset: 0x0025D17B
		private int BeesDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 3);
			}
		}

		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x060060EA RID: 24810 RVA: 0x0025EF86 File Offset: 0x0025D186
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/entomancer/entomancer_die";
			}
		}

		// Token: 0x060060EB RID: 24811 RVA: 0x0025EF90 File Offset: 0x0025D190
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<PersonalHivePower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x060060EC RID: 24812 RVA: 0x0025EFD4 File Offset: 0x0025D1D4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("PHEROMONE_SPIT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpitMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("BEES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BeesMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.BeesDamage, this.BeesRepeat)
			});
			MoveState moveState3 = new MoveState("SPEAR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpearMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SpearMoveDamage)
			});
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			moveState.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x060060ED RID: 24813 RVA: 0x0025F09C File Offset: 0x0025D29C
		private async Task SpitMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			PersonalHivePower personalHivePower = base.Creature.Powers.OfType<PersonalHivePower>().FirstOrDefault<PersonalHivePower>();
			if (personalHivePower != null && personalHivePower.Amount < 3)
			{
				await PowerCmd.Apply<PersonalHivePower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			}
			else
			{
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
			}
		}

		// Token: 0x060060EE RID: 24814 RVA: 0x0025F0E0 File Offset: 0x0025D2E0
		private async Task BeesMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BeesDamage).WithHitCount(this.BeesRepeat).FromMonster(this)
				.WithAttackerAnim("attack_ranged", 0.3f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/entomancer/entomancer_attack_ranged", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060060EF RID: 24815 RVA: 0x0025F124 File Offset: 0x0025D324
		private async Task SpearMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SpearMoveDamage).FromMonster(this).WithAttackerAnim("Attack", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/entomancer/entomancer_attack_ranged", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060060F0 RID: 24816 RVA: 0x0025F168 File Offset: 0x0025D368
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			AnimState animState6 = new AnimState("attack_ranged", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState6.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("attack_ranged", animState6, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x04002595 RID: 9621
		private const string _rangedAttackMove = "attack_ranged";

		// Token: 0x04002596 RID: 9622
		private const string _attackRangedSfx = "event:/sfx/enemy/enemy_attacks/entomancer/entomancer_attack_ranged";
	}
}
