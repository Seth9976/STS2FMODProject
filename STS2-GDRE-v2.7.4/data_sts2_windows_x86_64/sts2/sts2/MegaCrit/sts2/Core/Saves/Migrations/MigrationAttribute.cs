using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// Attribute used to mark a class as a migration.
	/// </summary>
	// Token: 0x020000D9 RID: 217
	[NullableContext(1)]
	[Nullable(0)]
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class MigrationAttribute : Attribute
	{
		/// <summary>
		/// The type of save that this migration operates on.
		/// </summary>
		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x000462C0 File Offset: 0x000444C0
		public Type SaveType { get; }

		/// <summary>
		/// The version to migrate from.
		/// </summary>
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x000462C8 File Offset: 0x000444C8
		public int FromVersion { get; }

		/// <summary>
		/// The version to migrate to.
		/// </summary>
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x000462D0 File Offset: 0x000444D0
		public int ToVersion { get; }

		/// <summary>
		/// Creates a new migration attribute.
		/// </summary>
		/// <param name="saveType">The type of save that this migration operates on</param>
		/// <param name="fromVersion">The version to migrate from</param>
		/// <param name="toVersion">The version to migrate to</param>
		// Token: 0x06000A57 RID: 2647 RVA: 0x000462D8 File Offset: 0x000444D8
		public MigrationAttribute(Type saveType, int fromVersion, int toVersion)
		{
			this.SaveType = saveType;
			this.FromVersion = fromVersion;
			this.ToVersion = toVersion;
		}
	}
}
