using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations.SettingsSaves
{
	/// <summary>
	/// No-op migration for release save wipe. All pre-release saves are invalidated
	/// via minimum supported version bump.
	/// </summary>
	// Token: 0x020000E4 RID: 228
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SettingsSave), 3, 4)]
	public class SettingsSaveV3ToV4 : MigrationBase<SettingsSave>
	{
		// Token: 0x06000A8D RID: 2701 RVA: 0x000486D4 File Offset: 0x000468D4
		protected override void ApplyMigration(MigratingData saveData)
		{
		}
	}
}
