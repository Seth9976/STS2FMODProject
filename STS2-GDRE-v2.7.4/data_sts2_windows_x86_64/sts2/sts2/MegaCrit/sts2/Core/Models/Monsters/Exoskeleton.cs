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
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007B4 RID: 1972
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Exoskeleton : MonsterModel
	{
		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x060060F3 RID: 24819 RVA: 0x0025F23F File Offset: 0x0025D43F
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 25, 24);
			}
		}

		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x060060F4 RID: 24820 RVA: 0x0025F24B File Offset: 0x0025D44B
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 29, 28);
			}
		}

		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x060060F5 RID: 24821 RVA: 0x0025F257 File Offset: 0x0025D457
		private int SkitterDamage
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x060060F6 RID: 24822 RVA: 0x0025F25A File Offset: 0x0025D45A
		private int SkitterRepeats
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x170016C2 RID: 5826
		// (get) Token: 0x060060F7 RID: 24823 RVA: 0x0025F265 File Offset: 0x0025D465
		private int MandiblesDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x060060F8 RID: 24824 RVA: 0x0025F271 File Offset: 0x0025D471
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/roaches/roaches_attack";
			}
		}

		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x060060F9 RID: 24825 RVA: 0x0025F278 File Offset: 0x0025D478
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/roaches/roaches_die";
			}
		}

		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x060060FA RID: 24826 RVA: 0x0025F27F File Offset: 0x0025D47F
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Insect;
			}
		}

		// Token: 0x060060FB RID: 24827 RVA: 0x0025F284 File Offset: 0x0025D484
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<HardToKillPower>(new ThrowingPlayerChoiceContext(), base.Creature, 9m, base.Creature, null, false);
		}

		// Token: 0x060060FC RID: 24828 RVA: 0x0025F2C8 File Offset: 0x0025D4C8
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SKITTER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SkitterMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.SkitterDamage, this.SkitterRepeats)
			});
			MoveState moveState2 = new MoveState("MANDIBLES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.MandiblesMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.MandiblesDamage)
			});
			MoveState moveState3 = new MoveState("ENRAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnrageMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat, 1f);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat, 1f);
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("INIT_MOVE");
			conditionalBranchState.AddState(moveState, () => base.Creature.SlotName == "first");
			conditionalBranchState.AddState(moveState2, () => base.Creature.SlotName == "second");
			conditionalBranchState.AddState(moveState3, () => base.Creature.SlotName == "third");
			conditionalBranchState.AddState(randomBranchState, () => base.Creature.SlotName == "fourth");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = randomBranchState;
			list.Add(conditionalBranchState);
			list.Add(randomBranchState);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, conditionalBranchState);
		}

		// Token: 0x060060FD RID: 24829 RVA: 0x0025F428 File Offset: 0x0025D628
		private async Task SkitterMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SkitterDamage).WithHitCount(this.SkitterRepeats).FromMonster(this)
				.OnlyPlayAnimOnce()
				.WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060060FE RID: 24830 RVA: 0x0025F46C File Offset: 0x0025D66C
		private async Task MandiblesMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.MandiblesDamage).FromMonster(this).WithAttackerAnim("HeavyAttack", 0.3f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/roaches/roaches_attack_heavy", null)
				.WithHitFx("vfx/vfx_bite", null, null)
				.Execute(null);
		}

		// Token: 0x060060FF RID: 24831 RVA: 0x0025F4B0 File Offset: 0x0025D6B0
		private async Task EnrageMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/roaches/roaches_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Buff", 0.3f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x06006100 RID: 24832 RVA: 0x0025F4F4 File Offset: 0x0025D6F4
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("die", false);
			AnimState animState3 = new AnimState("hurt", false);
			AnimState animState4 = new AnimState("cast", false);
			AnimState animState5 = new AnimState("buff", false);
			AnimState animState6 = new AnimState("attack", false);
			AnimState animState7 = new AnimState("attack_heavy", false);
			animState6.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState3.NextState = animState;
			animState7.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Hit", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState2, null);
			creatureAnimator.AddAnyState("Cast", animState4, null);
			creatureAnimator.AddAnyState("Attack", animState6, null);
			creatureAnimator.AddAnyState("HeavyAttack", animState7, null);
			creatureAnimator.AddAnyState("Buff", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x04002597 RID: 9623
		private const string _buffTrigger = "Buff";

		// Token: 0x04002598 RID: 9624
		private const int _buffAmount = 2;

		// Token: 0x04002599 RID: 9625
		private const string _heavyAttackTrigger = "HeavyAttack";

		// Token: 0x0400259A RID: 9626
		private const string _attackHeavySfx = "event:/sfx/enemy/enemy_attacks/roaches/roaches_attack_heavy";

		// Token: 0x0400259B RID: 9627
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/roaches/roaches_buff";
	}
}
