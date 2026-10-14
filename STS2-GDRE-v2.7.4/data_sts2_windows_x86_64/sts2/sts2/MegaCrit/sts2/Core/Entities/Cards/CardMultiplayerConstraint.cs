using System;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	/// <summary>
	/// Manages card restrictions based on how many players there are in a run.
	/// </summary>
	// Token: 0x02000CAF RID: 3247
	public enum CardMultiplayerConstraint
	{
		// Token: 0x04002D8F RID: 11663
		None,
		// Token: 0x04002D90 RID: 11664
		MultiplayerOnly,
		// Token: 0x04002D91 RID: 11665
		SingleplayerOnly
	}
}
