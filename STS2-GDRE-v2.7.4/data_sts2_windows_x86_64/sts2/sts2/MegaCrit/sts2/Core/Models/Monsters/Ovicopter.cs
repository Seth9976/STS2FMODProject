using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x020007DA RID: 2010
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Ovicopter : MonsterModel
	{
		// Token: 0x170017BA RID: 6074
		// (get) Token: 0x0600634A RID: 25418 RVA: 0x00266676 File Offset: 0x00264876
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 126, 124);
			}
		}

		// Token: 0x170017BB RID: 6075
		// (get) Token: 0x0600634B RID: 25419 RVA: 0x00266682 File Offset: 0x00264882
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 132, 130);
			}
		}

		// Token: 0x170017BC RID: 6076
		// (get) Token: 0x0600634C RID: 25420 RVA: 0x00266694 File Offset: 0x00264894
		private int SmashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 16);
			}
		}

		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x0600634D RID: 25421 RVA: 0x002666A1 File Offset: 0x002648A1
		private int TenderizerDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x0600634E RID: 25422 RVA: 0x002666AC File Offset: 0x002648AC
		private int NutritionalPasteStrengthAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x0600634F RID: 25423 RVA: 0x002666B7 File Offset: 0x002648B7
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_die";
			}
		}

		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x06006350 RID: 25424 RVA: 0x002666BE File Offset: 0x002648BE
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_attack";
			}
		}

		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x06006351 RID: 25425 RVA: 0x002666C5 File Offset: 0x002648C5
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x06006352 RID: 25426 RVA: 0x002666C8 File Offset: 0x002648C8
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			SfxCmd.PlayLoop("event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_idle_loop", true);
		}

		// Token: 0x06006353 RID: 25427 RVA: 0x0026670B File Offset: 0x0026490B
		public override void BeforeRemovedFromRoom()
		{
			SfxCmd.StopLoop("event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_idle_loop");
		}

		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x06006354 RID: 25428 RVA: 0x00266717 File Offset: 0x00264917
		private bool CanLay
		{
			get
			{
				return base.CombatState.GetTeammatesOf(base.Creature).Count<Creature>((Creature c) => c.IsAlive) <= 3;
			}
		}

		// Token: 0x06006355 RID: 25429 RVA: 0x00266754 File Offset: 0x00264954
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("LAY_EGGS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LayEggsMove), new AbstractIntent[]
			{
				new SummonIntent()
			});
			MoveState moveState2 = new MoveState("SMASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SmashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SmashDamage)
			});
			MoveState moveState3 = new MoveState("TENDERIZER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TenderizerMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.TenderizerDamage),
				new DebuffIntent(false)
			});
			MoveState moveState4 = new MoveState("NUTRITIONAL_PASTE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.NutritionalPasteMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("SUMMON_BRANCH_STATE");
			moveState.FollowUpState = moveState2;
			moveState4.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = conditionalBranchState;
			conditionalBranchState.AddState(moveState, () => this.CanLay);
			conditionalBranchState.AddState(moveState4, () => !this.CanLay);
			list.Add(moveState4);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(conditionalBranchState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006356 RID: 25430 RVA: 0x00266890 File Offset: 0x00264A90
		private async Task LayEggsMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_lay", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "layTrigger", 1f);
			for (int i = 0; i < 3; i++)
			{
				EncounterModel encounter = base.CombatState.Encounter;
				string text = ((encounter != null) ? encounter.Slots.LastOrDefault<string>((string s) => base.CombatState.Enemies.All<Creature>((Creature c) => c.SlotName != s)) : null);
				if (text != null)
				{
					Creature creature = await CreatureCmd.Add<ToughEgg>(base.CombatState, text);
					await PowerCmd.Apply<MinionPower>(new ThrowingPlayerChoiceContext(), creature, 1m, base.Creature, null, false);
				}
			}
		}

		// Token: 0x06006357 RID: 25431 RVA: 0x002668D4 File Offset: 0x00264AD4
		private async Task NutritionalPasteMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_lay", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "buffTrigger", 1f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.NutritionalPasteStrengthAmount, base.Creature, null, false);
		}

		// Token: 0x06006358 RID: 25432 RVA: 0x00266918 File Offset: 0x00264B18
		private async Task SmashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SmashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006359 RID: 25433 RVA: 0x0026695C File Offset: 0x00264B5C
		private async Task TenderizerMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.TenderizerDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x0600635A RID: 25434 RVA: 0x002669A8 File Offset: 0x00264BA8
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("buff", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			AnimState animState7 = new AnimState("lay", false);
			animState3.NextState = animState;
			animState2.NextState = animState;
			animState4.NextState = animState;
			animState7.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("layTrigger", animState7, null);
			creatureAnimator.AddAnyState("buffTrigger", animState3, null);
			return creatureAnimator;
		}

		// Token: 0x0600635B RID: 25435 RVA: 0x00266A93 File Offset: 0x00264C93
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "TENDERIZER_MOVE";
		}

		// Token: 0x0400263F RID: 9791
		private const string _tenderizerMove = "TENDERIZER_MOVE";

		// Token: 0x04002640 RID: 9792
		private const string _layTrigger = "layTrigger";

		// Token: 0x04002641 RID: 9793
		private const string _buffTrigger = "buffTrigger";

		// Token: 0x04002642 RID: 9794
		private const string _idleLoop = "event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_idle_loop";

		// Token: 0x04002643 RID: 9795
		private const string _laySfx = "event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_lay";
	}
}
