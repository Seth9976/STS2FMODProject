using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.SourceGeneration;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// Base interface for all migrations.
	/// </summary>
	// Token: 0x020000D6 RID: 214
	[NullableContext(1)]
	[GenerateSubtypes(DynamicallyAccessedMemberTypes = DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
	public interface IMigration
	{
		/// <summary>
		/// The version to migrate from.
		/// </summary>
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000A3B RID: 2619
		int FromVersion { get; }

		/// <summary>
		/// The version to migrate to.
		/// </summary>
		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000A3C RID: 2620
		int ToVersion { get; }

		/// <summary>
		/// The type of save that this migration operates on.
		/// </summary>
		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000A3D RID: 2621
		Type SaveType { get; }

		/// <summary>
		/// Migrates the JsonObject from the old version to the new version.
		/// </summary>
		/// <param name="saveData">The JsonObject to migrate</param>
		/// <returns>The migrated JsonObject</returns>
		// Token: 0x06000A3E RID: 2622
		MigratingData Migrate(MigratingData saveData);
	}
}
