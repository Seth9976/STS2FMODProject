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
	// Token: 0x020007D8 RID: 2008
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OneHpMonster : MonsterModel
	{
		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x06006338 RID: 25400 RVA: 0x0026640B File Offset: 0x0026460B
		public override LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");
			}
		}

		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x06006339 RID: 25401 RVA: 0x00266417 File Offset: 0x00264617
		public override int MinInitialHp
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x0600633A RID: 25402 RVA: 0x0026641A File Offset: 0x0026461A
		public override int MaxInitialHp
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600633B RID: 25403 RVA: 0x00266420 File Offset: 0x00264620
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

		// Token: 0x0600633C RID: 25404 RVA: 0x0026646D File Offset: 0x0026466D
		private Task NothingMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}
	}
}
