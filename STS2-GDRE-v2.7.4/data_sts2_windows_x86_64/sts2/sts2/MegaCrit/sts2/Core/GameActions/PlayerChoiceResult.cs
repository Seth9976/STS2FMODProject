using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Models;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.GameActions
{
	/// <summary>
	/// Represents the result of a choice that the player makes while playing a card or using a potion. For example, it
	/// could contain the choice of card to discard during a Survivor play, or the choice of card to add to hand at the
	/// beginning of combat from Toolbox.
	/// </summary>
	// Token: 0x02000C29 RID: 3113
	[NullableContext(1)]
	[Nullable(0)]
	public class PlayerChoiceResult
	{
		// Token: 0x1700232E RID: 9006
		// (get) Token: 0x06008083 RID: 32899 RVA: 0x002B8F86 File Offset: 0x002B7186
		// (set) Token: 0x06008084 RID: 32900 RVA: 0x002B8F8E File Offset: 0x002B718E
		public PlayerChoiceType ChoiceType { get; private set; }

		// Token: 0x06008085 RID: 32901 RVA: 0x002B8F98 File Offset: 0x002B7198
		public unsafe static PlayerChoiceResult FromCanonicalCard([Nullable(2)] CardModel canonicalCard)
		{
			if (canonicalCard != null)
			{
				canonicalCard.AssertCanonical();
			}
			PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult();
			playerChoiceResult.ChoiceType = PlayerChoiceType.CanonicalCard;
			List<CardModel> list;
			if (canonicalCard == null)
			{
				list = new List<CardModel>();
			}
			else
			{
				int num = 1;
				List<CardModel> list2 = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list2, num);
				list = list2;
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list2);
				int num2 = 0;
				*span[num2] = canonicalCard;
			}
			playerChoiceResult._canonicalCards = list;
			return playerChoiceResult;
		}

		// Token: 0x06008086 RID: 32902 RVA: 0x002B8FEC File Offset: 0x002B71EC
		public unsafe static PlayerChoiceResult FromMutableCombatCard([Nullable(2)] CardModel combatCard)
		{
			if (combatCard != null)
			{
				combatCard.AssertMutable();
			}
			CardPile cardPile = ((combatCard != null) ? combatCard.Pile : null);
			if (cardPile != null && !cardPile.IsCombatPile)
			{
				throw new InvalidOperationException("Card must be in a combat pile!");
			}
			PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult();
			playerChoiceResult.ChoiceType = PlayerChoiceType.CombatCard;
			List<CardModel> list;
			if (combatCard == null)
			{
				list = new List<CardModel>();
			}
			else
			{
				int num = 1;
				List<CardModel> list2 = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list2, num);
				list = list2;
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list2);
				int num2 = 0;
				*span[num2] = combatCard;
			}
			playerChoiceResult._combatCards = list;
			return playerChoiceResult;
		}

		// Token: 0x06008087 RID: 32903 RVA: 0x002B9064 File Offset: 0x002B7264
		public unsafe static PlayerChoiceResult FromMutableDeckCard([Nullable(2)] CardModel deckCard)
		{
			if (deckCard != null)
			{
				deckCard.AssertMutable();
			}
			bool flag;
			if (deckCard != null)
			{
				CardPile pile = deckCard.Pile;
				if (pile != null)
				{
					PileType type = pile.Type;
					if (type == PileType.Deck)
					{
						goto IL_0025;
					}
				}
				flag = true;
				goto IL_0027;
			}
			IL_0025:
			flag = false;
			IL_0027:
			if (flag)
			{
				throw new InvalidOperationException("Card must be in a deck!");
			}
			PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult();
			playerChoiceResult.ChoiceType = PlayerChoiceType.DeckCard;
			List<CardModel> list;
			if (deckCard == null)
			{
				list = new List<CardModel>();
			}
			else
			{
				int num = 1;
				List<CardModel> list2 = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list2, num);
				list = list2;
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list2);
				int num2 = 0;
				*span[num2] = deckCard;
			}
			playerChoiceResult._deckCards = list;
			return playerChoiceResult;
		}

		// Token: 0x06008088 RID: 32904 RVA: 0x002B90E7 File Offset: 0x002B72E7
		public static PlayerChoiceResult FromCanonicalCards(IEnumerable<CardModel> canonicalCards)
		{
			return new PlayerChoiceResult
			{
				ChoiceType = PlayerChoiceType.CanonicalCard,
				_canonicalCards = canonicalCards.ToList<CardModel>()
			};
		}

		// Token: 0x06008089 RID: 32905 RVA: 0x002B9101 File Offset: 0x002B7301
		public static PlayerChoiceResult FromMutableCombatCards(IEnumerable<CardModel> combatCards)
		{
			return new PlayerChoiceResult
			{
				ChoiceType = PlayerChoiceType.CombatCard,
				_combatCards = combatCards.ToList<CardModel>()
			};
		}

		// Token: 0x0600808A RID: 32906 RVA: 0x002B911B File Offset: 0x002B731B
		public static PlayerChoiceResult FromMutableDeckCards(IEnumerable<CardModel> deckCards)
		{
			return new PlayerChoiceResult
			{
				ChoiceType = PlayerChoiceType.DeckCard,
				_deckCards = deckCards.ToList<CardModel>()
			};
		}

		// Token: 0x0600808B RID: 32907 RVA: 0x002B9138 File Offset: 0x002B7338
		public unsafe static PlayerChoiceResult FromMutableCard([Nullable(2)] CardModel mutableCard)
		{
			if (mutableCard != null)
			{
				mutableCard.AssertMutable();
			}
			PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult();
			playerChoiceResult.ChoiceType = PlayerChoiceType.MutableCard;
			List<CardModel> list;
			if (mutableCard == null)
			{
				list = new List<CardModel>();
			}
			else
			{
				int num = 1;
				List<CardModel> list2 = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list2, num);
				list = list2;
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list2);
				int num2 = 0;
				*span[num2] = mutableCard;
			}
			playerChoiceResult._mutableCards = list;
			return playerChoiceResult;
		}

		// Token: 0x0600808C RID: 32908 RVA: 0x002B918C File Offset: 0x002B738C
		public static PlayerChoiceResult FromMutableCards(IEnumerable<CardModel> mutableCards)
		{
			CardModel[] array = mutableCards.ToArray<CardModel>();
			foreach (CardModel cardModel in array)
			{
				cardModel.AssertMutable();
				if (cardModel.Owner != array[0].Owner)
				{
					throw new InvalidOperationException("All cards passed to FromMutableCards must have the same owner!");
				}
			}
			return new PlayerChoiceResult
			{
				ChoiceType = PlayerChoiceType.MutableCard,
				_mutableCards = array.ToList<CardModel>()
			};
		}

		// Token: 0x0600808D RID: 32909 RVA: 0x002B91F0 File Offset: 0x002B73F0
		public static PlayerChoiceResult FromCards(IEnumerable<CardModel> cards, PlayerChoiceType choiceType)
		{
			PlayerChoiceResult playerChoiceResult;
			switch (choiceType)
			{
			case PlayerChoiceType.CanonicalCard:
				playerChoiceResult = new PlayerChoiceResult
				{
					ChoiceType = choiceType,
					_canonicalCards = cards.ToList<CardModel>()
				};
				break;
			case PlayerChoiceType.CombatCard:
				playerChoiceResult = new PlayerChoiceResult
				{
					ChoiceType = choiceType,
					_combatCards = cards.ToList<CardModel>()
				};
				break;
			case PlayerChoiceType.DeckCard:
				playerChoiceResult = new PlayerChoiceResult
				{
					ChoiceType = choiceType,
					_deckCards = cards.ToList<CardModel>()
				};
				break;
			case PlayerChoiceType.MutableCard:
				playerChoiceResult = new PlayerChoiceResult
				{
					ChoiceType = choiceType,
					_mutableCards = cards.ToList<CardModel>()
				};
				break;
			default:
				throw new ArgumentOutOfRangeException("choiceType", choiceType, null);
			}
			return playerChoiceResult;
		}

		// Token: 0x0600808E RID: 32910 RVA: 0x002B9296 File Offset: 0x002B7496
		public static PlayerChoiceResult FromPlayerId(ulong? playerId)
		{
			return new PlayerChoiceResult
			{
				ChoiceType = PlayerChoiceType.Player,
				_playerId = playerId
			};
		}

		// Token: 0x0600808F RID: 32911 RVA: 0x002B92AC File Offset: 0x002B74AC
		public unsafe static PlayerChoiceResult FromIndex(int? index)
		{
			PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult();
			playerChoiceResult.ChoiceType = PlayerChoiceType.Index;
			List<int> list;
			if (index == null)
			{
				list = new List<int>();
			}
			else
			{
				int num = 1;
				List<int> list2 = new List<int>(num);
				CollectionsMarshal.SetCount<int>(list2, num);
				list = list2;
				Span<int> span = CollectionsMarshal.AsSpan<int>(list2);
				int num2 = 0;
				*span[num2] = index.Value;
			}
			playerChoiceResult._indexes = list;
			return playerChoiceResult;
		}

		// Token: 0x06008090 RID: 32912 RVA: 0x002B9303 File Offset: 0x002B7503
		public static PlayerChoiceResult FromIndexes(List<int> indexes)
		{
			return new PlayerChoiceResult
			{
				ChoiceType = PlayerChoiceType.Index,
				_indexes = indexes
			};
		}

		/// <summary>
		/// Returns the selection assuming that it was a single canonical card.
		/// Will be null if the player skipped the choice.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not a canonical card.</exception>
		// Token: 0x06008091 RID: 32913 RVA: 0x002B9318 File Offset: 0x002B7518
		[NullableContext(2)]
		public CardModel AsCanonicalCard()
		{
			if (this.ChoiceType != PlayerChoiceType.CanonicalCard)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get canonical card from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._canonicalCards.FirstOrDefault<CardModel>();
		}

		/// <summary>
		/// Returns the selection assuming that it was a collection of canonical cards.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not a collection of combat cards.</exception>
		// Token: 0x06008092 RID: 32914 RVA: 0x002B9378 File Offset: 0x002B7578
		public IEnumerable<CardModel> AsCanonicalCards()
		{
			if (this.ChoiceType != PlayerChoiceType.CanonicalCard)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get canonical cards from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._canonicalCards;
		}

		/// <summary>
		/// Returns the selection assuming that it was a collection of combat cards.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not a collection of combat cards.</exception>
		// Token: 0x06008093 RID: 32915 RVA: 0x002B93D0 File Offset: 0x002B75D0
		public IEnumerable<CardModel> AsCombatCards()
		{
			if (this.ChoiceType != PlayerChoiceType.CombatCard)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get combat cards from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._combatCards;
		}

		/// <summary>
		/// Returns the selection assuming that it was a collection of deck cards.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not a collection of deck cards.</exception>
		// Token: 0x06008094 RID: 32916 RVA: 0x002B9428 File Offset: 0x002B7628
		public IEnumerable<CardModel> AsDeckCards()
		{
			if (this.ChoiceType != PlayerChoiceType.DeckCard)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get deck cards from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._deckCards;
		}

		/// <summary>
		/// Returns the selection assuming that it was a single mutable card without owner.
		/// Will be null if the player skipped the choice.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not a mutable card without owner.</exception>
		// Token: 0x06008095 RID: 32917 RVA: 0x002B9480 File Offset: 0x002B7680
		[NullableContext(2)]
		public CardModel AsMutableCard()
		{
			if (this.ChoiceType != PlayerChoiceType.MutableCard)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get mutable cards from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._mutableCards.FirstOrDefault<CardModel>();
		}

		/// <summary>
		/// Returns the selection assuming that it was a collection of mutable cards without owners.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not a collection of mutable cards without owners.</exception>
		// Token: 0x06008096 RID: 32918 RVA: 0x002B94E0 File Offset: 0x002B76E0
		public IEnumerable<CardModel> AsMutableCards()
		{
			if (this.ChoiceType != PlayerChoiceType.MutableCard)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get mutable cards from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._mutableCards;
		}

		/// <summary>
		/// Returns a set of card models based on the type passed.
		/// </summary>
		/// <param name="type">The type of selection that the remote player chose.</param>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not of the passed type.</exception>
		// Token: 0x06008097 RID: 32919 RVA: 0x002B9538 File Offset: 0x002B7738
		public IEnumerable<CardModel> AsCards(PlayerChoiceType type)
		{
			if (this.ChoiceType != type)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get cards of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(type);
				defaultInterpolatedStringHandler.AppendLiteral(" from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<CardModel> list;
			switch (type)
			{
			case PlayerChoiceType.CanonicalCard:
				list = this._canonicalCards;
				break;
			case PlayerChoiceType.CombatCard:
				list = this._combatCards;
				break;
			case PlayerChoiceType.DeckCard:
				list = this._deckCards;
				break;
			case PlayerChoiceType.MutableCard:
				list = this._mutableCards;
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			return list;
		}

		/// <summary>
		/// Returns the selection assuming that it is a player.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not a player.</exception>
		// Token: 0x06008098 RID: 32920 RVA: 0x002B95F0 File Offset: 0x002B77F0
		public ulong? AsPlayerId()
		{
			if (this.ChoiceType != PlayerChoiceType.Player)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get player ID from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._playerId;
		}

		/// <summary>
		/// Returns the selection assuming that it is a non-null index.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not an index.</exception>
		// Token: 0x06008099 RID: 32921 RVA: 0x002B9648 File Offset: 0x002B7848
		public int AsIndex()
		{
			if (this.ChoiceType != PlayerChoiceType.Index)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get index from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<int> indexes = this._indexes;
			if (indexes == null)
			{
				return -1;
			}
			return indexes.FirstOrDefault<int>();
		}

		/// <summary>
		/// Returns the selection as an index, or null if there is no index.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not an index.</exception>
		// Token: 0x0600809A RID: 32922 RVA: 0x002B96AC File Offset: 0x002B78AC
		public int? AsIndexOrNull()
		{
			if (this.ChoiceType != PlayerChoiceType.Index)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get index from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<int> indexes = this._indexes;
			if (indexes == null || indexes.Count <= 0)
			{
				return null;
			}
			return new int?(this._indexes[0]);
		}

		/// <summary>
		/// Returns the selection assuming that it is a list of indexes.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the selection was not a list of indexes.</exception>
		// Token: 0x0600809B RID: 32923 RVA: 0x002B972C File Offset: 0x002B792C
		public List<int> AsIndexes()
		{
			if (this.ChoiceType != PlayerChoiceType.Index)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get indexes from player choice result of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceType>(this.ChoiceType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._indexes;
		}

		/// <summary>
		/// Generates a player choice result from a serialized network data.
		/// Remember that player choices may involve cards that are created within combat! For example, if a remote player
		/// plays a Survivor which discards a Shiv that was just created using a Blade Dance, it is possible to receive an
		/// exception from this method if it is called at a point before the Shiv was created. It is the responsibility of
		/// the caller to ensure that this is not the case.
		/// </summary>
		// Token: 0x0600809C RID: 32924 RVA: 0x002B9784 File Offset: 0x002B7984
		public static PlayerChoiceResult FromNetData(Player sender, IPlayerCollection players, NetPlayerChoiceResult netData)
		{
			PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult
			{
				ChoiceType = netData.type
			};
			switch (playerChoiceResult.ChoiceType)
			{
			case PlayerChoiceType.CanonicalCard:
				playerChoiceResult._canonicalCards = netData.canonicalCards;
				return playerChoiceResult;
			case PlayerChoiceType.CombatCard:
				playerChoiceResult._combatCards = netData.combatCards.Select<NetCombatCard, CardModel>((NetCombatCard c) => c.ToCardModel()).ToList<CardModel>();
				return playerChoiceResult;
			case PlayerChoiceType.DeckCard:
				playerChoiceResult._deckCards = netData.deckCards.Select<NetDeckCard, CardModel>((NetDeckCard c) => c.ToCardModel(sender)).ToList<CardModel>();
				return playerChoiceResult;
			case PlayerChoiceType.MutableCard:
			{
				PlayerChoiceResult playerChoiceResult2 = playerChoiceResult;
				IEnumerable<SerializableCard> mutableCards = netData.mutableCards;
				Func<SerializableCard, CardModel> func;
				if ((func = PlayerChoiceResult.<>O.<0>__FromSerializable) == null)
				{
					func = (PlayerChoiceResult.<>O.<0>__FromSerializable = new Func<SerializableCard, CardModel>(CardModel.FromSerializable));
				}
				playerChoiceResult2._mutableCards = mutableCards.Select<SerializableCard, CardModel>(func).ToList<CardModel>();
				if (netData.mutableCardOwner == null)
				{
					return playerChoiceResult;
				}
				using (List<CardModel>.Enumerator enumerator = playerChoiceResult._mutableCards.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CardModel cardModel = enumerator.Current;
						cardModel.Owner = players.GetPlayer(netData.mutableCardOwner.Value);
					}
					return playerChoiceResult;
				}
				break;
			}
			case PlayerChoiceType.Player:
				break;
			case PlayerChoiceType.Index:
				playerChoiceResult._indexes = netData.indexes;
				return playerChoiceResult;
			default:
				throw new ArgumentOutOfRangeException();
			}
			playerChoiceResult._playerId = netData.playerId;
			return playerChoiceResult;
		}

		// Token: 0x0600809D RID: 32925 RVA: 0x002B990C File Offset: 0x002B7B0C
		public NetPlayerChoiceResult ToNetData()
		{
			NetPlayerChoiceResult netPlayerChoiceResult = default(NetPlayerChoiceResult);
			netPlayerChoiceResult.type = this.ChoiceType;
			List<CardModel> combatCards = this._combatCards;
			List<NetCombatCard> list;
			if (combatCards == null)
			{
				list = null;
			}
			else
			{
				Func<CardModel, NetCombatCard> func;
				if ((func = PlayerChoiceResult.<>O.<1>__FromModel) == null)
				{
					func = (PlayerChoiceResult.<>O.<1>__FromModel = new Func<CardModel, NetCombatCard>(NetCombatCard.FromModel));
				}
				list = combatCards.Select<CardModel, NetCombatCard>(func).ToList<NetCombatCard>();
			}
			netPlayerChoiceResult.combatCards = list;
			List<CardModel> deckCards = this._deckCards;
			List<NetDeckCard> list2;
			if (deckCards == null)
			{
				list2 = null;
			}
			else
			{
				Func<CardModel, NetDeckCard> func2;
				if ((func2 = PlayerChoiceResult.<>O.<2>__FromModel) == null)
				{
					func2 = (PlayerChoiceResult.<>O.<2>__FromModel = new Func<CardModel, NetDeckCard>(NetDeckCard.FromModel));
				}
				list2 = deckCards.Select<CardModel, NetDeckCard>(func2).ToList<NetDeckCard>();
			}
			netPlayerChoiceResult.deckCards = list2;
			List<CardModel> canonicalCards = this._canonicalCards;
			netPlayerChoiceResult.canonicalCards = ((canonicalCards != null) ? canonicalCards.ToList<CardModel>() : null);
			List<CardModel> mutableCards = this._mutableCards;
			List<SerializableCard> list3;
			if (mutableCards == null)
			{
				list3 = null;
			}
			else
			{
				list3 = mutableCards.Select<CardModel, SerializableCard>((CardModel c) => c.ToSerializable()).ToList<SerializableCard>();
			}
			netPlayerChoiceResult.mutableCards = list3;
			List<CardModel> mutableCards2 = this._mutableCards;
			ulong? num;
			if (mutableCards2 == null)
			{
				num = null;
			}
			else
			{
				CardModel cardModel = mutableCards2.FirstOrDefault<CardModel>();
				num = ((cardModel != null) ? new ulong?(cardModel.Owner.NetId) : null);
			}
			netPlayerChoiceResult.mutableCardOwner = num;
			netPlayerChoiceResult.playerId = this._playerId;
			netPlayerChoiceResult.indexes = this._indexes;
			return netPlayerChoiceResult;
		}

		// Token: 0x0600809E RID: 32926 RVA: 0x002B9A50 File Offset: 0x002B7C50
		public override string ToString()
		{
			switch (this.ChoiceType)
			{
			case PlayerChoiceType.CanonicalCard:
				return "PlayerChoiceResult canonical " + string.Join<CardModel>(",", this._canonicalCards ?? new List<CardModel>());
			case PlayerChoiceType.CombatCard:
				return "PlayerChoiceResult combat " + string.Join<CardModel>(",", this._combatCards ?? new List<CardModel>());
			case PlayerChoiceType.DeckCard:
				return "PlayerChoiceResult deck " + string.Join<CardModel>(",", this._deckCards ?? new List<CardModel>());
			case PlayerChoiceType.MutableCard:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 3);
				defaultInterpolatedStringHandler.AppendFormatted("PlayerChoiceResult");
				defaultInterpolatedStringHandler.AppendLiteral(" mutable cards ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<CardModel>(",", this._mutableCards ?? new List<CardModel>()));
				defaultInterpolatedStringHandler.AppendLiteral(", owner: ");
				List<CardModel> mutableCards = this._mutableCards;
				ulong? num;
				if (mutableCards == null)
				{
					num = null;
				}
				else
				{
					CardModel cardModel = mutableCards.FirstOrDefault<CardModel>();
					num = ((cardModel != null) ? new ulong?(cardModel.Owner.NetId) : null);
				}
				defaultInterpolatedStringHandler.AppendFormatted<ulong?>(num);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			case PlayerChoiceType.Player:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 2);
				defaultInterpolatedStringHandler.AppendFormatted("PlayerChoiceResult");
				defaultInterpolatedStringHandler.AppendLiteral(" player ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong?>(this._playerId);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			case PlayerChoiceType.Index:
				return "PlayerChoiceResult indexes " + string.Join<int>(",", this._indexes ?? new List<int>());
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04002B06 RID: 11014
		[Nullable(new byte[] { 2, 1 })]
		private List<CardModel> _canonicalCards;

		// Token: 0x04002B07 RID: 11015
		[Nullable(new byte[] { 2, 1 })]
		private List<CardModel> _combatCards;

		// Token: 0x04002B08 RID: 11016
		[Nullable(new byte[] { 2, 1 })]
		private List<CardModel> _deckCards;

		// Token: 0x04002B09 RID: 11017
		[Nullable(new byte[] { 2, 1 })]
		private List<CardModel> _mutableCards;

		// Token: 0x04002B0A RID: 11018
		[Nullable(2)]
		private List<int> _indexes;

		// Token: 0x04002B0B RID: 11019
		private ulong? _playerId;

		// Token: 0x02002362 RID: 9058
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040095FF RID: 38399
			[Nullable(0)]
			public static Func<SerializableCard, CardModel> <0>__FromSerializable;

			// Token: 0x04009600 RID: 38400
			[Nullable(0)]
			public static Func<CardModel, NetCombatCard> <1>__FromModel;

			// Token: 0x04009601 RID: 38401
			[Nullable(0)]
			public static Func<CardModel, NetDeckCard> <2>__FromModel;
		}
	}
}
