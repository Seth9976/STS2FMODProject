using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations.PrefsSaves
{
	/// <summary>
	/// No-op migration for release save wipe. All pre-release saves are invalidated
	/// via minimum supported version bump.
	/// </summary>
	// Token: 0x020000F3 RID: 243
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(PrefsSave), 1, 2)]
	public class PrefsSaveV1ToV2 : MigrationBase<PrefsSave>
	{
		// Token: 0x06000AAE RID: 2734 RVA: 0x00049552 File Offset: 0x00047752
		protected override void ApplyMigration(MigratingData saveData)
		{
		}
	}
}
