using System;

namespace MegaCrit.Sts2.Core.Modding
{
	// Token: 0x02000B96 RID: 2966
	public enum ModLoadState
	{
		// Token: 0x04002925 RID: 10533
		None,
		/// <summary>
		/// Mod was successfully loaded.
		/// </summary>
		// Token: 0x04002926 RID: 10534
		Loaded,
		/// <summary>
		/// We tried to load the mod but it failed to load.
		/// </summary>
		// Token: 0x04002927 RID: 10535
		Failed,
		/// <summary>
		/// We didn't try to load the mod because the user has not enabled mod loading, or this specific mod is marked as
		/// disabled in SettingsSave.
		/// </summary>
		// Token: 0x04002928 RID: 10536
		Disabled,
		/// <summary>
		/// We didn't try to load the mod because the user has this mod loaded both via Steam and local mods directory.
		/// </summary>
		// Token: 0x04002929 RID: 10537
		DisabledDuplicate,
		/// <summary>
		/// We didn't try to load the mod because it was added too late.
		/// </summary>
		// Token: 0x0400292A RID: 10538
		AddedAtRuntime
	}
}
