using System;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	// Token: 0x02000258 RID: 600
	public enum EpochSlotState
	{
		// Token: 0x04000C0E RID: 3086
		None,
		/// <summary>
		/// The Epoch is revealed and the player has inspected it before.
		/// </summary>
		// Token: 0x04000C0F RID: 3087
		Complete,
		/// <summary>
		/// The Epoch is obtained so the player is able to click on the slot in the Timeline to reveal/complete it!
		/// </summary>
		// Token: 0x04000C10 RID: 3088
		Obtained,
		/// <summary>
		/// This Epoch has never been obtained. Used to show empty slots, can be useful to communicate how to obtain the Epoch.
		/// </summary>
		// Token: 0x04000C11 RID: 3089
		NotObtained
	}
}
