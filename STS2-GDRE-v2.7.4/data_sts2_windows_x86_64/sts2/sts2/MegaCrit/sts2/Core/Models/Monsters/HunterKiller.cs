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
	// Token: 0x020007C4 RID: 1988
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HunterKiller : MonsterModel
	{
		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x060061DE RID: 25054 RVA: 0x00262231 File Offset: 0x00260431
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 126, 121);
			}
		}

		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x060061DF RID: 25055 RVA: 0x0026223D File Offset: 0x0026043D
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x060061E0 RID: 25056 RVA: 0x00262245 File Offset: 0x00260445
		private int BiteDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 17);
			}
		}

		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x060061E1 RID: 25057 RVA: 0x00262252 File Offset: 0x00260452
		private int PunctureDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x060061E2 RID: 25058 RVA: 0x0026225D File Offset: 0x0026045D
		public override string TakeDamageSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/hunter_killer/hunter_killer_hurt";
			}
		}

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x060061E3 RID: 25059 RVA: 0x00262264 File Offset: 0x00260464
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/hunter_killer/hunter_killer_die";
			}
		}

		// Token: 0x060061E4 RID: 25060 RVA: 0x0026226C File Offset: 0x0026046C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("TENDERIZING_GOOP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GoopMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			MoveState moveState2 = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BiteDamage)
			});
			MoveState moveState3 = new MoveState("PUNCTURE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PunctureMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.PunctureDamage, 3)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState3, 2);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(randomBranchState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060061E5 RID: 25061 RVA: 0x00262358 File Offset: 0x00260558
		private async Task GoopMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.4f);
			await PowerCmd.Apply<TenderPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x060061E6 RID: 25062 RVA: 0x002623A4 File Offset: 0x002605A4
		private async Task BiteMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BiteDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_bite", null, null)
				.Execute(null);
		}

		// Token: 0x060061E7 RID: 25063 RVA: 0x002623E8 File Offset: 0x002605E8
		private async Task PunctureMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.PunctureDamage).WithHitCount(3).OnlyPlayAnimOnce()
				.FromMonster(this)
				.WithAttackerAnim("TripleAttack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060061E8 RID: 25064 RVA: 0x0026242C File Offset: 0x0026062C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("attack_triple", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("TripleAttack", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x040025D0 RID: 9680
		private const string _tripleAttackTrigger = "TripleAttack";

		// Token: 0x040025D1 RID: 9681
		private const int _punctureRepeat = 3;
	}
}
