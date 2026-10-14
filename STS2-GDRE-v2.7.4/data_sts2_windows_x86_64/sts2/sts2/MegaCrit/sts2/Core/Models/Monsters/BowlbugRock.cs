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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x0200079F RID: 1951
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BowlbugRock : MonsterModel
	{
		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x06005FA4 RID: 24484 RVA: 0x0025B897 File Offset: 0x00259A97
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 46, 45);
			}
		}

		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x06005FA5 RID: 24485 RVA: 0x0025B8A3 File Offset: 0x00259AA3
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 49, 48);
			}
		}

		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x06005FA6 RID: 24486 RVA: 0x0025B8AF File Offset: 0x00259AAF
		public static int HeadbuttDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 15);
			}
		}

		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x06005FA7 RID: 24487 RVA: 0x0025B8BC File Offset: 0x00259ABC
		// (set) Token: 0x06005FA8 RID: 24488 RVA: 0x0025B8C4 File Offset: 0x00259AC4
		public bool IsOffBalance
		{
			get
			{
				return this._isOffBalance;
			}
			set
			{
				base.AssertMutable();
				this._isOffBalance = value;
			}
		}

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x06005FA9 RID: 24489 RVA: 0x0025B8D3 File Offset: 0x00259AD3
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/workbug_rock/workbug_rock_die";
			}
		}

		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x06005FAA RID: 24490 RVA: 0x0025B8DA File Offset: 0x00259ADA
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/workbug_rock/workbug_rock_attack";
			}
		}

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x06005FAB RID: 24491 RVA: 0x0025B8E1 File Offset: 0x00259AE1
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Insect;
			}
		}

		// Token: 0x06005FAC RID: 24492 RVA: 0x0025B8E4 File Offset: 0x00259AE4
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			skeleton.SetSkin(skeleton.GetData().FindSkin("rock"));
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06005FAD RID: 24493 RVA: 0x0025B904 File Offset: 0x00259B04
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<ImbalancedPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x06005FAE RID: 24494 RVA: 0x0025B948 File Offset: 0x00259B48
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("HEADBUTT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HeadbuttMove), new AbstractIntent[]
			{
				new SingleAttackIntent(BowlbugRock.HeadbuttDamage)
			});
			MoveState moveState2 = new MoveState("DIZZY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DizzyMove), new AbstractIntent[]
			{
				new StunIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("POST_HEADBUTT");
			moveState.FollowUpState = conditionalBranchState;
			moveState2.FollowUpState = moveState;
			conditionalBranchState.AddState(moveState2, () => this.IsOffBalance);
			conditionalBranchState.AddState(moveState, () => !this.IsOffBalance);
			list.Add(moveState2);
			list.Add(conditionalBranchState);
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005FAF RID: 24495 RVA: 0x0025BA08 File Offset: 0x00259C08
		private async Task HeadbuttMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(BowlbugRock.HeadbuttDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			if (this.IsOffBalance)
			{
				await this.Stun();
			}
		}

		// Token: 0x06005FB0 RID: 24496 RVA: 0x0025BA4C File Offset: 0x00259C4C
		private async Task Stun()
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/workbug_rock/workbug_rock_stun", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Stun", 0.6f);
			await CreatureCmd.Stun(base.Creature, new Func<IReadOnlyList<Creature>, Task>(this.DizzyMove), null);
		}

		// Token: 0x06005FB1 RID: 24497 RVA: 0x0025BA90 File Offset: 0x00259C90
		private async Task DizzyMove(IReadOnlyList<Creature> targets)
		{
			this.IsOffBalance = false;
			await CreatureCmd.TriggerAnim(base.Creature, "Unstun", 0.6f);
		}

		// Token: 0x06005FB2 RID: 24498 RVA: 0x0025BAD4 File Offset: 0x00259CD4
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("buff", false);
			AnimState animState3 = new AnimState("headbutt", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("hurt_stunned", false);
			AnimState animState6 = new AnimState("wake_up", false);
			AnimState animState7 = new AnimState("die", false);
			AnimState animState8 = new AnimState("stun", false);
			AnimState animState9 = new AnimState("stunned_loop", true);
			animState2.NextState = animState;
			animState4.NextState = animState;
			animState3.NextState = animState;
			animState8.NextState = animState9;
			animState5.NextState = animState9;
			animState6.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState7, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Hit", animState4, () => !this.IsOffBalance);
			creatureAnimator.AddAnyState("Hit", animState5, () => this.IsOffBalance);
			creatureAnimator.AddAnyState("Stun", animState8, null);
			creatureAnimator.AddAnyState("Unstun", animState6, null);
			return creatureAnimator;
		}

		// Token: 0x06005FB3 RID: 24499 RVA: 0x0025BC08 File Offset: 0x00259E08
		public override List<BestiaryMonsterMove> GenerateBestiaryMoveList([Nullable(2)] NCreatureVisuals creatureVisuals)
		{
			List<BestiaryMonsterMove> list = base.GenerateBestiaryMoveList(creatureVisuals);
			list.Insert(1, BestiaryMonsterMove.FromStun(new Func<Task>(this.Stun)));
			return list;
		}

		// Token: 0x0400254A RID: 9546
		private const string _dizzyMove = "DIZZY_MOVE";

		// Token: 0x0400254B RID: 9547
		private const string _stunTrigger = "Stun";

		// Token: 0x0400254C RID: 9548
		private const string _wakeUpTrigger = "Unstun";

		// Token: 0x0400254D RID: 9549
		private bool _isOffBalance;

		// Token: 0x0400254E RID: 9550
		private const string _stunSfx = "event:/sfx/enemy/enemy_attacks/workbug_rock/workbug_rock_stun";

		// Token: 0x0400254F RID: 9551
		private const string _spineSkin = "rock";
	}
}
