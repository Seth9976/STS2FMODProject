using System;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	/// <summary>
	/// Used for determining which Card Pile. Useful when flying cards around, knowing if a card is in your Hand or
	/// in the Discard Pile and things like that.
	/// </summary>
	// Token: 0x02000CC4 RID: 3268
	public enum PileType
	{
		// Token: 0x04002DF1 RID: 11761
		None,
		/// <summary>
		/// Where cards are drawn from.
		/// </summary>
		// Token: 0x04002DF2 RID: 11762
		Draw,
		/// <summary>
		/// Where cards are manually played from.
		/// </summary>
		// Token: 0x04002DF3 RID: 11763
		Hand,
		/// <summary>
		/// Where cards are discarded/flushed to.
		/// </summary>
		// Token: 0x04002DF4 RID: 11764
		Discard,
		/// <summary>
		/// Where cards are exhausted to.
		/// </summary>
		// Token: 0x04002DF5 RID: 11765
		Exhaust,
		/// <summary>
		/// A temporary pile that a card lives in while it's mid-play, so that it isn't counted towards your hand or your
		/// discard pile.
		/// </summary>
		// Token: 0x04002DF6 RID: 11766
		Play,
		/// <summary>
		/// Where cards live between rooms.
		/// When a new combat starts, all cards in here are cloned into your draw pile, so modifications to cards in combat
		/// won't modify cards in here.
		/// </summary>
		// Token: 0x04002DF7 RID: 11767
		Deck
	}
}
