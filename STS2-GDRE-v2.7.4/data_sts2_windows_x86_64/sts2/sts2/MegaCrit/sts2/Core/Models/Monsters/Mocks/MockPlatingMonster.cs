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
	// Token: 0x02000811 RID: 2065
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockPlatingMonster : MonsterModel
	{
		// Token: 0x1700194D RID: 6477
		// (get) Token: 0x0600670A RID: 26378 RVA: 0x00271912 File Offset: 0x0026FB12
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700194E RID: 6478
		// (get) Token: 0x0600670B RID: 26379 RVA: 0x00271915 File Offset: 0x0026FB15
		public override LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");
			}
		}

		// Token: 0x1700194F RID: 6479
		// (get) Token: 0x0600670C RID: 26380 RVA: 0x00271921 File Offset: 0x0026FB21
		protected override string VisualsPath
		{
			get
			{
				return SceneHelper.GetScenePath("creature_visuals/defect");
			}
		}

		// Token: 0x17001950 RID: 6480
		// (get) Token: 0x0600670D RID: 26381 RVA: 0x0027192D File Offset: 0x0026FB2D
		public override int MinInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x17001951 RID: 6481
		// (get) Token: 0x0600670E RID: 26382 RVA: 0x00271934 File Offset: 0x0026FB34
		public override int MaxInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x17001952 RID: 6482
		// (get) Token: 0x0600670F RID: 26383 RVA: 0x0027193B File Offset: 0x0026FB3B
		// (set) Token: 0x06006710 RID: 26384 RVA: 0x00271943 File Offset: 0x0026FB43
		public int PlatingAmount
		{
			get
			{
				return this._platingAmount;
			}
			set
			{
				base.AssertMutable();
				this._platingAmount = value;
			}
		}

		// Token: 0x06006711 RID: 26385 RVA: 0x00271954 File Offset: 0x0026FB54
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

		// Token: 0x06006712 RID: 26386 RVA: 0x002719A4 File Offset: 0x0026FBA4
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.PlatingAmount, base.Creature, null, false);
		}

		// Token: 0x06006713 RID: 26387 RVA: 0x002719E7 File Offset: 0x0026FBE7
		private Task NothingMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}

		// Token: 0x0400274D RID: 10061
		private int _platingAmount;
	}
}
