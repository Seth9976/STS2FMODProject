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
	// Token: 0x020007FC RID: 2044
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheLost : MonsterModel
	{
		// Token: 0x170018B9 RID: 6329
		// (get) Token: 0x060065A0 RID: 26016 RVA: 0x0026D6CE File Offset: 0x0026B8CE
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 99, 93);
			}
		}

		// Token: 0x170018BA RID: 6330
		// (get) Token: 0x060065A1 RID: 26017 RVA: 0x0026D6DA File Offset: 0x0026B8DA
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170018BB RID: 6331
		// (get) Token: 0x060065A2 RID: 26018 RVA: 0x0026D6E2 File Offset: 0x0026B8E2
		private int EyeLasersDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
			}
		}

		// Token: 0x170018BC RID: 6332
		// (get) Token: 0x060065A3 RID: 26019 RVA: 0x0026D6ED File Offset: 0x0026B8ED
		private int DebilitatingSmogStrengthStealAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 2);
			}
		}

		// Token: 0x170018BD RID: 6333
		// (get) Token: 0x060065A4 RID: 26020 RVA: 0x0026D6F8 File Offset: 0x0026B8F8
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Stone;
			}
		}

		// Token: 0x060065A5 RID: 26021 RVA: 0x0026D6FC File Offset: 0x0026B8FC
		public override async Task AfterAddedToRoom()
		{
			await PowerCmd.Apply<PossessStrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, null, null, false);
		}

		// Token: 0x060065A6 RID: 26022 RVA: 0x0026D740 File Offset: 0x0026B940
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("DEBILITATING_SMOG", new Func<IReadOnlyList<Creature>, Task>(this.DebilitatingSmogMove), new AbstractIntent[]
			{
				new DebuffIntent(false),
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("EYE_LASERS", new Func<IReadOnlyList<Creature>, Task>(this.EyeLasersMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.EyeLasersDamage, 2)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060065A7 RID: 26023 RVA: 0x0026D7D0 File Offset: 0x0026B9D0
		private async Task DebilitatingSmogMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), targets, -this.DebilitatingSmogStrengthStealAmount, base.Creature, null, false);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.DebilitatingSmogStrengthStealAmount, base.Creature, null, false);
		}

		// Token: 0x060065A8 RID: 26024 RVA: 0x0026D81C File Offset: 0x0026BA1C
		private async Task EyeLasersMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.EyeLasersDamage).WithHitCount(2).FromMonster(this)
				.WithAttackerAnim("Attack", 0.15f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x060065A9 RID: 26025 RVA: 0x0026D860 File Offset: 0x0026BA60
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

		// Token: 0x040026DF RID: 9951
		private const int _eyeLasersRepeat = 2;
	}
}
