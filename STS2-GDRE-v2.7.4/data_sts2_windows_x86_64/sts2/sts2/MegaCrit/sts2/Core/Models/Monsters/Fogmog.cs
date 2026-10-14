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
	// Token: 0x020007BB RID: 1979
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Fogmog : MonsterModel
	{
		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x06006158 RID: 24920 RVA: 0x002608F7 File Offset: 0x0025EAF7
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 78, 74);
			}
		}

		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x06006159 RID: 24921 RVA: 0x00260903 File Offset: 0x0025EB03
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x0600615A RID: 24922 RVA: 0x0026090B File Offset: 0x0025EB0B
		private int SwipeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x170016E9 RID: 5865
		// (get) Token: 0x0600615B RID: 24923 RVA: 0x00260917 File Offset: 0x0025EB17
		private int HeadbuttDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
			}
		}

		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x0600615C RID: 24924 RVA: 0x00260924 File Offset: 0x0025EB24
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Plant;
			}
		}

		// Token: 0x0600615D RID: 24925 RVA: 0x00260928 File Offset: 0x0025EB28
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("ILLUSION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.IllusionMove), new AbstractIntent[]
			{
				new SummonIntent()
			});
			MoveState moveState2 = new MoveState("SWIPE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwipeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SwipeDamage),
				new BuffIntent()
			});
			MoveState moveState3 = new MoveState("SWIPE_RANDOM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwipeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SwipeDamage),
				new BuffIntent()
			});
			MoveState moveState4 = new MoveState("HEADBUTT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HeadbuttMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.HeadbuttDamage)
			});
			RandomBranchState randomBranchState = new RandomBranchState("BRANCH");
			randomBranchState.AddBranch(moveState3, MoveRepeatType.CannotRepeat, () => 0.4f);
			randomBranchState.AddBranch(moveState4, MoveRepeatType.CannotRepeat, () => 0.6f);
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(randomBranchState);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600615E RID: 24926 RVA: 0x00260A9C File Offset: 0x0025EC9C
		private async Task IllusionMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/fogmog/fogmog_summon", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Summon", 0.75f);
			if (base.CombatState.IsLiveCombat())
			{
				await CreatureCmd.Add<EyeWithTeeth>(base.CombatState, "illusion");
			}
		}

		// Token: 0x0600615F RID: 24927 RVA: 0x00260AE0 File Offset: 0x0025ECE0
		private async Task SwipeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SwipeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x06006160 RID: 24928 RVA: 0x00260B24 File Offset: 0x0025ED24
		private async Task HeadbuttMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.HeadbuttDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006161 RID: 24929 RVA: 0x00260B68 File Offset: 0x0025ED68
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("summon", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			creatureAnimator.AddAnyState("Summon", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			return creatureAnimator;
		}

		// Token: 0x06006162 RID: 24930 RVA: 0x00260C0B File Offset: 0x0025EE0B
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "SWIPE_RANDOM_MOVE" && moveStateId != "HEADBUTT_MOVE";
		}

		// Token: 0x040025B1 RID: 9649
		private const string _swipeRandomMove = "SWIPE_RANDOM_MOVE";

		// Token: 0x040025B2 RID: 9650
		private const string _headbuttMove = "HEADBUTT_MOVE";

		// Token: 0x040025B3 RID: 9651
		private const string _summonTrigger = "Summon";

		// Token: 0x040025B4 RID: 9652
		private const string _sporesSfx = "event:/sfx/enemy/enemy_attacks/fogmog/fogmog_summon";
	}
}
