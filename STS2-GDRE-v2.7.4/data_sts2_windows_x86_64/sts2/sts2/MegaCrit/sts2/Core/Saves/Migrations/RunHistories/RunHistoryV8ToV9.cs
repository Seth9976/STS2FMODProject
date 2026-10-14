using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Migrations.Shared;

namespace MegaCrit.Sts2.Core.Saves.Migrations.RunHistories
{
	/// <summary>
	/// Migrates renamed/deleted ModelIds in completed run history saves.
	/// Same renames as SerializableRunV15ToV16, applied to the RunHistory schema.
	/// </summary>
	// Token: 0x020000EF RID: 239
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(RunHistory), 8, 9)]
	public class RunHistoryV8ToV9 : MigrationBase<RunHistory>
	{
		// Token: 0x06000AA6 RID: 2726 RVA: 0x000493FD File Offset: 0x000475FD
		protected override void ApplyMigration(MigratingData saveData)
		{
			Log.Info("RunHistory migration v8 -> v9: Migrating renamed/deleted ModelIds", 2);
			SharedMigrationHelper.ReplaceModelIds(saveData.GetRawNode(), SharedMigrationHelper.v100Renames);
		}
	}
}
