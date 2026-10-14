using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007F7 RID: 2039
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheAdversaryMkOne : MonsterModel
	{
		// Token: 0x17001895 RID: 6293
		// (get) Token: 0x06006554 RID: 25940 RVA: 0x0026C943 File Offset: 0x0026AB43
		public override int MinInitialHp
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x17001896 RID: 6294
		// (get) Token: 0x06006555 RID: 25941 RVA: 0x0026C947 File Offset: 0x0026AB47
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001897 RID: 6295
		// (get) Token: 0x06006556 RID: 25942 RVA: 0x0026C94F File Offset: 0x0026AB4F
		private int SmashDamage
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x06006557 RID: 25943 RVA: 0x0026C953 File Offset: 0x0026AB53
		private int BeamDamage
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x06006558 RID: 25944 RVA: 0x0026C957 File Offset: 0x0026AB57
		private int BarrageDamage
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x1700189A RID: 6298
		// (get) Token: 0x06006559 RID: 25945 RVA: 0x0026C95A File Offset: 0x0026AB5A
		private int BarrageRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700189B RID: 6299
		// (get) Token: 0x0600655A RID: 25946 RVA: 0x0026C95D File Offset: 0x0026AB5D
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x0600655B RID: 25947 RVA: 0x0026C960 File Offset: 0x0026AB60
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), base.Creature, 0m, base.Creature, null, false);
		}

		// Token: 0x0600655C RID: 25948 RVA: 0x0026C9A4 File Offset: 0x0026ABA4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SMASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SmashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SmashDamage)
			});
			MoveState moveState2 = new MoveState("BEAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BeamMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BeamDamage)
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

		// Token: 0x0600655D RID: 25949 RVA: 0x0026CA78 File Offset: 0x0026AC78
		private async Task SmashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SmashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600655E RID: 25950 RVA: 0x0026CABC File Offset: 0x0026ACBC
		private async Task BeamMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BeamDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600655F RID: 25951 RVA: 0x0026CB00 File Offset: 0x0026AD00
		private async Task BarrageMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BarrageDamage).WithHitCount(this.BarrageRepeat).FromMonster(this)
				.WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}
	}
}
