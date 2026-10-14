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

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007DB RID: 2011
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OwlMagistrate : MonsterModel
	{
		// Token: 0x170017C3 RID: 6083
		// (get) Token: 0x06006361 RID: 25441 RVA: 0x00266AFA File Offset: 0x00264CFA
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 247, 231);
			}
		}

		// Token: 0x170017C4 RID: 6084
		// (get) Token: 0x06006362 RID: 25442 RVA: 0x00266B0C File Offset: 0x00264D0C
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x06006363 RID: 25443 RVA: 0x00266B14 File Offset: 0x00264D14
		private int VerdictDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 36, 33);
			}
		}

		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x06006364 RID: 25444 RVA: 0x00266B21 File Offset: 0x00264D21
		private int ScrutinyDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 16);
			}
		}

		// Token: 0x170017C7 RID: 6087
		// (get) Token: 0x06006365 RID: 25445 RVA: 0x00266B2E File Offset: 0x00264D2E
		private int PeckAssaultDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 4);
			}
		}

		// Token: 0x170017C8 RID: 6088
		// (get) Token: 0x06006366 RID: 25446 RVA: 0x00266B39 File Offset: 0x00264D39
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x06006367 RID: 25447 RVA: 0x00266B3C File Offset: 0x00264D3C
		public override string HurtSfx
		{
			get
			{
				if (!this.IsFlying)
				{
					return "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_hurt";
				}
				return "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_hurt_flying";
			}
		}

		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x06006368 RID: 25448 RVA: 0x00266B51 File Offset: 0x00264D51
		public override string DeathSfx
		{
			get
			{
				if (!this.IsFlying)
				{
					return "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_die";
				}
				return "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_die_flying";
			}
		}

		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x06006369 RID: 25449 RVA: 0x00266B66 File Offset: 0x00264D66
		// (set) Token: 0x0600636A RID: 25450 RVA: 0x00266B6E File Offset: 0x00264D6E
		private bool IsFlying
		{
			get
			{
				return this._isFlying;
			}
			set
			{
				base.AssertMutable();
				this._isFlying = value;
			}
		}

		// Token: 0x0600636B RID: 25451 RVA: 0x00266B7D File Offset: 0x00264D7D
		public override void BeforeRemovedFromRoom()
		{
			if (this.IsFlying)
			{
				SfxCmd.StopLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_fly_loop");
			}
		}

		// Token: 0x0600636C RID: 25452 RVA: 0x00266B98 File Offset: 0x00264D98
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("MAGISTRATE_SCRUTINY", new Func<IReadOnlyList<Creature>, Task>(this.MagistrateScrutinyMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ScrutinyDamage)
			});
			MoveState moveState2 = new MoveState("PECK_ASSAULT", new Func<IReadOnlyList<Creature>, Task>(this.PeckAssaultMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.PeckAssaultDamage, 6)
			});
			MoveState moveState3 = new MoveState("JUDICIAL_FLIGHT", new Func<IReadOnlyList<Creature>, Task>(this.JudicialFlightMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState4 = new MoveState("VERDICT", new Func<IReadOnlyList<Creature>, Task>(this.VerdictMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.VerdictDamage),
				new DebuffIntent(false)
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

		// Token: 0x0600636D RID: 25453 RVA: 0x00266CA0 File Offset: 0x00264EA0
		private async Task MagistrateScrutinyMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ScrutinyDamage).FromMonster(this).WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_attack_peck", null)
				.WithAttackerAnim("Attack", 0.5f, null)
				.WithHitFx("vfx/vfx_gaze", null, null)
				.Execute(null);
		}

		// Token: 0x0600636E RID: 25454 RVA: 0x00266CE4 File Offset: 0x00264EE4
		private async Task PeckAssaultMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.PeckAssaultDamage).WithHitCount(6).FromMonster(this)
				.WithAttackerAnim("Attack", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_attack_peck", null)
				.OnlyPlayAnimOnce()
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600636F RID: 25455 RVA: 0x00266D28 File Offset: 0x00264F28
		private async Task JudicialFlightMove(IReadOnlyList<Creature> targets)
		{
			this.IsFlying = true;
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_take_off", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "TakeOff", 0f);
			await Cmd.Wait(1.25f, false);
			await PowerCmd.Apply<SoarPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			SfxCmd.PlayLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_fly_loop");
		}

		// Token: 0x06006370 RID: 25456 RVA: 0x00266D6C File Offset: 0x00264F6C
		private async Task VerdictMove(IReadOnlyList<Creature> targets)
		{
			this.IsFlying = false;
			SfxCmd.StopLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_fly_loop");
			await DamageCmd.Attack(this.VerdictDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithHitFx("vfx/vfx_attack_slash", "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_attack_dive", null)
				.Execute(null);
			await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, 4m, base.Creature, null, false);
			await PowerCmd.Remove<SoarPower>(base.Creature);
			await Cmd.Wait(1f, false);
		}

		// Token: 0x06006371 RID: 25457 RVA: 0x00266DB8 File Offset: 0x00264FB8
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack_peck", false);
			AnimState animState3 = new AnimState("hurt", false);
			AnimState animState4 = new AnimState("die", false);
			AnimState animState5 = new AnimState("take_off", false);
			AnimState animState6 = new AnimState("fly_loop", true)
			{
				BoundsContainer = "FlyingBounds"
			};
			AnimState animState7 = new AnimState("attack_dive", false)
			{
				BoundsContainer = "IdleBounds"
			};
			AnimState animState8 = new AnimState("hurt_flying", false);
			AnimState animState9 = new AnimState("die_flying", false);
			animState.AddBranch("Attack", animState2, null);
			animState.AddBranch("Hit", animState3, null);
			animState.AddBranch("Dead", animState4, null);
			animState2.NextState = animState;
			animState2.AddBranch("Hit", animState3, null);
			animState2.AddBranch("Dead", animState4, null);
			animState3.NextState = animState;
			animState3.AddBranch("Attack", animState2, null);
			animState3.AddBranch("Dead", animState4, null);
			animState3.AddBranch("Hit", animState3, null);
			animState5.NextState = animState6;
			animState5.AddBranch("Hit", animState8, null);
			animState5.AddBranch("Dead", animState9, null);
			animState6.AddBranch("Attack", animState7, null);
			animState6.AddBranch("Hit", animState8, null);
			animState6.AddBranch("Dead", animState9, null);
			animState8.NextState = animState6;
			animState8.AddBranch("Attack", animState7, null);
			animState8.AddBranch("Dead", animState9, null);
			animState7.NextState = animState;
			animState7.AddBranch("Attack", animState2, null);
			animState7.AddBranch("Dead", animState4, null);
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("TakeOff", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x04002644 RID: 9796
		private const int _peckAssaultRepeat = 6;

		// Token: 0x04002645 RID: 9797
		private const string _attackPeckAnimId = "attack_peck";

		// Token: 0x04002646 RID: 9798
		private const string _takeOffTrigger = "TakeOff";

		// Token: 0x04002647 RID: 9799
		private const string _attackPeckSfx = "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_attack_peck";

		// Token: 0x04002648 RID: 9800
		private const string _attackDiveSfx = "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_attack_dive";

		// Token: 0x04002649 RID: 9801
		private const string _takeOffSfx = "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_take_off";

		// Token: 0x0400264A RID: 9802
		private const string _flyLoopSfx = "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_fly_loop";

		// Token: 0x0400264B RID: 9803
		private bool _isFlying;
	}
}
