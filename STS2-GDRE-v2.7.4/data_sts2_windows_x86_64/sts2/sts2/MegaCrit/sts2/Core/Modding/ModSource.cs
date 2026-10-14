using System;

namespace MegaCrit.Sts2.Core.Modding
{
	/// <summary>
	/// Where a mod was loaded from.
	/// </summary>
	// Token: 0x02000B9C RID: 2972
	public enum ModSource
	{
		// Token: 0x04002949 RID: 10569
		None,
		/// <summary>
		/// The mod is located in the mods directory next to the executable.
		/// </summary>
		// Token: 0x0400294A RID: 10570
		ModsDirectory,
		/// <summary>
		/// The mod was loaded from Steam Workshop.
		/// </summary>
		// Token: 0x0400294B RID: 10571
		SteamWorkshop
	}
}
