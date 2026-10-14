using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters.Mocks
{
	// Token: 0x0200080F RID: 2063
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockAttackMonster : MonsterModel
	{
		// Token: 0x17001944 RID: 6468
		// (get) Token: 0x060066FA RID: 26362 RVA: 0x0027175F File Offset: 0x0026F95F
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001945 RID: 6469
		// (get) Token: 0x060066FB RID: 26363 RVA: 0x00271762 File Offset: 0x0026F962
		protected override string VisualsPath
		{
			get
			{
				return SceneHelper.GetScenePath("creature_visuals/defect");
			}
		}

		// Token: 0x17001946 RID: 6470
		// (get) Token: 0x060066FC RID: 26364 RVA: 0x0027176E File Offset: 0x0026F96E
		public override int MinInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x17001947 RID: 6471
		// (get) Token: 0x060066FD RID: 26365 RVA: 0x00271775 File Offset: 0x0026F975
		public override int MaxInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x060066FE RID: 26366 RVA: 0x0027177C File Offset: 0x0026F97C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("ATTACK", new Func<IReadOnlyList<Creature>, Task>(this.AttackMove), new AbstractIntent[]
			{
				new SingleAttackIntent(1)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060066FF RID: 26367 RVA: 0x002717CC File Offset: 0x0026F9CC
		private async Task AttackMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(1m).FromMonster(this).Execute(null);
		}
	}
}
