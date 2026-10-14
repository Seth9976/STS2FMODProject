using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000806 RID: 2054
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TwigSlimeS : MonsterModel
	{
		// Token: 0x17001904 RID: 6404
		// (get) Token: 0x0600665B RID: 26203 RVA: 0x0026FA63 File Offset: 0x0026DC63
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 8, 7);
			}
		}

		// Token: 0x17001905 RID: 6405
		// (get) Token: 0x0600665C RID: 26204 RVA: 0x0026FA6D File Offset: 0x0026DC6D
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 12, 11);
			}
		}

		// Token: 0x17001906 RID: 6406
		// (get) Token: 0x0600665D RID: 26205 RVA: 0x0026FA79 File Offset: 0x0026DC79
		private int TackleDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
			}
		}

		// Token: 0x17001907 RID: 6407
		// (get) Token: 0x0600665E RID: 26206 RVA: 0x0026FA84 File Offset: 0x0026DC84
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x0600665F RID: 26207 RVA: 0x0026FA88 File Offset: 0x0026DC88
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("TACKLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TackleMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.TackleDamage)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006660 RID: 26208 RVA: 0x0026FADC File Offset: 0x0026DCDC
		private async Task TackleMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.TackleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_slime_impact", null, null)
				.Execute(null);
		}
	}
}
