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
	// Token: 0x020007FA RID: 2042
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheForgotten : MonsterModel
	{
		// Token: 0x170018AB RID: 6315
		// (get) Token: 0x0600657F RID: 25983 RVA: 0x0026CF97 File Offset: 0x0026B197
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 111, 106);
			}
		}

		// Token: 0x170018AC RID: 6316
		// (get) Token: 0x06006580 RID: 25984 RVA: 0x0026CFA3 File Offset: 0x0026B1A3
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170018AD RID: 6317
		// (get) Token: 0x06006581 RID: 25985 RVA: 0x0026CFAC File Offset: 0x0026B1AC
		private int DreadDamage
		{
			get
			{
				int valueIfAscension = AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
				return valueIfAscension + base.Creature.GetPowerAmount<DexterityPower>();
			}
		}

		// Token: 0x170018AE RID: 6318
		// (get) Token: 0x06006582 RID: 25986 RVA: 0x0026CFD4 File Offset: 0x0026B1D4
		private int DebilitatingSmogDexStealAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 2);
			}
		}

		// Token: 0x170018AF RID: 6319
		// (get) Token: 0x06006583 RID: 25987 RVA: 0x0026CFDF File Offset: 0x0026B1DF
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Stone;
			}
		}

		// Token: 0x06006584 RID: 25988 RVA: 0x0026CFE4 File Offset: 0x0026B1E4
		public override async Task AfterAddedToRoom()
		{
			await PowerCmd.Apply<PossessSpeedPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, null, null, false);
		}

		// Token: 0x06006585 RID: 25989 RVA: 0x0026D028 File Offset: 0x0026B228
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("MIASMA", new Func<IReadOnlyList<Creature>, Task>(this.MiasmaMove), new AbstractIntent[]
			{
				new DebuffIntent(false),
				new DefendIntent(),
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("DREAD", new Func<IReadOnlyList<Creature>, Task>(this.DreadMove), new AbstractIntent[]
			{
				new SingleAttackIntent(() => this.DreadDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006586 RID: 25990 RVA: 0x0026D0C8 File Offset: 0x0026B2C8
		private async Task MiasmaMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), targets, -this.DebilitatingSmogDexStealAmount, base.Creature, null, false);
			await CreatureCmd.GainBlock(base.Creature, 8m, ValueProp.Move, null, false);
			await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.DebilitatingSmogDexStealAmount, base.Creature, null, false);
		}

		// Token: 0x06006587 RID: 25991 RVA: 0x0026D114 File Offset: 0x0026B314
		private async Task DreadMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.DreadDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006588 RID: 25992 RVA: 0x0026D158 File Offset: 0x0026B358
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("debuff", false);
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
	}
}
