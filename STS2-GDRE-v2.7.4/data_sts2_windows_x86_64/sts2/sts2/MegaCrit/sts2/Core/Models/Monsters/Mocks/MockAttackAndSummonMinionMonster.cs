using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters.Mocks
{
	// Token: 0x0200080E RID: 2062
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockAttackAndSummonMinionMonster : MonsterModel
	{
		// Token: 0x17001940 RID: 6464
		// (get) Token: 0x060066F3 RID: 26355 RVA: 0x002716AA File Offset: 0x0026F8AA
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001941 RID: 6465
		// (get) Token: 0x060066F4 RID: 26356 RVA: 0x002716AD File Offset: 0x0026F8AD
		protected override string VisualsPath
		{
			get
			{
				return SceneHelper.GetScenePath("creature_visuals/defect");
			}
		}

		// Token: 0x17001942 RID: 6466
		// (get) Token: 0x060066F5 RID: 26357 RVA: 0x002716B9 File Offset: 0x0026F8B9
		public override int MinInitialHp
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x17001943 RID: 6467
		// (get) Token: 0x060066F6 RID: 26358 RVA: 0x002716BD File Offset: 0x0026F8BD
		public override int MaxInitialHp
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060066F7 RID: 26359 RVA: 0x002716C4 File Offset: 0x0026F8C4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("ATTACK_AND_SUMMON_MINION", new Func<IReadOnlyList<Creature>, Task>(this.AttackAndSummonMinionMove), new AbstractIntent[]
			{
				new SingleAttackIntent(1)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060066F8 RID: 26360 RVA: 0x00271714 File Offset: 0x0026F914
		private async Task AttackAndSummonMinionMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(1m).FromMonster(this).Execute(null);
			Creature creature = await CreatureCmd.Add<BigDummy>(base.CombatState, null);
			await PowerCmd.Apply<MinionPower>(new ThrowingPlayerChoiceContext(), creature, 1m, base.Creature, null, false);
		}
	}
}
