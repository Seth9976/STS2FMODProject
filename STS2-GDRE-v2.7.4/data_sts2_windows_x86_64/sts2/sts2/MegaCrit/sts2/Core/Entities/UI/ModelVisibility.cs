using System;

namespace MegaCrit.Sts2.Core.Entities.UI
{
	/// <summary>
	/// Represents how visible a piece of content (RelicModel, PotionModel) is to the player.
	/// </summary>
	// Token: 0x02000C59 RID: 3161
	public enum ModelVisibility
	{
		/// <summary>
		/// Invalid sentinel value.
		/// </summary>
		// Token: 0x04002BAC RID: 11180
		None,
		/// <summary>
		/// The content is both unlocked and has been seen by the player in a run.
		/// </summary>
		// Token: 0x04002BAD RID: 11181
		Visible,
		/// <summary>
		/// The content has been unlocked, but has not yet been seen by the player in a run.
		/// </summary>
		// Token: 0x04002BAE RID: 11182
		NotSeen,
		/// <summary>
		/// The content is locked (behind an epoch) and cannot be seen.
		/// </summary>
		// Token: 0x04002BAF RID: 11183
		Locked
	}
}
