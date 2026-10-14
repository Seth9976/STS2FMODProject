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
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x0200080C RID: 2060
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Zapbot : MonsterModel
	{
		// Token: 0x17001937 RID: 6455
		// (get) Token: 0x060066E0 RID: 26336 RVA: 0x002714C3 File Offset: 0x0026F6C3
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 18);
			}
		}

		// Token: 0x17001938 RID: 6456
		// (get) Token: 0x060066E1 RID: 26337 RVA: 0x002714CF File Offset: 0x0026F6CF
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 24, 23);
			}
		}

		// Token: 0x17001939 RID: 6457
		// (get) Token: 0x060066E2 RID: 26338 RVA: 0x002714DB File Offset: 0x0026F6DB
		private int ZapDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 14);
			}
		}

		// Token: 0x1700193A RID: 6458
		// (get) Token: 0x060066E3 RID: 26339 RVA: 0x002714E8 File Offset: 0x0026F6E8
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x060066E4 RID: 26340 RVA: 0x002714EC File Offset: 0x0026F6EC
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			if (TestMode.IsOff)
			{
				FabricatorNormal.SetBotFallPosition(NCombatRoom.Instance.GetCreatureNode(base.Creature));
			}
			await PowerCmd.Apply<HighVoltagePower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x060066E5 RID: 26341 RVA: 0x00271530 File Offset: 0x0026F730
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("ZAP", new Func<IReadOnlyList<Creature>, Task>(this.ZapMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ZapDamage)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060066E6 RID: 26342 RVA: 0x00271584 File Offset: 0x0026F784
		private async Task ZapMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ZapDamage).FromMonster(this).WithAttackerAnim("Attack", 0.6f, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}
	}
}
