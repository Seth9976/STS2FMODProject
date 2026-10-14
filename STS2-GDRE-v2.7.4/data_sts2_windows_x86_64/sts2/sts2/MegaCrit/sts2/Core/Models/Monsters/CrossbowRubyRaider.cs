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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007A9 RID: 1961
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CrossbowRubyRaider : MonsterModel
	{
		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x0600605B RID: 24667 RVA: 0x0025D74A File Offset: 0x0025B94A
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 18);
			}
		}

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x0600605C RID: 24668 RVA: 0x0025D756 File Offset: 0x0025B956
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 21);
			}
		}

		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x0600605D RID: 24669 RVA: 0x0025D762 File Offset: 0x0025B962
		private int FireDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
			}
		}

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x0600605E RID: 24670 RVA: 0x0025D76F File Offset: 0x0025B96F
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x0600605F RID: 24671 RVA: 0x0025D772 File Offset: 0x0025B972
		// (set) Token: 0x06006060 RID: 24672 RVA: 0x0025D77A File Offset: 0x0025B97A
		private bool IsCrossbowReloaded
		{
			get
			{
				return this._isCrossbowReloaded;
			}
			set
			{
				base.AssertMutable();
				this._isCrossbowReloaded = value;
			}
		}

		// Token: 0x06006061 RID: 24673 RVA: 0x0025D78C File Offset: 0x0025B98C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("FIRE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FireMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.FireDamage)
			});
			MoveState moveState2 = new MoveState("RELOAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReloadMove), new AbstractIntent[]
			{
				new DefendIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState2);
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x06006062 RID: 24674 RVA: 0x0025D814 File Offset: 0x0025BA14
		private async Task FireMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.FireDamage).FromMonster(this).WithAttackerAnim("Attack", 0.25f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			this.IsCrossbowReloaded = false;
		}

		// Token: 0x06006063 RID: 24675 RVA: 0x0025D858 File Offset: 0x0025BA58
		private async Task ReloadMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/crossbow_ruby_raider/crossbow_ruby_raider_reload", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Reload", 0.25f);
			await CreatureCmd.GainBlock(base.Creature, 3m, ValueProp.Move, null, false);
			this.IsCrossbowReloaded = true;
		}

		// Token: 0x06006064 RID: 24676 RVA: 0x0025D89C File Offset: 0x0025BA9C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("hurt", false);
			AnimState animState3 = new AnimState("die", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt_empty", false);
			AnimState animState6 = new AnimState("idle_loop_empty", true);
			AnimState animState7 = new AnimState("hurt_empty", false);
			AnimState animState8 = new AnimState("die_empty", false);
			AnimState animState9 = new AnimState("reload", false);
			animState2.NextState = animState;
			animState4.NextState = animState6;
			animState5.NextState = animState6;
			animState7.NextState = animState6;
			animState9.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState6, controller);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("Reload", animState9, null);
			creatureAnimator.AddAnyState("Hit", animState7, () => !this.IsCrossbowReloaded);
			creatureAnimator.AddAnyState("Hit", animState2, () => this.IsCrossbowReloaded);
			creatureAnimator.AddAnyState("Dead", animState8, () => !this.IsCrossbowReloaded);
			creatureAnimator.AddAnyState("Dead", animState3, () => this.IsCrossbowReloaded);
			return creatureAnimator;
		}

		// Token: 0x04002574 RID: 9588
		private const string _reloadTrigger = "Reload";

		// Token: 0x04002575 RID: 9589
		private const string _reloadSfx = "event:/sfx/enemy/enemy_attacks/crossbow_ruby_raider/crossbow_ruby_raider_reload";

		// Token: 0x04002576 RID: 9590
		private bool _isCrossbowReloaded;
	}
}
