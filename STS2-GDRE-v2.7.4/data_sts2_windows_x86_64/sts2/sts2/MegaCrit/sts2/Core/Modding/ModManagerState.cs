using System;

namespace MegaCrit.Sts2.Core.Modding
{
	// Token: 0x02000B99 RID: 2969
	public enum ModManagerState
	{
		/// <summary>
		/// ModManager is not yet initialized.
		/// </summary>
		// Token: 0x04002939 RID: 10553
		None,
		/// <summary>
		/// ModManager is fully initialized. It may or may not have loaded mods.
		/// </summary>
		// Token: 0x0400293A RID: 10554
		Initialized,
		/// <summary>
		/// ModManager skipped initialization for some reason:
		///  - We're in test mode
		///  - This is an AOT build
		///  - nomods command line argument was passed
		/// </summary>
		// Token: 0x0400293B RID: 10555
		Skipped
	}
}
