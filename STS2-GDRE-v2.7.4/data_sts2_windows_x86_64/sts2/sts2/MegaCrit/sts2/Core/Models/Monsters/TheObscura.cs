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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007FD RID: 2045
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheObscura : MonsterModel
	{
		// Token: 0x170018BE RID: 6334
		// (get) Token: 0x060065AB RID: 26027 RVA: 0x0026D90B File Offset: 0x0026BB0B
		protected override bool HasPhobiaSpineSkin
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170018BF RID: 6335
		// (get) Token: 0x060065AC RID: 26028 RVA: 0x0026D90E File Offset: 0x0026BB0E
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/obscura/obscura_attack";
			}
		}

		// Token: 0x170018C0 RID: 6336
		// (get) Token: 0x060065AD RID: 26029 RVA: 0x0026D915 File Offset: 0x0026BB15
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/obscura/obscura_die";
			}
		}

		// Token: 0x170018C1 RID: 6337
		// (get) Token: 0x060065AE RID: 26030 RVA: 0x0026D91C File Offset: 0x0026BB1C
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 129, 123);
			}
		}

		// Token: 0x170018C2 RID: 6338
		// (get) Token: 0x060065AF RID: 26031 RVA: 0x0026D92B File Offset: 0x0026BB2B
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170018C3 RID: 6339
		// (get) Token: 0x060065B0 RID: 26032 RVA: 0x0026D933 File Offset: 0x0026BB33
		private int PiercingGazeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
			}
		}

		// Token: 0x170018C4 RID: 6340
		// (get) Token: 0x060065B1 RID: 26033 RVA: 0x0026D940 File Offset: 0x0026BB40
		private int HardeningStrikeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x060065B2 RID: 26034 RVA: 0x0026D94B File Offset: 0x0026BB4B
		private int HardeningStrikeBlock
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x170018C6 RID: 6342
		// (get) Token: 0x060065B3 RID: 26035 RVA: 0x0026D956 File Offset: 0x0026BB56
		// (set) Token: 0x060065B4 RID: 26036 RVA: 0x0026D95E File Offset: 0x0026BB5E
		private bool HasSummoned
		{
			get
			{
				return this._hasSummoned;
			}
			set
			{
				base.AssertMutable();
				this._hasSummoned = value;
			}
		}

		// Token: 0x060065B5 RID: 26037 RVA: 0x0026D970 File Offset: 0x0026BB70
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("ILLUSION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.IllusionMove), new AbstractIntent[]
			{
				new SummonIntent()
			});
			MoveState moveState2 = new MoveState("PIERCING_GAZE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PiercingGazeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.PiercingGazeDamage)
			});
			MoveState moveState3 = new MoveState("SAIL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WailMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState4 = new MoveState("HARDENING_STRIKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HardeningStrikeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.HardeningStrikeDamage),
				new DefendIntent()
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = randomBranchState;
			moveState4.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState3, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState4, MoveRepeatType.CannotRepeat);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			list.Add(randomBranchState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060065B6 RID: 26038 RVA: 0x0026DAA4 File Offset: 0x0026BCA4
		private async Task IllusionMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/obscura/obscura_summon", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Summon", 0.15f);
			if (base.CombatState.IsLiveCombat())
			{
				await CreatureCmd.Add<Parafright>(base.CombatState, "illusion");
			}
			this.HasSummoned = true;
		}

		// Token: 0x060065B7 RID: 26039 RVA: 0x0026DAE8 File Offset: 0x0026BCE8
		private async Task PiercingGazeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.PiercingGazeDamage).FromMonster(this).WithAttackerFx(null, this.AttackSfx, null)
				.WithAttackerAnim("Attack", 0.3f, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060065B8 RID: 26040 RVA: 0x0026DB2C File Offset: 0x0026BD2C
		private async Task WailMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/obscura/obscura_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.7f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.CombatState.GetTeammatesOf(base.Creature), 3m, base.Creature, null, false);
		}

		// Token: 0x060065B9 RID: 26041 RVA: 0x0026DB70 File Offset: 0x0026BD70
		private async Task HardeningStrikeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.HardeningStrikeDamage).FromMonster(this).WithAttackerFx(null, this.AttackSfx, null)
				.WithAttackerAnim("Attack", 0.3f, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, this.HardeningStrikeBlock, ValueProp.Move, null, false);
		}

		// Token: 0x060065BA RID: 26042 RVA: 0x0026DBB4 File Offset: 0x0026BDB4
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack", false);
			AnimState animState3 = new AnimState("die", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("cast", false);
			AnimState animState6 = new AnimState("cast_intro", false);
			AnimState animState7 = new AnimState("hurt_intro", false);
			AnimState animState8 = new AnimState("die_intro", false);
			AnimState animState9 = new AnimState("intro_loop", true);
			animState2.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState6.NextState = animState;
			animState7.NextState = animState9;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState9, controller);
			creatureAnimator.AddAnyState("Attack", animState2, null);
			creatureAnimator.AddAnyState("Hit", animState4, () => this.HasSummoned);
			creatureAnimator.AddAnyState("Hit", animState7, () => !this.HasSummoned);
			creatureAnimator.AddAnyState("Dead", animState3, () => this.HasSummoned);
			creatureAnimator.AddAnyState("Dead", animState8, () => !this.HasSummoned);
			creatureAnimator.AddAnyState("Cast", animState5, null);
			creatureAnimator.AddAnyState("Summon", animState6, null);
			return creatureAnimator;
		}

		// Token: 0x060065BB RID: 26043 RVA: 0x0026DCF7 File Offset: 0x0026BEF7
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "PIERCING_GAZE_MOVE";
		}

		// Token: 0x040026E0 RID: 9952
		private const string _summonTrigger = "Summon";

		// Token: 0x040026E1 RID: 9953
		private const string _piercingGazeMove = "PIERCING_GAZE_MOVE";

		// Token: 0x040026E2 RID: 9954
		private const string _summonSfx = "event:/sfx/enemy/enemy_attacks/obscura/obscura_summon";

		// Token: 0x040026E3 RID: 9955
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/obscura/obscura_buff";

		// Token: 0x040026E4 RID: 9956
		private bool _hasSummoned;
	}
}
