using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations.ProgressSaves
{
	/// <summary>
	/// No-op migration for release save wipe. All pre-release saves are invalidated
	/// via minimum supported version bump.
	/// </summary>
	// Token: 0x020000F0 RID: 240
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SerializableProgress), 20, 21)]
	public class ProgressSaveV20ToV21 : MigrationBase<SerializableProgress>
	{
		// Token: 0x06000AA8 RID: 2728 RVA: 0x00049422 File Offset: 0x00047622
		protected override void ApplyMigration(MigratingData saveData)
		{
		}
	}
}
