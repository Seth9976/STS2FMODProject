using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
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
	// Token: 0x020007F8 RID: 2040
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheAdversaryMkThree : MonsterModel
	{
		// Token: 0x1700189C RID: 6300
		// (get) Token: 0x06006562 RID: 25954 RVA: 0x0026CB53 File Offset: 0x0026AD53
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 300, 300);
			}
		}

		// Token: 0x1700189D RID: 6301
		// (get) Token: 0x06006563 RID: 25955 RVA: 0x0026CB65 File Offset: 0x0026AD65
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x1700189E RID: 6302
		// (get) Token: 0x06006564 RID: 25956 RVA: 0x0026CB6D File Offset: 0x0026AD6D
		private int CrashDamage
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x1700189F RID: 6303
		// (get) Token: 0x06006565 RID: 25957 RVA: 0x0026CB71 File Offset: 0x0026AD71
		private int FlameBeamDamage
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x170018A0 RID: 6304
		// (get) Token: 0x06006566 RID: 25958 RVA: 0x0026CB75 File Offset: 0x0026AD75
		private int BarrageDamage
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170018A1 RID: 6305
		// (get) Token: 0x06006567 RID: 25959 RVA: 0x0026CB79 File Offset: 0x0026AD79
		private int BarrageRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170018A2 RID: 6306
		// (get) Token: 0x06006568 RID: 25960 RVA: 0x0026CB7C File Offset: 0x0026AD7C
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x06006569 RID: 25961 RVA: 0x0026CB80 File Offset: 0x0026AD80
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x0600656A RID: 25962 RVA: 0x0026CBC4 File Offset: 0x0026ADC4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("CRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CrashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.CrashDamage)
			});
			MoveState moveState2 = new MoveState("FLAME_BEAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FlameBeamMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.FlameBeamDamage)
			});
			MoveState moveState3 = new MoveState("BARRAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BarrageMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.BarrageDamage, this.BarrageRepeat),
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

		// Token: 0x0600656B RID: 25963 RVA: 0x0026CC98 File Offset: 0x0026AE98
		private async Task CrashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.CrashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600656C RID: 25964 RVA: 0x0026CCDC File Offset: 0x0026AEDC
		private async Task FlameBeamMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.FlameBeamDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600656D RID: 25965 RVA: 0x0026CD20 File Offset: 0x0026AF20
		private async Task BarrageMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BarrageDamage).WithHitCount(this.BarrageRepeat).FromMonster(this)
				.WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 4m, base.Creature, null, false);
		}
	}
}
