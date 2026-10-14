using System;

namespace MegaCrit.Sts2.Core.Entities.Gold
{
	/// <summary>
	/// Used to help flavor how the gold was lost in the Map History.
	/// </summary>
	// Token: 0x02000C9D RID: 3229
	public enum GoldLossType
	{
		// Token: 0x04002D29 RID: 11561
		None,
		/// <summary>
		/// Gold was spent in a transaction. Typically from a shop.
		/// </summary>
		// Token: 0x04002D2A RID: 11562
		Spent,
		/// <summary>
		/// Gold was lost without involving another entity (i.e. dropping it down a hole in an event).
		/// </summary>
		// Token: 0x04002D2B RID: 11563
		Lost,
		/// <summary>
		/// Gold was taken by someone (i.e. <see cref="T:MegaCrit.Sts2.Core.Models.Powers.ThieveryPower" />)
		/// </summary>
		// Token: 0x04002D2C RID: 11564
		Stolen
	}
}
