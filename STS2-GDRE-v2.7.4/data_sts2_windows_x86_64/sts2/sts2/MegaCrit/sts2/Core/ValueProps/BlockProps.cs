using System;

namespace MegaCrit.Sts2.Core.ValueProps
{
	// Token: 0x02000030 RID: 48
	public static class BlockProps
	{
		/// <summary>
		/// Normal block granted by a card.
		/// </summary>
		// Token: 0x040000D1 RID: 209
		public const ValueProp card = ValueProp.Move;

		/// <summary>
		/// Unpowered block granted by a card.
		/// Example: <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Entrench" /> ("Double your Block") should not pick up extra block from <see cref="T:MegaCrit.Sts2.Core.Models.Powers.DexterityPower" />.
		/// </summary>
		// Token: 0x040000D2 RID: 210
		public const ValueProp cardUnpowered = ValueProp.Unpowered | ValueProp.Move;

		/// <summary>
		/// Normal block granted by a monster move.
		/// </summary>
		// Token: 0x040000D3 RID: 211
		public const ValueProp monsterMove = ValueProp.Move;

		/// <summary>
		/// Unpowered block granted by a non-card source.
		/// Example: The <see cref="T:MegaCrit.Sts2.Core.Models.Powers.PlatingPower" /> power ("At the end of your turn, gain X Block") should not pick up extra
		/// block from <see cref="T:MegaCrit.Sts2.Core.Models.Powers.DexterityPower" />.
		/// </summary>
		// Token: 0x040000D4 RID: 212
		public const ValueProp nonCardUnpowered = ValueProp.Unpowered;
	}
}
