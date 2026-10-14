using System;

namespace MegaCrit.Sts2.Core.Entities.Merchant
{
	// Token: 0x02000C9B RID: 3227
	public enum PurchaseStatus
	{
		/// <summary>
		/// The purchase was successful.
		/// </summary>
		// Token: 0x04002D10 RID: 11536
		Success,
		/// <summary>
		/// The purchase failed due to lack of gold.
		/// </summary>
		// Token: 0x04002D11 RID: 11537
		FailureGold,
		/// <summary>
		/// The purchase failed due to lack of space in the player's inventory.
		/// </summary>
		// Token: 0x04002D12 RID: 11538
		FailureSpace,
		/// <summary>
		/// The purchase failed because some external effect prevented it (like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.Sozu" /> for a potion purchase).
		/// </summary>
		// Token: 0x04002D13 RID: 11539
		FailureForbidden,
		/// <summary>
		/// The purchase failed because the Merchant does not have this slot stocked.
		/// </summary>
		// Token: 0x04002D14 RID: 11540
		FailureOutOfStock
	}
}
