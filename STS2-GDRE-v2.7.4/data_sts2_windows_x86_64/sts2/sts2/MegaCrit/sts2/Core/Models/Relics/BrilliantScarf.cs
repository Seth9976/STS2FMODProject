using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000511 RID: 1297
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BrilliantScarf : RelicModel
	{
		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06004F8D RID: 20365 RVA: 0x0023D927 File Offset: 0x0023BB27
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06004F8E RID: 20366 RVA: 0x0023D92A File Offset: 0x0023BB2A
		public override bool ShowCounter
		{
			get
			{
				return CombatManager.Instance.IsInProgress && this.CardsPlayedThisTurn < base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06004F8F RID: 20367 RVA: 0x0023D952 File Offset: 0x0023BB52
		public override int DisplayAmount
		{
			get
			{
				return this.CardsPlayedThisTurn;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06004F90 RID: 20368 RVA: 0x0023D95A File Offset: 0x0023BB5A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(5));
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06004F91 RID: 20369 RVA: 0x0023D967 File Offset: 0x0023BB67
		// (set) Token: 0x06004F92 RID: 20370 RVA: 0x0023D96F File Offset: 0x0023BB6F
		private int CardsPlayedThisTurn
		{
			get
			{
				return this._cardsPlayedThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._cardsPlayedThisTurn = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x0023D984 File Offset: 0x0023BB84
		private void UpdateDisplay()
		{
			int intValue = base.DynamicVars.Cards.IntValue;
			base.Status = ((this.CardsPlayedThisTurn == intValue - 1) ? RelicStatus.Active : RelicStatus.Normal);
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x0023D9BD File Offset: 0x0023BBBD
		public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			if (!this.ShouldModifyCost(card))
			{
				return false;
			}
			modifiedCost = 0m;
			return true;
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x0023D9D9 File Offset: 0x0023BBD9
		public override bool TryModifyStarCost(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			if (!this.ShouldModifyCost(card))
			{
				return false;
			}
			modifiedCost = 0m;
			return true;
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x0023D9F5 File Offset: 0x0023BBF5
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.CardsPlayedThisTurn = 0;
			return Task.CompletedTask;
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x0023DA1C File Offset: 0x0023BC1C
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.IsAutoPlay)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			int cardsPlayedThisTurn = this.CardsPlayedThisTurn;
			this.CardsPlayedThisTurn = cardsPlayedThisTurn + 1;
			return Task.CompletedTask;
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x0023DA77 File Offset: 0x0023BC77
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this.CardsPlayedThisTurn = 0;
			return Task.CompletedTask;
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x0023DA88 File Offset: 0x0023BC88
		private bool ShouldModifyCost(CardModel card)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return false;
			}
			if (card.Owner.Creature != base.Owner.Creature)
			{
				return false;
			}
			if (this.CardsPlayedThisTurn != base.DynamicVars.Cards.BaseValue - 1m)
			{
				return false;
			}
			CardPile pile = card.Pile;
			PileType? pileType = ((pile != null) ? new PileType?(pile.Type) : null);
			if (pileType != null)
			{
				PileType valueOrDefault = pileType.GetValueOrDefault();
				if (valueOrDefault == PileType.Hand || valueOrDefault == PileType.Play)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400242E RID: 9262
		private int _cardsPlayedThisTurn;
	}
}
