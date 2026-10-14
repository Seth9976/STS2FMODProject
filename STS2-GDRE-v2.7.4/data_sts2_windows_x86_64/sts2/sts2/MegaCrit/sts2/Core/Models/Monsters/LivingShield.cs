using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x020007CE RID: 1998
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LivingShield : MonsterModel
	{
		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x060062A1 RID: 25249 RVA: 0x00264864 File Offset: 0x00262A64
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 65, 55);
			}
		}

		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x060062A2 RID: 25250 RVA: 0x00264870 File Offset: 0x00262A70
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x060062A3 RID: 25251 RVA: 0x00264878 File Offset: 0x00262A78
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x060062A4 RID: 25252 RVA: 0x0026487B File Offset: 0x00262A7B
		public override bool HasDeathSfx
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x060062A5 RID: 25253 RVA: 0x0026487E File Offset: 0x00262A7E
		private int ShieldSlamDamage
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x060062A6 RID: 25254 RVA: 0x00264881 File Offset: 0x00262A81
		private int SmashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 16);
			}
		}

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x060062A7 RID: 25255 RVA: 0x0026488E File Offset: 0x00262A8E
		private int EnrageStr
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060062A8 RID: 25256 RVA: 0x00264894 File Offset: 0x00262A94
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<RampartPower>(new ThrowingPlayerChoiceContext(), base.Creature, 25m, base.Creature, null, false);
		}

		// Token: 0x060062A9 RID: 25257 RVA: 0x002648D8 File Offset: 0x00262AD8
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SHIELD_SLAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ShieldSlamMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ShieldSlamDamage)
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("SHIELD_SLAM_BRANCH");
			MoveState moveState2 = new MoveState("SMASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SmashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SmashDamage),
				new BuffIntent()
			});
			moveState.FollowUpState = conditionalBranchState;
			conditionalBranchState.AddState(moveState, () => this.GetAllyCount() > 0);
			conditionalBranchState.AddState(moveState2, () => this.GetAllyCount() == 0);
			moveState2.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(conditionalBranchState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060062AA RID: 25258 RVA: 0x002649A4 File Offset: 0x00262BA4
		private async Task ShieldSlamMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ShieldSlamDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060062AB RID: 25259 RVA: 0x002649E8 File Offset: 0x00262BE8
		private async Task SmashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SmashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.EnrageStr, base.Creature, null, false);
		}

		// Token: 0x060062AC RID: 25260 RVA: 0x00264A2B File Offset: 0x00262C2B
		private int GetAllyCount()
		{
			return base.CombatState.GetTeammatesOf(base.Creature).Count<Creature>((Creature c) => c.IsAlive && c != base.Creature);
		}
	}
}
