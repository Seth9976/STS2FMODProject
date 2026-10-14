using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Enchantments
{
	/// <summary>
	/// Utility object representing a random enchantment that may be applied to a card.
	/// </summary>
	// Token: 0x02000C9F RID: 3231
	[NullableContext(1)]
	[Nullable(0)]
	public struct EnchantmentOption
	{
		/// <summary>
		/// Utility object representing a random enchantment that may be applied to a card.
		/// </summary>
		// Token: 0x060083F9 RID: 33785 RVA: 0x002C8867 File Offset: 0x002C6A67
		public EnchantmentOption(EnchantmentModel enchantment, int minAmount, int maxAmount)
		{
			this.enchantment = enchantment;
			this.minAmount = minAmount;
			this.maxAmount = maxAmount;
		}

		// Token: 0x04002D3E RID: 11582
		public readonly EnchantmentModel enchantment;

		// Token: 0x04002D3F RID: 11583
		public readonly int minAmount;

		// Token: 0x04002D40 RID: 11584
		public readonly int maxAmount;
	}
}
