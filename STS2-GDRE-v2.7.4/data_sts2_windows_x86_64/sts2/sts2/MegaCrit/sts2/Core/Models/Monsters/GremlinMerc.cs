using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007C1 RID: 1985
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GremlinMerc : MonsterModel
	{
		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x060061B7 RID: 25015 RVA: 0x00261B68 File Offset: 0x0025FD68
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 51, 47);
			}
		}

		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x060061B8 RID: 25016 RVA: 0x00261B74 File Offset: 0x0025FD74
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 53, 49);
			}
		}

		// Token: 0x1700170F RID: 5903
		// (get) Token: 0x060061B9 RID: 25017 RVA: 0x00261B80 File Offset: 0x0025FD80
		public override bool HasDeathSfx
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001710 RID: 5904
		// (get) Token: 0x060061BA RID: 25018 RVA: 0x00261B83 File Offset: 0x0025FD83
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Fur;
			}
		}

		// Token: 0x17001711 RID: 5905
		// (get) Token: 0x060061BB RID: 25019 RVA: 0x00261B86 File Offset: 0x0025FD86
		private int GimmeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 8, 7);
			}
		}

		// Token: 0x17001712 RID: 5906
		// (get) Token: 0x060061BC RID: 25020 RVA: 0x00261B90 File Offset: 0x0025FD90
		private int GimmeRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001713 RID: 5907
		// (get) Token: 0x060061BD RID: 25021 RVA: 0x00261B93 File Offset: 0x0025FD93
		private int DoubleSmashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 7, 6);
			}
		}

		// Token: 0x17001714 RID: 5908
		// (get) Token: 0x060061BE RID: 25022 RVA: 0x00261B9D File Offset: 0x0025FD9D
		private int DoubleSmashRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001715 RID: 5909
		// (get) Token: 0x060061BF RID: 25023 RVA: 0x00261BA0 File Offset: 0x0025FDA0
		private int HeheDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 9, 8);
			}
		}

		// Token: 0x060061C0 RID: 25024 RVA: 0x00261BAC File Offset: 0x0025FDAC
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<SurprisePower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			foreach (Player player in base.CombatState.Players)
			{
				ThieveryPower thieveryPower = (ThieveryPower)ModelDb.Power<ThieveryPower>().ToMutable(0);
				thieveryPower.Target = player.Creature;
				await PowerCmd.Apply(new ThrowingPlayerChoiceContext(), thieveryPower, base.Creature, 20m, base.Creature, null, false);
			}
			IEnumerator<Player> enumerator = null;
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x00261BF0 File Offset: 0x0025FDF0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("GIMME_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GimmeMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.GimmeDamage, this.GimmeRepeat)
			});
			MoveState moveState2 = new MoveState("DOUBLE_SMASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DoubleSmashMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.DoubleSmashDamage, this.DoubleSmashRepeat),
				new DebuffIntent(false)
			});
			MoveState moveState3 = new MoveState("HEHE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HeheMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.HeheDamage),
				new BuffIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x00261CD4 File Offset: 0x0025FED4
		private async Task GimmeMove(IReadOnlyList<Creature> targets)
		{
			if (!this._hasSpoken)
			{
				this._hasSpoken = true;
				LocString locString = MonsterModel.L10NMonsterLookup("GREMLIN_MERC.moves.GIMME.banter");
				TalkCmd.Play(locString, base.Creature, VfxColor.Purple, VfxDuration.VeryShort);
				await Cmd.CustomScaledWait(0.25f, 0.5f, false, default(CancellationToken));
			}
			VfxCmd.PlayOnCreatureCenters(targets, "vfx/vfx_coin_explosion_regular");
			await DamageCmd.Attack(this.GimmeDamage).WithHitCount(this.GimmeRepeat).FromMonster(this)
				.WithAttackerAnim("AttackDouble", 0.15f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			foreach (ThieveryPower thieveryPower in base.Creature.GetPowerInstances<ThieveryPower>())
			{
				await thieveryPower.Steal();
			}
			IEnumerator<ThieveryPower> enumerator = null;
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x00261D20 File Offset: 0x0025FF20
		private async Task DoubleSmashMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.AttackSfx, 1f);
			VfxCmd.PlayOnCreatureCenters(targets, "vfx/vfx_coin_explosion_regular");
			await DamageCmd.Attack(this.DoubleSmashDamage).WithHitCount(this.DoubleSmashRepeat).FromMonster(this)
				.WithAttackerAnim("AttackDouble", 0.15f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			foreach (ThieveryPower thieveryPower in base.Creature.GetPowerInstances<ThieveryPower>())
			{
				await thieveryPower.Steal();
			}
			IEnumerator<ThieveryPower> enumerator = null;
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x060061C4 RID: 25028 RVA: 0x00261D6C File Offset: 0x0025FF6C
		private async Task HeheMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.HeheDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/gremlin_merc/gremlin_merc_attack_buff", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			foreach (ThieveryPower thieveryPower in base.Creature.GetPowerInstances<ThieveryPower>())
			{
				await thieveryPower.Steal();
			}
			IEnumerator<ThieveryPower> enumerator = null;
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x060061C5 RID: 25029 RVA: 0x00261DB0 File Offset: 0x0025FFB0
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack_single", false);
			AnimState animState3 = new AnimState("attack_double", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Attack", animState2, null);
			creatureAnimator.AddAnyState("AttackDouble", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x040025CC RID: 9676
		private const string _attackBuffSfx = "event:/sfx/enemy/enemy_attacks/gremlin_merc/gremlin_merc_attack_buff";

		// Token: 0x040025CD RID: 9677
		private bool _hasSpoken;

		// Token: 0x040025CE RID: 9678
		private const string _attackDoubleTrigger = "AttackDouble";
	}
}
