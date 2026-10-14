using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns
{
	/// <summary>
	/// No-op migration for release save wipe. All pre-release saves are invalidated
	/// via minimum supported version bump.
	/// </summary>
	// Token: 0x020000E7 RID: 231
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SerializableRun), 12, 13)]
	public class SerializableRunV12ToV13 : MigrationBase<SerializableRun>
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x00048A28 File Offset: 0x00046C28
		protected override void ApplyMigration(MigratingData saveData)
		{
		}
	}
}
