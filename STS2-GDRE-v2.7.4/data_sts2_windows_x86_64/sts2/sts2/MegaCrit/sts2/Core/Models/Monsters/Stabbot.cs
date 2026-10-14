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
	// Token: 0x020007F3 RID: 2035
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Stabbot : MonsterModel
	{
		// Token: 0x17001870 RID: 6256
		// (get) Token: 0x060064F2 RID: 25842 RVA: 0x0026B836 File Offset: 0x00269A36
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 18);
			}
		}

		// Token: 0x17001871 RID: 6257
		// (get) Token: 0x060064F3 RID: 25843 RVA: 0x0026B842 File Offset: 0x00269A42
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 24, 23);
			}
		}

		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x060064F4 RID: 25844 RVA: 0x0026B84E File Offset: 0x00269A4E
		private int StabDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 11);
			}
		}

		// Token: 0x17001873 RID: 6259
		// (get) Token: 0x060064F5 RID: 25845 RVA: 0x0026B85B File Offset: 0x00269A5B
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x060064F6 RID: 25846 RVA: 0x0026B860 File Offset: 0x00269A60
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			if (TestMode.IsOff)
			{
				FabricatorNormal.SetBotFallPosition(NCombatRoom.Instance.GetCreatureNode(base.Creature));
			}
		}

		// Token: 0x060064F7 RID: 25847 RVA: 0x0026B8A4 File Offset: 0x00269AA4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("STAB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StabMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.StabDamage),
				new DebuffIntent(false)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060064F8 RID: 25848 RVA: 0x0026B900 File Offset: 0x00269B00
		private async Task StabMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.StabDamage).FromMonster(this).WithAttackerAnim("Attack", 0.6f, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}
	}
}
