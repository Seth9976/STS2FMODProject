using System;

namespace MegaCrit.Sts2.Core.Entities.Rewards
{
	// Token: 0x02000C64 RID: 3172
	public enum PostAlternateCardRewardAction
	{
		// Token: 0x04002BDF RID: 11231
		None,
		/// <summary>
		/// After the alternate reward button is pressed, end card selection, but don't complete it - the player may
		/// re-enter card selection.
		/// Used for the skip button.
		/// </summary>
		// Token: 0x04002BE0 RID: 11232
		EndSelectionAndDoNotCompleteReward,
		/// <summary>
		/// After the alternate reward button is pressed, end card selection and complete the card reward - the player may
		/// not go back and select the reward.
		/// Used for the Sacrifice button.
		/// </summary>
		// Token: 0x04002BE1 RID: 11233
		EndSelectionAndCompleteReward,
		/// <summary>
		/// After the alternate reward button is pressed, nothing happens. The reward screen stays up and the player is
		/// allowed to select a reward.
		/// Used when card rewards are rerolled.
		/// </summary>
		// Token: 0x04002BE2 RID: 11234
		DoNothing
	}
}
