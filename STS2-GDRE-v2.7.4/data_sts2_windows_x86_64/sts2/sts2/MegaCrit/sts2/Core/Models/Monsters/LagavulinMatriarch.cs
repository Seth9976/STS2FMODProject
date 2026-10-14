using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007CA RID: 1994
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LagavulinMatriarch : MonsterModel
	{
		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x0600625A RID: 25178 RVA: 0x00263B52 File Offset: 0x00261D52
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 233, 222);
			}
		}

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x0600625B RID: 25179 RVA: 0x00263B64 File Offset: 0x00261D64
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x0600625C RID: 25180 RVA: 0x00263B6C File Offset: 0x00261D6C
		private int SlashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 21, 19);
			}
		}

		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x0600625D RID: 25181 RVA: 0x00263B79 File Offset: 0x00261D79
		private int Slash2Damage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
			}
		}

		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x0600625E RID: 25182 RVA: 0x00263B86 File Offset: 0x00261D86
		private int Slash2Block
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 14, 12);
			}
		}

		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x0600625F RID: 25183 RVA: 0x00263B92 File Offset: 0x00261D92
		private int DisembowelDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
			}
		}

		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x06006260 RID: 25184 RVA: 0x00263B9F File Offset: 0x00261D9F
		private int DisembowelRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x06006261 RID: 25185 RVA: 0x00263BA2 File Offset: 0x00261DA2
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.ArmorBig;
			}
		}

		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x06006262 RID: 25186 RVA: 0x00263BA5 File Offset: 0x00261DA5
		// (set) Token: 0x06006263 RID: 25187 RVA: 0x00263BAD File Offset: 0x00261DAD
		public bool IsAwake
		{
			get
			{
				return this._isAwake;
			}
			set
			{
				base.AssertMutable();
				this._isAwake = value;
			}
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x06006264 RID: 25188 RVA: 0x00263BBC File Offset: 0x00261DBC
		// (set) Token: 0x06006265 RID: 25189 RVA: 0x00263BC4 File Offset: 0x00261DC4
		public bool IsShellAwake
		{
			get
			{
				return this._isShellAwake;
			}
			set
			{
				base.AssertMutable();
				this._isShellAwake = value;
			}
		}

		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x06006266 RID: 25190 RVA: 0x00263BD3 File Offset: 0x00261DD3
		// (set) Token: 0x06006267 RID: 25191 RVA: 0x00263BDB File Offset: 0x00261DDB
		[Nullable(2)]
		private NSleepingVfx SleepingVfx
		{
			[NullableContext(2)]
			get
			{
				return this._sleepingVfx;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._sleepingVfx = value;
			}
		}

		// Token: 0x06006268 RID: 25192 RVA: 0x00263BEA File Offset: 0x00261DEA
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			spine.GetAnimationState().SetAnimation("_tracks/eyes_closed_loop", true, 1);
		}

		// Token: 0x06006269 RID: 25193 RVA: 0x00263C00 File Offset: 0x00261E00
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await this.Sleep();
		}

		// Token: 0x0600626A RID: 25194 RVA: 0x00263C44 File Offset: 0x00261E44
		private async Task Sleep()
		{
			this.IsAwake = false;
			await CreatureCmd.TriggerAnim(base.Creature, "Sleep", 0f);
			await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), base.Creature, 12m, base.Creature, null, false);
			await PowerCmd.Apply<AsleepPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
			NCreature creatureNode = base.Creature.GetCreatureNode();
			Marker2D marker2D = ((creatureNode != null) ? creatureNode.GetSpecialNode<Marker2D>("%SleepVfxPos") : null);
			if (marker2D != null)
			{
				this.SleepingVfx = NSleepingVfx.Create(marker2D.GlobalPosition, true);
				marker2D.AddChildSafely(this.SleepingVfx);
				this.SleepingVfx.Position = Vector2.Zero;
			}
		}

		// Token: 0x0600626B RID: 25195 RVA: 0x00263C88 File Offset: 0x00261E88
		public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target != base.Creature)
			{
				return Task.CompletedTask;
			}
			NSleepingVfx sleepingVfx = this.SleepingVfx;
			if (sleepingVfx != null)
			{
				sleepingVfx.Stop();
			}
			this.SleepingVfx = null;
			if (base.Creature.CurrentHp <= base.Creature.MaxHp / 2 && !this.IsShellAwake)
			{
				NCreature creatureNode = base.Creature.GetCreatureNode();
				if (creatureNode != null)
				{
					creatureNode.SpineAnimation.SetAnimation("_tracks/eyes_open", false, 1);
				}
				if (creatureNode != null)
				{
					creatureNode.SpineAnimation.AddAnimation("_tracks/eyes_open_loop", 0f, true, 1);
				}
				this.IsShellAwake = true;
			}
			return Task.CompletedTask;
		}

		// Token: 0x0600626C RID: 25196 RVA: 0x00263D2C File Offset: 0x00261F2C
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature != base.Creature)
			{
				return Task.CompletedTask;
			}
			NCreature creatureNode = base.Creature.GetCreatureNode();
			if (creatureNode != null)
			{
				creatureNode.SpineAnimation.SetAnimation("_tracks/eyes_dead", false, 1);
			}
			NSleepingVfx sleepingVfx = this.SleepingVfx;
			if (sleepingVfx != null)
			{
				sleepingVfx.Stop();
			}
			this.SleepingVfx = null;
			return Task.CompletedTask;
		}

		// Token: 0x0600626D RID: 25197 RVA: 0x00263D8C File Offset: 0x00261F8C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SLEEP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SleepMove), new AbstractIntent[]
			{
				new SleepIntent()
			});
			MoveState moveState2 = new MoveState("SLASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SlashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SlashDamage)
			});
			MoveState moveState3 = new MoveState("SLASH2_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.Slash2Move), new AbstractIntent[]
			{
				new SingleAttackIntent(this.Slash2Damage),
				new DefendIntent()
			});
			MoveState moveState4 = new MoveState("DISEMBOWEL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DisembowelMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.DisembowelDamage, this.DisembowelRepeat)
			});
			MoveState moveState5 = new MoveState("SOUL_SIPHON_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SoulSiphonMove), new AbstractIntent[]
			{
				new DebuffIntent(false),
				new BuffIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("SLEEP_BRANCH");
			moveState.FollowUpState = conditionalBranchState;
			moveState2.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState5;
			moveState5.FollowUpState = moveState2;
			conditionalBranchState.AddState(moveState, () => base.Creature.HasPower<AsleepPower>());
			conditionalBranchState.AddState(moveState2, () => !base.Creature.HasPower<AsleepPower>());
			list.Add(conditionalBranchState);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState5);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600626E RID: 25198 RVA: 0x00263F13 File Offset: 0x00262113
		private Task SleepMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}

		// Token: 0x0600626F RID: 25199 RVA: 0x00263F1C File Offset: 0x0026211C
		public async Task WakeUpMove(IReadOnlyList<Creature> _)
		{
			if (!this._isAwake)
			{
				SfxCmd.Play("event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_awaken", 1f);
				await CreatureCmd.TriggerAnim(base.Creature, "Wake", 0.6f);
				NSleepingVfx sleepingVfx = this.SleepingVfx;
				if (sleepingVfx != null)
				{
					sleepingVfx.Stop();
				}
				this.SleepingVfx = null;
				this.IsAwake = true;
			}
		}

		// Token: 0x06006270 RID: 25200 RVA: 0x00263F60 File Offset: 0x00262160
		private async Task SlashMove(IReadOnlyList<Creature> targets)
		{
			NSleepingVfx sleepingVfx = this.SleepingVfx;
			if (sleepingVfx != null)
			{
				sleepingVfx.Stop();
			}
			this.SleepingVfx = null;
			await DamageCmd.Attack(this.SlashDamage).FromMonster(this).WithAttackerAnim("AttackHeavy", 0.3f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_slam", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006271 RID: 25201 RVA: 0x00263FA4 File Offset: 0x002621A4
		private async Task Slash2Move(IReadOnlyList<Creature> targets)
		{
			NSleepingVfx sleepingVfx = this.SleepingVfx;
			if (sleepingVfx != null)
			{
				sleepingVfx.Stop();
			}
			this.SleepingVfx = null;
			await DamageCmd.Attack(this.Slash2Damage).FromMonster(this).WithAttackerAnim("AttackHeavy", 0.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_slam", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, this.Slash2Block, ValueProp.Move, null, false);
		}

		// Token: 0x06006272 RID: 25202 RVA: 0x00263FE8 File Offset: 0x002621E8
		private async Task DisembowelMove(IReadOnlyList<Creature> targets)
		{
			NSleepingVfx sleepingVfx = this.SleepingVfx;
			if (sleepingVfx != null)
			{
				sleepingVfx.Stop();
			}
			this.SleepingVfx = null;
			await DamageCmd.Attack(this.DisembowelDamage).WithHitCount(this.DisembowelRepeat).FromMonster(this)
				.WithAttackerAnim("AttackDouble", 0.15f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_attack_stab", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006273 RID: 25203 RVA: 0x0026402C File Offset: 0x0026222C
		private async Task SoulSiphonMove(IReadOnlyList<Creature> targets)
		{
			NSleepingVfx sleepingVfx = this.SleepingVfx;
			if (sleepingVfx != null)
			{
				sleepingVfx.Stop();
			}
			this.SleepingVfx = null;
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_cast", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.6f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), targets, -2m, base.Creature, null, false);
			await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), targets, -2m, base.Creature, null, false);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x06006274 RID: 25204 RVA: 0x00264078 File Offset: 0x00262278
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("sleep_loop", true);
			AnimState animState2 = new AnimState("hurt_sleeping", false);
			AnimState animState3 = new AnimState("wake_up", false);
			AnimState animState4 = new AnimState("idle_loop", true);
			AnimState animState5 = new AnimState("cast", false);
			AnimState animState6 = new AnimState("attack_heavy", false);
			AnimState animState7 = new AnimState("attack_double", false);
			AnimState animState8 = new AnimState("hurt", false);
			AnimState animState9 = new AnimState("die", false);
			animState2.NextState = animState3;
			animState3.NextState = animState4;
			animState5.NextState = animState4;
			animState6.NextState = animState4;
			animState7.NextState = animState4;
			animState8.NextState = animState4;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState4, controller);
			creatureAnimator.AddAnyState("Sleep", animState, null);
			creatureAnimator.AddAnyState("Wake", animState3, () => !this.IsAwake);
			creatureAnimator.AddAnyState("Cast", animState5, null);
			creatureAnimator.AddAnyState("AttackHeavy", animState6, null);
			creatureAnimator.AddAnyState("AttackDouble", animState7, null);
			creatureAnimator.AddAnyState("Dead", animState9, null);
			creatureAnimator.AddAnyState("Hit", animState8, () => this.IsAwake);
			creatureAnimator.AddAnyState("Hit", animState2, () => !this.IsAwake);
			return creatureAnimator;
		}

		// Token: 0x06006275 RID: 25205 RVA: 0x002641C5 File Offset: 0x002623C5
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "SLEEP_MOVE";
		}

		// Token: 0x06006276 RID: 25206 RVA: 0x002641D4 File Offset: 0x002623D4
		public override List<BestiaryMonsterMove> GenerateBestiaryMoveList([Nullable(2)] NCreatureVisuals creatureVisuals)
		{
			List<BestiaryMonsterMove> list = base.GenerateBestiaryMoveList(creatureVisuals);
			list.Insert(0, BestiaryMonsterMove.FromAction(base.GetBestiaryMoveName("SLEEP"), new Func<Task>(this.Sleep)));
			list.Insert(1, BestiaryMonsterMove.FromNonStateMove(base.GetBestiaryMoveName("WAKE_UP"), new Func<IReadOnlyList<Creature>, Task>(this.WakeUpMove)));
			return list;
		}

		// Token: 0x04002602 RID: 9730
		private const string _sleepMoveId = "SLEEP_MOVE";

		// Token: 0x04002603 RID: 9731
		public const string slashMoveId = "SLASH_MOVE";

		// Token: 0x04002604 RID: 9732
		private const string _sleepTrigger = "Sleep";

		// Token: 0x04002605 RID: 9733
		public const string wakeTrigger = "Wake";

		// Token: 0x04002606 RID: 9734
		private const string _attackHeavyTrigger = "AttackHeavy";

		// Token: 0x04002607 RID: 9735
		private const string _attackDoubleTrigger = "AttackDouble";

		// Token: 0x04002608 RID: 9736
		private const string _slamSfx = "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_slam";

		// Token: 0x04002609 RID: 9737
		private const string _castSfx = "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_cast";

		// Token: 0x0400260A RID: 9738
		public const string awakenSfx = "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_awaken";

		// Token: 0x0400260B RID: 9739
		private const string _attackStabSfx = "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_attack_stab";

		// Token: 0x0400260C RID: 9740
		private bool _isAwake;

		// Token: 0x0400260D RID: 9741
		private bool _isShellAwake;

		// Token: 0x0400260E RID: 9742
		[Nullable(2)]
		private NSleepingVfx _sleepingVfx;
	}
}
