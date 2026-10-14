using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007CC RID: 1996
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LeafSlimeS : MonsterModel
	{
		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x06006286 RID: 25222 RVA: 0x002643C3 File Offset: 0x002625C3
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 12, 11);
			}
		}

		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x06006287 RID: 25223 RVA: 0x002643CF File Offset: 0x002625CF
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 16, 15);
			}
		}

		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x06006288 RID: 25224 RVA: 0x002643DB File Offset: 0x002625DB
		private int TackleDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x06006289 RID: 25225 RVA: 0x002643E6 File Offset: 0x002625E6
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x0600628A RID: 25226 RVA: 0x002643EC File Offset: 0x002625EC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("TACKLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TackleMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.TackleDamage)
			});
			MoveState moveState2 = new MoveState("GOOP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GoopMove), new AbstractIntent[]
			{
				new StatusIntent(1)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(randomBranchState);
			return new MonsterMoveStateMachine(list, randomBranchState);
		}

		// Token: 0x0600628B RID: 25227 RVA: 0x00264498 File Offset: 0x00262698
		private async Task TackleMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.TackleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_slime_impact", null, null)
				.Execute(null);
		}

		// Token: 0x0600628C RID: 25228 RVA: 0x002644DC File Offset: 0x002626DC
		private async Task GoopMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			SfxCmd.Play(this.AttackSfx, 1f);
			VfxCmd.PlayOnCreatureCenters(targets, "vfx/vfx_slime_impact");
			await CardPileCmd.AddToCombatAndPreview<Slimed>(targets, PileType.Discard, 1, null, CardPilePosition.Bottom);
		}

		// Token: 0x04002610 RID: 9744
		private const int _goopAmount = 1;
	}
}
