using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007F4 RID: 2036
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TenHpMonster : MonsterModel
	{
		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x060064FB RID: 25851 RVA: 0x0026B95B File Offset: 0x00269B5B
		public override LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");
			}
		}

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x060064FC RID: 25852 RVA: 0x0026B967 File Offset: 0x00269B67
		public override int MinInitialHp
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x060064FD RID: 25853 RVA: 0x0026B96B File Offset: 0x00269B6B
		public override int MaxInitialHp
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060064FE RID: 25854 RVA: 0x0026B970 File Offset: 0x00269B70
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("NOTHING", new Func<IReadOnlyList<Creature>, Task>(this.NothingMove), new AbstractIntent[]
			{
				new HiddenIntent()
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060064FF RID: 25855 RVA: 0x0026B9BD File Offset: 0x00269BBD
		private Task NothingMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}
	}
}
