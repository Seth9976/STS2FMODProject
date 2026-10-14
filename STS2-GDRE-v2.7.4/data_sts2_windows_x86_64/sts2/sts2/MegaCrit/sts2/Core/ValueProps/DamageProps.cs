using System;

namespace MegaCrit.Sts2.Core.ValueProps
{
	// Token: 0x02000031 RID: 49
	public static class DamageProps
	{
		/// <summary>
		/// Normal damage done by a card.
		/// </summary>
		// Token: 0x040000D5 RID: 213
		public const ValueProp card = ValueProp.Move;

		/// <summary>
		/// Unpowered damage done by a card.
		/// Generally used by Curse/Status cards that do damage to the player.
		/// </summary>
		// Token: 0x040000D6 RID: 214
		public const ValueProp cardUnpowered = ValueProp.Unpowered | ValueProp.Move;

		/// <summary>
		/// HP loss from a card.
		/// Used by cards that have a "Lose X HP" drawback like Offering.
		/// </summary>
		// Token: 0x040000D7 RID: 215
		public const ValueProp cardHpLoss = ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move;

		/// <summary>
		/// Normal damage done by a monster move
		/// </summary>
		// Token: 0x040000D8 RID: 216
		public const ValueProp monsterMove = ValueProp.Move;

		/// <summary>
		/// Unpowered damage done by a source other than a card or monster move.
		/// Most commonly used by damaging powers whose damage should still be blockable, like Thorns.
		/// </summary>
		// Token: 0x040000D9 RID: 217
		public const ValueProp nonCardUnpowered = ValueProp.Unpowered;

		/// <summary>
		/// HP loss from a source other than a card or monster move.
		/// Most commonly used by damaging powers whose damage should be unblockable, like Poison.
		/// </summary>
		// Token: 0x040000DA RID: 218
		public const ValueProp nonCardHpLoss = ValueProp.Unblockable | ValueProp.Unpowered;
	}
}
