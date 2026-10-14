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
	// Token: 0x020007F2 RID: 2034
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpinyToad : MonsterModel
	{
		// Token: 0x17001868 RID: 6248
		// (get) Token: 0x060064DF RID: 25823 RVA: 0x0026B4BF File Offset: 0x002696BF
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 121, 116);
			}
		}

		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x060064E0 RID: 25824 RVA: 0x0026B4CB File Offset: 0x002696CB
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 124, 119);
			}
		}

		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x060064E1 RID: 25825 RVA: 0x0026B4D7 File Offset: 0x002696D7
		private int LashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 17);
			}
		}

		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x060064E2 RID: 25826 RVA: 0x0026B4E4 File Offset: 0x002696E4
		private int ExplosionDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 25, 23);
			}
		}

		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x060064E3 RID: 25827 RVA: 0x0026B4F1 File Offset: 0x002696F1
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_lick";
			}
		}

		// Token: 0x1700186D RID: 6253
		// (get) Token: 0x060064E4 RID: 25828 RVA: 0x0026B4F8 File Offset: 0x002696F8
		// (set) Token: 0x060064E5 RID: 25829 RVA: 0x0026B500 File Offset: 0x00269700
		public bool IsSpiny
		{
			get
			{
				return this._isSpiny;
			}
			set
			{
				base.AssertMutable();
				this._isSpiny = value;
			}
		}

		// Token: 0x1700186E RID: 6254
		// (get) Token: 0x060064E6 RID: 25830 RVA: 0x0026B50F File Offset: 0x0026970F
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_die";
			}
		}

		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x060064E7 RID: 25831 RVA: 0x0026B516 File Offset: 0x00269716
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x060064E8 RID: 25832 RVA: 0x0026B51C File Offset: 0x0026971C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("PROTRUDING_SPIKES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpikesMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("SPIKE_EXPLOSION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ExplosionMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ExplosionDamage)
			});
			MoveState moveState3 = new MoveState("TONGUE_LASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.LashDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060064E9 RID: 25833 RVA: 0x0026B5DC File Offset: 0x002697DC
		private async Task SpikesMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_protrude", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Spiked", 0.5f);
			this.IsSpiny = true;
			await PowerCmd.Apply<ThornsPower>(new ThrowingPlayerChoiceContext(), base.Creature, 5m, base.Creature, null, false);
		}

		// Token: 0x060064EA RID: 25834 RVA: 0x0026B620 File Offset: 0x00269820
		private async Task ExplosionMove(IReadOnlyList<Creature> targets)
		{
			this.IsSpiny = false;
			await DamageCmd.Attack(this.ExplosionDamage).FromMonster(this).WithAttackerAnim("Unspiked", 0.7f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_explode", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<ThornsPower>(new ThrowingPlayerChoiceContext(), base.Creature, -5m, base.Creature, null, false);
			await Cmd.Wait(1f, false);
		}

		// Token: 0x060064EB RID: 25835 RVA: 0x0026B664 File Offset: 0x00269864
		private async Task LashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.LashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060064EC RID: 25836 RVA: 0x0026B6A8 File Offset: 0x002698A8
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("hurt", false);
			AnimState animState3 = new AnimState("die", false);
			AnimState animState4 = new AnimState("protrude", false);
			AnimState animState5 = new AnimState("lick", false);
			AnimState animState6 = new AnimState("explode", false);
			AnimState animState7 = new AnimState("idle_naked_loop", true);
			AnimState animState8 = new AnimState("hurt_naked", false);
			AnimState animState9 = new AnimState("die_naked", false);
			animState7.AddBranch("Spiked", animState4, null);
			animState.AddBranch("Unspiked", animState6, null);
			animState4.NextState = animState;
			animState8.NextState = animState7;
			animState8.AddBranch("Spiked", animState4, null);
			animState2.NextState = animState;
			animState2.AddBranch("Unspiked", animState6, null);
			animState5.NextState = animState7;
			animState6.NextState = animState7;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState7, controller);
			creatureAnimator.AddAnyState("Attack", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState8, () => !this.IsSpiny);
			creatureAnimator.AddAnyState("Hit", animState2, () => this.IsSpiny);
			creatureAnimator.AddAnyState("Dead", animState9, () => !this.IsSpiny);
			creatureAnimator.AddAnyState("Dead", animState3, () => this.IsSpiny);
			return creatureAnimator;
		}

		// Token: 0x040026B0 RID: 9904
		private const string _spikeTrigger = "Spiked";

		// Token: 0x040026B1 RID: 9905
		private const string _unSpikeTrigger = "Unspiked";

		// Token: 0x040026B2 RID: 9906
		private const string _attackHeavySfx = "event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_explode";

		// Token: 0x040026B3 RID: 9907
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_protrude";

		// Token: 0x040026B4 RID: 9908
		private bool _isSpiny;
	}
}
