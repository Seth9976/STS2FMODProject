using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters.Mocks
{
	// Token: 0x02000810 RID: 2064
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockIntangibleMonster : MonsterModel
	{
		// Token: 0x17001948 RID: 6472
		// (get) Token: 0x06006701 RID: 26369 RVA: 0x00271817 File Offset: 0x0026FA17
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001949 RID: 6473
		// (get) Token: 0x06006702 RID: 26370 RVA: 0x0027181A File Offset: 0x0026FA1A
		public override LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");
			}
		}

		// Token: 0x1700194A RID: 6474
		// (get) Token: 0x06006703 RID: 26371 RVA: 0x00271826 File Offset: 0x0026FA26
		protected override string VisualsPath
		{
			get
			{
				return SceneHelper.GetScenePath("creature_visuals/defect");
			}
		}

		// Token: 0x1700194B RID: 6475
		// (get) Token: 0x06006704 RID: 26372 RVA: 0x00271832 File Offset: 0x0026FA32
		public override int MinInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x1700194C RID: 6476
		// (get) Token: 0x06006705 RID: 26373 RVA: 0x00271839 File Offset: 0x0026FA39
		public override int MaxInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x06006706 RID: 26374 RVA: 0x00271840 File Offset: 0x0026FA40
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("INTANGIBLE", new Func<IReadOnlyList<Creature>, Task>(this.IntangibleMove), new AbstractIntent[]
			{
				new HiddenIntent()
			});
			MoveState moveState2 = new MoveState("NOTHING", new Func<IReadOnlyList<Creature>, Task>(this.NothingMove), new AbstractIntent[]
			{
				new HiddenIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006707 RID: 26375 RVA: 0x002718C0 File Offset: 0x0026FAC0
		private async Task IntangibleMove(IReadOnlyList<Creature> targets)
		{
			await PowerCmd.Apply<IntangiblePower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x06006708 RID: 26376 RVA: 0x00271903 File Offset: 0x0026FB03
		private Task NothingMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}
	}
}
