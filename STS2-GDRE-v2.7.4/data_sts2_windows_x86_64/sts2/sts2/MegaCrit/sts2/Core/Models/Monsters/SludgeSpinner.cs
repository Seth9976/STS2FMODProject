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
	// Token: 0x020007EB RID: 2027
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SludgeSpinner : MonsterModel
	{
		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x0600646C RID: 25708 RVA: 0x00269E4F File Offset: 0x0026804F
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 41, 37);
			}
		}

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x0600646D RID: 25709 RVA: 0x00269E5B File Offset: 0x0026805B
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 42, 39);
			}
		}

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x0600646E RID: 25710 RVA: 0x00269E67 File Offset: 0x00268067
		private int OilSprayDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x0600646F RID: 25711 RVA: 0x00269E73 File Offset: 0x00268073
		private int SlamDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 11);
			}
		}

		// Token: 0x1700183D RID: 6205
		// (get) Token: 0x06006470 RID: 25712 RVA: 0x00269E80 File Offset: 0x00268080
		private int RageDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x1700183E RID: 6206
		// (get) Token: 0x06006471 RID: 25713 RVA: 0x00269E8B File Offset: 0x0026808B
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Stone;
			}
		}

		// Token: 0x06006472 RID: 25714 RVA: 0x00269E90 File Offset: 0x00268090
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("OIL_SPRAY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.OilSprayMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.OilSprayDamage),
				new DebuffIntent(false)
			});
			MoveState moveState2 = new MoveState("SLAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SlamMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SlamDamage)
			});
			MoveState moveState3 = new MoveState("RAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RageMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.RageDamage),
				new BuffIntent()
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState3, MoveRepeatType.CannotRepeat);
			list.Add(randomBranchState);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006473 RID: 25715 RVA: 0x00269F98 File Offset: 0x00268198
		private async Task OilSprayMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.OilSprayDamage).FromMonster(this).WithAttackerAnim("Cast", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/sludge_spinner/sludge_spinner_attack_spin", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x06006474 RID: 25716 RVA: 0x00269FE4 File Offset: 0x002681E4
		private async Task SlamMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SlamDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/sludge_spinner/sludge_spinner_attack_dash", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006475 RID: 25717 RVA: 0x0026A028 File Offset: 0x00268228
		private async Task RageMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.RageDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/sludge_spinner/sludge_spinner_attack_dash", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
		}

		// Token: 0x06006476 RID: 25718 RVA: 0x0026A06C File Offset: 0x0026826C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("slam", false);
			AnimState animState3 = new AnimState("spray", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Attack", animState2, null);
			creatureAnimator.AddAnyState("Cast", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x06006477 RID: 25719 RVA: 0x0026A10F File Offset: 0x0026830F
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "RAGE_MOVE";
		}

		// Token: 0x04002690 RID: 9872
		private const string _rageMove = "RAGE_MOVE";

		// Token: 0x04002691 RID: 9873
		private const string _dashAttackSfx = "event:/sfx/enemy/enemy_attacks/sludge_spinner/sludge_spinner_attack_dash";

		// Token: 0x04002692 RID: 9874
		private const string _spinAttackSfx = "event:/sfx/enemy/enemy_attacks/sludge_spinner/sludge_spinner_attack_spin";
	}
}
