using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	/// <summary>
	/// Represents a single instance of a card being played.
	/// This is important for distinguishing between multiple plays of the same card during the same turn.
	/// </summary>
	// Token: 0x02000CB3 RID: 3251
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class CardPlay
	{
		/// <summary>
		/// The card that was played.
		/// </summary>
		// Token: 0x1700243E RID: 9278
		// (get) Token: 0x060084BD RID: 33981 RVA: 0x002CABE6 File Offset: 0x002C8DE6
		// (set) Token: 0x060084BE RID: 33982 RVA: 0x002CABEE File Offset: 0x002C8DEE
		[RequiredMember]
		public CardModel Card { get; set; }

		/// <summary>
		/// The player that played the card.
		/// Note: This may be different from Card.Owner, since some cards (like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.TheBall" />) can change owners
		/// after being played.
		/// </summary>
		// Token: 0x1700243F RID: 9279
		// (get) Token: 0x060084BF RID: 33983 RVA: 0x002CABF7 File Offset: 0x002C8DF7
		// (set) Token: 0x060084C0 RID: 33984 RVA: 0x002CABFF File Offset: 0x002C8DFF
		[RequiredMember]
		public Player Player { get; set; }

		/// <summary>
		/// The creature that was targeted by the card.
		/// Null for un-targeted cards.
		/// </summary>
		// Token: 0x17002440 RID: 9280
		// (get) Token: 0x060084C1 RID: 33985 RVA: 0x002CAC08 File Offset: 0x002C8E08
		// (set) Token: 0x060084C2 RID: 33986 RVA: 0x002CAC10 File Offset: 0x002C8E10
		[Nullable(2)]
		[RequiredMember]
		public Creature Target
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		/// <summary>
		/// The pile that the card should be put in after this play is over.
		/// </summary>
		// Token: 0x17002441 RID: 9281
		// (get) Token: 0x060084C3 RID: 33987 RVA: 0x002CAC19 File Offset: 0x002C8E19
		// (set) Token: 0x060084C4 RID: 33988 RVA: 0x002CAC21 File Offset: 0x002C8E21
		[RequiredMember]
		public PileType ResultPile { get; set; }

		/// <summary>
		/// Info about the resources used during this card play.
		/// </summary>
		// Token: 0x17002442 RID: 9282
		// (get) Token: 0x060084C5 RID: 33989 RVA: 0x002CAC2A File Offset: 0x002C8E2A
		// (set) Token: 0x060084C6 RID: 33990 RVA: 0x002CAC32 File Offset: 0x002C8E32
		[RequiredMember]
		public ResourceInfo Resources { get; set; }

		/// <summary>
		/// Whether this card is being auto-played.
		/// False when the player plays the card manually from their hand.
		/// True when played automatically by an effect like <see cref="T:MegaCrit.Sts2.Core.Models.Powers.MayhemPower" />.
		/// </summary>
		// Token: 0x17002443 RID: 9283
		// (get) Token: 0x060084C7 RID: 33991 RVA: 0x002CAC3B File Offset: 0x002C8E3B
		// (set) Token: 0x060084C8 RID: 33992 RVA: 0x002CAC43 File Offset: 0x002C8E43
		[RequiredMember]
		public bool IsAutoPlay { get; set; }

		/// <summary>
		/// The index of the current card play relative to playCount. Usually 0, but when a card has an effect like Replay,
		/// the second play will have playIndex=1, the third will have playIndex=2, etc.
		/// </summary>
		// Token: 0x17002444 RID: 9284
		// (get) Token: 0x060084C9 RID: 33993 RVA: 0x002CAC4C File Offset: 0x002C8E4C
		// (set) Token: 0x060084CA RID: 33994 RVA: 0x002CAC54 File Offset: 0x002C8E54
		[RequiredMember]
		public int PlayIndex { get; set; }

		/// <summary>
		/// The total number of times this card is going to be played. Usually 1, but when a card has an effect like Replay,
		/// it will be higher (Replay 1 will have playCount=2, Replay 2 will have playCount=3, etc.).
		/// You can compare this with playCount to see if you're on the final play of the card.
		/// </summary>
		// Token: 0x17002445 RID: 9285
		// (get) Token: 0x060084CB RID: 33995 RVA: 0x002CAC5D File Offset: 0x002C8E5D
		// (set) Token: 0x060084CC RID: 33996 RVA: 0x002CAC65 File Offset: 0x002C8E65
		[RequiredMember]
		public int PlayCount { get; set; }

		/// <summary>
		/// Is this the first card play in the series?
		/// Usually true, but when a card has an effect like Replay, this will only be true on the card's first play.
		/// For example, if a card has Replay 2, this will be true for its first CardPlay and false for the last two.
		/// </summary>
		// Token: 0x17002446 RID: 9286
		// (get) Token: 0x060084CD RID: 33997 RVA: 0x002CAC6E File Offset: 0x002C8E6E
		public bool IsFirstInSeries
		{
			get
			{
				return this.PlayIndex == 0;
			}
		}

		/// <summary>
		/// Is this the last card play in the series?
		/// Usually true, but when a card has an effect like Replay, this will only be true on the card's last play.
		/// For example, if a card has Replay 2, this will be false for its first two CardPlays and true for the third.
		/// </summary>
		// Token: 0x17002447 RID: 9287
		// (get) Token: 0x060084CE RID: 33998 RVA: 0x002CAC79 File Offset: 0x002C8E79
		public bool IsLastInSeries
		{
			get
			{
				return this.PlayIndex == this.PlayCount - 1;
			}
		}

		// Token: 0x060084CF RID: 33999 RVA: 0x002CAC8B File Offset: 0x002C8E8B
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public CardPlay()
		{
		}
	}
}
