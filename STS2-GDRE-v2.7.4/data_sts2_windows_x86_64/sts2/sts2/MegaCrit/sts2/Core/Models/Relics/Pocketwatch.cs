using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005BA RID: 1466
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Pocketwatch : RelicModel
	{
		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x060053DD RID: 21469 RVA: 0x002458E7 File Offset: 0x00243AE7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x060053DE RID: 21470 RVA: 0x002458EA File Offset: 0x00243AEA
		public override bool ShowCounter
		{
			get
			{
				return CombatManager.Instance.IsInProgress;
			}
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x060053DF RID: 21471 RVA: 0x002458F6 File Offset: 0x00243AF6
		public override int DisplayAmount
		{
			get
			{
				return this._cardsPlayedThisTurn;
			}
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x060053E0 RID: 21472 RVA: 0x002458FE File Offset: 0x00243AFE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("CardThreshold", 3m),
					new CardsVar(3)
				});
			}
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x00245928 File Offset: 0x00243B28
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return Task.CompletedTask;
			}
			this._cardsPlayedThisTurn++;
			this.RefreshCounter();
			return Task.CompletedTask;
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x0024597C File Offset: 0x00243B7C
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner)
			{
				return count;
			}
			if (base.Owner.PlayerCombatState.TurnNumber == 1)
			{
				return count;
			}
			if (this._cardsPlayedLastTurn > base.DynamicVars["CardThreshold"].BaseValue)
			{
				return count;
			}
			return count + base.DynamicVars.Cards.BaseValue;
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x002459E8 File Offset: 0x00243BE8
		public override Task AfterModifyingHandDraw()
		{
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x002459F5 File Offset: 0x00243BF5
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this._cardsPlayedLastTurn = this._cardsPlayedThisTurn;
			this._cardsPlayedThisTurn = 0;
			return Task.CompletedTask;
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x00245A28 File Offset: 0x00243C28
		public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.RefreshCounter();
			return Task.CompletedTask;
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x00245A4E File Offset: 0x00243C4E
		private void RefreshCounter()
		{
			base.Status = ((this._cardsPlayedThisTurn <= base.DynamicVars["CardThreshold"].BaseValue) ? RelicStatus.Active : RelicStatus.Normal);
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x00245A87 File Offset: 0x00243C87
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this._cardsPlayedThisTurn = 0;
			this._cardsPlayedLastTurn = 0;
			base.Status = RelicStatus.Normal;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x040024A2 RID: 9378
		private const string _cardThresholdKey = "CardThreshold";

		// Token: 0x040024A3 RID: 9379
		private int _cardsPlayedThisTurn;

		// Token: 0x040024A4 RID: 9380
		private int _cardsPlayedLastTurn;
	}
}
