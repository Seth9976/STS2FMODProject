using System;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x02000100 RID: 256
	public enum CardCreationSource
	{
		// Token: 0x040003EB RID: 1003
		None,
		/// <summary>
		/// This card was created for a post-encounter card reward.
		/// </summary>
		// Token: 0x040003EC RID: 1004
		Encounter,
		/// <summary>
		/// This card was created to be shown in a shop.
		/// </summary>
		// Token: 0x040003ED RID: 1005
		Shop,
		/// <summary>
		/// This card was created for a reward generated from an event or relic.
		/// </summary>
		// Token: 0x040003EE RID: 1006
		Other
	}
}
