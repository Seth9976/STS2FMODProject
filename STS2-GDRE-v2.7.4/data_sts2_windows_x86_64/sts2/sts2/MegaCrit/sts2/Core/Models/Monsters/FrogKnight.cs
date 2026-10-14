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
	// Token: 0x020007BD RID: 1981
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FrogKnight : MonsterModel
	{
		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x06006175 RID: 24949 RVA: 0x00260F81 File Offset: 0x0025F181
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 199, 191);
			}
		}

		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x06006176 RID: 24950 RVA: 0x00260F93 File Offset: 0x0025F193
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170016F6 RID: 5878
		// (get) Token: 0x06006177 RID: 24951 RVA: 0x00260F9B File Offset: 0x0025F19B
		private int StrikeDownEvilDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 23, 21);
			}
		}

		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x06006178 RID: 24952 RVA: 0x00260FA8 File Offset: 0x0025F1A8
		private int TongueLashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 13);
			}
		}

		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x06006179 RID: 24953 RVA: 0x00260FB5 File Offset: 0x0025F1B5
		private int BeetleChargeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 40, 35);
			}
		}

		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x0600617A RID: 24954 RVA: 0x00260FC2 File Offset: 0x0025F1C2
		private int PlatingAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 15);
			}
		}

		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x0600617B RID: 24955 RVA: 0x00260FCE File Offset: 0x0025F1CE
		// (set) Token: 0x0600617C RID: 24956 RVA: 0x00260FD6 File Offset: 0x0025F1D6
		private bool HasBeetleCharged
		{
			get
			{
				return this._hasBeetleCharged;
			}
			set
			{
				base.AssertMutable();
				this._hasBeetleCharged = value;
			}
		}

		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x0600617D RID: 24957 RVA: 0x00260FE5 File Offset: 0x0025F1E5
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x0600617E RID: 24958 RVA: 0x00260FE8 File Offset: 0x0025F1E8
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.PlatingAmount, base.Creature, null, false);
			this.HasBeetleCharged = false;
		}

		// Token: 0x0600617F RID: 24959 RVA: 0x0026102C File Offset: 0x0025F22C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("FOR_THE_QUEEN", new Func<IReadOnlyList<Creature>, Task>(this.ForTheQueenMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("STRIKE_DOWN_EVIL", new Func<IReadOnlyList<Creature>, Task>(this.StrikeDownEvilMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.StrikeDownEvilDamage)
			});
			MoveState moveState3 = new MoveState("TONGUE_LASH", new Func<IReadOnlyList<Creature>, Task>(this.TongueLashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.TongueLashDamage),
				new DebuffIntent(false)
			});
			MoveState moveState4 = new MoveState("BEETLE_CHARGE", new Func<IReadOnlyList<Creature>, Task>(this.BeetleChargeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BeetleChargeDamage)
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("HALF_HEALTH");
			conditionalBranchState.AddState(moveState3, () => this.HasBeetleCharged || base.Creature.CurrentHp >= base.Creature.MaxHp / 2);
			conditionalBranchState.AddState(moveState4, () => !this.HasBeetleCharged && base.Creature.CurrentHp < base.Creature.MaxHp / 2);
			moveState.FollowUpState = conditionalBranchState;
			moveState2.FollowUpState = moveState;
			moveState3.FollowUpState = moveState2;
			moveState4.FollowUpState = moveState3;
			list.Add(conditionalBranchState);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState3);
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x00261170 File Offset: 0x0025F370
		private async Task ForTheQueenMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Buff", 0.4f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 5m, base.Creature, null, false);
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x002611B4 File Offset: 0x0025F3B4
		private async Task StrikeDownEvilMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.StrikeDownEvilDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006182 RID: 24962 RVA: 0x002611F8 File Offset: 0x0025F3F8
		private async Task TongueLashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.TongueLashDamage).FromMonster(this).WithAttackerAnim("Lash", 0.3f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_tongue_lash", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x06006183 RID: 24963 RVA: 0x00261244 File Offset: 0x0025F444
		private async Task BeetleChargeMove(IReadOnlyList<Creature> targets)
		{
			this.HasBeetleCharged = true;
			await DamageCmd.Attack(this.BeetleChargeDamage).FromMonster(this).WithAttackerAnim("charge", 0.6f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_charge", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await Cmd.Wait(1f, false);
		}

		// Token: 0x06006184 RID: 24964 RVA: 0x00261288 File Offset: 0x0025F488
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("buff", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("attack_tongue", false);
			AnimState animState5 = new AnimState("charge", false);
			AnimState animState6 = new AnimState("hurt", false);
			AnimState animState7 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState6.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Buff", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Lash", animState4, null);
			creatureAnimator.AddAnyState("charge", animState5, null);
			creatureAnimator.AddAnyState("Dead", animState7, null);
			animState.AddBranch("Hit", animState6, null);
			animState2.AddBranch("Hit", animState6, null);
			animState3.AddBranch("Hit", animState6, null);
			animState4.AddBranch("Hit", animState6, null);
			animState6.AddBranch("Hit", animState6, null);
			return creatureAnimator;
		}

		// Token: 0x040025B9 RID: 9657
		private const string _buffTrigger = "Buff";

		// Token: 0x040025BA RID: 9658
		private const string _lashTrigger = "Lash";

		// Token: 0x040025BB RID: 9659
		private const string _chargeTrigger = "charge";

		// Token: 0x040025BC RID: 9660
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_buff";

		// Token: 0x040025BD RID: 9661
		private const string _chargeSfx = "event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_charge";

		// Token: 0x040025BE RID: 9662
		private const string _tongueLashSfx = "event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_tongue_lash";

		// Token: 0x040025BF RID: 9663
		private bool _hasBeetleCharged;
	}
}
