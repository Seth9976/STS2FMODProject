using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000808 RID: 2056
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Vantom : MonsterModel
	{
		// Token: 0x17001912 RID: 6418
		// (get) Token: 0x06006680 RID: 26240 RVA: 0x00270179 File Offset: 0x0026E379
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 183, 173);
			}
		}

		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x06006681 RID: 26241 RVA: 0x0027018B File Offset: 0x0026E38B
		public int SlipperyAmt
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 9, 8);
			}
		}

		// Token: 0x17001914 RID: 6420
		// (get) Token: 0x06006682 RID: 26242 RVA: 0x00270196 File Offset: 0x0026E396
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001915 RID: 6421
		// (get) Token: 0x06006683 RID: 26243 RVA: 0x0027019E File Offset: 0x0026E39E
		private int InkBlotDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x17001916 RID: 6422
		// (get) Token: 0x06006684 RID: 26244 RVA: 0x002701A9 File Offset: 0x0026E3A9
		private int InkyLanceDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x17001917 RID: 6423
		// (get) Token: 0x06006685 RID: 26245 RVA: 0x002701B4 File Offset: 0x0026E3B4
		private int DismemberDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 30, 26);
			}
		}

		// Token: 0x17001918 RID: 6424
		// (get) Token: 0x06006686 RID: 26246 RVA: 0x002701C1 File Offset: 0x0026E3C1
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Magic;
			}
		}

		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x06006687 RID: 26247 RVA: 0x002701C4 File Offset: 0x0026E3C4
		public override bool ShouldDisappearFromDoom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700191A RID: 6426
		// (get) Token: 0x06006688 RID: 26248 RVA: 0x002701C7 File Offset: 0x0026E3C7
		// (set) Token: 0x06006689 RID: 26249 RVA: 0x002701CF File Offset: 0x0026E3CF
		[Nullable(2)]
		private Tween ScaleTween
		{
			[NullableContext(2)]
			get
			{
				return this._scaleTween;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._scaleTween = value;
			}
		}

		// Token: 0x0600668A RID: 26250 RVA: 0x002701E0 File Offset: 0x0026E3E0
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<SlipperyPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.SlipperyAmt, base.Creature, null, false);
		}

		// Token: 0x0600668B RID: 26251 RVA: 0x00270223 File Offset: 0x0026E423
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature != base.Creature)
			{
				return Task.CompletedTask;
			}
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("vantom_progress", 5f);
			}
			return Task.CompletedTask;
		}

		// Token: 0x0600668C RID: 26252 RVA: 0x00270254 File Offset: 0x0026E454
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaAnimationState animationState = spine.GetAnimationState();
			animationState.SetAnimation("_tracks/charge_up_1", false, 1);
			animationState.AddAnimation("_tracks/charged_1", 0f, true, 1);
		}

		// Token: 0x0600668D RID: 26253 RVA: 0x00270288 File Offset: 0x0026E488
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("INK_BLOT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.InkBlotMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.InkBlotDamage)
			});
			MoveState moveState2 = new MoveState("INKY_LANCE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.InkyLanceMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.InkyLanceDamage, 2)
			});
			MoveState moveState3 = new MoveState("DISMEMBER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DismemberMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.DismemberDamage),
				new StatusIntent(3)
			});
			MoveState moveState4 = new MoveState("PREPARE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PrepareMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600668E RID: 26254 RVA: 0x00270390 File Offset: 0x0026E590
		private async Task InkBlotMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.InkBlotDamage).FromMonster(this).WithAttackerAnim("Attack", 0.35f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/vantom/vantom_inky_lance", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			if (TestMode.IsOff && base.Creature.IsAlive)
			{
				await Cmd.CustomScaledWait(1f, 1f, false, default(CancellationToken));
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.UpdateMusicParameter("vantom_progress", 1f);
				}
				SfxCmd.Play("event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_2", 1f);
				await CreatureCmd.TriggerAnim(base.Creature, "CHARGE_UP", 0.15f);
				NCombatRoom instance2 = NCombatRoom.Instance;
				NCreature ncreature = ((instance2 != null) ? instance2.GetCreatureNode(base.Creature) : null);
				MegaAnimationState megaAnimationState = ((ncreature != null) ? ncreature.SpineAnimation.GetAnimationState() : null);
				if (megaAnimationState != null)
				{
					megaAnimationState.SetAnimation("_tracks/charge_up_2", false, 1);
				}
				if (megaAnimationState != null)
				{
					megaAnimationState.AddAnimation("_tracks/charged_2", 0f, true, 1);
				}
			}
		}

		// Token: 0x0600668F RID: 26255 RVA: 0x002703D4 File Offset: 0x0026E5D4
		private async Task InkyLanceMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.InkyLanceDamage).WithHitCount(2).FromMonster(this)
				.WithAttackerAnim("ATTACK_DOUBLE", 0.4f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/vantom/vantom_inky_lance", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			if (TestMode.IsOff && base.Creature.IsAlive)
			{
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.UpdateMusicParameter("vantom_progress", 2f);
				}
				await Cmd.CustomScaledWait(1f, 1f, false, default(CancellationToken));
				SfxCmd.Play("event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_2", 1f);
				NCombatRoom instance2 = NCombatRoom.Instance;
				NCreature ncreature = ((instance2 != null) ? instance2.GetCreatureNode(base.Creature) : null);
				MegaAnimationState megaAnimationState = ((ncreature != null) ? ncreature.SpineAnimation.GetAnimationState() : null);
				if (megaAnimationState != null)
				{
					megaAnimationState.SetAnimation("_tracks/charge_up_3", false, 1);
				}
				if (megaAnimationState != null)
				{
					megaAnimationState.AddAnimation("_tracks/charged_3", 0f, true, 1);
				}
				await CreatureCmd.TriggerAnim(base.Creature, "CHARGE_UP", 0.15f);
			}
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x00270418 File Offset: 0x0026E618
		private async Task DismemberMove(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff && base.Creature.IsAlive)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(base.Creature) : null);
				MegaAnimationState megaAnimationState = ((ncreature != null) ? ncreature.SpineAnimation.GetAnimationState() : null);
				if (megaAnimationState != null)
				{
					megaAnimationState.SetAnimation("_tracks/attack_heavy", false, 1);
				}
				if (megaAnimationState != null)
				{
					megaAnimationState.AddAnimation("_tracks/charged_0", 0f, true, 1);
				}
			}
			NRunMusicController instance2 = NRunMusicController.Instance;
			if (instance2 != null)
			{
				instance2.UpdateMusicParameter("vantom_progress", 3f);
			}
			await CreatureCmd.TriggerAnim(base.Creature, "ATTACK_HEAVY", 0f);
			await Cmd.Wait(0.25f, false);
			NCombatRoom instance3 = NCombatRoom.Instance;
			if (instance3 != null)
			{
				instance3.RadialBlur(VfxPosition.Left);
			}
			NGame instance4 = NGame.Instance;
			if (instance4 != null)
			{
				instance4.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
			}
			await DamageCmd.Attack(this.DismemberDamage).FromMonster(this).WithNoAttackerAnim()
				.WithHitFx("vfx/vfx_giant_horizontal_slash", "event:/sfx/enemy/enemy_attacks/vantom/vantom_dismember", null)
				.Execute(null);
			NGame instance5 = NGame.Instance;
			if (instance5 != null)
			{
				instance5.DoHitStop(ShakeStrength.Weak, ShakeDuration.Short);
			}
			await Cmd.Wait(0.5f, false);
			await CardPileCmd.AddToCombatAndPreview<Wound>(targets, PileType.Discard, 3, null, CardPilePosition.Bottom);
		}

		// Token: 0x06006691 RID: 26257 RVA: 0x00270464 File Offset: 0x0026E664
		private async Task PrepareMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/vantom/vantom_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "BUFF", 0.6f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
			if (TestMode.IsOff && base.Creature.IsAlive)
			{
				await Cmd.CustomScaledWait(1f, 1f, false, default(CancellationToken));
				SfxCmd.Play("event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_1", 1f);
				NCombatRoom instance = NCombatRoom.Instance;
				NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(base.Creature) : null);
				MegaAnimationState megaAnimationState = ((ncreature != null) ? ncreature.SpineAnimation.GetAnimationState() : null);
				if (megaAnimationState != null)
				{
					megaAnimationState.SetAnimation("_tracks/charge_up_1", false, 1);
				}
				if (megaAnimationState != null)
				{
					megaAnimationState.AddAnimation("_tracks/charged_1", 0f, true, 1);
				}
				await CreatureCmd.TriggerAnim(base.Creature, "CHARGE_UP", 0.25f);
				NRunMusicController instance2 = NRunMusicController.Instance;
				if (instance2 != null)
				{
					instance2.UpdateMusicParameter("vantom_progress", 1f);
				}
			}
		}

		// Token: 0x06006692 RID: 26258 RVA: 0x002704A8 File Offset: 0x0026E6A8
		public void ScaleTo(float scale, float duration)
		{
			NCreature creatureNode = base.Creature.GetCreatureNode();
			Node2D node2D = ((creatureNode != null) ? creatureNode.GetSpecialNode<Node2D>("Visuals/ScalingBone") : null);
			if (node2D != null)
			{
				Tween scaleTween = this.ScaleTween;
				if (scaleTween != null)
				{
					scaleTween.FastForwardToCompletion();
				}
				this.ScaleTween = node2D.CreateTween();
				this.ScaleTween.TweenProperty(node2D, "scale", Vector2.One * scale, (double)duration).SetEase(2L).SetTrans(1L);
			}
		}

		// Token: 0x06006693 RID: 26259 RVA: 0x0027052C File Offset: 0x0026E72C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("buff", false);
			AnimState animState3 = new AnimState("debuff", false);
			AnimState animState4 = new AnimState("attack_double", false);
			AnimState animState5 = new AnimState("attack", false);
			AnimState animState6 = new AnimState("hurt", false);
			AnimState animState7 = new AnimState("die", false);
			AnimState animState8 = new AnimState("charge_up", false);
			AnimState animState9 = new AnimState("attack_heavy", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState;
			animState6.NextState = animState;
			animState8.NextState = animState;
			animState9.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("CHARGE_UP", animState8, null);
			creatureAnimator.AddAnyState("ATTACK_HEAVY", animState9, null);
			creatureAnimator.AddAnyState("BUFF", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState5, null);
			creatureAnimator.AddAnyState("Dead", animState7, null);
			creatureAnimator.AddAnyState("Hit", animState6, null);
			creatureAnimator.AddAnyState("DEBUFF", animState3, null);
			creatureAnimator.AddAnyState("ATTACK_DOUBLE", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x04002719 RID: 10009
		private const string _vantomCustomTrackName = "vantom_progress";

		// Token: 0x0400271A RID: 10010
		private const int _inkyLanceRepeat = 2;

		// Token: 0x0400271B RID: 10011
		private const int _dismemberWounds = 3;

		// Token: 0x0400271C RID: 10012
		private const int _prepareStrength = 2;

		// Token: 0x0400271D RID: 10013
		private const string _chargeUpTrigger = "CHARGE_UP";

		// Token: 0x0400271E RID: 10014
		private const string _buffTrigger = "BUFF";

		// Token: 0x0400271F RID: 10015
		private const string _debuffTrigger = "DEBUFF";

		// Token: 0x04002720 RID: 10016
		private const string _attackDoubleTrigger = "ATTACK_DOUBLE";

		// Token: 0x04002721 RID: 10017
		private const string _heavyAttackTrigger = "ATTACK_HEAVY";

		// Token: 0x04002722 RID: 10018
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_buff";

		// Token: 0x04002723 RID: 10019
		private const string _dismemberSfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_dismember";

		// Token: 0x04002724 RID: 10020
		private const string _extend1Sfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_1";

		// Token: 0x04002725 RID: 10021
		private const string _extend2Sfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_2";

		// Token: 0x04002726 RID: 10022
		private const string _extend3Sfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_2";

		// Token: 0x04002727 RID: 10023
		private const string _inkyLanceSfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_inky_lance";

		// Token: 0x04002728 RID: 10024
		[Nullable(2)]
		private Tween _scaleTween;
	}
}
