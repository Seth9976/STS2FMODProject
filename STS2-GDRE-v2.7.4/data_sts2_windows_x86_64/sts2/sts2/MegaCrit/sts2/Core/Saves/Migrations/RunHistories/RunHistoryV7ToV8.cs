using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Saves.Migrations.RunHistories
{
	/// <summary>
	/// No-op migration for release save wipe. All pre-release saves are invalidated
	/// via minimum supported version bump.
	/// </summary>
	// Token: 0x020000EE RID: 238
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(RunHistory), 7, 8)]
	public class RunHistoryV7ToV8 : MigrationBase<RunHistory>
	{
		// Token: 0x06000AA4 RID: 2724 RVA: 0x000493F3 File Offset: 0x000475F3
		protected override void ApplyMigration(MigratingData saveData)
		{
		}
	}
}
