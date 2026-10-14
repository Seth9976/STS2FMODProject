using System;

namespace MegaCrit.Sts2.Core.Entities.Ancients
{
	/// <summary>
	/// Who should attack during The Architect dialogue?
	/// </summary>
	// Token: 0x02000CD4 RID: 3284
	public enum ArchitectAttackers
	{
		/// <summary>
		/// No attacks should occur during the specified portion of the dialogue.
		/// </summary>
		// Token: 0x04002E3A RID: 11834
		None,
		/// <summary>
		/// The Player attacks.
		/// </summary>
		// Token: 0x04002E3B RID: 11835
		Player,
		/// <summary>
		/// The Architect attacks.
		/// </summary>
		// Token: 0x04002E3C RID: 11836
		Architect,
		/// <summary>
		/// The player attacks, then the Architect immediately attacks back.
		/// </summary>
		// Token: 0x04002E3D RID: 11837
		Both
	}
}
