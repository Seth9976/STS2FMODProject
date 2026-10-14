using System;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	/// <summary>
	/// Card Keywords are extra "automatic behaviors" you can add to a card, which also automatically add some extra
	/// text to the card's description.
	///
	/// Examples of things that SHOULD be CardKeywords:
	/// * Exhaust: automatically exhausts when played, puts "Exhaust" in the card description.
	/// * Unplayable: blocks the card from being played, puts "Unplayable" in the card description.
	///
	/// Examples of things that should NOT be CardKeywords:
	/// * Strike: this has no logic or extra text, just tells Perfected Strike (and some other models) that it's a
	///     Strike card.
	/// * Block: while this is used on cards that have "Block" in their descriptions, it doesn't automatically add
	///     it, nor does it automatically grant block; it's just for HoverTips and effects that care about which cards
	///     give block (like the Nimble enchantment, which should only apply to cards that give block).
	/// </summary>
	// Token: 0x02000CAB RID: 3243
	public enum CardKeyword
	{
		// Token: 0x04002D80 RID: 11648
		None,
		// Token: 0x04002D81 RID: 11649
		Exhaust,
		// Token: 0x04002D82 RID: 11650
		Ethereal,
		// Token: 0x04002D83 RID: 11651
		Innate,
		// Token: 0x04002D84 RID: 11652
		Unplayable,
		// Token: 0x04002D85 RID: 11653
		Retain,
		// Token: 0x04002D86 RID: 11654
		Sly,
		// Token: 0x04002D87 RID: 11655
		Eternal
	}
}
