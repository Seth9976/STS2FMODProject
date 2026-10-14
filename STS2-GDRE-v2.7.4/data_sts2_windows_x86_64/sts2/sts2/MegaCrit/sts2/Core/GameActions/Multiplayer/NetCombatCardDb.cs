using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	// Token: 0x02000C3C RID: 3132
	[NullableContext(1)]
	[Nullable(0)]
	public class NetCombatCardDb
	{
		// Token: 0x17002353 RID: 9043
		// (get) Token: 0x06008140 RID: 33088 RVA: 0x002BD79F File Offset: 0x002BB99F
		public static NetCombatCardDb Instance { get; } = new NetCombatCardDb();

		// Token: 0x06008141 RID: 33089 RVA: 0x002BD7A8 File Offset: 0x002BB9A8
		public void StartCombat(IReadOnlyList<Player> players)
		{
			this._nextId = 0U;
			this._idToCard.Clear();
			this._cardToId.Clear();
			foreach (Player player in players)
			{
				if (player.PlayerCombatState != null || !TestMode.IsOn)
				{
					foreach (CardModel cardModel in player.PlayerCombatState.AllPiles.SelectMany<CardPile, CardModel>((CardPile p) => p.Cards))
					{
						this.IdCardIfNecessary(cardModel);
					}
					foreach (CardPile cardPile in player.PlayerCombatState.AllPiles)
					{
						NetCombatCardDb.Subscription subscription = default(NetCombatCardDb.Subscription);
						subscription.pile = cardPile;
						subscription.action = delegate
						{
							this.OnPileContentsChanged(subscription.pile);
						};
						this._subscriptions.Add(subscription);
						cardPile.ContentsChanged += subscription.action;
					}
				}
			}
			CombatManager.Instance.CombatEnded += this.OnCombatEnded;
		}

		// Token: 0x06008142 RID: 33090 RVA: 0x002BD968 File Offset: 0x002BBB68
		private void OnCombatEnded(CombatRoom _)
		{
			foreach (NetCombatCardDb.Subscription subscription in this._subscriptions)
			{
				subscription.pile.ContentsChanged -= subscription.action;
			}
			this._subscriptions.Clear();
			CombatManager.Instance.CombatEnded -= this.OnCombatEnded;
		}

		// Token: 0x06008143 RID: 33091 RVA: 0x002BD9E8 File Offset: 0x002BBBE8
		public uint GetCardId(CardModel card)
		{
			uint num;
			if (!this.TryGetCardId(card, out num))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Card ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral(" could not be found in combat ID database!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return num;
		}

		// Token: 0x06008144 RID: 33092 RVA: 0x002BDA38 File Offset: 0x002BBC38
		public CardModel GetCard(uint id)
		{
			CardModel cardModel;
			if (!this.TryGetCard(id, out cardModel))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Could not map ID ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(id);
				defaultInterpolatedStringHandler.AppendLiteral(" to any card!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return cardModel;
		}

		// Token: 0x06008145 RID: 33093 RVA: 0x002BDA88 File Offset: 0x002BBC88
		public bool TryGetCardId(CardModel card, out uint id)
		{
			if (!card.IsMutable)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get ID for canonical card ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral("! Use the ModelId instead");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._cardToId.TryGetValue(card, out id);
		}

		// Token: 0x06008146 RID: 33094 RVA: 0x002BDAE1 File Offset: 0x002BBCE1
		[NullableContext(2)]
		public bool TryGetCard(uint id, out CardModel card)
		{
			return this._idToCard.TryGetValue(id, out card);
		}

		// Token: 0x06008147 RID: 33095 RVA: 0x002BDAF0 File Offset: 0x002BBCF0
		private void OnPileContentsChanged(CardPile pile)
		{
			foreach (CardModel cardModel in pile.Cards)
			{
				this.IdCardIfNecessary(cardModel);
			}
		}

		// Token: 0x06008148 RID: 33096 RVA: 0x002BDB40 File Offset: 0x002BBD40
		private void IdCardIfNecessary(CardModel card)
		{
			if (this._cardToId.ContainsKey(card))
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (card.Owner == null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to ID combat card ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral(" without an owner! This is not allowed");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			LogLevel logLevel = LogLevel.Debug;
			LogType logType = LogType.Network;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 4);
			defaultInterpolatedStringHandler.AppendLiteral("ID card ");
			defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
			defaultInterpolatedStringHandler.AppendLiteral(" owned by ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(card.Owner.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" in pile ");
			CardPile pile = card.Pile;
			defaultInterpolatedStringHandler.AppendFormatted<PileType?>((pile != null) ? new PileType?(pile.Type) : null);
			defaultInterpolatedStringHandler.AppendLiteral(" with id: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this._nextId);
			Log.LogMessage(logLevel, logType, defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this._cardToId[card] = this._nextId;
			this._idToCard[this._nextId] = card;
			this._nextId += 1U;
		}

		// Token: 0x06008149 RID: 33097 RVA: 0x002BDC64 File Offset: 0x002BBE64
		public uint IdCardForTesting(CardModel card)
		{
			this.IdCardIfNecessary(card);
			return this._cardToId[card];
		}

		// Token: 0x0600814A RID: 33098 RVA: 0x002BDC79 File Offset: 0x002BBE79
		public void ClearCardsForTesting()
		{
			this._nextId = 0U;
			this._idToCard.Clear();
			this._cardToId.Clear();
		}

		// Token: 0x04002B4E RID: 11086
		private uint _nextId;

		// Token: 0x04002B4F RID: 11087
		private readonly Dictionary<uint, CardModel> _idToCard = new Dictionary<uint, CardModel>();

		// Token: 0x04002B50 RID: 11088
		private readonly Dictionary<CardModel, uint> _cardToId = new Dictionary<CardModel, uint>();

		// Token: 0x04002B51 RID: 11089
		private readonly List<NetCombatCardDb.Subscription> _subscriptions = new List<NetCombatCardDb.Subscription>();

		// Token: 0x02002374 RID: 9076
		[Nullable(0)]
		private struct Subscription
		{
			// Token: 0x04009647 RID: 38471
			public CardPile pile;

			// Token: 0x04009648 RID: 38472
			public Action action;
		}
	}
}
