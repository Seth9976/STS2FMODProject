using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Runs
{
	/// <summary>
	/// Options passed when creating cards for rewards.
	/// Various combinations of these options will do different things, so read carefully when you are using this for a
	/// new circumstance.
	/// </summary>
	// Token: 0x020000FF RID: 255
	[NullableContext(1)]
	[Nullable(0)]
	public class CardCreationOptions : IEquatable<CardCreationOptions>
	{
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0004BEBD File Offset: 0x0004A0BD
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(CardCreationOptions);
			}
		}

		/// <summary>
		/// The card pools from which rewards will be pulled.
		/// </summary>
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x0004BEC9 File Offset: 0x0004A0C9
		public IReadOnlyCollection<CardPoolModel> CardPools
		{
			get
			{
				return this._cardPools;
			}
		}

		/// <summary>
		/// The filter to apply to the card pool when generating rewards.
		/// We use this instead of a custom card pool because it allows effects that modify the available card pools (like
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Relics.PrismaticGem" />) to properly apply the same filter.
		/// WARNING: This cannot be serialized, so don't use this when adding extra rewards to combat rewards.
		/// </summary>
		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0004BED1 File Offset: 0x0004A0D1
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x0004BED9 File Offset: 0x0004A0D9
		[Nullable(new byte[] { 2, 1 })]
		public Func<CardModel, bool> CardPoolFilter
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			private set;
		}

		/// <summary>
		/// The source from which this reward is being generated from.
		/// Used to indicate to modification hooks whether they should apply to this reward or not. For example, some relics
		/// specify that they only apply to encounter rewards, and not to rewards from events or shops.
		/// </summary>
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0004BEE2 File Offset: 0x0004A0E2
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x0004BEEA File Offset: 0x0004A0EA
		public CardCreationSource Source { get; private set; }

		/// <summary>
		/// The odds to use when generating the rarity of the cards in the reward.
		/// Hooks should look at the source to determine where the cards come from, not this.
		/// </summary>
		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0004BEF3 File Offset: 0x0004A0F3
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x0004BEFB File Offset: 0x0004A0FB
		public CardRarityOddsType RarityOdds { get; private set; }

		/// <summary>
		/// Restrictions applied when determining what can modify the generated rewards.
		/// </summary>
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0004BF04 File Offset: 0x0004A104
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x0004BF0C File Offset: 0x0004A10C
		public CardCreationFlags Flags { get; private set; }

		/// <summary>
		/// If set, we use this override over the default one in the factory
		/// Used in the Crystal Sphere Items so that we can pass the event rng to be used and not hit state divergences in multiplayer
		/// since we don't sync the player rng sets in that case
		/// </summary>
		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x0004BF15 File Offset: 0x0004A115
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x0004BF1D File Offset: 0x0004A11D
		[Nullable(2)]
		public Rng RngOverride
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		/// <summary>
		/// Constructor which takes a card pool model.
		/// </summary>
		/// <param name="cardPools">The card pools from which rewards will be generated.</param>
		/// <param name="source">The purpose for which the rewards are being generated.</param>
		/// <param name="rarityOdds">The odds to apply when rolling card rarities.</param>
		/// <param name="cardPoolFilter">A filter to apply to the card pool when generating rewards.</param>
		// Token: 0x06000B1F RID: 2847 RVA: 0x0004BF26 File Offset: 0x0004A126
		public CardCreationOptions(IEnumerable<CardPoolModel> cardPools, CardCreationSource source, CardRarityOddsType rarityOdds, [Nullable(new byte[] { 2, 1 })] Func<CardModel, bool> cardPoolFilter = null)
		{
			this._cardPools = new List<CardPoolModel>();
			base..ctor();
			this._cardPools.AddRange(cardPools);
			this.Source = source;
			this.RarityOdds = rarityOdds;
			this.CardPoolFilter = cardPoolFilter;
		}

		/// <summary>
		/// Creates a default instance of CardCreationOptions for use in generating end-of-combat rewards or shop rewards for
		/// the given room type.
		/// Do not use this in events or relics.
		/// </summary>
		// Token: 0x06000B20 RID: 2848 RVA: 0x0004BF5C File Offset: 0x0004A15C
		public static CardCreationOptions ForRoom(Player player, RoomType roomType)
		{
			CardCreationSource cardCreationSource;
			switch (roomType)
			{
			case RoomType.Monster:
			case RoomType.Elite:
			case RoomType.Boss:
				cardCreationSource = CardCreationSource.Encounter;
				goto IL_0037;
			case RoomType.Shop:
				cardCreationSource = CardCreationSource.Shop;
				goto IL_0037;
			case RoomType.Event:
				throw new InvalidOperationException("ForRoom should not be used in event rooms");
			}
			cardCreationSource = CardCreationSource.Other;
			IL_0037:
			CardCreationSource cardCreationSource2 = cardCreationSource;
			CardRarityOddsType cardRarityOddsType;
			switch (roomType)
			{
			case RoomType.Monster:
				cardRarityOddsType = CardRarityOddsType.RegularEncounter;
				goto IL_0069;
			case RoomType.Elite:
				cardRarityOddsType = CardRarityOddsType.EliteEncounter;
				goto IL_0069;
			case RoomType.Boss:
				cardRarityOddsType = CardRarityOddsType.BossEncounter;
				goto IL_0069;
			case RoomType.Shop:
				cardRarityOddsType = CardRarityOddsType.Shop;
				goto IL_0069;
			}
			cardRarityOddsType = CardRarityOddsType.RegularEncounter;
			IL_0069:
			CardRarityOddsType cardRarityOddsType2 = cardRarityOddsType;
			return new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool), cardCreationSource2, cardRarityOddsType2, null);
		}

		/// <summary>
		/// Generates an instance of CardCreationOptions which can be used when generating rewards for events or relics.
		/// This uses CardRarityOddsType.RegularEncounter to generate card rarities, meaning that common cards will be far
		/// more likely to show up than rare ones.
		/// </summary>
		// Token: 0x06000B21 RID: 2849 RVA: 0x0004BFEC File Offset: 0x0004A1EC
		public static CardCreationOptions ForNonCombatWithDefaultOdds(IEnumerable<CardPoolModel> cardPools, [Nullable(new byte[] { 2, 1 })] Func<CardModel, bool> cardPoolFilter = null)
		{
			return new CardCreationOptions(cardPools, CardCreationSource.Other, CardRarityOddsType.RegularEncounter, cardPoolFilter).WithFlags(CardCreationFlags.NoUpgradeRoll);
		}

		/// <summary>
		/// Generates an instance of CardCreationOptions which can be used when generating rewards for events or relics.
		/// This uses CardRarityOddsType.Uniform to generate card rarities, meaning that all cards in the pool will have an
		/// even chance of being rolled.
		/// </summary>
		// Token: 0x06000B22 RID: 2850 RVA: 0x0004BFFD File Offset: 0x0004A1FD
		public static CardCreationOptions ForNonCombatWithUniformOdds(IEnumerable<CardPoolModel> cardPools, [Nullable(new byte[] { 2, 1 })] Func<CardModel, bool> cardPoolFilter = null)
		{
			return new CardCreationOptions(cardPools, CardCreationSource.Other, CardRarityOddsType.Uniform, cardPoolFilter).WithFlags(CardCreationFlags.NoUpgradeRoll);
		}

		/// <summary>
		/// Get all the cards that could be created for the specified player based on this set of options.
		/// </summary>
		// Token: 0x06000B23 RID: 2851 RVA: 0x0004C010 File Offset: 0x0004A210
		public IEnumerable<CardModel> GetPossibleCards(Player player)
		{
			return from c in this.CardPools.SelectMany<CardPoolModel, CardModel>((CardPoolModel p) => p.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint))
				where this.CardPoolFilter == null || this.CardPoolFilter(c)
				select c;
		}

		/// <summary>
		/// Sets the card pool on this instance of CardCreationOptions.
		/// </summary>
		// Token: 0x06000B24 RID: 2852 RVA: 0x0004C05C File Offset: 0x0004A25C
		public CardCreationOptions WithCardPools(IEnumerable<CardPoolModel> pools)
		{
			List<CardPoolModel> list = pools.ToList<CardPoolModel>();
			this._cardPools.Clear();
			this._cardPools.AddRange(list);
			return this;
		}

		/// <summary>
		/// Sets the card pool filter on this instance of CardCreationOptions.
		/// </summary>
		// Token: 0x06000B25 RID: 2853 RVA: 0x0004C088 File Offset: 0x0004A288
		public CardCreationOptions WithFilter(Func<CardModel, bool> filter)
		{
			this.CardPoolFilter = filter;
			return this;
		}

		/// <summary>
		/// Sets flags on this instance of CardCreationOptions.
		/// </summary>
		// Token: 0x06000B26 RID: 2854 RVA: 0x0004C092 File Offset: 0x0004A292
		public CardCreationOptions WithFlags(CardCreationFlags flag)
		{
			this.Flags |= flag;
			return this;
		}

		/// <summary>
		/// Sets the rarity odds on this instance of CardCreationOptions.
		/// </summary>
		// Token: 0x06000B27 RID: 2855 RVA: 0x0004C0A3 File Offset: 0x0004A2A3
		public CardCreationOptions WithRarityOdds(CardRarityOddsType rarityOdds)
		{
			this.RarityOdds = rarityOdds;
			return this;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0004C0AD File Offset: 0x0004A2AD
		public CardCreationOptions WithRngOverride(Rng rng)
		{
			this.RngOverride = rng;
			return this;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0004C0B8 File Offset: 0x0004A2B8
		public CardRarity? TryGetSingleRarityInPool()
		{
			List<CardModel> list = this.CardPools.SelectMany<CardPoolModel, CardModel>((CardPoolModel c) => c.AllCards).ToList<CardModel>();
			if (this.CardPoolFilter != null)
			{
				list = list.Where<CardModel>((CardModel c) => this.CardPoolFilter(c)).ToList<CardModel>();
			}
			CardModel first = list.FirstOrDefault<CardModel>();
			if (first != null && list.All<CardModel>((CardModel c) => c.Rarity == first.Rarity))
			{
				return new CardRarity?(first.Rarity);
			}
			return null;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0004C164 File Offset: 0x0004A364
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CardCreationOptions");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0004C1B0 File Offset: 0x0004A3B0
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("CardPools = ");
			builder.Append(this.CardPools);
			builder.Append(", CardPoolFilter = ");
			builder.Append(this.CardPoolFilter);
			builder.Append(", Source = ");
			builder.Append(this.Source.ToString());
			builder.Append(", RarityOdds = ");
			builder.Append(this.RarityOdds.ToString());
			builder.Append(", Flags = ");
			builder.Append(this.Flags.ToString());
			builder.Append(", RngOverride = ");
			builder.Append(this.RngOverride);
			return true;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0004C283 File Offset: 0x0004A483
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(CardCreationOptions left, CardCreationOptions right)
		{
			return !(left == right);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0004C28F File Offset: 0x0004A48F
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(CardCreationOptions left, CardCreationOptions right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0004C2A4 File Offset: 0x0004A4A4
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<List<CardPoolModel>>.Default.GetHashCode(this._cardPools)) * -1521134295 + EqualityComparer<Func<CardModel, bool>>.Default.GetHashCode(this.<CardPoolFilter>k__BackingField)) * -1521134295 + EqualityComparer<CardCreationSource>.Default.GetHashCode(this.<Source>k__BackingField)) * -1521134295 + EqualityComparer<CardRarityOddsType>.Default.GetHashCode(this.<RarityOdds>k__BackingField)) * -1521134295 + EqualityComparer<CardCreationFlags>.Default.GetHashCode(this.<Flags>k__BackingField)) * -1521134295 + EqualityComparer<Rng>.Default.GetHashCode(this.<RngOverride>k__BackingField);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0004C34B File Offset: 0x0004A54B
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as CardCreationOptions);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0004C35C File Offset: 0x0004A55C
		[NullableContext(2)]
		[CompilerGenerated]
		public virtual bool Equals(CardCreationOptions other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<List<CardPoolModel>>.Default.Equals(this._cardPools, other._cardPools) && EqualityComparer<Func<CardModel, bool>>.Default.Equals(this.<CardPoolFilter>k__BackingField, other.<CardPoolFilter>k__BackingField) && EqualityComparer<CardCreationSource>.Default.Equals(this.<Source>k__BackingField, other.<Source>k__BackingField) && EqualityComparer<CardRarityOddsType>.Default.Equals(this.<RarityOdds>k__BackingField, other.<RarityOdds>k__BackingField) && EqualityComparer<CardCreationFlags>.Default.Equals(this.<Flags>k__BackingField, other.<Flags>k__BackingField) && EqualityComparer<Rng>.Default.Equals(this.<RngOverride>k__BackingField, other.<RngOverride>k__BackingField));
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0004C428 File Offset: 0x0004A628
		[CompilerGenerated]
		protected CardCreationOptions(CardCreationOptions original)
		{
			this._cardPools = original._cardPools;
			this.CardPoolFilter = original.<CardPoolFilter>k__BackingField;
			this.Source = original.<Source>k__BackingField;
			this.RarityOdds = original.<RarityOdds>k__BackingField;
			this.Flags = original.<Flags>k__BackingField;
			this.RngOverride = original.<RngOverride>k__BackingField;
		}

		// Token: 0x040003E4 RID: 996
		private readonly List<CardPoolModel> _cardPools;
	}
}
