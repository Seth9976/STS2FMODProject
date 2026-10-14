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
	// Token: 0x0200080D RID: 2061
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockArtifactMonster : MonsterModel
	{
		// Token: 0x1700193B RID: 6459
		// (get) Token: 0x060066E9 RID: 26345 RVA: 0x002715D7 File Offset: 0x0026F7D7
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700193C RID: 6460
		// (get) Token: 0x060066EA RID: 26346 RVA: 0x002715DA File Offset: 0x0026F7DA
		public override LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");
			}
		}

		// Token: 0x1700193D RID: 6461
		// (get) Token: 0x060066EB RID: 26347 RVA: 0x002715E6 File Offset: 0x0026F7E6
		protected override string VisualsPath
		{
			get
			{
				return SceneHelper.GetScenePath("creature_visuals/defect");
			}
		}

		// Token: 0x1700193E RID: 6462
		// (get) Token: 0x060066EC RID: 26348 RVA: 0x002715F2 File Offset: 0x0026F7F2
		public override int MinInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x1700193F RID: 6463
		// (get) Token: 0x060066ED RID: 26349 RVA: 0x002715F9 File Offset: 0x0026F7F9
		public override int MaxInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x060066EE RID: 26350 RVA: 0x00271600 File Offset: 0x0026F800
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

		// Token: 0x060066EF RID: 26351 RVA: 0x00271650 File Offset: 0x0026F850
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x060066F0 RID: 26352 RVA: 0x00271693 File Offset: 0x0026F893
		private Task NothingMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}
	}
}
