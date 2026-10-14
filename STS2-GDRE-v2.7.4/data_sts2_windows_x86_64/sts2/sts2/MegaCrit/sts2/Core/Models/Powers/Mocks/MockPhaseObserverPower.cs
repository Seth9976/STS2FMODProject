using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	/// <summary>
	/// Test-only power that records <see cref="P:MegaCrit.Sts2.Core.Entities.Players.PlayerCombatState.Phase" /> at the moment each turn lifecycle hook fires.
	/// Tests should call <see cref="M:MegaCrit.Sts2.Core.Models.Powers.Mocks.MockPhaseObserverPower.ResetObservations" /> before each scenario and read <see cref="P:MegaCrit.Sts2.Core.Models.Powers.Mocks.MockPhaseObserverPower.Observations" /> after.
	/// </summary>
	// Token: 0x0200073A RID: 1850
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockPhaseObserverPower : PowerModel
	{
		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x06005CEC RID: 23788 RVA: 0x0025728F File Offset: 0x0025548F
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001497 RID: 5271
		// (get) Token: 0x06005CED RID: 23789 RVA: 0x00257292 File Offset: 0x00255492
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001498 RID: 5272
		// (get) Token: 0x06005CEE RID: 23790 RVA: 0x00257295 File Offset: 0x00255495
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001499 RID: 5273
		// (get) Token: 0x06005CEF RID: 23791 RVA: 0x00257298 File Offset: 0x00255498
		[TupleElementNames(new string[] { "Hook", "Phase" })]
		[Nullable(new byte[] { 1, 0, 1 })]
		public static List<ValueTuple<string, PlayerTurnPhase>> Observations
		{
			[return: TupleElementNames(new string[] { "Hook", "Phase" })]
			[return: Nullable(new byte[] { 1, 0, 1 })]
			get;
		} = new List<ValueTuple<string, PlayerTurnPhase>>();

		/// <summary>
		/// Optional callback invoked during <see cref="M:MegaCrit.Sts2.Core.Models.Powers.Mocks.MockPhaseObserverPower.Record(System.String,MegaCrit.Sts2.Core.Entities.Players.Player)" />. Useful for multiplayer tests that need to
		/// inspect other players' phases at the moment a hook fires.
		/// </summary>
		// Token: 0x1700149A RID: 5274
		// (get) Token: 0x06005CF0 RID: 23792 RVA: 0x0025729F File Offset: 0x0025549F
		// (set) Token: 0x06005CF1 RID: 23793 RVA: 0x002572A6 File Offset: 0x002554A6
		[Nullable(new byte[] { 2, 1, 1 })]
		public static Action<string, Player> OnRecordCallback
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			set;
		}

		// Token: 0x06005CF2 RID: 23794 RVA: 0x002572AE File Offset: 0x002554AE
		public static void ResetObservations()
		{
			MockPhaseObserverPower.Observations.Clear();
			MockPhaseObserverPower.OnRecordCallback = null;
		}

		// Token: 0x06005CF3 RID: 23795 RVA: 0x002572C0 File Offset: 0x002554C0
		public override Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			this.Record("BeforeHandDraw", player);
			return Task.CompletedTask;
		}

		// Token: 0x06005CF4 RID: 23796 RVA: 0x002572D3 File Offset: 0x002554D3
		public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			this.Record("AfterPlayerTurnStart", player);
			return Task.CompletedTask;
		}

		// Token: 0x06005CF5 RID: 23797 RVA: 0x002572E6 File Offset: 0x002554E6
		public override Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
		{
			this.Record("AfterAutoPrePlayPhaseEntered", player);
			return Task.CompletedTask;
		}

		// Token: 0x06005CF6 RID: 23798 RVA: 0x002572F9 File Offset: 0x002554F9
		public override Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
		{
			this.Record("AfterAutoPostPlayPhaseEntered", player);
			return Task.CompletedTask;
		}

		// Token: 0x06005CF7 RID: 23799 RVA: 0x0025730C File Offset: 0x0025550C
		public override Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			this.Record("BeforeSideTurnEnd", base.Owner.Player);
			return Task.CompletedTask;
		}

		// Token: 0x06005CF8 RID: 23800 RVA: 0x0025733D File Offset: 0x0025553D
		public override Task BeforeFlush(PlayerChoiceContext choiceContext, Player player)
		{
			this.Record("BeforeFlush", player);
			return Task.CompletedTask;
		}

		// Token: 0x06005CF9 RID: 23801 RVA: 0x00257350 File Offset: 0x00255550
		public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			this.Record("AfterSideTurnEnd", base.Owner.Player);
			return Task.CompletedTask;
		}

		// Token: 0x06005CFA RID: 23802 RVA: 0x00257381 File Offset: 0x00255581
		private void Record(string hook, Player player)
		{
			MockPhaseObserverPower.Observations.Add(new ValueTuple<string, PlayerTurnPhase>(hook, player.PlayerCombatState.Phase));
			Action<string, Player> onRecordCallback = MockPhaseObserverPower.OnRecordCallback;
			if (onRecordCallback == null)
			{
				return;
			}
			onRecordCallback(hook, player);
		}
	}
}
