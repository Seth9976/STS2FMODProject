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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007D6 RID: 2006
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Nibbit : MonsterModel
	{
		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x0600631A RID: 25370 RVA: 0x00265F91 File Offset: 0x00264191
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 44, 42);
			}
		}

		// Token: 0x170017A3 RID: 6051
		// (get) Token: 0x0600631B RID: 25371 RVA: 0x00265F9D File Offset: 0x0026419D
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 48, 46);
			}
		}

		// Token: 0x170017A4 RID: 6052
		// (get) Token: 0x0600631C RID: 25372 RVA: 0x00265FA9 File Offset: 0x002641A9
		private int ButtDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 12);
			}
		}

		// Token: 0x170017A5 RID: 6053
		// (get) Token: 0x0600631D RID: 25373 RVA: 0x00265FB6 File Offset: 0x002641B6
		private int SliceBlock
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 6, 5);
			}
		}

		// Token: 0x170017A6 RID: 6054
		// (get) Token: 0x0600631E RID: 25374 RVA: 0x00265FC0 File Offset: 0x002641C0
		private int SliceDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x0600631F RID: 25375 RVA: 0x00265FCB File Offset: 0x002641CB
		private int HissStrengthGain
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x06006320 RID: 25376 RVA: 0x00265FD6 File Offset: 0x002641D6
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/nibbit/nibbit_die";
			}
		}

		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x06006321 RID: 25377 RVA: 0x00265FDD File Offset: 0x002641DD
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x06006322 RID: 25378 RVA: 0x00265FE0 File Offset: 0x002641E0
		// (set) Token: 0x06006323 RID: 25379 RVA: 0x00265FE8 File Offset: 0x002641E8
		public bool IsFront
		{
			get
			{
				return this._isFront;
			}
			set
			{
				base.AssertMutable();
				this._isFront = value;
			}
		}

		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x06006324 RID: 25380 RVA: 0x00265FF7 File Offset: 0x002641F7
		// (set) Token: 0x06006325 RID: 25381 RVA: 0x00265FFF File Offset: 0x002641FF
		public bool IsAlone
		{
			get
			{
				return this._isAlone;
			}
			set
			{
				base.AssertMutable();
				this._isAlone = value;
			}
		}

		// Token: 0x06006326 RID: 25382 RVA: 0x00266010 File Offset: 0x00264210
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("BUTT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ButtMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ButtDamage)
			});
			MoveState moveState2 = new MoveState("SLICE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SliceMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SliceDamage),
				new DefendIntent()
			});
			MoveState moveState3 = new MoveState("HISS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HissMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("INIT_MOVE");
			if (this._isAlone)
			{
				conditionalBranchState.AddState(moveState, () => ((Nibbit)base.Creature.Monster).IsAlone);
			}
			else
			{
				conditionalBranchState.AddState(moveState3, () => !((Nibbit)base.Creature.Monster).IsFront);
				conditionalBranchState.AddState(moveState2, () => ((Nibbit)base.Creature.Monster).IsFront);
			}
			moveState2.FollowUpState = moveState3;
			moveState.FollowUpState = moveState2;
			moveState3.FollowUpState = moveState;
			list.Add(conditionalBranchState);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, conditionalBranchState);
		}

		// Token: 0x06006327 RID: 25383 RVA: 0x00266134 File Offset: 0x00264334
		private async Task ButtMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ButtDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006328 RID: 25384 RVA: 0x00266178 File Offset: 0x00264378
		private async Task SliceMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SliceDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, this.SliceBlock, ValueProp.Move, null, false);
		}

		// Token: 0x06006329 RID: 25385 RVA: 0x002661BC File Offset: 0x002643BC
		private async Task HissMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.6f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.HissStrengthGain, base.Creature, null, false);
		}

		// Token: 0x0600632A RID: 25386 RVA: 0x00266200 File Offset: 0x00264400
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("hiss", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x0600632B RID: 25387 RVA: 0x002662A3 File Offset: 0x002644A3
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "SLICE_MOVE";
		}

		// Token: 0x04002638 RID: 9784
		private const string _sliceMove = "SLICE_MOVE";

		// Token: 0x04002639 RID: 9785
		private bool _isFront;

		// Token: 0x0400263A RID: 9786
		private bool _isAlone;
	}
}
