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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007E6 RID: 2022
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShrinkerBeetle : MonsterModel
	{
		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x0600642E RID: 25646 RVA: 0x00269231 File Offset: 0x00267431
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 40, 38);
			}
		}

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x0600642F RID: 25647 RVA: 0x0026923D File Offset: 0x0026743D
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 42, 40);
			}
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x06006430 RID: 25648 RVA: 0x00269249 File Offset: 0x00267449
		private int ChompDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x06006431 RID: 25649 RVA: 0x00269254 File Offset: 0x00267454
		private int StompDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 13);
			}
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x06006432 RID: 25650 RVA: 0x00269261 File Offset: 0x00267461
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Insect;
			}
		}

		// Token: 0x06006433 RID: 25651 RVA: 0x00269264 File Offset: 0x00267464
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SHRINKER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ShrinkMove), new AbstractIntent[]
			{
				new DebuffIntent(true)
			});
			MoveState moveState2 = new MoveState("CHOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ChompMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ChompDamage)
			});
			MoveState moveState3 = new MoveState("STOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StompMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.StompDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006434 RID: 25652 RVA: 0x00269324 File Offset: 0x00267524
		private async Task ShrinkMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.RadialBlur(VfxPosition.Left);
			}
			await PowerCmd.Apply<ShrinkPower>(new ThrowingPlayerChoiceContext(), targets, -1m, base.Creature, null, false);
		}

		// Token: 0x06006435 RID: 25653 RVA: 0x00269370 File Offset: 0x00267570
		private async Task ChompMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ChompDamage).FromMonster(this).WithAttackerAnim("Attack", 0.25f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006436 RID: 25654 RVA: 0x002693B4 File Offset: 0x002675B4
		private async Task StompMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.StompDamage).FromMonster(this).WithAttackerAnim("Attack", 0.25f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006437 RID: 25655 RVA: 0x002693F7 File Offset: 0x002675F7
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "STOMP_MOVE";
		}

		// Token: 0x0400267E RID: 9854
		private const string _stompMove = "STOMP_MOVE";
	}
}
