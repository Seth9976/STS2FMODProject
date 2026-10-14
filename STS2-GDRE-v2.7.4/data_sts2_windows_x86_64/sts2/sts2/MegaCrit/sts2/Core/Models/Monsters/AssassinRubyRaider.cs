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
	// Token: 0x02000796 RID: 1942
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AssassinRubyRaider : MonsterModel
	{
		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x06005F4A RID: 24394 RVA: 0x0025A92B File Offset: 0x00258B2B
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 18);
			}
		}

		// Token: 0x1700160A RID: 5642
		// (get) Token: 0x06005F4B RID: 24395 RVA: 0x0025A937 File Offset: 0x00258B37
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 24, 23);
			}
		}

		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x06005F4C RID: 24396 RVA: 0x0025A943 File Offset: 0x00258B43
		private static int KillshotDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
			}
		}

		// Token: 0x1700160C RID: 5644
		// (get) Token: 0x06005F4D RID: 24397 RVA: 0x0025A950 File Offset: 0x00258B50
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x06005F4E RID: 24398 RVA: 0x0025A954 File Offset: 0x00258B54
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("KILLSHOT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.KillshotMove), new AbstractIntent[]
			{
				new SingleAttackIntent(AssassinRubyRaider.KillshotDamage)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005F4F RID: 24399 RVA: 0x0025A9A8 File Offset: 0x00258BA8
		private async Task KillshotMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(AssassinRubyRaider.KillshotDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}
	}
}
