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
	// Token: 0x020007F9 RID: 2041
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheAdversaryMkTwo : MonsterModel
	{
		// Token: 0x170018A3 RID: 6307
		// (get) Token: 0x06006570 RID: 25968 RVA: 0x0026CD73 File Offset: 0x0026AF73
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 200, 200);
			}
		}

		// Token: 0x170018A4 RID: 6308
		// (get) Token: 0x06006571 RID: 25969 RVA: 0x0026CD85 File Offset: 0x0026AF85
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170018A5 RID: 6309
		// (get) Token: 0x06006572 RID: 25970 RVA: 0x0026CD8D File Offset: 0x0026AF8D
		private int BashDamage
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x170018A6 RID: 6310
		// (get) Token: 0x06006573 RID: 25971 RVA: 0x0026CD91 File Offset: 0x0026AF91
		private int FlameBeamDamage
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x170018A7 RID: 6311
		// (get) Token: 0x06006574 RID: 25972 RVA: 0x0026CD95 File Offset: 0x0026AF95
		private int FlameBeamStatusCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x06006575 RID: 25973 RVA: 0x0026CD98 File Offset: 0x0026AF98
		private int BarrageDamage
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x06006576 RID: 25974 RVA: 0x0026CD9C File Offset: 0x0026AF9C
		private int BarrageRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170018AA RID: 6314
		// (get) Token: 0x06006577 RID: 25975 RVA: 0x0026CD9F File Offset: 0x0026AF9F
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x06006578 RID: 25976 RVA: 0x0026CDA4 File Offset: 0x0026AFA4
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x06006579 RID: 25977 RVA: 0x0026CDE8 File Offset: 0x0026AFE8
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("BASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BashDamage)
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

		// Token: 0x0600657A RID: 25978 RVA: 0x0026CEBC File Offset: 0x0026B0BC
		private async Task BashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600657B RID: 25979 RVA: 0x0026CF00 File Offset: 0x0026B100
		private async Task FlameBeamMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.FlameBeamDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600657C RID: 25980 RVA: 0x0026CF44 File Offset: 0x0026B144
		private async Task BarrageMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BarrageDamage).WithHitCount(this.BarrageRepeat).FromMonster(this)
				.WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
		}
	}
}
