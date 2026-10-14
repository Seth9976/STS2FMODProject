using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005A4 RID: 1444
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsEye : RelicModel
	{
		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x06005323 RID: 21283 RVA: 0x00244288 File Offset: 0x00242488
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06005324 RID: 21284 RVA: 0x0024428B File Offset: 0x0024248B
		// (set) Token: 0x06005325 RID: 21285 RVA: 0x00244293 File Offset: 0x00242493
		private bool UsedThisCombat
		{
			get
			{
				return this._usedThisCombat;
			}
			set
			{
				base.AssertMutable();
				this._usedThisCombat = value;
			}
		}

		/// <summary>
		/// If two players both have Pael's Eye in multiplayer, and only one player uses it, then only that player gets a turn.
		/// In this case, we force the other player not to draw any cards and we auto-end their turn. But even though they
		/// haven't played any cards, that "turn" shouldn't count towards Pael's Eye for them.
		/// </summary>
		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x06005326 RID: 21286 RVA: 0x002442A2 File Offset: 0x002424A2
		// (set) Token: 0x06005327 RID: 21287 RVA: 0x002442AA File Offset: 0x002424AA
		private bool WasOwnerPartOfLastPlayerTurn
		{
			get
			{
				return this._wasOwnerPartOfLastPlayerTurn;
			}
			set
			{
				base.AssertMutable();
				this._wasOwnerPartOfLastPlayerTurn = value;
			}
		}

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06005328 RID: 21288 RVA: 0x002442B9 File Offset: 0x002424B9
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x002442C6 File Offset: 0x002424C6
		public override Task AfterObtained()
		{
			this.WasOwnerPartOfLastPlayerTurn = CombatManager.Instance.IsPartOfPlayerTurn(base.Owner);
			return Task.CompletedTask;
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x002442E4 File Offset: 0x002424E4
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return Task.CompletedTask;
			}
			if (this.UsedThisCombat)
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
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x00244344 File Offset: 0x00242544
		public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (side != base.Owner.Creature.Side)
			{
				return Task.CompletedTask;
			}
			if (this.UsedThisCombat)
			{
				return Task.CompletedTask;
			}
			if (participants.Contains(base.Owner.Creature))
			{
				base.Status = RelicStatus.Active;
				this.WasOwnerPartOfLastPlayerTurn = true;
			}
			else
			{
				base.Status = RelicStatus.Normal;
				this.WasOwnerPartOfLastPlayerTurn = false;
			}
			return Task.CompletedTask;
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x002443AE File Offset: 0x002425AE
		public override bool ShouldTakeExtraTurn(Player player)
		{
			return !this.UsedThisCombat && !this.AnyCardsPlayedThisTurn() && this.WasOwnerPartOfLastPlayerTurn && player == base.Owner;
		}

		// Token: 0x0600532D RID: 21293 RVA: 0x002443D4 File Offset: 0x002425D4
		public override async Task BeforeSideTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (!this.UsedThisCombat)
				{
					if (!this.AnyCardsPlayedThisTurn())
					{
						if (this.WasOwnerPartOfLastPlayerTurn)
						{
							foreach (CardModel cardModel in CardPile.GetCards(base.Owner, new PileType[] { PileType.Hand }).ToList<CardModel>())
							{
								await CardCmd.Exhaust(choiceContext, cardModel, false, false);
							}
							List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
						}
					}
				}
			}
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x00244427 File Offset: 0x00242627
		public override Task AfterTakingExtraTurn(Player player)
		{
			if (player != base.Owner)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			base.Status = RelicStatus.Normal;
			this.UsedThisCombat = true;
			return Task.CompletedTask;
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x00244451 File Offset: 0x00242651
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			this.UsedThisCombat = false;
			return Task.CompletedTask;
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x00244468 File Offset: 0x00242668
		private bool AnyCardsPlayedThisTurn()
		{
			PlayerCombatState playerCombatState = base.Owner.PlayerCombatState;
			if (playerCombatState != null && playerCombatState.TurnNumber == 1)
			{
				if (base.Owner.Relics.Any<RelicModel>((RelicModel r) => r is WhisperingEarring))
				{
					return true;
				}
			}
			return CombatManager.Instance.History.CardPlaysFinished.Any<CardPlayFinishedEntry>((CardPlayFinishedEntry e) => e.Actor == base.Owner.Creature && e.HappenedThisTurn(base.Owner.Creature.CombatState) && !e.CardPlay.IsAutoPlay);
		}

		// Token: 0x04002487 RID: 9351
		private bool _usedThisCombat;

		// Token: 0x04002488 RID: 9352
		private bool _wasOwnerPartOfLastPlayerTurn = true;
	}
}
