using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Modding
{
	/// <summary>
	/// Information about a loaded mod.
	/// </summary>
	// Token: 0x02000B90 RID: 2960
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class Mod
	{
		// Token: 0x06007BB7 RID: 31671 RVA: 0x002A1354 File Offset: 0x0029F554
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public Mod()
		{
		}

		/// <summary>
		/// Where the mod originated from.
		/// </summary>
		// Token: 0x04002916 RID: 10518
		public ModSource modSource;

		/// <summary>
		/// Path where the mod files are located.
		/// </summary>
		// Token: 0x04002917 RID: 10519
		[RequiredMember]
		public string path;

		/// <summary>
		/// Whether the mod was loaded, and if it was not loaded, why.
		/// Since there's no way to unload mods while the game is running, this value cannot change after the initial mod
		/// initialization occurs.
		/// Even if the mod is set to disabled in SettingsSave, this value is the true source of whether or not the mod was
		/// loaded into the game.
		/// </summary>
		// Token: 0x04002918 RID: 10520
		public ModLoadState state;

		/// <summary>
		/// The mod manifest.
		/// </summary>
		// Token: 0x04002919 RID: 10521
		[Nullable(2)]
		public ModManifest manifest;

		/// <summary>
		/// The version parsed from the mod manifest.
		/// Null if the version is not present or is not a valid semantic version.
		/// </summary>
		// Token: 0x0400291A RID: 10522
		[Nullable(2)]
		public SemanticVersion version;

		/// <summary>
		/// The C# assemblies loaded with the mod, if any.
		/// There are usually only zero or one in the list. Mods can register more assemblies, e.g. in the situation where
		/// they create dynamic assemblies.
		/// </summary>
		// Token: 0x0400291B RID: 10523
		public List<Assembly> assemblies = new List<Assembly>();

		/// <summary>
		/// If null, then no errors occurred while loading the mod.
		/// If this is set, then there was an error loading the mod that should be displayed. That doesn't necessarily mean
		/// the mod failed to load; see <see cref="F:MegaCrit.Sts2.Core.Modding.Mod.state" /> for that.
		/// </summary>
		// Token: 0x0400291C RID: 10524
		[Nullable(new byte[] { 2, 1 })]
		public List<LocString> errors;

		/// <summary>
		/// The ID of the workshop item, if the mod source is Steam Workshop.
		/// This can be used as the value of a PublishedFileId_t, but we use ulong here so that we don't have to guard this
		/// with DISABLESTEAMWORKS.
		/// </summary>
		// Token: 0x0400291D RID: 10525
		public ulong? workshopId;
	}
}
