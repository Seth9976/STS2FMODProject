using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CB0 RID: 3248
	[NullableContext(1)]
	[Nullable(0)]
	public class CardPile
	{
		// Token: 0x060084A1 RID: 33953 RVA: 0x002CA551 File Offset: 0x002C8751
		public CardPile(PileType type)
		{
		}

		/// <summary>
		/// This is functionally a constant but is a property for modding ergonomics.
		/// </summary>
		// Token: 0x17002438 RID: 9272
		// (get) Token: 0x060084A2 RID: 33954 RVA: 0x002CA56B File Offset: 0x002C876B
		public static int MaxCardsInHand
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x060084A3 RID: 33955 RVA: 0x002CA570 File Offset: 0x002C8770
		// (remove) Token: 0x060084A4 RID: 33956 RVA: 0x002CA5A8 File Offset: 0x002C87A8
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action ContentsChanged;

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x060084A5 RID: 33957 RVA: 0x002CA5E0 File Offset: 0x002C87E0
		// (remove) Token: 0x060084A6 RID: 33958 RVA: 0x002CA618 File Offset: 0x002C8818
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CardModel> CardAdded;

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x060084A7 RID: 33959 RVA: 0x002CA650 File Offset: 0x002C8850
		// (remove) Token: 0x060084A8 RID: 33960 RVA: 0x002CA688 File Offset: 0x002C8888
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CardModel> CardRemoved;

		// Token: 0x140000CD RID: 205
		// (add) Token: 0x060084A9 RID: 33961 RVA: 0x002CA6C0 File Offset: 0x002C88C0
		// (remove) Token: 0x060084AA RID: 33962 RVA: 0x002CA6F8 File Offset: 0x002C88F8
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action CardAddFinished;

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x060084AB RID: 33963 RVA: 0x002CA730 File Offset: 0x002C8930
		// (remove) Token: 0x060084AC RID: 33964 RVA: 0x002CA768 File Offset: 0x002C8968
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action CardRemoveFinished;

		// Token: 0x17002439 RID: 9273
		// (get) Token: 0x060084AD RID: 33965 RVA: 0x002CA79D File Offset: 0x002C899D
		public PileType Type { get; } = type;

		// Token: 0x1700243A RID: 9274
		// (get) Token: 0x060084AE RID: 33966 RVA: 0x002CA7A5 File Offset: 0x002C89A5
		public IReadOnlyList<CardModel> Cards
		{
			get
			{
				return this._cards;
			}
		}

		// Token: 0x1700243B RID: 9275
		// (get) Token: 0x060084AF RID: 33967 RVA: 0x002CA7AD File Offset: 0x002C89AD
		public bool IsEmpty
		{
			get
			{
				return !this.Cards.Any<CardModel>();
			}
		}

		// Token: 0x1700243C RID: 9276
		// (get) Token: 0x060084B0 RID: 33968 RVA: 0x002CA7BD File Offset: 0x002C89BD
		public bool IsCombatPile
		{
			get
			{
				return this.Type.IsCombatPile();
			}
		}

		// Token: 0x1700243D RID: 9277
		// (get) Token: 0x060084B1 RID: 33969 RVA: 0x002CA7CA File Offset: 0x002C89CA
		public int UpgradableCardCount
		{
			get
			{
				return this._cards.Count<CardModel>((CardModel card) => card.IsUpgradable);
			}
		}

		// Token: 0x060084B2 RID: 33970 RVA: 0x002CA7F8 File Offset: 0x002C89F8
		[return: Nullable(2)]
		public static CardPile Get(PileType type, Player player)
		{
			CardPile cardPile;
			switch (type)
			{
			case PileType.None:
				cardPile = null;
				break;
			case PileType.Draw:
			{
				PlayerCombatState playerCombatState = player.PlayerCombatState;
				cardPile = ((playerCombatState != null) ? playerCombatState.DrawPile : null);
				break;
			}
			case PileType.Hand:
			{
				PlayerCombatState playerCombatState2 = player.PlayerCombatState;
				cardPile = ((playerCombatState2 != null) ? playerCombatState2.Hand : null);
				break;
			}
			case PileType.Discard:
			{
				PlayerCombatState playerCombatState3 = player.PlayerCombatState;
				cardPile = ((playerCombatState3 != null) ? playerCombatState3.DiscardPile : null);
				break;
			}
			case PileType.Exhaust:
			{
				PlayerCombatState playerCombatState4 = player.PlayerCombatState;
				cardPile = ((playerCombatState4 != null) ? playerCombatState4.ExhaustPile : null);
				break;
			}
			case PileType.Play:
			{
				PlayerCombatState playerCombatState5 = player.PlayerCombatState;
				cardPile = ((playerCombatState5 != null) ? playerCombatState5.PlayPile : null);
				break;
			}
			case PileType.Deck:
				cardPile = player.Deck;
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			return cardPile;
		}

		// Token: 0x060084B3 RID: 33971 RVA: 0x002CA8B8 File Offset: 0x002C8AB8
		public static IEnumerable<CardModel> GetCards(Player player, params PileType[] piles)
		{
			return piles.SelectMany<PileType, CardModel>((PileType p) => p.GetPile(player).Cards);
		}

		/// <summary>
		/// Randomize the order of the cards in this pile.
		/// Do NOT call this when your intention is to do a standard shuffle action.
		/// Use <see cref="M:MegaCrit.Sts2.Core.Commands.CardPileCmd.Shuffle(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player)" /> instead.
		/// This is primarily meant for randomizing the order of the draw pile when a combat first starts.
		/// </summary>
		// Token: 0x060084B4 RID: 33972 RVA: 0x002CA8E4 File Offset: 0x002C8AE4
		public void RandomizeOrderInternal(Player player, Rng rng, CombatState state)
		{
			this._cards.UnstableShuffle<CardModel>(rng);
			Action<List<CardModel>> action = TestRngInjector.ConsumeInitialShuffleOverride();
			if (action != null)
			{
				action(this._cards);
			}
			Hook.ModifyShuffleOrder(state, player, this._cards, true);
		}

		/// <summary>
		/// Add a card to the bottom of this pile.
		/// </summary>
		/// <param name="card">Card to add.</param>
		/// <param name="index">Index to insert the card into. -1 will cause it to be added to the end ("bottom").</param>
		/// <param name="silent">If true, no events will be called.</param>
		/// <exception cref="T:System.InvalidOperationException">Thrown if this specific CardModel instance is already in the pile.</exception>
		// Token: 0x060084B5 RID: 33973 RVA: 0x002CA924 File Offset: 0x002C8B24
		public void AddInternal(CardModel card, int index = -1, bool silent = false)
		{
			card.AssertMutable();
			if (this.Cards.Contains(card))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Card pile already contains ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (index >= 0)
			{
				this._cards.Insert(index, card);
			}
			else
			{
				this._cards.Add(card);
			}
			if (this.IsCombatPile && CombatManager.Instance.IsInProgress)
			{
				CombatManager.Instance.StateTracker.Subscribe(card);
			}
			if (!silent)
			{
				Action<CardModel> cardAdded = this.CardAdded;
				if (cardAdded != null)
				{
					cardAdded(card);
				}
				this.InvokeContentsChanged();
			}
		}

		/// <summary>
		/// Remove a card from this pile.
		/// </summary>
		/// <param name="card">Card to remove.</param>
		/// <param name="silent">If set to true, no events will be sent to update the UI.</param>
		/// <exception cref="T:System.InvalidOperationException">Thrown if this specific CardModel instance is not in the pile.</exception>
		// Token: 0x060084B6 RID: 33974 RVA: 0x002CA9DC File Offset: 0x002C8BDC
		public void RemoveInternal(CardModel card, bool silent = false)
		{
			if (!this.Cards.Contains(card))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Card pile does not contain ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._cards.Remove(card);
			if (this.IsCombatPile)
			{
				CombatManager.Instance.StateTracker.Unsubscribe(card);
			}
			if (!silent)
			{
				Action<CardModel> cardRemoved = this.CardRemoved;
				if (cardRemoved != null)
				{
					cardRemoved(card);
				}
				this.InvokeContentsChanged();
				this.InvokeCardRemoveFinished();
			}
		}

		/// <summary>
		/// Move a card to the bottom of this pile.
		/// The card must already be in this pile.
		/// This does not send any events to update the UI, so you should only use this in combination with other methods
		/// that do.
		/// </summary>
		/// <param name="card">Card that we're moving to the bottom.</param>
		/// <exception cref="T:System.InvalidOperationException"></exception>
		// Token: 0x060084B7 RID: 33975 RVA: 0x002CAA74 File Offset: 0x002C8C74
		public void MoveToBottomInternal(CardModel card)
		{
			if (!this.Cards.Contains(card))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Card pile does not contain ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._cards.Remove(card);
			this._cards.Add(card);
		}

		/// <summary>
		/// Move a card to the top of this pile.
		/// The card must already be in this pile.
		/// This does not send any events to update the UI, so you should only use this in combination with other methods
		/// that do.
		/// </summary>
		/// <param name="card">Card that we're moving to the top.</param>
		/// <exception cref="T:System.InvalidOperationException"></exception>
		// Token: 0x060084B8 RID: 33976 RVA: 0x002CAAE0 File Offset: 0x002C8CE0
		public void MoveToTopInternal(CardModel card)
		{
			if (!this.Cards.Contains(card))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Card pile does not contain ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._cards.Remove(card);
			this._cards.Insert(0, card);
		}

		// Token: 0x060084B9 RID: 33977 RVA: 0x002CAB4C File Offset: 0x002C8D4C
		public void Clear(bool silent = false)
		{
			foreach (CardModel cardModel in this.Cards.ToList<CardModel>())
			{
				this.RemoveInternal(cardModel, silent);
			}
			this._cards.Clear();
		}

		// Token: 0x060084BA RID: 33978 RVA: 0x002CABB0 File Offset: 0x002C8DB0
		public void InvokeCardAddFinished()
		{
			Action cardAddFinished = this.CardAddFinished;
			if (cardAddFinished == null)
			{
				return;
			}
			cardAddFinished();
		}

		// Token: 0x060084BB RID: 33979 RVA: 0x002CABC2 File Offset: 0x002C8DC2
		public void InvokeCardRemoveFinished()
		{
			Action cardRemoveFinished = this.CardRemoveFinished;
			if (cardRemoveFinished == null)
			{
				return;
			}
			cardRemoveFinished();
		}

		// Token: 0x060084BC RID: 33980 RVA: 0x002CABD4 File Offset: 0x002C8DD4
		public void InvokeContentsChanged()
		{
			Action contentsChanged = this.ContentsChanged;
			if (contentsChanged == null)
			{
				return;
			}
			contentsChanged();
		}

		// Token: 0x04002D92 RID: 11666
		private readonly List<CardModel> _cards = new List<CardModel>();
	}
}
