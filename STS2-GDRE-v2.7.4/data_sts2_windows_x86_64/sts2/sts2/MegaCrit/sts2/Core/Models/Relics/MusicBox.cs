using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000591 RID: 1425
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MusicBox : RelicModel
	{
		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x060052A9 RID: 21161 RVA: 0x002434CE File Offset: 0x002416CE
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x060052AA RID: 21162 RVA: 0x002434D1 File Offset: 0x002416D1
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x060052AB RID: 21163 RVA: 0x002434DE File Offset: 0x002416DE
		// (set) Token: 0x060052AC RID: 21164 RVA: 0x002434E6 File Offset: 0x002416E6
		private bool WasUsedThisTurn
		{
			get
			{
				return this._wasUsedThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._wasUsedThisTurn = value;
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x060052AD RID: 21165 RVA: 0x002434F5 File Offset: 0x002416F5
		// (set) Token: 0x060052AE RID: 21166 RVA: 0x002434FD File Offset: 0x002416FD
		[Nullable(2)]
		private CardModel CardBeingPlayed
		{
			[NullableContext(2)]
			get
			{
				return this._cardBeingPlayed;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._cardBeingPlayed = value;
			}
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x0024350C File Offset: 0x0024170C
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (this.CardBeingPlayed != null)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (this.WasUsedThisTurn)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Type != CardType.Attack)
			{
				return Task.CompletedTask;
			}
			this.CardBeingPlayed = cardPlay.Card;
			return Task.CompletedTask;
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x00243574 File Offset: 0x00241774
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card == this.CardBeingPlayed)
			{
				base.Flash();
				CardModel cardModel = cardPlay.Card.CreateClone();
				CardCmd.ApplyKeyword(cardModel, new CardKeyword[] { CardKeyword.Ethereal });
				await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner, CardPilePosition.Bottom);
				this.WasUsedThisTurn = true;
				this.CardBeingPlayed = null;
			}
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x002435BF File Offset: 0x002417BF
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.WasUsedThisTurn = false;
			this.CardBeingPlayed = null;
			return Task.CompletedTask;
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x002435ED File Offset: 0x002417ED
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this.WasUsedThisTurn = false;
			this.CardBeingPlayed = null;
			return Task.CompletedTask;
		}

		// Token: 0x0400247B RID: 9339
		private bool _wasUsedThisTurn;

		// Token: 0x0400247C RID: 9340
		[Nullable(2)]
		private CardModel _cardBeingPlayed;
	}
}
