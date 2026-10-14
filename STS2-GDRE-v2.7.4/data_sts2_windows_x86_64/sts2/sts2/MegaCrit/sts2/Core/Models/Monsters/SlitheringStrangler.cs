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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007EA RID: 2026
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlitheringStrangler : MonsterModel
	{
		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x06006461 RID: 25697 RVA: 0x00269B88 File Offset: 0x00267D88
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 54, 53);
			}
		}

		// Token: 0x17001835 RID: 6197
		// (get) Token: 0x06006462 RID: 25698 RVA: 0x00269B94 File Offset: 0x00267D94
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 56, 55);
			}
		}

		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x06006463 RID: 25699 RVA: 0x00269BA0 File Offset: 0x00267DA0
		private int ThwackDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x06006464 RID: 25700 RVA: 0x00269BAB File Offset: 0x00267DAB
		private int LashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 12);
			}
		}

		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x06006465 RID: 25701 RVA: 0x00269BB8 File Offset: 0x00267DB8
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Plant;
			}
		}

		// Token: 0x06006466 RID: 25702 RVA: 0x00269BBC File Offset: 0x00267DBC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("CONSTRICT", new Func<IReadOnlyList<Creature>, Task>(this.ConstrictMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			MoveState moveState2 = new MoveState("THWACK", new Func<IReadOnlyList<Creature>, Task>(this.ThwackMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ThwackDamage),
				new DefendIntent()
			});
			MoveState moveState3 = new MoveState("LASH", new Func<IReadOnlyList<Creature>, Task>(this.LashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.LashDamage)
			});
			RandomBranchState randomBranchState = new RandomBranchState("rand");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = moveState;
			moveState3.FollowUpState = moveState;
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CanRepeatForever);
			randomBranchState.AddBranch(moveState3, MoveRepeatType.CanRepeatForever);
			list.Add(randomBranchState);
			list.Add(moveState2);
			list.Add(moveState);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006467 RID: 25703 RVA: 0x00269CAC File Offset: 0x00267EAC
		private async Task ConstrictMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.6f);
			await PowerCmd.Apply<ConstrictPower>(new ThrowingPlayerChoiceContext(), targets, 3m, base.Creature, null, false);
		}

		// Token: 0x06006468 RID: 25704 RVA: 0x00269CF8 File Offset: 0x00267EF8
		private async Task ThwackMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ThwackDamage).FromMonster(this).WithAttackerAnim("AttackDefendTrigger", 0.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_attack_headbutt", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, 5m, ValueProp.Move, null, false);
		}

		// Token: 0x06006469 RID: 25705 RVA: 0x00269D3C File Offset: 0x00267F3C
		private async Task LashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.LashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_tail", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600646A RID: 25706 RVA: 0x00269D80 File Offset: 0x00267F80
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("constrict", false);
			AnimState animState3 = new AnimState("attack_defend", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState3.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("AttackDefendTrigger", animState3, null);
			return creatureAnimator;
		}

		// Token: 0x0400268C RID: 9868
		private const string _attackDefendTrigger = "AttackDefendTrigger";

		// Token: 0x0400268D RID: 9869
		private const string _attackHeadbuttSfx = "event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_attack_headbutt";

		// Token: 0x0400268E RID: 9870
		private const string _attackTailSfx = "event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_tail";

		// Token: 0x0400268F RID: 9871
		private const string _castSfx = "event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast";
	}
}
