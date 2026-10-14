using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007E2 RID: 2018
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Rocket : MonsterModel
	{
		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x060063E8 RID: 25576 RVA: 0x002684C3 File Offset: 0x002666C3
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_die";
			}
		}

		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x060063E9 RID: 25577 RVA: 0x002684CA File Offset: 0x002666CA
		public override bool ShouldFadeAfterDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x060063EA RID: 25578 RVA: 0x002684CD File Offset: 0x002666CD
		public override bool ShouldDisappearFromDoom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x060063EB RID: 25579 RVA: 0x002684D0 File Offset: 0x002666D0
		public override float DeathAnimLengthOverride
		{
			get
			{
				return 2.5f;
			}
		}

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x060063EC RID: 25580 RVA: 0x002684D8 File Offset: 0x002666D8
		[Nullable(2)]
		private NKaiserCrabBossBackground Background
		{
			[NullableContext(2)]
			get
			{
				base.AssertMutable();
				if (this._background == null)
				{
					NCombatRoom instance = NCombatRoom.Instance;
					NCombatBackground ncombatBackground;
					if ((ncombatBackground = ((instance != null) ? instance.Background : null)) == null)
					{
						NBestiary instance2 = NBestiary.Instance;
						ncombatBackground = ((instance2 != null) ? instance2.Layout : null);
					}
					Control control = ncombatBackground;
					this._background = ((control != null) ? control.GetNode<NKaiserCrabBossBackground>("%KaiserCrab") : null);
				}
				return this._background;
			}
		}

		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x060063ED RID: 25581 RVA: 0x0026853C File Offset: 0x0026673C
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 209, 199);
			}
		}

		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x060063EE RID: 25582 RVA: 0x0026854E File Offset: 0x0026674E
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x060063EF RID: 25583 RVA: 0x00268556 File Offset: 0x00266756
		private int TargetingReticleDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x060063F0 RID: 25584 RVA: 0x00268561 File Offset: 0x00266761
		private int PrecisionBeamDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 20, 18);
			}
		}

		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x060063F1 RID: 25585 RVA: 0x0026856E File Offset: 0x0026676E
		private int LaserDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 35, 31);
			}
		}

		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x060063F2 RID: 25586 RVA: 0x0026857B File Offset: 0x0026677B
		private int ChargeUpStrengthGain
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x060063F3 RID: 25587 RVA: 0x00268588 File Offset: 0x00266788
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("TARGETING_RETICLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TargetingReticleMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.TargetingReticleDamage)
			});
			MoveState moveState2 = new MoveState("PRECISION_BEAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PrecisionBeamMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.PrecisionBeamDamage)
			});
			MoveState moveState3 = new MoveState("CHARGE_UP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ChargeUpMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState4 = new MoveState("LASER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LaserMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.LaserDamage)
			});
			MoveState moveState5 = new MoveState("RECHARGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RechargeMove), new AbstractIntent[]
			{
				new SleepIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState5;
			moveState5.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			list.Add(moveState5);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060063F4 RID: 25588 RVA: 0x002686BC File Offset: 0x002668BC
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<SurroundedPower>(new ThrowingPlayerChoiceContext(), base.CombatState.GetOpponentsOf(base.Creature), 1m, base.Creature, null, false);
			await PowerCmd.Apply<BackAttackRightPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			await PowerCmd.Apply<CrabRagePower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			NKaiserCrabBossBackground background = this.Background;
			if (background != null)
			{
				background.SetVisible(true);
			}
		}

		// Token: 0x060063F5 RID: 25589 RVA: 0x002686FF File Offset: 0x002668FF
		public override Task AfterCurrentHpChanged(Creature creature, decimal delta)
		{
			if (creature != base.Creature || delta >= 0m)
			{
				return Task.CompletedTask;
			}
			NKaiserCrabBossBackground background = this.Background;
			if (background != null)
			{
				background.PlayHurtAnim(NKaiserCrabBossBackground.ArmSide.Right);
			}
			return Task.CompletedTask;
		}

		// Token: 0x060063F6 RID: 25590 RVA: 0x00268734 File Offset: 0x00266934
		public override Task BeforeDeath(Creature creature)
		{
			if (creature != base.Creature)
			{
				return Task.CompletedTask;
			}
			NAudioManager.Instance.PlayOneShot(this.DeathSfx, 1f);
			NKaiserCrabBossBackground background = this.Background;
			if (background != null)
			{
				background.PlayArmDeathAnim(NKaiserCrabBossBackground.ArmSide.Right);
			}
			if (CombatManager.Instance.IsOverOrEnding)
			{
				NKaiserCrabBossBackground background2 = this.Background;
				if (background2 != null)
				{
					background2.PlayBodyDeathAnim();
				}
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.UpdateMusicParameter("kaiser_crab_progress", 5f);
				}
			}
			else
			{
				NRunMusicController instance2 = NRunMusicController.Instance;
				if (instance2 != null)
				{
					instance2.UpdateMusicParameter("kaiser_crab_progress", 1f);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x060063F7 RID: 25591 RVA: 0x002687CF File Offset: 0x002669CF
		public override void BeforeRemovedFromRoom()
		{
			SfxCmd.StopLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_buff");
		}

		// Token: 0x060063F8 RID: 25592 RVA: 0x002687E4 File Offset: 0x002669E4
		private async Task TargetingReticleMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_attack_snap", 1f);
			NKaiserCrabBossBackground background = this.Background;
			await (((background != null) ? background.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Right, "attack", 0.35f) : null) ?? Task.CompletedTask);
			await DamageCmd.Attack(this.TargetingReticleDamage).FromMonster(this).WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060063F9 RID: 25593 RVA: 0x00268828 File Offset: 0x00266A28
		private async Task PrecisionBeamMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_attack_slam", 1f);
			NKaiserCrabBossBackground background = this.Background;
			await (((background != null) ? background.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Right, "attack_med", 0.5f) : null) ?? Task.CompletedTask);
			await DamageCmd.Attack(this.PrecisionBeamDamage).FromMonster(this).WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(null);
		}

		// Token: 0x060063FA RID: 25594 RVA: 0x0026886C File Offset: 0x00266A6C
		private async Task ChargeUpMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.PlayLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_buff");
			NKaiserCrabBossBackground background = this.Background;
			await (((background != null) ? background.PlayRightSideChargeUpAnim(0.7f) : null) ?? Task.CompletedTask);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.ChargeUpStrengthGain, base.Creature, null, false);
		}

		// Token: 0x060063FB RID: 25595 RVA: 0x002688B0 File Offset: 0x00266AB0
		private async Task LaserMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.StopLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_buff");
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_rocket", 1f);
			NKaiserCrabBossBackground background = this.Background;
			await (((background != null) ? background.PlayRightSideHeavy(0.5f) : null) ?? Task.CompletedTask);
			await DamageCmd.Attack(this.LaserDamage).FromMonster(this).WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x060063FC RID: 25596 RVA: 0x002688F4 File Offset: 0x00266AF4
		private async Task RechargeMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_regrow", 1f);
			NKaiserCrabBossBackground background = this.Background;
			await (((background != null) ? background.PlayRightRecharge(0.5f) : null) ?? Task.CompletedTask);
		}

		// Token: 0x0400266B RID: 9835
		private const string _attackSlamSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_attack_slam";

		// Token: 0x0400266C RID: 9836
		private const string _attackSnapSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_attack_snap";

		// Token: 0x0400266D RID: 9837
		private const string _attackRegrowSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_regrow";

		// Token: 0x0400266E RID: 9838
		private const string _buffSfxLoop = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_buff";

		// Token: 0x0400266F RID: 9839
		private const string _rocketSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_rocket";

		// Token: 0x04002670 RID: 9840
		[Nullable(2)]
		private NKaiserCrabBossBackground _background;
	}
}
