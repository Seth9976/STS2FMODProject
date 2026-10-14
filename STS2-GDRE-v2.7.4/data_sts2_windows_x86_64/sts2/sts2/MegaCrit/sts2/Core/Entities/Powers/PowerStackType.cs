using System;

namespace MegaCrit.Sts2.Core.Entities.Powers
{
	// Token: 0x02000C72 RID: 3186
	public enum PowerStackType
	{
		// Token: 0x04002BFF RID: 11263
		None,
		/// <summary>
		/// Amount is visible, and must be manually incremented/decremented via an action.
		/// </summary>
		// Token: 0x04002C00 RID: 11264
		Counter,
		/// <summary>
		/// Amount is hidden, and is always 1.
		/// </summary>
		// Token: 0x04002C01 RID: 11265
		Single
	}
}
